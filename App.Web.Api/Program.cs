#region using
using App.DAL;
using App.DAL.Repositories;
using App.DDD.Domain.Base.EntityBase;
using App.DDD.Domain.Base.Security.JWT.Model;
using App.DDD.Domain.Models;
using App.Services.Services;
using Common;
using Common.Localization;
using Common.Localization.Model;
using Extension.Base;
using Identity.Extensions;
using Interfaces;
using Interfaces.IServices;
using MassTransitAndRabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using WatchDog;
using WatchDog.src.Enums;
//using WatchDog;
#endregion

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        //builder.Ho.Logging.AddConsole();

        var services = builder.Services;

        var connectionType = builder.Configuration["SettingBD:SelectDB"];
        //builder.Configuration.AddJsonFile("fewf",true,true);
        var RemoveDB = builder.Configuration["SettingBD:RemoveDB"].StringToBool();//UtilExtension 
        var connection_string = builder.Configuration["SettingBD:ConnectionStrings:" + connectionType];
        ConfigurationsDB(services, connectionType, connection_string);
        ConfigurationsService(builder);

        var app = builder.Build();
        using var scope = app.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DBInitializer>().InitializationAsync();


        app.LocalizationMiddleware();//для первода сайта или WebApi  Ставить выше авторизации!
        ConfigurationsWathDog(app);

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}
        app.UseExceptionHandler("/Error");
        app.UseHsts();
        //напиать свою мидлу которая будет  определять язык выбранного на сайте

        app.UseRouting(); //пути маршрута
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles(); //wwwroot
        app.UseCookiePolicy();//

        //app.UseResponseCompression(); //сжимаем ответ 
        app.UseResponseCaching();//use cache ,чтобы включить настройте службу в сервисах 
        app.MapControllers();
        app.UseHttpsRedirection(); //избежать ошибок когда указана не верная культура 

        //app.MapGet("/security/getMessage",
        //() => "Hello World!").RequireAuthorization();
        //app.UseWindowsService();
        app.Run();


        void ConfigurationsWathDog(WebApplication app)
        {
            var log = builder.Configuration["SettingsWathDog:login"];
            var pass = builder.Configuration["SettingsWathDog:password"];
            app.UseWatchDogExceptionLogger(); //./app.UseWatchDog();
            app.UseWatchDog(o =>
            {
                o.WatchPageUsername = log; //логин и пароль для  веб морды 
                o.WatchPagePassword = pass;
                // o.Blacklist = //"Test/weatherforecast";- укзаать Route который не надо отлеживать
                ///SettingsWathDog
            });
        }


        /// <summary>
        /// Конфигурация баз данных
        /// </summary>
        void ConfigurationsDB(IServiceCollection services, string? connectionType, string connection_string)
        {
            switch (connectionType)
            {
                case "SqlServer":
                    services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(connection_string, o => o.MigrationsAssembly("App.DAL.MSSQL")));
                    break;
                case "Postgres":
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //Для работы с датой и временем в Postgres //https://www.npgsql.org/doc/types/datetime.html
                    services.AddDbContext<AppDBContext>(opt => opt.UseNpgsql(connection_string, o => o.MigrationsAssembly("App.DAL.Postgres")));
                    break;
                case "Sqlite":
                    services.AddDbContext<AppDBContext>(opt => opt.UseSqlite(connection_string, o => o.MigrationsAssembly("App.DAL.SqLite")));
                    break;
                default:
                    throw new InvalidOperationException($"Тип БД не поддерживается: {connectionType}");

            }
            ///Рецепт по Intialization миграции
            ///перед каждей миграцией прописывать DBServer -тип кейса для миграции
            //Add-Migration Initial -v Update-Database Terminal 
            //для каждой таблицы в проекте которая прописана в конфиге.выбираем проекты по умолчанию

        }
        void ConfigurationsService(WebApplicationBuilder builder)
        {
            services = builder.Services; // IServiceCollection services

            services.AddResponseCaching();

            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("30SecondsCaching", new CacheProfile
                {
                    Duration = 30,
                });
            });
            services.AddStackExchangeRedisCache(opt => //Кеширования в REDIS (Docker)
            {
                opt.Configuration = builder.Configuration["RedisURL"];
            });



            services.AddDistributedMemoryCache();
            AddIOptions(services);
            AddCORS(services);
            AddRabbitMq(builder);

            AddSwagger(services);//Конфигурация Swagger
            AddWathDog(services);//Просмотр запросов поступающий на сервер мини- RabbitMQ
            AddAuthentication(services);

            RegistrationServiceAutoOrManual(services); //авторегистрация DI  контейнера в указанных .Dll 

            TryAddTransient(services);
            TryAddScoped(services);
            TryAddSingleton(services);
            AddHostedService(services);

           
        }



        /// далее в главном методе его настройка
        void AddWathDog(IServiceCollection services)
        {
            services.AddWatchDogServices(set =>
            {
                set.IsAutoClear = true;
                set.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
                //set.SqlDriverOption = WatchDogSqlDriverEnum.PostgreSql;
                //set.SetExternalDbConnString = "Server=localhost;Database=testLoggingDb;User Id=sa;Password=password;";

                //чистить логи старше одного месяца 
            });//Просмотр логов в удобном веб интерфейсе

        }

        //Запуск фоновых задач(Служб)
        void AddHostedService(IServiceCollection services)
        {
            services.AddHostedService<WathJsonFileHostedService>(); //отслеживает файлы при их изменении и создании,
        }

        //Если указать суфиксы по соглашению -этот метод Авторег. в DI
        void RegistrationServiceAutoOrManual(IServiceCollection services)
        {
            //BLL Services
            //ГАйд, чтобы автоматически зарегистрировать ваш контейнер в нужной области видимости укажите на конце класса сервиса 3 разных суфикса
            typeof(Program).Assembly.GetTypeFromAssembly(new List<string>()
    {
        "App.Services",//это названия перечесления сборок которые ссылаются на текущую, если они не указаны,произойдет поиск только в запускаемой сборке,так можно но не правильно
        "App.DAL",
    }).RegistrationAutoOrManual(services);
            //typeof(Program).Assembly.GetTypeFromAssembly().RegistrationAutoOrManual(services);

            //так же можно сделаь ручную регистрацию с более глубокой настройкой, пожалуйста используйте TryAdd 
        }

        //Конфигурация Swagger
        void AddSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "REST API", Version = "v1" }); // то чтовидно как переходим на страницу в sw
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
                });
            });


        }

        //Настройка междоменных запросов требуется для Angular,React
        void AddCORS(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(b =>
                {
                    b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
        }


        //получаем конфигурацию из json один раз, рекмендую  для динамичности использовать IOptionsSnapshot
        void AddIOptions(IServiceCollection services)
        {
            var addJWT = builder.Configuration.GetSection(nameof(JwtToken));
            var PathLocalization = builder.Configuration.GetSection(nameof(SettingLocalization));
            services.Configure<JwtToken>(addJWT);
            services.Configure<SettingLocalization>(PathLocalization);


        }

        //Работа с регистрацией jwt требуется сборка Identity
        void AddAuthentication(IServiceCollection services)
        {
            services.ConfigureIdentity(); //Станадртная форма управления Users,Role,Claim требуется сборка Identity

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "App.Web"; //имя куки
                opt.Cookie.HttpOnly = false; // Значение true, если файл cookie не должен быть доступен клиентским скриптом; в противном случае — значение false.
                                             //opt.Cookie.Expiration =TimeSpan.FromDays(10);// через  10 дней кука протухает и данные потеряют актуальность
                //opt.ExpireTimeSpan = TimeSpan.FromDays(10);// через  10 дней кука протухает и данные потеряют актуальность
                opt.LoginPath = "/Account/Login";  //страница авторизации 
                opt.LogoutPath = "/Account/Logout";  //страница авторизации 
                opt.AccessDeniedPath = "/Account/AccessDenied";  //перенаправить если доступ запрещен
                opt.SlidingExpiration = true;//менять id анонимным юзерам сайта,которые гуляли по нему до момента регистрации
            });
            services.ConfigureJWT(builder.Configuration,new TimeSpan(0,30,0));// требуется сборка Identity





        }

        //class1Repository
        void TryAddTransient(IServiceCollection services)
        {
            //services.TryAddTransient<DBInitializer>();
            //services.TryAddTransient<LocalizerMiddleware>();//регистрируем 
        }
        void TryAddScoped(IServiceCollection services)
        {
            services.TryAddScoped<DBInitializer>();
            services.TryAddScoped<LocalizerMiddleware>();
            //services.TryAddScoped(typeof(IRepositoryAsync<,>), RealFromAbstract2);
        }
        void TryAddSingleton(IServiceCollection services)
        {
            //services.TryAddSingleton<ITestService>();
        }
    }

    /// <summary>
    /// Подключение Rabbit и MassTransit
    /// </summary>
    /// <param name="services"></param>
    private static void AddRabbitMq(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.ConfigureRabbitMqAndMassTransit(builder.Configuration);


       
    }
}
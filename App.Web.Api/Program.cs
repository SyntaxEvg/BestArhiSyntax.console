#region using
using App.DAL;
using App.DAL.MySLQ;
using App.DAL.Postgres;
using App.DAL.SqLite;
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
#endregion

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        var services = builder.Services;

        var connectionType = builder.Configuration["SettingBD:SelectDB"];
        //builder.Configuration.AddJsonFile("fewf",true,true);
        var RemoveDB = builder.Configuration["SettingBD:RemoveDB"].StringToBool();//UtilExtension 
        var connection_string = builder.Configuration["SettingBD:ConnectionStrings:" + connectionType];
        ConfigurationsDB(services, connectionType, connection_string);
        ConfigurationsService(builder);

        var app = builder.Build();
        using var scope = app.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DBInitializer>().InitializationAsync(AutoMigrate: true);


        app.LocalizationMiddleware();//��� ������� ����� ��� WebApi  ������� ���� �����������!
        ConfigurationsWathDog(app);

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}
        app.UseExceptionHandler("/Error");
        app.UseHsts();
        app.UseRouting(); //���� ��������
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles(); //wwwroot
        app.UseCookiePolicy();//

        //app.UseResponseCompression(); //������� ����� 
        app.UseResponseCaching();//use cache ,����� �������� ��������� ������ � �������� 
        app.MapControllers();
        app.UseHttpsRedirection(); //�������� ������ ����� ������� �� ������ �������� 

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
                o.WatchPageUsername = log; //����� � ������ ���  ��� ����� 
                o.WatchPagePassword = pass;
                // o.Blacklist = //"Test/weatherforecast";- ������� Route ������� �� ���� ����������
                ///SettingsWathDog
            });
        }


        /// <summary>
        /// ������������ ��� ������
        /// </summary>
        void ConfigurationsDB(IServiceCollection services, string? connectionType, string connection_string)
        {
            switch (connectionType)
            {
                case "SqlServer":
                    services.UseSqlServer(connection_string);
                    break;
                case "Postgres":
                    services.UseNpgsql(connection_string);
                   break;
                case "Sqlite":
                    services.UseSqlite(connection_string);
                    break;
                default:
                    throw new InvalidOperationException($"��� �� �� ��������������: {connectionType}");

            }
            ///������ �� Intialization ��������
            ///����� ������ ��������� ����������� DBServer �� appconfig -��� (switch)����� ��� ��������
            //Add-Migration Initial -v Update-Database Terminal 
            //��� ������ ������� � ������� ������� ��������� � �������.�������� ������� �� ���������

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
            services.AddStackExchangeRedisCache(opt => //����������� � REDIS (Docker)
            {
                opt.Configuration = builder.Configuration["RedisURL"];
            });



            services.AddDistributedMemoryCache();
            AddIOptions(services);
            AddCORS(services);
            AddRabbitMq(builder);
            AddHttpClient(services);
            AddSwagger(services);//������������ Swagger
            AddWathDog(services);//�������� �������� ����������� �� ������ ����- RabbitMQ
            AddAuthentication(services);

            RegistrationServiceAutoOrManual(services); //��������������� DI  ���������� � ��������� .Dll 

            TryAddTransient(services);
            TryAddScoped(services);
            TryAddSingleton(services);
            AddHostedService(services);


        }



        /// ����� � ������� ������ ��� ���������
        void AddWathDog(IServiceCollection services)
        {
            services.AddWatchDogServices(set =>
            {
                set.IsAutoClear = true;
                set.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
                //set.SqlDriverOption = WatchDogSqlDriverEnum.PostgreSql;
                //set.SetExternalDbConnString = "Server=localhost;Database=testLoggingDb;User Id=sa;Password=password;";

                //������� ���� ������ ������ ������ 
            });//�������� ����� � ������� ��� ����������

        }

        //������ ������� �����(�����)
        void AddHostedService(IServiceCollection services)
        {
            services.AddHostedService<WathJsonFileHostedService>(); //����������� ����� ��� �� ��������� � ��������,
        }

        //���� ������� ������� �� ���������� -���� ����� �������. � DI
        void RegistrationServiceAutoOrManual(IServiceCollection services)
        {
            //BLL Services
            //����, ����� ������������� ���������������� ��� ��������� � ������ ������� ��������� ������� �� ����� ������ ������� 3 ������ �������
            typeof(Program).Assembly.GetTypeFromAssembly(new List<string>()
            {
                "App.Services",//��� �������� ������������ ������ ������� ��������� �� �������, ���� ��� �� �������,���������� ����� ������ � ����������� ������,��� ����� �� �� ���������
                "App.DAL",
            }).RegistrationAutoOrManual(services);
            //typeof(Program).Assembly.GetTypeFromAssembly().RegistrationAutoOrManual(services);

            //��� �� ����� ������ ������ ����������� � ����� �������� ����������, ���������� ����������� TryAdd 
        }

        //������������ Swagger
        void AddSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "REST API", Version = "v1" }); // �� �������� ��� ��������� �� �������� � sw
                //option.ParameterFilter
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                //option.ParameterFilter(new OpenApiParameter()
                //{
                //    Name = "X-Transaction-Id",
                //    In = ParameterLocation.Header,
                //    Required = true,
                //    AllowEmptyValue = false,
                //    Description = "Transaction Id to track the issue"
                //});
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
                }});;
            });


        }

        //��������� ����������� �������� ��������� ��� Angular,React
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


        //�������� ������������ �� json ���� ���, ���������  ��� ������������ ������������ IOptionsSnapshot
        void AddIOptions(IServiceCollection services)
        {
            var addJWT = builder.Configuration.GetSection(nameof(JwtToken));
            var PathLocalization = builder.Configuration.GetSection(nameof(SettingLocalization));
            services.Configure<JwtToken>(addJWT);
            services.Configure<SettingLocalization>(PathLocalization);


        }

        //������ � ������������ jwt ��������� ������ Identity
        void AddAuthentication(IServiceCollection services)
        {
            services.ConfigureIdentity(); //����������� ����� ���������� Users,Role,Claim ��������� ������ Identity

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "App.Web"; //��� ����
                opt.Cookie.HttpOnly = false; // �������� true, ���� ���� cookie �� ������ ���� �������� ���������� ��������; � ��������� ������ � �������� false.
                                             //opt.Cookie.Expiration =TimeSpan.FromDays(10);// �����  10 ���� ���� ��������� � ������ �������� ������������
                                             //opt.ExpireTimeSpan = TimeSpan.FromDays(10);// �����  10 ���� ���� ��������� � ������ �������� ������������
                opt.LoginPath = "/Account/Login";  //�������� ����������� 
                opt.LogoutPath = "/Account/Logout";  //�������� ����������� 
                opt.AccessDeniedPath = "/Account/AccessDenied";  //������������� ���� ������ ��������
                opt.SlidingExpiration = true;//������ id ��������� ������ �����,������� ������ �� ���� �� ������� �����������
                opt.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents()
                {//������� ����������� ��� ����� � ������� ��������� � �.�
                    OnSignedIn = async cont => //��� ��������� �� ��� ��������  � ������ ����� ������ �� ����� 
                    {
                        await Task.CompletedTask;
                    },
                    OnSigningIn = async cont =>
                    {
                        await Task.CompletedTask;
                    },
                    OnValidatePrincipal = async cont =>
                    {
                        await Task.CompletedTask;
                    },

                };
            });
            services.ConfigureJWT(builder.Configuration);// ��������� ������ Identit/





        }

        //class1Repository
        void TryAddTransient(IServiceCollection services)
        {
            //services.TryAddTransient<DBInitializer>();
            //services.TryAddTransient<LocalizerMiddleware>();//������������ 
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

    private static void AddHttpClient(IServiceCollection services)
    {
        //services.AddHttpClient("MyApiService").AddPolicyHandler(HttpPolicies.ExponentialRetry)
        //                                      .AddPolicyHandler(HttpPolicies.CircuitBreaker);

    }

    /// <summary>
    /// ����������� Rabbit � MassTransit
    /// </summary>
    /// <param name="services"></param>
    private static void AddRabbitMq(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.ConfigureRabbitMqAndMassTransit(builder.Configuration);



    }
}
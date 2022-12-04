

using App.DAL;

var builder = WebApplication.CreateBuilder(args);

var services =builder.Services;
var connectionType = builder.Configuration["SelectDB"];
 var connection_string = builder.Configuration.GetConnectionString(connectionType);
SettingsDB(services, connectionType);






// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


/// <summary>
/// Конфигурация баз данных
/// </summary>
void SettingsDB(IServiceCollection services, string? connectionType)
{
    switch (connectionType)
    {
        case "SqlServer":
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(connection_string, o => o.MigrationsAssembly("App.DAL.MSSQL")));
            break;
        case "Postgres":
            services.AddDbContext<AppDBContext>(opt => opt.UseNpgsql(connection_string, o => o.MigrationsAssembly("App.DAL.Postgres")));
            break;
        case "Sqlite":
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(connection_string, o => o.MigrationsAssembly("App.DAL.SqLite")));
            break;
    }
    ///Рецепт по Intialization миграции
    //Add-Migration Initial -v Update-Database Terminal //для каждой таблицы в проекте которая прописана в конфиге
    //  
}

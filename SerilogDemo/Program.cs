using Serilog;
using Serilog.Formatting.Compact;
using SerilogDemo;

var configuration = new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json")
                      .Build();
var nameAssembly = ReturnsAssemblyMethodCurrent.GetEntryAssembly();

Log.Logger = new LoggerConfiguration()
    .Enrich.WithMachineName()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft",Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console(new CompactJsonFormatter())
    .WriteTo.File(new CompactJsonFormatter(),$"Log/Log-{nameAssembly}_.json",rollingInterval: RollingInterval.Day)
   .CreateLogger();

Log.Logger.Information("Logging1 is working fine");
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

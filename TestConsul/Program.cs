using Consul;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration;
using TestConsul;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var consulHost = "http://localhost:8500";// http://localhost:8500/ui/dc1/kv/ConsulDemoKey%20/edit

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
var url =builder.Configuration["Consul:clientAddress"];
builder.Services.UseConul(url, "TestDC");

var IP =builder.Configuration["serviceInfo:ip"];
var port =builder.Configuration["serviceInfo:port"];

var urlsUse = $"http://{IP}:{port}";

builder.WebHost.UseUrls(urlsUse);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AddHostedService(builder.Services);


var AdressHealt = builder.Configuration["serviceInfo:healthCheckAddress"];
var app = builder.Build();
app.UseHealthChecks(AdressHealt);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
//app.UseMvc();
//app.UseDiscoveryClient();
app.Run();

void AddHostedService(IServiceCollection services)
{
    
}
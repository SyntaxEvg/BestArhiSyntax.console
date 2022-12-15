using App.DDD.Domain.Models;
using App.Services.Services;
using Common.SystemRX;
using MassTransit;
using MassTransitAndRabbitMQ;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigurationsService(builder);


        
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
    }

    private static void ConfigurationsService(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var Uri = builder.Configuration["RabbitMqSettings:Uri"];
        var UserName = builder.Configuration["RabbitMqSettings:UserName"];
        var Password = builder.Configuration["RabbitMqSettings:Password"];

        builder.Services.AddMassTransit(mt =>
        {
            mt.UsingRabbitMq((cntxt, cfg) =>
            {
                cfg.Host(new Uri(Uri), c =>
                {
                    c.Username(UserName);
                    c.Password(Password);
                });
                //cfg.ReceiveEndpoint("SendMail", (c) => { //в rabbit  отобразитьс€ это им€, создано дл€ понима€,кто  я подписчик )))
                //                                         //cfg.ReceiveEndpoint(new TemporaryEndpointDefinition(), (c) => { /Ќекоторым потребител€м нужно получать сообщени€ только при подключении, и любые сообщени€, опубликованные при отключении, следует отбрасывать
                //c.Consumer<ReceiverMessageService>();
                //    //c.Consumer<ReceiverMessageService>();
                //    //c.Consumer<ReceiverMessageService>();
                //    //c.Consumer<ReceiverMessageService>();
                //});
                cfg.ReceiveEndpoint("SendMail", e =>
                {
                    e.Handler<CommandMessageRequest>(async context => //простой обработчик который кладет в очередь  сообщени€
                    {
                        //SendMailQueue.Receive(context.Message);
                        if (SubscrMessageHostedService.subject is not null)
                        {
                            SubscrMessageHostedService.subject.OnNext(context.Message);
                        }
                       
                        await context.RespondAsync<CommandMessageResponse>(new { context.Message.id, status = true }).ConfigureAwait(false);
                        // await Console.Out.WriteLineAsync($"Submit Order Received: {context.Message}");
                    });
                    // e.ad
                });

                // cfg.ConfigureEndpoints(cntxt);
                //


            });
            mt.AddRequestClient<CommandMessageResponse>(new Uri("exchange:Test"));
        });

        AddHostedService(builder.Services);
    }

    private static void AddHostedService(IServiceCollection services)
    {
        services.AddHostedService<SubscrMessageHostedService>(); //отслеживает файлы при их изменении и создании,
    }
}
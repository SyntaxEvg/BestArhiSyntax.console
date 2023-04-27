using App.DDD.Domain.Models;
//using App.Services.Services;
using Common;
using MassTransit;
//using MassTransitAndRabbitMQ;
using Sender.Services.Sender;
using Sender.Services.Services;
using Sender.Services.Services.SystemRX;

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
        var services =builder.Services;
        var configuration = builder.Configuration;
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        services.AddHttpClient("MyApiService").AddPolicyHandler(HttpPolicies.ExponentialRetry)
                                              .AddPolicyHandler(HttpPolicies.CircuitBreaker);

        services.AddEmailSender(configuration); //Подключения сервиса  отправки почты
        //var Uri = builder.Configuration["RabbitMqSettings:Uri"];
        //var UserName = builder.Configuration["RabbitMqSettings:UserName"];
        //var Password = builder.Configuration["RabbitMqSettings:Password"];

        //builder.Services.AddMassTransit(mt =>
        //{
        //    mt.UsingRabbitMq((cntxt, cfg) =>
        //    {
        //        cfg.Host(new Uri(Uri), c =>
        //        {
        //            c.Username(UserName);
        //            c.Password(Password);
        //        });
        //        //cfg.ReceiveEndpoint("SendMail", (c) => { //в rabbit  отобразиться это имя, создано для понимая,кто  Я подписчик )))
        //        //                                         //cfg.ReceiveEndpoint(new TemporaryEndpointDefinition(), (c) => { /Некоторым потребителям нужно получать сообщения только при подключении, и любые сообщения, опубликованные при отключении, следует отбрасывать
        //        //c.Consumer<ReceiverMessageService>();
        //        //});
        //        cfg.ReceiveEndpoint("SendMail", e =>
        //        {
        //            e.Handler<CommandMessageRequest>(async context => //простой обработчик который кладет в очередь  сообщения
        //            {
        //                //SendMailQueue.Receive(context.Message);
        //                if (SubscrubeMessage.subjectMail is not null)
        //                {
        //                    //проверка трех полей,если они есть, создаем нового подписчика
        //                    SubscrubeMessage.subjectMail.OnNext(context.Message);
        //                }
                       
        //                await context.RespondAsync<CommandMessageResponse>(new { context.Message.id, status = true }).ConfigureAwait(false);
        //                // await Console.Out.WriteLineAsync($"Submit Order Received: {context.Message}");
        //            });
        //            // e.ad
        //        });

        //        // cfg.ConfigureEndpoints(cntxt);
        //        //


        //    });
        //    mt.AddRequestClient<CommandMessageResponse>(new Uri("exchange:Test"));
        //});
            // AddHostedService(builder.Services);
    }

    //private static void AddHostedService(IServiceCollection services)
    //{
    //    services.AddHostedService<SubscrubeMessageHostedService>();//Так как используется Rx,нам нужно прописывать место где создается прописка в текущей сборке, //Подписка на получения сообщен,и отправка их в сервис почты

    //}
}
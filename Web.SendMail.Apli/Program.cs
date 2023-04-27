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

        services.AddEmailSender(configuration); //����������� �������  �������� �����
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
        //        //cfg.ReceiveEndpoint("SendMail", (c) => { //� rabbit  ������������ ��� ���, ������� ��� �������,���  � ��������� )))
        //        //                                         //cfg.ReceiveEndpoint(new TemporaryEndpointDefinition(), (c) => { /��������� ������������ ����� �������� ��������� ������ ��� �����������, � ����� ���������, �������������� ��� ����������, ������� �����������
        //        //c.Consumer<ReceiverMessageService>();
        //        //});
        //        cfg.ReceiveEndpoint("SendMail", e =>
        //        {
        //            e.Handler<CommandMessageRequest>(async context => //������� ���������� ������� ������ � �������  ���������
        //            {
        //                //SendMailQueue.Receive(context.Message);
        //                if (SubscrubeMessage.subjectMail is not null)
        //                {
        //                    //�������� ���� �����,���� ��� ����, ������� ������ ����������
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
    //    services.AddHostedService<SubscrubeMessageHostedService>();//��� ��� ������������ Rx,��� ����� ����������� ����� ��� ��������� �������� � ������� ������, //�������� �� ��������� �������,� �������� �� � ������ �����

    //}
}
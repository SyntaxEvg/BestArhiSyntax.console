using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using MassTransit.Configuration;

namespace MassTransitAndRabbitMQ
{
    public static class RabbitMqConfigureExtension
    {
        public static void ConfigureRabbitMqAndMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            //Настройки MassTransit, получаем сексиюи из  APPConfig.json  приводим и к типу и используем
            var Uri = configuration["RabbitMqSettings:Uri"];
            var UserName = configuration["RabbitMqSettings:UserName"];
            var Password = configuration["RabbitMqSettings:Password"];

            services.AddMassTransit(mt =>
            mt.AddMassTransit(x =>
            { //стандартная настройка с офф сайта  //IPublishEndpoint -рег. автоматически вызывается на отправители
                //x.AddConsumer();
                x.UsingRabbitMq((cntxt, cfg) => //x.UsingRabbitMq() -дефолтные настройки на локальной машине
                {
                    cfg.Host(new Uri(Uri), "/",
                        c =>
                        {
                            c.Username(UserName);
                            c.Password(Password);
                        });
                    cfg.UseMessageRetry(r => r.Immediate(5)); //5 неудов, потом попадает в стек ошибок 
                    cfg.ConfigureEndpoints(cntxt);

                });
               
            }));
        }
    }

}
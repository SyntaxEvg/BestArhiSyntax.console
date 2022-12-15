using App.DDD.Domain.Models;
using MassTransit;

namespace Web.SendMail.Apli.Model
{
    public interface IConsumer<in TMessage> : IConsumer where TMessage : class
    {
        Task Consume(ConsumeContext<TMessage> context);
    }
    public interface IConsumer { }

    
}

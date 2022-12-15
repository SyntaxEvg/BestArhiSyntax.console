using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitAndRabbitMQ.Consumer
{

    //public interface IConsumer<in TMessage> : IConsumer where TMessage : class
    //{
    //    Task Consume(ConsumeContext<TMessage> context);
    //}
    //public interface IConsumer { }

    //public class CommandMessageConsumer : IConsumer<CommandMessage>
    //{
    //    public async Task Consume(ConsumeContext<CommandMessage> context)
    //    {
    //        var message = context.Message;
    //        await Console.Out.WriteLineAsync($"Message from Producer : {message.MessageString}");
    //        // Do something useful with the message
    //    }
    //}

}

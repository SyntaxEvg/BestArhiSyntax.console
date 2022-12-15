using App.DDD.Domain.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reactive.Subjects;


public class read1 : IObserver<CommandMessageRequest>
{
    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(CommandMessageRequest value)
    {
        Console.WriteLine(value.messageString);

    }
}

public class read3// ..: <CommandMessageRequest>
{ 
   
}


public static class SendMailQueue
{
    public readonly static Subject<CommandMessageRequest> subject;
    static SendMailQueue()
    {
        subject = new();
    }

    public static void Receive(CommandMessageRequest command)
    {
        subject.OnNext(command);
        subject.Subscribe();

    }

    static int i=0;
    /// <summary>
    /// Принимаем почту помещает в событие System.Reactive
    /// </summary>
    /// <param name="CommandMessage"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static void Receive1(CommandMessageRequest CommandMessage)
    {

        Debug.WriteLine(CommandMessage.messageString +";" + i++);
    }
}
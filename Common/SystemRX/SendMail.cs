using App.DDD.Domain.Models;

namespace Common.SystemRX
{
    public class SendMail : IObserver<CommandMessageRequest>
    {
        private readonly string typeMessage;

        public SendMail(string TypeMessage)
        {
            typeMessage = TypeMessage;
        }

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
}

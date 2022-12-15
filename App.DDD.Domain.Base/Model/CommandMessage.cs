namespace App.DDD.Domain.Models
{
    public class CommandMessageRequest
    {
        public Guid id { get; init; }
        public string messageString { get; init; }

        public CommandMessageRequest()
        {

        }
    } 
    public class CommandMessageResponse
    {
        public Guid id { get; init; }
        public bool status { get; init; }

        public CommandMessageResponse()
        {

        }
    }
}

//MailKitSimplified.Receiver
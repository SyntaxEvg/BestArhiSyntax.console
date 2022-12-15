using MassTransit;

namespace MassTransitAndRabbitMQ
{
    /// <summary>
    /// используется для Ioption Config
    /// </summary>
    public class RabbitMqSettings
    {
        public string Uri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
   

}
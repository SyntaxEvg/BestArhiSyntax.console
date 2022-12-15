
using Microsoft.AspNetCore.Mvc;

namespace Web.SendMail.Apli.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendMessageController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<SendMessageController> _logger;

        public SendMessageController(ILogger<SendMessageController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //[Route("send")]
        //public IEnumerable<WeatherForecast> send()
        //{
        //    public class CommandMessageConsumer : IConsumer<CommandMessage>
        //{
        //    public async Task Consume(ConsumeContext<CommandMessage> context)
        //    {
        //        var message = context.Message;
        //        await Console.Out.WriteLineAsync($ "Message from Producer : {message.MessageString}");
        //        // Do something useful with the message
        //    }
        //}

        //    //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    //{
        //    //    Date = DateTime.Now.AddDays(index),
        //    //    TemperatureC = Random.Shared.Next(-20, 55),
        //    //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    //})
        //    //.ToArray();
        //}
    }
}
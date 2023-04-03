
using App.DDD.Domain.Models;
using App.Services.Services;
using Common.Util;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Sender.Services.Services.SystemRX;

namespace Web.SendMail.Apli.Controllers
{
    [ApiVersion("1.0")] //добавим бутафорию, чтобы показать,какая версия клиента используется в выз.методе
    [ApiController]
    [Route("[controller]")]
    public class SendMessageController : ControllerBase
    {
        private readonly ILogger<SendMessageController> _logger;

        public SendMessageController(ILogger<SendMessageController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> message(CommandMessageRequest commandMessage, string lang = "ru-RU")
        {
            if (SubscrubeMessage.subjectMail is not null)
            {
                //проверка трех полей,если они есть, создаем нового подписчика
                SubscrubeMessage.subjectMail.OnNext(commandMessage);
            }
            return Ok(new ResponseMessage(true, "Message sent"));
        }
    }
}
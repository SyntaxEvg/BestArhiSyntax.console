using App.DDD.Domain.Models;
using Interfaces.Base.Base;
using MassTransit;
using MassTransitAndRabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Api.Controllers
{
    [ApiController]
    public class SendMessageController : Controller
    {
        private readonly ISendMessageScopedService<CommandMessageRequest> sendMessageScopedService;

        //public IActionResult Index()
        //{
        //    return View();
        //}
        public SendMessageController(ISendMessageScopedService<CommandMessageRequest> sendMessageScopedService)
        {
            this.sendMessageScopedService = sendMessageScopedService;
        }



        [HttpPost]
        [Route("sendmessage")]
        public async Task<IActionResult> Sendmessage(CommandMessageRequest commandMessage)
        {
            await sendMessageScopedService.Send(commandMessage);
            return Ok(commandMessage);
        }
    }
}

﻿using App.DDD.Domain.Models;
using Common.Util;
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
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage(CommandMessageRequest commandMessage,string lang ="ru-RU" )
        {
            await sendMessageScopedService.Send(commandMessage);
            return Ok(new ResponseMessage(true, "Message sent"));
        }
    }
}

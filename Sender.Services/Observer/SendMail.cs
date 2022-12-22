using App.DDD.Domain.Models;
using Extension.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Sender.Services.Interfaces;
using Sender.Services.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Services.Events
{
    /// <summary>
    /// Класс отправляет письма из очереди Singalton
    /// </summary>
    public sealed class SendMail : IObserver<CommandMessageRequest>
    {
        private readonly string typeMessage;
      
        private readonly SmtpSender _smtpSender;
        private readonly ILogger<SendMail> _logger;
        private object Lock = new object();
        private readonly ConcurrentDictionary<string, SmtpSender> CollectionSmtpSender = new();

        public SendMail(string TypeMessage, SmtpSender smtpSender, ILogger<SendMail> logger)//IOptions<EmailSenderOptions> senderOptions
        {
            typeMessage = TypeMessage;
            this._smtpSender = smtpSender;
            _logger = logger;
            Task.Run(async () =>
            {
                await ClearInterval();
            });
        }
       
       /// <summary>
       /// Очиста устаревших email подписок    
       /// </summary>
        private async Task ClearInterval()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromHours(0.01)).ContinueWith(x =>
                {
                    if (CollectionSmtpSender.Count > 0)
                    {
                        List<string> tempKey = new List<string>();
                        foreach (var item in CollectionSmtpSender)
                        {
                            if (item.Value.DateExpirationSmtpSender < DateTime.UtcNow)
                                tempKey.Add(item.Key);
                        }
                        foreach (var key in tempKey)
                        {
                            CollectionSmtpSender.TryRemove(key, out var smtpSender);
                            smtpSender.Dispose();
                        }
                        tempKey = null;



                    }
                });    
            }
            

        }



        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public  void OnNext(CommandMessageRequest value)
        {
           
            bool newConncet = value.password is not null && (value.From is not null && value.From.Contains("@")) && value.SmtpHost is not null;
            if (newConncet) //тогда создаем новую конфигурацию помещаем ее в словарь 
            {
                if (CollectionSmtpSender.TryGetValue(value.From, out var smtpSender)) 
                {
                     WriteEmail(value, _smtpSender);

                }
                else
                {
                    var smptSett = $"{value.SmtpHost}:{value.SmtpPort}";
                    NetworkCredential smtpCredential = new NetworkCredential
                    {
                        Password = value.password,
                        UserName = value.From
                    };
                    ushort.TryParse(value.SmtpPort, out var Port);
                    if (Port < 1)
                    {// не забыть сделать валидацию полей
                        return;
                    }
                    var setCred = new SmtpSender(smtpHost:value.SmtpHost,
                                                 smtpPort: Port,
                                                 smtpCredential: smtpCredential);//.Create(smptSett).SetCredential(value.From, value.password);

                    CollectionSmtpSender.TryAdd(value.From, setCred);
                    WriteEmail(value, setCred);
                    return;
                }
            }
            WriteEmail(value, _smtpSender);



        }

        private (bool,string) WriteEmail(CommandMessageRequest value, SmtpSender smtpSender)
        {
            lock (Lock)
            {
                var res = smtpSender.WriteEmail
                .From(value.From)
                .To(value.To)
                .Bcc(value.Bcc)
                .Subject(value.Subject)
                .BodyHtml(value.BodyHtml)
                .Cc(value.Cc)
                .Attach(value.AttachFile.Base64ToStream(), value.NameFile)
                .TrySendAsync(default).ConfigureAwait(false).GetAwaiter().GetResult();
                if (res.Item1)
                {
                    //успешно отправлено, возбудить событие и отправить  в шину 
                }
                else
                {
                    _logger.LogError($"Возникли проблемы, смотрите логи в LogTrace{value.id};{res.Item2}");//вернуть клиенту
                    //возникли проблемы 
                }
                return res;
            }
        }
    }
}

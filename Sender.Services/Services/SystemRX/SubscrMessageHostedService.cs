using App.DDD.Domain.Models;
using Extension.Base;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sender.Services.Events;
using Sender.Services.Models;
using Sender.Services.Services;
using Sender.Services.Services.SystemRX;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Data;
using System.Reactive.Subjects;
using System.Text.Json;

namespace Sender.Services.Services
{
    /// <summary>
    /// Сервис следит за изменениями файлов перевода и складывает их динамический словарь 
    /// </summary>
    public class SubscrubeMessageHostedService : BackgroundService
    {
        
       

        private readonly ILogger<SubscrubeMessageHostedService> _logger;
        private readonly ILogger<SendMail> _loggerSmtpSender;
        private readonly IOptions<EmailSenderOptions> _options;

        public SubscrubeMessageHostedService(ILogger<SubscrubeMessageHostedService> logger,
            ILogger<SendMail> loggerSmtpSender,
            IOptions<EmailSenderOptions> options)
        {
            _logger = logger;
            _loggerSmtpSender = loggerSmtpSender;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //SendMail //Подписался передал параметры и забыл
            var smtpSender = new SmtpSender(_options);
            smtpSender.DateExpirationSmtpSender = DateTime.MaxValue;//дефолтное отправление не чистим,все остальные временные храним сутки,потом удалаем
            SubscrubeMessage.subjectMail.Subscribe(new SendMail("SendMail",smtpSender, _loggerSmtpSender)); //дефолт
            //подписка на очистку 
          

        _logger.LogInformation($"Subscribe Starting: {nameof(SubscrubeMessageHostedService)}");
            return base.StartAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _logger.LogInformation($"Disposing: {nameof(SubscrubeMessageHostedService)}");
            SubscrubeMessage.subjectMail.Dispose();
            base.Dispose();
        }
    }
}

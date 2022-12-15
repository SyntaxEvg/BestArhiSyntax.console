using App.DDD.Domain.Models;
using Common.Localization;
using Common.Localization.Model;
using Extension.Base;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Text.Json;

namespace Common.SystemRX
{
    /// <summary>
    /// Сервис следит за изменениями файлов перевода и складывает их динамический словарь 
    /// </summary>
    public class SubscrMessageHostedService : BackgroundService
    {
        public readonly static Subject<CommandMessageRequest> subject = new();

        private readonly ILogger<SubscrMessageHostedService> _logger;

        public SubscrMessageHostedService(ILogger<SubscrMessageHostedService> logger, IOptions<SettingLocalization> options)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //SendMail
            subject.Subscribe(new SendMail("SendMail"));
            _logger.LogInformation($"Subscribe Starting: {nameof(WathJsonFileHostedService)}");
            return base.StartAsync(cancellationToken);
        }

        private async Task<ConcurrentDictionary<string, string>> EnumerateFiles(FileInfo fileInfo)
        {
            try
            {
                var @string = await fileInfo.ReadTextBuilder();
                var values = JsonSerializer.Deserialize<ConcurrentDictionary<string, string>>(@string.ToString());
                return values;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error OpenFileChange: {nameof(WathJsonFileHostedService)},{ex.Message}");
                return null;
            }
        }


        public override void Dispose()
        {
            _logger.LogInformation($"Disposing: {nameof(WathJsonFileHostedService)}");
            subject.Dispose();
            base.Dispose();
        }
    }
}

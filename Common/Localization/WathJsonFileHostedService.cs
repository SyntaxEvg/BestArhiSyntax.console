using Common.Localization;
using Common.Localization.Model;
using Extension.Base;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Common
{
    /// <summary>
    /// Сервис следит за изменениями файлов перевода и складывает их динамический словарь 
    /// </summary>
    public class WathJsonFileHostedService : BackgroundService
    {
        private readonly ILogger<WathJsonFileHostedService> _logger;
        private FileSystemWatcher _folderWatcher;
        private readonly string _inputFolder;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> langDictionary = ConcurrentDictionaryLang._Lang;
        private DateTime lastRead = DateTime.MinValue;

        public WathJsonFileHostedService(ILogger<WathJsonFileHostedService> logger, IOptions<SettingLocalization> options)
        {
            _logger = logger;
            if (options.Value.Path is not null &&  options.Value.Path != "Localization")
            {
                _inputFolder = options.Value.Path;
            }
            else
            _inputFolder = "Localization";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
        public override  Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Service Starting: {nameof(WathJsonFileHostedService)}");
            var CurrentDirectoryPath = _inputFolder;
            if (_inputFolder == "Localization")//это означает полный путь к папке в каком то другом месте 
            {
                CurrentDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localization");

            }
            DirectoryInfo dir = new(CurrentDirectoryPath);
            if (!(dir.Exists))
            {
                _logger.LogError($"Error folder path not found: {nameof(WathJsonFileHostedService)},{CurrentDirectoryPath}");
                Environment.Exit(1);
            }
            _folderWatcher = new FileSystemWatcher(CurrentDirectoryPath, "*.json")
            {
                NotifyFilter = NotifyFilters.LastWrite// | NotifyFilters.Size

            };
            _folderWatcher.Changed += ReloadDictionary;//можно не только обновлять но и создавать культуры не выключаея сервис
            _folderWatcher.Created += ReloadDictionary;
            _folderWatcher.EnableRaisingEvents = true;


            foreach (var file in dir.EnumerateFiles("*.json", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var values = EnumerateFiles(file).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (values is null) continue;
                    var FN = file.Name.Replace(file.Extension, "", StringComparison.OrdinalIgnoreCase);
                    langDictionary.AddOrUpdate(FN, values, (x, y) => y = values);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error folder path not foreach: {nameof(WathJsonFileHostedService)},{ex.StackTrace}");
                }
            }


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
        public void TryGetValueDelegate1()
        {
            langDictionary.TryGetValue("key", out var eef);

        }

        private ConcurrentDictionary<string, string> Update(string key, ConcurrentDictionary<string, string> value)
        {
            if (langDictionary.TryGetValue(key, out var old))
            {
                if (langDictionary.TryUpdate(key, value, old))
                {
                    return value;
                }
            }
            return null;
        }

        /// <summary>
        /// Событие обновляет словарики с переводом,можно создать в той же папки новый словарь, важно чтобы он соответвовал культуре, новый язык появится автоматически на сайте  
        /// </summary>
        /// <param name="source"></param>
        /// <param name="file"></param>
        protected async void ReloadDictionary(object source, FileSystemEventArgs file)
        {

            var FN = file.Name!.Replace(".json","",StringComparison.OrdinalIgnoreCase);
            FileInfo fileInfo = new FileInfo(file.FullPath);
            DateTime lastWriteTime = fileInfo.LastWriteTimeUtc;//проверка по последнему изменению помогает отсеивать ложные события
            if (lastWriteTime != lastRead)
            {
                var values = await EnumerateFiles(fileInfo).ConfigureAwait(true);//.GetAwaiter().GetResult();
                if (values is null)
                {
                    return;
                }
                //langDictionary.AddOrUpdate(FN, values, Update);
                lastRead = lastWriteTime;

                langDictionary.AddOrUpdate(FN, values, (x,y) =>y = values);
                var mess = $"File {file.ChangeType}: {file.Name}";
                _logger.LogInformation(mess);
            }
        }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping Service: {nameof(WathJsonFileHostedService)}");
            _folderWatcher.EnableRaisingEvents = false;
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Disposing: {nameof(WathJsonFileHostedService)}");
            _folderWatcher.Dispose();
            base.Dispose();
        }
    }
}

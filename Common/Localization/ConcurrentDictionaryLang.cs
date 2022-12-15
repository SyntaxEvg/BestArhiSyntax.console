using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Localization
{
    /// <summary>
    /// Хранения переводов на все время жизни приложения
    /// </summary>
    public static class ConcurrentDictionaryLang
    {
        public static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _Lang = new();
    }
}

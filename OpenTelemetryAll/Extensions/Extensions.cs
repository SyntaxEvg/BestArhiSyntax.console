using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenTelemetryAll.Extensions
{
    public static class ExtensionsTelemetry
    {
        /// <summary>
        /// Возвращает сборку метода, вызывавшего текущий исполняемый метод.
        /// </summary>
        public static string? GetEntryAssembly()
        {
            var NameAssembly = Assembly.GetEntryAssembly().GetName().Name;
            //return NameAssembly +"-" + Guid.NewGuid().ToString().Substring(0,8);
            return NameAssembly;// +"-" + Guid.NewGuid().ToString().Substring(0,8);
        }
    }
}

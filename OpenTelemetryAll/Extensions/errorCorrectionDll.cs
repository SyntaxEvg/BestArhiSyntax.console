using Microsoft.Extensions.Logging;
using OpenTelemetry.Instrumentation.StackExchangeRedis;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTelemetryAll.Extensions
{
    public static class errorCorrectionDll
    {
        public static TracerProviderBuilder AddRedisInstrumentationFix(this TracerProviderBuilder builder, IConnectionMultiplexer connection = null,Action<StackExchangeRedisCallsInstrumentationOptions> configure = null)
        {
            if (builder is not IDeferredTracerProviderBuilder deferredTracerProviderBuilder)
            {
                if (connection == null)
                {
                    return builder;
                }
                builder.AddRedisInstrumentation(connection, configure);
                return builder;
            }
            return builder;
        }
        public static ILoggingBuilder AddSerilogFix(this ILoggingBuilder builder, Serilog.Core.Logger? logger, bool dispose = false)//, LoggerConfiguration logger, bool dispose = false)
        {

            if (logger == null)
            {
                return builder;
            }
            return builder.AddSerilog(logger, dispose);           
        }
    }
}

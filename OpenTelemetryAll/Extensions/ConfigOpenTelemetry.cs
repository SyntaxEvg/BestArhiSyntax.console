using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using OpenTelemetry;
//using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetryAll.Models.Section;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTelemetryAll.Extensions
{
    public static class ConfigOpenTelemetry
    {
        /// <summary>
        /// ServiceName -Имя которое будет отображаться в трасировке
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="ServiceName"></param>
        /// <param name="isSerilog">включает логирования в файл заданный в конфигурации</param>
        /// <returns></returns>
        [Obsolete]
        public static WebApplicationBuilder UseOpenTelemetryTracing(this WebApplicationBuilder? builder, string ServiceName = "test",bool isSerilog=true)
        {
            try
            {
                var attributes = new Dictionary<string, object>
                {
                    ["host.name"] = Environment.MachineName,
                    ["os.description"] = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                    ["deployment.environment"] = builder?.Environment?.EnvironmentName?.ToLowerInvariant()!
                };
                //builder?.Services.Configure<JaegerExporterOptions>(builder.Configuration.GetSection("Jaeger"));
                var serviceJaeger = builder?.Configuration.GetValue<string>("Jaeger:ServiceName");
                var OtlpExporter = builder?.Configuration.GetSection("OpenTelemetry:OtlpExporter").Get<OtlpExporter>();

                var resourceBuilder = ResourceBuilder.CreateDefault()
                                       .AddService(ServiceName)
                                       .AddTelemetrySdk()
                                       .AddAttributes(attributes);
                // #Configure metrics

                string customMeterName = "MyMeter";
                var meter = new Meter(customMeterName);
                var counter = meter.CreateCounter<long>("app.request-counter"); //это как пример кол-во запросов
               

                #region config sirilog 
                var logger = isSerilog ? new LoggerConfiguration().ReadFrom.Configuration(builder?.Configuration).Enrich.FromLogContext().CreateLogger(): null;
                #endregion
                builder?.Services.AddOpenTelemetryMetrics(opt =>
                {
                    opt.AddHttpClientInstrumentation()
                   .AddAspNetCoreInstrumentation()
                   .AddMeter(meter.Name)
                   .SetResourceBuilder(resourceBuilder)
                   .AddOtlpExporter(opt =>
                   {
                       opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                       opt.Endpoint = new Uri(OtlpExporter?.Endpoint + OtlpExporter?.Port);
                       //opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                   });
                });
                //# Configure logging
                builder?.Services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders()
                    .AddSerilogFix(logger)
                    .AddFilter("*", LogLevel.Information)
                    .AddJsonConsole()
                    
                    .AddOpenTelemetry(logOpt =>
                    {
                        logOpt
                            .SetResourceBuilder(resourceBuilder)
                            .AddProcessor(new OpenTelemetryAll.LogProcessor.LogProcessor())
                            .AddOtlpExporter(opt =>
                            {
                                opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                                opt.Endpoint = new Uri(OtlpExporter?.Endpoint + OtlpExporter?.Port);
                                opt.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                                {
                                    MaxQueueSize = 2048,
                                    ScheduledDelayMilliseconds = 5000,
                                    ExporterTimeoutMilliseconds = 30000,
                                    MaxExportBatchSize = 512,
                                };
                            });
                        logOpt.IncludeFormattedMessage = true;
                        logOpt.IncludeScopes = true;
                        logOpt.ParseStateValues = true;
                        logOpt.IncludeScopes = true;
                    });
                });




                ////builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
                ////{
                ////    tracerProviderBuilder
                ////        .AddSource(ServiceName)
                ////        .SetResourceBuilder(resourceBuilder)
                ////        .AddAspNetCoreInstrumentation()
                ////        .AddOtlpExporter(opt =>
                ////        {
                ////            opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                ////            var url = OtlpExporter?.Endpoint + OtlpExporter?.Port;
                ////            opt.Endpoint = new Uri(OtlpExporter?.Endpoint + OtlpExporter?.Port);
                ////            opt.Endpoint = new Uri(url);
                ////            opt.Endpoint = new Uri("http://host.docker.internal:4317");
                ////        }
                ////    );
                ////});


                builder?.Services.AddOpenTelemetryTracing((opt) => opt

                                    .AddSource(ServiceName)
                                    .SetResourceBuilder(resourceBuilder)
                                    .AddAspNetCoreInstrumentation()
                                    .AddHttpClientInstrumentation()
                                    .AddGrpcCoreInstrumentation()
                                    .AddRedisInstrumentationFix()
                                    .AddEntityFrameworkCoreInstrumentation(opt =>
                                        {
                                            opt.SetDbStatementForText = true;
                                            opt.SetDbStatementForStoredProcedure = true;
                                        }) //покажет запрос к базе данных                                                                  
                                    .AddOtlpExporter(opt =>
                                        {
                                            opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                                            opt.Endpoint = new Uri(OtlpExporter?.Endpoint + OtlpExporter?.Port);
                                            //opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                                        })

                                    .AddNpgsql()

                                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return builder;
        }
    }
}

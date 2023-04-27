using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTelemetryAll.Models.Section
{
    public class Rootobject
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public string UseExporter { get; set; }
        public bool UseLogging { get; set; }
        public Jaeger Jaeger { get; set; }
        public Opentelemetryсollector opentelemetryСollector { get; set; }
    }
    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class Jaeger
    {
        public string ServiceName { get; set; }
        public string AgentHost { get; set; }
        public int AgentPort { get; set; }
    }

    public class Opentelemetryсollector
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
    public class OtlpExporter
    {
        public string Endpoint { get; set; }
        public int Port { get; set; }
        public string ExportProcessorType { get; set; }
        public string Protocol { get; set; }
    }


}

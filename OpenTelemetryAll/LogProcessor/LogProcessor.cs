using OpenTelemetry;
using OpenTelemetry.Logs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTelemetryAll.LogProcessor
{
    public class LogProcessor: BaseProcessor<LogRecord>
    {
        public override void OnStart(LogRecord data)
        {
            base.OnStart(data); 
        }
        public override void OnEnd(LogRecord data)
        {
            base.OnEnd(data);
        }
    }
    internal class MyEnrichingProcessor : BaseProcessor<Activity>
    {
        public override void OnEnd(Activity activity)
        {
            // Enrich activity with additional tags.
            activity.SetTag("myCustomTag", "myCustomTagValue");

            //// Enriching from Baggage.
            //// The below snippet adds every Baggage item.
            //foreach (var baggage in Baggage.GetBaggage())
            //{
            //    activity.SetTag(baggage.Key, baggage.Value);
            //}

            // The below snippet adds specific Baggage item.
            var deviceTypeFromBaggage = Baggage.GetBaggage("device.type");
            if (deviceTypeFromBaggage != null)
            {
                activity.SetTag("device.type", deviceTypeFromBaggage);
            }
        }
    }
}

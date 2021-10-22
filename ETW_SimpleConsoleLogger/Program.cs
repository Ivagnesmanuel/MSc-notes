using Microsoft.Diagnostics.Tracing;
using System;

namespace ETW_SimpleConsoleLogger
{
    class Program
    {
        private const string TracingEtlPath = @"C:\Program Files\ETW_TraceCollector\tracingSession.etl";

        static void Main(string[] args)
        {
            using (var source = new ETWTraceEventSource(TracingEtlPath))
            {
                // Set up the callbacks
                source.Dynamic.All += delegate (TraceEvent data) {

                    Console.WriteLine($" --- [[{data.ProviderName} -> {data.EventName} \tIndex: {data.EventIndex} ]] ---");
                    Console.WriteLine($"Timestamp: {data.TimeStamp.ToUniversalTime()}");
                    for (int i = 0; i < data.PayloadNames.Length; i++)
                    {
                        string payloadName = data.PayloadNames[i];
                        Console.WriteLine($"line {i}  =>  {payloadName}:\t{data.PayloadByName(payloadName)} \t");
                    }
                    Console.WriteLine($"Message: {data.FormattedMessage}\n\n");
                };

                source.Process();
            }
        }
    }
}

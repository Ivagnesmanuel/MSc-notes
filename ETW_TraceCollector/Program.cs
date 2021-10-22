using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using ETW_TraceCollector.CustomParsers;


namespace ETW_TraceCollector
{
    class Program
    {
        // settings
        private const string TracingSessionName = "MyTracingSession";
        private const string TracingEtlPath = @"C:\Program Files\ETW_TraceCollector\";
        private const string TracingEtlFilePath = TracingEtlPath + "tracingSession.etl";


        // LIST PROVIDERS
        static List<string> Providers = new List<string> {
            "WindowsKernelTrace",                                 // Windows kernel trace  {9E814AAD-3204-11D2-9A82-006008A86939}
            "Microsoft-Windows-Kernel-Memory",                    // Memory {D1D93EF7-E1F2-4F45-9943-03D245FE6C00}
            //"Microsoft-Windows-Kernel-Network",                   // Network {7DD42A49-5329-4832-8DFD-43D979153A88}
            //"Microsoft-Windows-Kernel-Power",                     // Power {331C3B3A-2005-44C2-AC5E-77220C37D6B4}
        };


        static void Main(string[] args)
        {
            // checks admin privileges
            try
            {

                if (!Directory.Exists(TracingEtlPath))
                {
                    // Try to create the directory.
                    Directory.CreateDirectory(TracingEtlPath);
                    Console.WriteLine($"The logging directory {TracingEtlPath} was successfully created\n");
                }


                // Enable providers
                using var session = new TraceEventSession(TracingSessionName, TracingEtlFilePath);
                foreach (string provider in Providers)
                {
                    if (provider == "WindowsKernelTrace")
                    {
                        int flag = KernelCustomParser.CalculateKernelFlag();

                        if (flag == 0x0)
                            continue;

                        if (!session.EnableKernelProvider((KernelTraceEventParser.Keywords)flag))
                            throw new ArgumentException("Unable to abilitate Windows Kernel Trace Provider", "session");
                        else
                            Console.WriteLine("Enabled KernelProvider with follwing flags: {0}", Convert.ToString(flag, 2));
                    }
                    else
                    {
                        // return value is not consisent (do not check)
                        session.EnableProvider(provider);
                        Console.WriteLine($"Enabled provider: {provider}");
                    }

                }


                bool loop = true;
                Console.WriteLine($"Write <STOP> to stop collecting events:");
                while (loop)
                {
                    Thread.Sleep(1000);
                    if (Console.ReadLine().Equals("STOP"))
                        loop = false;
                }

            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Access Denied. Unauthorized ETW session creation. Try as Administrator.\nSource: {ex.Source}\nStackTrace: {ex.StackTrace}\nMessage: {ex.Message}");
                Environment.Exit(0);
            }
        }

    }
}


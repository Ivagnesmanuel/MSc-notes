using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.IO;
using System.Threading;

using ETW_TraceCollector.CustomParsers;


namespace ETW_TraceCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            // checks admin privileges
            try
            {

                if (!Directory.Exists(GlobalConstant.TracingEtlPath))
                {
                    // Try to create the directory.
                    Directory.CreateDirectory(GlobalConstant.TracingEtlPath);
                    Console.WriteLine($"The logging directory {GlobalConstant.TracingEtlPath} was successfully created\n");
                }


                // Enable providers
                using var session = new TraceEventSession(GlobalConstant.TracingSessionName, GlobalConstant.TracingEtlFilePath);
                foreach (string provider in GlobalConstant.Providers)
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


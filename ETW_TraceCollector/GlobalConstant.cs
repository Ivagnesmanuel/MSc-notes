using System.Collections.Generic;

namespace ETW_TraceCollector
{
    class GlobalConstant
    {
        public const string TracingSessionName = "MyTracingSession";
        public const string TracingEtlPath = @"C:\Windows\ETW_TraceCollector\";
        public const string TracingEtlFilePath = TracingEtlPath + "tracingSession.etl";

        // LIST PROVIDERS
        public static List<string> Providers = new List<string> {
            "WindowsKernelTrace",                                 // Windows kernel trace  {9E814AAD-3204-11D2-9A82-006008A86939}
            //"Microsoft-Windows-Kernel-Memory",                    // Memory {D1D93EF7-E1F2-4F45-9943-03D245FE6C00}
            //"Microsoft-Windows-Kernel-Network",                   // Network {7DD42A49-5329-4832-8DFD-43D979153A88}
            //"Microsoft-Windows-Kernel-Power",                     // Power {331C3B3A-2005-44C2-AC5E-77220C37D6B4}
            // other providers
        };
    }
}

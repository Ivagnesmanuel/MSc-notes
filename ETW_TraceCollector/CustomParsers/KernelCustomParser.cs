using System;
using System.Collections.Generic;

namespace ETW_TraceCollector.CustomParsers
{
    class KernelCustomParser
    {
        // kernel flags from 'logman query providers "{9E814AAD-3204-11D2-9A82-006008A86939}"'
        private static readonly Dictionary<string, int> EventKernelMap = new Dictionary<string, int>()
        {
            { "Process", 1 },
            { "Thread", 2 },
            { "ImageLoad", 4 },
            { "ProcessCounters", 8 },
            { "ContextSwitches", 16 },
            { "DeferredProcedureCalls", 32 },
            { "Interrupts", 64 },
            { "SystemCall", 128 },  
            { "DiskIO", 256 },
            { "FileDetails", 512 },
            { "DIskInit", 1024 },
            { "Dispatcher", 2048 },
            { "PageFaults", 4096 },
            { "HardPageFaults", 8192 },
            { "VirtualMemory", 16384 },
            { "NetworkTCPIP", 65536 },
            { "Registry", 131072 },
            { "ALPC", 1048576 },
            { "SplitIO", 2097152 },
            { "Driver", 8388608 },
            { "SampleBasedProfiling", 16777216 },
            { "FileIOcompletition", 33554432 },
            { "FileIO", 67108864 },
            { "Hidden1", 32768 },
            { "Hidden2", 262144 },
            { "Hidden3", 524288 },
            { "Hidden4", 4194304 },
        };

        // LIST KERNEL CATEGORIES
        private static readonly List<string> KernelCategories = new List<string> {
            "Process",
            "Thread",
            "ImageLoad",
            "ProcessCounters",
            "ContextSwitches",
            "DeferredProcedureCalls",
            "Interrupts",
            "SystemCall",
            "DiskIO",
            "FileDetails",
            "DIskInit",
            "Dispatcher",
            "PageFaults",
            "HardPageFaults",
            "VirtualMemory",
            "NetworkTCPIP",
            "Registry",
            "ALPC",
            "SplitIO", 
            "Driver",
            "SampleBasedProfiling",
            "FileIOcompletition",
            "FileIO",
            //"Hidden1", 
            //"Hidden2", -> error 
            //"Hidden3", -> error
            //"Hidden4", -> error
        };


        // To get only specified keywords in kernel provider
        public static int CalculateKernelFlag()
        {
            int flag = 0x0;

            foreach (string kernelCategory in KernelCategories)
            {
                if (!EventKernelMap.TryGetValue(kernelCategory, out int kFlag))
                {
                    Console.WriteLine($"Unable to find kernel flag associated to {kernelCategory}");
                    continue;
                }
                flag |= kFlag;
            }

            return flag;
        }

    }
}


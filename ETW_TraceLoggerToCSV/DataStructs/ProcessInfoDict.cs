using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System.Collections.Concurrent;

namespace ETW_TraceLoggerToCSV.DataStructs
{
    class ProcessInfoDict 
    {
        public ConcurrentDictionary<int, ProcessInfo> Processes { get; set; }

        public ProcessInfoDict()
        {
            Processes = new ConcurrentDictionary<int, ProcessInfo>();
        }


        // get ProcessName from PID
        public string TryGetProcessName(Microsoft.Diagnostics.Tracing.TraceEvent evt) 
        {
            if (!string.IsNullOrEmpty(evt.ProcessName))
                return evt.ProcessName;
            return Processes.TryGetValue(evt.ProcessID, out ProcessInfo info) ? info.ProcessName : string.Empty;
        }


        // get ParentName from ParentPID
        public string TryGetParentName(ProcessTraceData evt)
        {
            return Processes.TryGetValue(evt.ParentID, out ProcessInfo info) ? info.ProcessName : string.Empty;
        }

        public string TryGetParentProcessName(ThreadTraceData evt)
        {
            return Processes.TryGetValue(evt.ParentProcessID, out ProcessInfo info) ? info.ProcessName : string.Empty;
        }


        // save process in dictionary to save name
        public int AddProcessToDict(int ProcessID, string ProcessName, int attempt = 0)
        {
            try
            {   
                Processes.TryAdd(ProcessID, new ProcessInfo(ProcessID, ProcessName));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{ProcessName} added to RunningProcesses dictionary");
                Console.ForegroundColor = ConsoleColor.White;
                return 1;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Tried to add {ProcessName} with already existing PID: {ProcessID}");
                if (Processes.TryGetValue(ProcessID, out var processInstance))
                    Console.WriteLine($"\tSearch in dict gets: {processInstance.ProcessName} with PID: {processInstance.ProcessId}");
                Console.ForegroundColor = ConsoleColor.White;

                // using recycled PID
                RemoveProcessFromDict(ProcessID);
                if (attempt < 3){
                    AddProcessToDict(ProcessID, ProcessName, attempt++);
                }
                return 0;
            }
        }


        // remove process from dict and deallocare memory
        public int RemoveProcessFromDict(int ProcessID)
        {
            try
            {
                Processes.TryRemove(ProcessID, out ProcessInfo processInstance);
                processInstance = null;
                return 1;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Tried to remove process that is not in dictionary");
                Console.ForegroundColor = ConsoleColor.White;
                return 0;
            }
        }

    }
}

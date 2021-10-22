using System;
using System.Collections.Generic;


namespace ETW_TraceLoggerToCSV.DataStructs
{
    class ProcessInfo
    {
        public ProcessInfo(int id, string name)
        {
            ProcessId = id;
            ProcessName = name;
        }

        public int ProcessId { get; }
        public string ProcessName { get; }

    }
}

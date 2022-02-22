using System.IO;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;

using ETW_TraceLoggerToCSV.DataStructs;

namespace ETW_TraceLoggerToCSV.CustomParsers
{
    class KernelCustomParser
    {   
        private static void PrintAll(string parsedData, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly, string processName, string parentName = "")
        {
            outputPrintFile.WriteLine(parsedData);
            outputPrintFile.Flush();

            if (GlobalConstant.SampleName == processName || GlobalConstant.SampleName == parentName)
            {
                outputPrintFile_sampleOnly.WriteLine(parsedData);
                outputPrintFile_sampleOnly.Flush();
            }
        }


        public static void LogProcessStart(ProcessInfoDict RunningProcesses, ProcessTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string parentName = RunningProcesses.TryGetParentName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                data.ProcessName.ToString().Replace(",", " "),
                parentName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ParentID,
                data.CommandLine.Replace(",", " "),
                data.SessionID,
                data.Flags.ToString().Replace(",", " "),
                data.ExitStatus, 
                data.UniqueProcessKey);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName, parentName);
        }

        public static void LogProcessStop(ProcessInfoDict RunningProcesses, ProcessTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            string parentName = RunningProcesses.TryGetParentName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                parentName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ParentID,
                data.CommandLine.Replace(",", " "),
                data.SessionID,
                data.Flags.ToString().Replace(",", " "),
                data.ExitStatus,
                data.UniqueProcessKey);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName, parentName);
        }

        public static void LogProcessEvent(ProcessInfoDict RunningProcesses, ProcessTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            string parentName = RunningProcesses.TryGetParentName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
               data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
               data.ProviderName.ToString().Replace(",", " "),
               data.EventName.ToString().Replace(",", " "),
               pName.ToString().Replace(",", " "),
               parentName.ToString().Replace(",", " "),
               data.ProcessID,
               data.ParentID,
               data.ThreadID,
               data.CommandLine.Replace(",", " "),
               data.SessionID,
               data.Flags.ToString().Replace(",", " "),
               data.ExitStatus, 
               data.UniqueProcessKey);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName, parentName);
        }

        internal static void LogProcessDCEvent(ProcessInfoDict RunningProcesses, ProcessTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            string parentName = RunningProcesses.TryGetParentName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
               data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
               data.ProviderName.ToString().Replace(",", " "),
               data.EventName.ToString().Replace(",", " "),
               pName.ToString().Replace(",", " "),
               parentName.ToString().Replace(",", " "),
               data.ProcessID,
               data.ParentID,
               data.ThreadID,
               data.CommandLine.Replace(",", " "),
               data.SessionID,
               data.Flags.ToString().Replace(",", " "),
               data.ExitStatus,
               data.UniqueProcessKey);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName, parentName);
        }

        internal static void LogProcessCtr(ProcessInfoDict RunningProcesses, ProcessCtrTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3}",
               data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
               data.ProviderName.ToString().Replace(",", " "),
               data.EventName.ToString().Replace(",", " "),
               pName.ToString().Replace(",", " "));
            //"",
            //"",
            //"",
            //"",
            //"",
            //"",
            //"",
            //"");

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogThreadEvent(ProcessInfoDict RunningProcesses, ThreadTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            string parentName = RunningProcesses.TryGetParentProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                parentName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ParentProcessID,
                data.ParentThreadID,
                data.ThreadFlags.ToString().Replace(",", " "),
                data.IoPriority.ToString().Replace(",", " "));

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName, parentName);
        }

        public static void LogImageLoad(ProcessInfoDict RunningProcesses, ImageLoadTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            string DllName = Path.GetFileName(data.FileName);

            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                DllName.ToString().Replace(",", " "),
                data.FileName.ToString().Replace(",", " "));

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogImageUnLoad(ProcessInfoDict RunningProcesses, ImageLoadTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            string DllName = Path.GetFileName(data.FileName);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                DllName.ToString().Replace(",", " "),
                data.FileName.ToString().Replace(",", " "));

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        internal static void LogImageLoadBacked(ProcessInfoDict RunningProcesses, MemoryImageLoadBackedTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            string DllName = Path.GetFileName(data.FileName);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                DllName.ToString().Replace(",", " "),
                data.FileName.ToString().Replace(",", " "));

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        

        public static void LogDiskEvent(ProcessInfoDict RunningProcesses, DiskIOTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.FileName.ToString().Replace(",", " "),
                data.ThreadID,
                data.Irp,
                data.IrpFlags.ToString().Replace(",", " "),
                data.DiskNumber,
                data.TransferSize,
                data.Priority.ToString().Replace(",", " "));

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogDiskInitEvent(ProcessInfoDict RunningProcesses, DiskIOInitTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                "",
                data.ThreadID,
                data.Irp);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        internal static void LogDiskFlushBuff(ProcessInfoDict RunningProcesses, DiskIOFlushBuffersTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                "",
                data.ThreadID,
                data.Irp);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogFileCreate(ProcessInfoDict RunningProcesses, FileIOCreateTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ThreadID,
                data.FileName.ToString().Replace(",", " "),
                data.FileObject,
                "",
                data.IrpPtr,
                data.CreateOptions.ToString().Replace(",", " "),
                data.CreateDisposition.ToString().Replace(",", " "),
                data.FileAttributes.ToString().Replace(",", " "),
                data.ShareAccess.ToString().Replace(",", " "));

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogFileDelete(ProcessInfoDict RunningProcesses, FileIOInfoTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ThreadID,
                data.FileName.ToString().Replace(",", " "),
                data.FileObject,
                data.FileKey, 
                data.IrpPtr);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }



        public static void LogFileReadWrite(ProcessInfoDict RunningProcesses, FileIOReadWriteTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ThreadID,
                data.FileName.ToString().Replace(",", " "),
                data.FileObject,
                data.FileKey,
                data.IrpPtr,
                "",
                "",
                "",
                "",
                data.IoSize,
                data.IoFlags.ToString().Replace(",", " "));

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogFileClose(ProcessInfoDict RunningProcesses, FileIOSimpleOpTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ThreadID,
                data.FileName.ToString().Replace(",", " "),
                data.FileObject,
                data.FileKey,
                data.IrpPtr);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogFileInfo(ProcessInfoDict RunningProcesses, FileIOInfoTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ThreadID,
                data.FileName.ToString().Replace(",", " "),
                data.FileObject,
                data.FileKey,
                data.IrpPtr);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogFileIOFileEvent(ProcessInfoDict RunningProcesses, FileIONameTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ThreadID,
                data.FileName.ToString().Replace(",", " "),
                "",
                data.FileKey);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        public static void LogFileIODirEvent(ProcessInfoDict RunningProcesses, FileIODirEnumTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ThreadID,
                data.FileName.ToString().Replace(",", " "),
                data.FileObject,
                data.FileKey,
                data.IrpPtr);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        internal static void LogFileMapEvent(ProcessInfoDict RunningProcesses, MapFileTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID,
                data.ThreadID,
                data.FileName.ToString().Replace(",", " "),
                "",
                data.FileKey);

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }

        internal static void LogFileEndOp(ProcessInfoDict RunningProcesses, FileIOOpEndTraceData data, StreamWriter outputPrintFile, StreamWriter outputPrintFile_sampleOnly)
        {
            string pName = RunningProcesses.TryGetProcessName(data);
            var parsedData = string.Format("{0},{1},{2},{3},{4}",
                data.TimeStamp.ToUniversalTime().ToString().Replace(",", " "),
                data.ProviderName.ToString().Replace(",", " "),
                data.EventName.ToString().Replace(",", " "),
                pName.ToString().Replace(",", " "),
                data.ProcessID.ToString().Replace(",", " "),
                data.ThreadID);
            //"",
            //"",
            //"",
            //"");

            PrintAll(parsedData, outputPrintFile, outputPrintFile_sampleOnly, data.ProcessName);
        }
    }
}


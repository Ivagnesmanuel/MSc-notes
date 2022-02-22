using System.Collections.Generic;

namespace ETW_TraceLoggerToCSV
{
    public enum EventClass
    {
        ProcessStartStop,
        ProcessGeneric,
        Thread,
        Image,
        DiskIO,
        File,
        Generic
    }

    class GlobalConstant
    {
        // sample settings
        public const string TraceName = "";

        // etl name encoding: family_sampleName
        private static readonly string[] tmp = TraceName.Split('_');
        public static readonly string SampleName = tmp.Length > 1 ? tmp[1] : string.Empty; 

        // input etl path
        public static readonly string TracingEtlPath = $@"C:\_ETW_TraceLoggerToCSV_Data\ETW_tracingSessions\{TraceName}.etl";

        // out CSVs paths 
        private static readonly string UserName = System.IO.Path.GetFileName(System.Environment.UserName);
        public static readonly string PrintFiles_path = $@"C:\Users\{UserName}\OneDrive\Desktop\ETW_{TraceName}_CSVfiles\";
        public static readonly string PrintFiles_path_sampleOnly = PrintFiles_path + @"\sample_only\";


        // files for logging and Headers
        public static readonly Dictionary<EventClass, (string, string)> CSVFiles = new Dictionary<EventClass, (string, string)>()
        {
            { EventClass.ProcessStartStop, (
                "kernelProcessStartStopEvents.csv",
                "TimeStamp,ProviderName,EventName,ProcessName,ParentName,ProcessID,ParentID,CommandLine,SessionID,Flags,ExitStatus,UniqueProcessKey"
                )},
            { EventClass.ProcessGeneric, (
                "kernelProcessGenericEvents.csv",
                "TimeStamp,ProviderName,EventName,ProcessName,ParentName,ProcessID,ParentID,ThreadID,CommandLine,SessionID,Flags,ExitStatus,UniqueProcessKey"
                )},
            { EventClass.Thread, (
                "kernelThreadEvents.csv",
                "TimeStamp,ProviderName,EventName,ProcessName,ParentName,ProcessID,ParentID,ParentThreadID,ThreadFlags,IoPriority"
                )},
            { EventClass.Image, (
                "kernelImagesEvents.csv",
                "TimeStamp,ProviderName,EventName,ProcessName,ProcessID,DllName,FileName"
                )},
            { EventClass.DiskIO, (
                "kernelDiskIOEvents.csv",
                "TimeStamp,ProviderName,EventName,ProcessName,ProcessID,FileName,ThreadID,Irp,IrpFlags,DiskNumber,TransferSize,Priority"
                )},
            { EventClass.File, (
                "kernelFilesEvents.csv",
                "TimeStamp,ProviderName,EventName,ProcessName,ProcessID,ThreadID,Filename,FileObject,FileKey,IrpPtr,CreateOptions,CreateDisposition,FileAttributes,ShareAccess,IoSize,IoFlags"
                ) },
        };

        // filesPaths dictionary for events from all processes 
        public static Dictionary<EventClass, string> GlobalCSVFileNames = new Dictionary<EventClass, string>();
        // filesPaths dictionary for events from the selected process only
        public static Dictionary<EventClass, string> SpecificProcessCSVFileNames = new Dictionary<EventClass, string>();


    }
}

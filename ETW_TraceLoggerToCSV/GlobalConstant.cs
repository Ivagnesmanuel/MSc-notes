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
        public const string FamilyName = "";
        public const string SampleName = "code";

        // input etl path
        public const string EtlInputFilename = "tracingSession.etl";
        public const string TracingEtlPath = @"C:\ETW_ransomware_eventsLogger\ETW_ransomware_tracingSessions\" + EtlInputFilename;

        // out CSVs paths 
        public const string PrintFiles_path = @"C:\Users\ivagn\OneDrive\Desktop\ETW_" + FamilyName + "_" + SampleName + @"_files\";
        public const string PrintFiles_path_sampleOnly = PrintFiles_path + @"\sample_only\";


        // files for logging and Headers
        public static readonly Dictionary<EventClass, (string, string)> CSVFiles = new Dictionary<EventClass, (string, string)>()
        {
            { EventClass.ProcessStartStop, (
                "kernelStartStopEvents.csv",
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
                "TimeStamp,ProviderName,EventName,ProcessName,ProcessID,Filename,FileObject,FileKey,IrpPtr,CreateOptions,CreateDisposition,FileAttributes,ShareAccess,IoSize,IoFlags"
                ) },
        };

        // filesPaths dictionary for events from all processes 
        public static Dictionary<EventClass, string> GlobalCSVFileNames = new Dictionary<EventClass, string>();
        // filesPaths dictionary for events from the selected process only
        public static Dictionary<EventClass, string> SpecificProcessCSVFileNames = new Dictionary<EventClass, string>();


    }
}

using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System;
using System.IO;

using ETW_TraceLoggerToCSV.CustomParsers;
using ETW_TraceLoggerToCSV.DataStructs;
using ETW_TraceLoggerToCSV.Helpers;
using System.Collections.Generic;

namespace ETW_TraceLoggerToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            // checks admin privileges
            try
            {
                // initialize struct for processes data
                ProcessInfoDict RunningProcesses = new ProcessInfoDict();

                // initalize the logging folder
                Printer.InitPrintFolder();

                // initialize Streams dictionaries
                Dictionary<EventClass, StreamWriter> GlobalStreams = new Dictionary<EventClass, StreamWriter>();
                Dictionary<EventClass, StreamWriter> SpecificProcessStreams = new Dictionary<EventClass, StreamWriter>();

                // initialize writers 
                foreach (KeyValuePair<EventClass, (string, string)> CSVname in GlobalConstant.CSVFiles)
                {
                    // Item1 is the fileName
                    string GlobalPath = GlobalConstant.PrintFiles_path + CSVname.Value.Item1;
                    string SpecificPath = GlobalConstant.PrintFiles_path_sampleOnly + CSVname.Value.Item1;

                    // populate Paths dictionaries
                    GlobalConstant.GlobalCSVFileNames.Add(CSVname.Key, GlobalPath);
                    GlobalConstant.SpecificProcessCSVFileNames.Add(CSVname.Key, SpecificPath);

                    //using StreamWriter GlobalStream = new StreamWriter(GlobalPath, true);
                    //using StreamWriter SpecificStream = new StreamWriter(SpecificPath, true);

                    // populate Streams dictionaries
                    GlobalStreams.Add(CSVname.Key, new StreamWriter(GlobalPath, true));
                    SpecificProcessStreams.Add(CSVname.Key, new StreamWriter(SpecificPath, true));
                }



                // Events parsing
                using (var source = new Microsoft.Diagnostics.Tracing.ETWTraceEventSource(GlobalConstant.TracingEtlPath))
                {
                    var kernelParser = new KernelTraceEventParser(source);

                    // Kernel events parsing - Process start/stop
                    kernelParser.ProcessStart += delegate (ProcessTraceData data)
                    {
                        KernelCustomParser.LogProcessStart(RunningProcesses, data, GlobalStreams[EventClass.ProcessStartStop], SpecificProcessStreams[EventClass.ProcessStartStop]);
                        RunningProcesses.AddProcessToDict(data.ProcessID, data.ProcessName);
                    };

                    kernelParser.ProcessStop += delegate (ProcessTraceData data)
                    {
                        KernelCustomParser.LogProcessStop(RunningProcesses, data, GlobalStreams[EventClass.ProcessStartStop], SpecificProcessStreams[EventClass.ProcessStartStop]);
                        RunningProcesses.RemoveProcessFromDict(data.ProcessID);
                    };


                    // Kernel events parsing - Process events
                    kernelParser.ProcessStartGroup += delegate (ProcessTraceData data)
                    {
                        KernelCustomParser.LogProcessEvent(RunningProcesses, data, GlobalStreams[EventClass.ProcessGeneric], SpecificProcessStreams[EventClass.ProcessGeneric]);
                    };

                    kernelParser.ProcessEndGroup += delegate (ProcessTraceData data)
                    {
                        KernelCustomParser.LogProcessStop(RunningProcesses, data, GlobalStreams[EventClass.ProcessGeneric], SpecificProcessStreams[EventClass.ProcessGeneric]);
                    };

                    kernelParser.ProcessGroup += delegate (ProcessTraceData data)
                    {
                        KernelCustomParser.LogProcessEvent(RunningProcesses, data, GlobalStreams[EventClass.ProcessGeneric], SpecificProcessStreams[EventClass.ProcessGeneric]);
                    };

                    kernelParser.ProcessDCStart += delegate (ProcessTraceData data)
                    {
                        KernelCustomParser.LogProcessDCEvent(RunningProcesses, data, GlobalStreams[EventClass.ProcessGeneric], SpecificProcessStreams[EventClass.ProcessGeneric]);
                    };

                    kernelParser.ProcessDCStop += delegate (ProcessTraceData data)
                    {
                        KernelCustomParser.LogProcessDCEvent(RunningProcesses, data, GlobalStreams[EventClass.ProcessGeneric], SpecificProcessStreams[EventClass.ProcessGeneric]);
                    };

                    kernelParser.ProcessDefunct += delegate (ProcessTraceData data)
                    {
                        KernelCustomParser.LogProcessDCEvent(RunningProcesses, data, GlobalStreams[EventClass.ProcessGeneric], SpecificProcessStreams[EventClass.ProcessGeneric]);
                    };




                    // Kernel events parsing - Threads 
                    kernelParser.ThreadStart += delegate (ThreadTraceData data)
                    {
                        KernelCustomParser.LogThreadEvent(RunningProcesses, data, GlobalStreams[EventClass.Thread], SpecificProcessStreams[EventClass.Thread]);
                    };

                    kernelParser.ThreadStop += delegate (ThreadTraceData data)
                    {
                        KernelCustomParser.LogThreadEvent(RunningProcesses, data, GlobalStreams[EventClass.Thread], SpecificProcessStreams[EventClass.Thread]);
                    };

                    kernelParser.ThreadStartGroup += delegate (ThreadTraceData data)
                    {
                        KernelCustomParser.LogThreadEvent(RunningProcesses, data, GlobalStreams[EventClass.Thread], SpecificProcessStreams[EventClass.Thread]);
                    };

                    kernelParser.ThreadEndGroup += delegate (ThreadTraceData data)
                    {
                        KernelCustomParser.LogThreadEvent(RunningProcesses, data, GlobalStreams[EventClass.Thread], SpecificProcessStreams[EventClass.Thread]);
                    };




                    // Kernel events parsing - Image Load 
                    kernelParser.ImageLoad += delegate (ImageLoadTraceData data)
                    {
                        KernelCustomParser.LogImageLoad(RunningProcesses, data, GlobalStreams[EventClass.Image], SpecificProcessStreams[EventClass.Image]);
                    };

                    kernelParser.ImageUnload += delegate (ImageLoadTraceData data)
                    {
                        KernelCustomParser.LogImageUnLoad(RunningProcesses, data, GlobalStreams[EventClass.Image], SpecificProcessStreams[EventClass.Image]);
                    };

                    kernelParser.ImageDCStart += delegate (ImageLoadTraceData data)
                    {
                        KernelCustomParser.LogImageLoad(RunningProcesses, data, GlobalStreams[EventClass.Image], SpecificProcessStreams[EventClass.Image]);
                    };

                    kernelParser.ImageDCStop += delegate (ImageLoadTraceData data)
                    {
                        KernelCustomParser.LogImageLoad(RunningProcesses, data, GlobalStreams[EventClass.Image], SpecificProcessStreams[EventClass.Image]);
                    };

                    kernelParser.ImageGroup += delegate (ImageLoadTraceData data)
                    {
                        KernelCustomParser.LogImageLoad(RunningProcesses, data, GlobalStreams[EventClass.Image], SpecificProcessStreams[EventClass.Image]);
                    };

                    kernelParser.ImageLoadGroup += delegate (ImageLoadTraceData data)
                    {
                        KernelCustomParser.LogImageLoad(RunningProcesses, data, GlobalStreams[EventClass.Image], SpecificProcessStreams[EventClass.Image]);
                    };

                    kernelParser.ImageUnloadGroup += delegate (ImageLoadTraceData data)
                    {
                        KernelCustomParser.LogImageLoad(RunningProcesses, data, GlobalStreams[EventClass.Image], SpecificProcessStreams[EventClass.Image]);
                    };

                    kernelParser.MemoryImageLoadBacked += delegate (MemoryImageLoadBackedTraceData data)
                    {
                        KernelCustomParser.LogImageLoadBacked(RunningProcesses, data, GlobalStreams[EventClass.Image], SpecificProcessStreams[EventClass.Image]);
                    };




                    //// Kernel events parsing - Process Counter 
                    //kernelParser.ProcessPerfCtr += delegate (ProcessCtrTraceData data)
                    //{
                    //    KernelCustomParser.LogProcessCtr(RunningProcesses, data, kernelProcessStream, kernelProcessStream_sampleOnly);
                    //};

                    //kernelParser.ProcessPerfCtrRundown += delegate (ProcessCtrTraceData data)
                    //{
                    //    KernelCustomParser.LogProcessCtr(RunningProcesses, data, kernelProcessStream, kernelProcessStream_sampleOnly);
                    //};




                    //// Kernel events parsing - Thread Context switch
                    //kernelParser.ThreadCSwitch += delegate (CSwitchTraceData data)
                    //{
                    //    KernelCustomParser.LogCSwitch(RunningProcesses, data, kernelCSwitchStream, kernelCSwitchStream_sampleOnly);
                    //};




                    //// Kernel events parsing - deffered procedure calls 
                    //kernelParser.PerfInfoDPC += delegate (DPCTraceData data)
                    //{
                    //    KernelCustomParser.LogDPC(RunningProcesses, data, kernelDPCStream, kernelDPCStream_sampleOnly);
                    //};

                    //kernelParser.PerfInfoThreadedDPC += delegate (DPCTraceData data)
                    //{
                    //    KernelCustomParser.LogDPC(RunningProcesses, data, kernelDPCStream, kernelDPCStream_sampleOnly);
                    //};

                    //kernelParser.PerfInfoTimerDPC += delegate (DPCTraceData data)
                    //{
                    //    KernelCustomParser.LogDPC(RunningProcesses, data, kernelDPCStream, kernelDPCStream_sampleOnly);
                    //};



                    //kernelParser.PerfInfoISR += delegate (ISRTraceData data)
                    //{
                    //    KernelCustomParser.LogISR(RunningProcesses, data, kernelISRStream, kernelISRStream_sampleOnly);
                    //};



                    // Kernel events parsing - Disk
                    kernelParser.DiskIORead += delegate (DiskIOTraceData data)
                    {
                        KernelCustomParser.LogDiskEvent(RunningProcesses, data, GlobalStreams[EventClass.DiskIO], SpecificProcessStreams[EventClass.DiskIO]);
                    };

                    kernelParser.DiskIOWrite += delegate (DiskIOTraceData data)
                    {
                        KernelCustomParser.LogDiskEvent(RunningProcesses, data, GlobalStreams[EventClass.DiskIO], SpecificProcessStreams[EventClass.DiskIO]);
                    };

                    kernelParser.DiskIOFlushBuffers += delegate (DiskIOFlushBuffersTraceData data)
                    {
                        KernelCustomParser.LogDiskFlushBuff(RunningProcesses, data, GlobalStreams[EventClass.DiskIO], SpecificProcessStreams[EventClass.DiskIO]);
                    };

                    kernelParser.DiskIOReadInit += delegate (DiskIOInitTraceData data)
                    {
                        KernelCustomParser.LogDiskInitEvent(RunningProcesses, data, GlobalStreams[EventClass.DiskIO], SpecificProcessStreams[EventClass.DiskIO]);
                    };

                    kernelParser.DiskIOWriteInit += delegate (DiskIOInitTraceData data)
                    {
                        KernelCustomParser.LogDiskInitEvent(RunningProcesses, data, GlobalStreams[EventClass.DiskIO], SpecificProcessStreams[EventClass.DiskIO]);
                    };

                    kernelParser.DiskIOFlushInit += delegate (DiskIOInitTraceData data)
                    {
                        KernelCustomParser.LogDiskInitEvent(RunningProcesses, data, GlobalStreams[EventClass.DiskIO], SpecificProcessStreams[EventClass.DiskIO]);
                    };



                    // Kernel events parsing - File IO - basic
                    kernelParser.FileIOCreate += delegate (FileIOCreateTraceData data)
                    {
                        KernelCustomParser.LogFileCreate(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };


                    kernelParser.FileIORead += delegate (FileIOReadWriteTraceData data)
                    {
                        KernelCustomParser.LogFileReadWrite(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOWrite += delegate (FileIOReadWriteTraceData data)
                    {
                        KernelCustomParser.LogFileReadWrite(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };


                    kernelParser.FileIODelete += delegate (FileIOInfoTraceData data)
                    {
                        KernelCustomParser.LogFileDelete(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIORename += delegate (FileIOInfoTraceData data)
                    {
                        KernelCustomParser.LogFileInfo(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOQueryInfo += delegate (FileIOInfoTraceData data)
                    {
                        KernelCustomParser.LogFileInfo(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOFSControl += delegate (FileIOInfoTraceData data)
                    {
                        KernelCustomParser.LogFileInfo(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOSetInfo += delegate (FileIOInfoTraceData data)
                    {
                        KernelCustomParser.LogFileInfo(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };


                    kernelParser.FileIOClose += delegate (FileIOSimpleOpTraceData data)
                    {
                        KernelCustomParser.LogFileClose(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    //kernelParser.FileIOCleanup += delegate (FileIOSimpleOpTraceData data)
                    //{
                    //    KernelCustomParser.LogFileClose(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    //};

                    //kernelParser.FileIOFlush += delegate (FileIOSimpleOpTraceData data)
                    //{
                    //    KernelCustomParser.LogFileClose(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    //};


                    //kernelParser.FileIOOperationEnd += delegate (FileIOOpEndTraceData data)
                    //{
                    //    KernelCustomParser.LogFileEndOp(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    //};



                    // file IO - directories
                    kernelParser.FileIODirEnum += delegate (FileIODirEnumTraceData data)
                    {
                        KernelCustomParser.LogFileIODirEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIODirNotify += delegate (FileIODirEnumTraceData data)
                    {
                        KernelCustomParser.LogFileIODirEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };



                    // file IO - file mapping
                    kernelParser.FileIOMapFile += delegate (MapFileTraceData data)
                    {
                        KernelCustomParser.LogFileMapEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOMapFileDCStart += delegate (MapFileTraceData data)
                    {
                        KernelCustomParser.LogFileMapEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOMapFileDCStop += delegate (MapFileTraceData data)
                    {
                        KernelCustomParser.LogFileMapEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOUnmapFile += delegate (MapFileTraceData data)
                    {
                        KernelCustomParser.LogFileMapEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };




                    // file IO - extra
                    kernelParser.FileIOFileCreate += delegate (FileIONameTraceData data)
                    {
                        KernelCustomParser.LogFileIOFileEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOFileDelete += delegate (FileIONameTraceData data)
                    {
                        KernelCustomParser.LogFileIOFileEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOFileRundown += delegate (FileIONameTraceData data)
                    {
                        KernelCustomParser.LogFileIOFileEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };

                    kernelParser.FileIOName += delegate (FileIONameTraceData data)
                    {
                        KernelCustomParser.LogFileIOFileEvent(RunningProcesses, data, GlobalStreams[EventClass.File], SpecificProcessStreams[EventClass.File]);
                    };



                    //// Kernel events parsing - Perf Info
                    //kernelParser.PerfInfoCollectionStart += delegate (SampledProfileIntervalTraceData data)
                    //{
                    //    KernelCustomParser.LogPerfInfo(RunningProcesses, data, kernelPerfInfoStream, kernelPerfInfoStream_sampleOnly);
                    //};

                    //kernelParser.PerfInfoCollectionEnd += delegate (SampledProfileIntervalTraceData data)
                    //{
                    //    KernelCustomParser.LogPerfInfo(RunningProcesses, data, kernelPerfInfoStream, kernelPerfInfoStream_sampleOnly);
                    //};



                    source.Process(); // Invoke callbacks for events in the source
                }
                

                // close writers
                foreach (KeyValuePair<EventClass, StreamWriter> CSVstream in GlobalStreams)
                {
                    CSVstream.Value.Close();
                }
                foreach (KeyValuePair<EventClass, StreamWriter> CSVstream in SpecificProcessStreams)
                {
                    CSVstream.Value.Close();
                }

                Console.WriteLine("\nLOGGING COMPLETED\n\n");

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


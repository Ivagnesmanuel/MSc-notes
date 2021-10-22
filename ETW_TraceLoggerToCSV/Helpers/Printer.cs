using System;
using System.Collections.Generic;
using System.IO;

namespace ETW_TraceLoggerToCSV.Helpers
{
    class Printer
    {
        public static void InitPrintFolder()
        {
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(GlobalConstant.PrintFiles_path))
                {
                    // Try to create the directory.
                    Directory.CreateDirectory(GlobalConstant.PrintFiles_path);
                    Directory.CreateDirectory(GlobalConstant.PrintFiles_path_sampleOnly);
                    Console.WriteLine($"The logging directory was successfully created\n");
                }

                // events from all processes 
                foreach (KeyValuePair<EventClass, string> CSVpath in GlobalConstant.GlobalCSVFileNames)
                {
                    string path = CSVpath.Value;
                    string header = GlobalConstant.CSVFiles[CSVpath.Key].Item2;
                    File.WriteAllText(path, header + "\n");
                }

                // events from the selected process only
                foreach (KeyValuePair<EventClass, string> CSVpath in GlobalConstant.SpecificProcessCSVFileNames)
                {
                    string path = CSVpath.Value;
                    string header = GlobalConstant.CSVFiles[CSVpath.Key].Item2;
                    File.WriteAllText(path, header + "\n");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to make the loggin directory. \nError: {0}\n", e.ToString());
            }
        }
    }
}


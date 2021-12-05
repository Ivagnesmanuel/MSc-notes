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
                } else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Take care, the directory already exist!!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Do you want to overwrite the content? Y/N");
                    string input = Console.ReadLine();
                    if (input == "N")
                    {
                        System.Environment.Exit(0);
                    }
                }


                // Make files with events from all processes 
                foreach (KeyValuePair<EventClass, (string, string)> CSVname in GlobalConstant.CSVFiles)
                {
                    string path = GlobalConstant.PrintFiles_path + CSVname.Value.Item1;
                    string header = CSVname.Value.Item2;
                    File.WriteAllText(path, header + "\n");
                }

                // Make files with events from the selected process only
                foreach (KeyValuePair<EventClass, (string, string)> CSVname in GlobalConstant.CSVFiles)
                {
                    string path = GlobalConstant.PrintFiles_path_sampleOnly + CSVname.Value.Item1;
                    string header = CSVname.Value.Item2;
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


# ETW_TraceLoggerToCSV

The idea is to parse the events inside the .etl files generated through Event Tracing for Windows (ETW) and convert them into CSV files for data analytics purposes.

In each project, there is a file called GlobalConstant where you can set paths, providers, CSV categories and anything else.

---
### Solution structure
- **ETW_SimpleConsoleLogger**: simple console logger to analyze events from new providers until we have the correct parser

- **ETW_TraceCollector**: simple tracer with customable providers

- **ETW_TraceLoggerToCSV**: converts the .etl files into CSV files, dividing them in different categories. 
    - The list of categories is represented as an enumeration, and the "CSVFiles" Dictionary contains the filename and Header for each CSV file category.
    - For each category, we have a file with the events for all the processes inside the input .etl file and another file with the events from a specific process only.

---
### Note
Feel free to add new parsers (also from other providers) and help me increase the type of events we can analyze with a simple python script :)

N.B. I just started learning C# and .NET, so feel free to give me advice.



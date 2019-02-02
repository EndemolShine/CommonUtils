using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace CommonUtilsX64
{
    public class ApplicationLog
    {
        private readonly StreamWriter _logFile;
        private const int Ok = 1;
        private const int NOk = -1;

        public ApplicationLog(string pAppName)
        {
            try
            {
                var logfileDir = ConfigurationManager.AppSettings["LogfileDir"];

                if (logfileDir == null)
                {
                    Console.WriteLine("Error ==> LogfileDir not set in application.");
                    return;
                }

                var di = new DirectoryInfo(logfileDir);
                if (!di.Exists)
                    di.Create();

                _logFile = File.AppendText($"{logfileDir}{pAppName}.Log");

                var fi = new FileInfo(string.Format("{0}{1}.Log", logfileDir, pAppName));

                if (fi.Length / 1024 / 1024 < 1) return;

                _logFile.Close();
                fi.MoveTo(
                    $"{logfileDir}{pAppName}-{DateTime.Now.Year}{DateTime.Now.Month:00}{DateTime.Now.Day:00}{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}.Log");
                _logFile = File.AppendText($"{logfileDir}{pAppName}.Log");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error ==> {e.Message}");
                _logFile = null;
            }
        }

        public static int WriteEntry(string pErrorMsg, EventLogEntryType pMessageType, string pMessageString, string pAppName)
        {
            var w = new ApplicationLog(pAppName);
            if (w._logFile == null) return NOk;
            w._logFile.WriteLine($"{DateTime.Now.ToLocalTime()} {pMessageType} {pMessageString} {pErrorMsg}\n");
            w._logFile.Flush();
            w._logFile.Close();
            return Ok;
        }
    }
}

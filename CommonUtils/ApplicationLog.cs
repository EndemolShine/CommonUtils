using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace CommonUtilsX64
{
    public class ApplicationLog
    {
        //Globals
        private readonly StreamWriter _logFile;

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

                var logLevel = ConfigurationManager.AppSettings["LogLevel"];

                if (logLevel == null)
                {
                    Console.WriteLine("Error ==> LogLevel not set in application.");
                    return;
                }

                var di = new DirectoryInfo(logfileDir);
                if (!di.Exists)
                    di.Create();

                _logFile = File.AppendText($"{logfileDir}{pAppName}.Log");

                var fi = new FileInfo($"{logfileDir}{pAppName}.Log");

                if (fi.Length / Math.Pow(1024,2) < 1) return;

                _logFile.Close();
                fi.MoveTo($"{logfileDir}{pAppName}-{DateTime.Now.Year}{DateTime.Now.Month:00}{DateTime.Now.Day:00}{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}{DateTime.Now.Millisecond:000}.Log");
                _logFile = File.AppendText($"{logfileDir}{pAppName}.Log");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error ==> {e.Message}");
                _logFile = null;
            }


        }

        public static int WriteEntry(string pAppName)
        {
            var w = new ApplicationLog(pAppName);
            if (w._logFile == null) return -1;
            if (Convert.ToInt16(ConfigurationManager.AppSettings["LogLevel"]) == 3)
                w._logFile.WriteLine($"{DateTime.Now.ToLocalTime()}");
            w._logFile.Flush();
            w._logFile.Close();
            return 0;
        }

        public static int WriteEntry(string pMsg, EventLogEntryType pMessageType, string pFunction, string pAppName)
        {
            var w = new ApplicationLog(pAppName);
            var retval = 0;

            if (w._logFile == null) return -1;

            /* LogLevels
             *
             * 0 = No logging, 1 = Errors, 2 Errors & Warnings, 3 = Errors, Warnings & Information
             *
             */

            switch (pMessageType)
            {
                case EventLogEntryType.Information:
                    if (Convert.ToInt16(ConfigurationManager.AppSettings["LogLevel"]) == 3)
                        w._logFile.WriteLine($"{DateTime.Now.ToLocalTime()} {pMessageType}: {pMsg} in {pFunction}");
                    break;
                case EventLogEntryType.Warning:
                    if (Convert.ToInt16(ConfigurationManager.AppSettings["LogLevel"]) >= 2)
                        w._logFile.WriteLine($"{DateTime.Now.ToLocalTime()} {pMessageType}: {pMsg} in {pFunction}");
                    break;
                case EventLogEntryType.Error:
                    if (Convert.ToInt16(ConfigurationManager.AppSettings["LogLevel"]) >= 1)
                        w._logFile.WriteLine($"{DateTime.Now.ToLocalTime()} {pMessageType}: {pMsg} in {pFunction}");
                    retval++;
                    break;
                case EventLogEntryType.SuccessAudit:
                    if (Convert.ToInt16(ConfigurationManager.AppSettings["LogLevel"]) == 3)
                        w._logFile.WriteLine($"{DateTime.Now.ToLocalTime()} {pMessageType}: {pMsg} in {pFunction}");
                    break;
                case EventLogEntryType.FailureAudit:
                    if (Convert.ToInt16(ConfigurationManager.AppSettings["LogLevel"]) >= 1)
                        w._logFile.WriteLine($"{DateTime.Now.ToLocalTime()} {pMessageType}: {pMsg} in {pFunction}");
                    retval++;
                    break;
            }

            w._logFile.Flush();
            w._logFile.Close();
            return retval;
        }
    }
}

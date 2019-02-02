using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommonUtilsX64;

namespace CommonUtilsTests
{
    class Program
    {
        static void Main(string[] args)
        {

            // Test applicationLogs

            ApplicationLog.WriteEntry(Assembly.GetExecutingAssembly().GetName().Name);
            ApplicationLog.WriteEntry("Information message", EventLogEntryType.Information, MethodBase.GetCurrentMethod().Name, Assembly.GetExecutingAssembly().GetName().Name);
            ApplicationLog.WriteEntry("Warning message", EventLogEntryType.Warning, MethodBase.GetCurrentMethod().Name, Assembly.GetExecutingAssembly().GetName().Name);
            ApplicationLog.WriteEntry("Error message", EventLogEntryType.Error, MethodBase.GetCurrentMethod().Name, Assembly.GetExecutingAssembly().GetName().Name);



        }
    }
}

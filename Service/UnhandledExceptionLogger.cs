using System;

namespace UGC_API.Service
{    public class UnhandledExceptionLogger
    {
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            //Debug.WriteLine((e.ExceptionObject as Exception).Message);
            LoggingService.schreibeLogZeile((e.ExceptionObject as Exception).Message);
        }
    }
}

using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace UGC_API.Service
{
    public static class LoggingService
    {
        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 1000;
        private static string logfileDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @".\Logs\LogFiles\");

        public static void HeartbeatLog(object sender, ElapsedEventArgs e)
        {
            using (FileStream fs = new(logfileDir, FileMode.Append))
            {
                using (StreamWriter sw = new(fs))
                {
                    sw.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] - HB");
                }
            }
        }

        public async static void schreibeLogZeile(string content, string Zusatzinformationen = "")
        {
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    using (FileStream fs = new FileStream(logfileDir, FileMode.Append))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] " + content);

                            if (String.IsNullOrWhiteSpace(Zusatzinformationen) == false)
                            {
                                sw.WriteLine($"{Environment.NewLine}Zusatzinformationen:{Environment.NewLine}");
                                sw.WriteLine(Zusatzinformationen);
                                sw.Write(Environment.NewLine);
                            }
                        }
                    }
                    break;
                }
                catch (IOException e) when (i <= NumberOfRetries)
                {
                    Thread.Sleep(DelayOnRetry);
                }
            }
        }

        internal static void schreibeEDDNLog(string v)
        {
             var target = erstelleEDDNDatei();
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    using (FileStream fs = new FileStream(target, FileMode.Append))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(v);
                        }
                    }
                    break;
                }
                catch (IOException e) when (i <= NumberOfRetries)
                {
                    Thread.Sleep(DelayOnRetry);
                }
            }

        }

        public static void erstelleLogDatei()
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";

            Directory.CreateDirectory(logfileDir);

            StreamWriter sw = File.AppendText(logfileDir + fileName);
            sw.Close();

            logfileDir += fileName;
        }
        public static string erstelleEDDNDatei()
        {
            var dat = DateTime.Now.ToString("yyyy - MM - dd");
            string fileName = $"EDDN-{dat}.log";
            try
            {
                Directory.CreateDirectory(logfileDir);
            }
            catch ( IOException e )
            {

            }

            StreamWriter sw = File.AppendText(logfileDir + fileName);
            sw.Close();
            
            return (logfileDir + fileName);
        }
    }
}

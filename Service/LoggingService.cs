using System.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace UGC_API.Service
{
    public static class LoggingService
    {
        
        private static string logfileDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @".\Logs\LogFiles\");

        public static void HeartbeatLog(object sender, ElapsedEventArgs e)
        {
            using (FileStream fs = new(logfileDir, FileMode.Append))
            {
                using (StreamWriter sw = new(fs))
                {
                    sw.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] - HB");
                }
            }
        }

        public static void schreibeLogZeile(string content, string Zusatzinformationen = "")
        {
            using (FileStream fs = new FileStream(logfileDir, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] " + content);

                    if (String.IsNullOrWhiteSpace(Zusatzinformationen) == false)
                    {
                        sw.WriteLine($"{Environment.NewLine}Zusatzinformationen:{Environment.NewLine}");
                        sw.WriteLine(Zusatzinformationen);
                        sw.Write(Environment.NewLine);
                    }
                }
            }
        }

        public static void erstelleLogDatei()
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff") + ".log";

            Directory.CreateDirectory(logfileDir);

            StreamWriter sw = File.CreateText(logfileDir + fileName);
            sw.Close();

            logfileDir += fileName;
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace UGC_API.Config
{
    public static class Configs
    {
        public static bool Debug { get; set; }
        public static string[] Systems { get; set; }
        public static string[] Events { get; set; }
        public static int UpdateSystems { get; set; }
    }
    public static class DatabaseConfig
    {
        
        public static string Host = "";
        public static string User = "";
        public static string Password = "";
        public static string Port = "";
        public static string Database = "";
        public static void ReadDBConfig()
        {
            string cfgFile = @"..\..\DatabaseConfig.json";
            string path = "";
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile)))
            {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile);
                WriteDBConfig(path);
            }
            else
            {
                //($"Database Config not Found.\n New config file created at {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
                using (StreamWriter sw = File.AppendText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile)))
                {
                    sw.WriteLine("{");
                    sw.WriteLine("\"Host\":\"localhost\",");
                    sw.WriteLine("\"User\":\"root\",");
                    sw.WriteLine("\"Password\":\"password\",");
                    sw.WriteLine("\"Port\":\"3306\",");
                    sw.WriteLine("\"Database\":\"test\",");
                    sw.WriteLine("\"Debug\":\"true\"");
                    sw.WriteLine("}");
                }
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile);
                WriteDBConfig(path);
            }
        }
        public static void WriteDBConfig(string path)
        {
            var json = System.IO.File.ReadAllText(path);
            var cfg = JObject.Parse(json);
            Host = Convert.ToString(cfg["Host"]);
            User = Convert.ToString(cfg["User"]);
            Password = Convert.ToString(cfg["Password"]);
            Port = Convert.ToString(cfg["Port"]);
            Database = Convert.ToString(cfg["Database"]);
            Configs.Debug = Convert.ToBoolean(cfg["Debug"]);
        }
    }
}

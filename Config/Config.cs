using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace UGC_API.Config
{
    internal static class Configs
    {
        public static bool Debug { get; set; }
        public static string[] Systems { get; set; }
        public static string[] Events { get; set; }
        public static int UpdateSystems { get; set; }
    }
    internal static class DatabaseConfig
    {      
        public static string Host = "";
        public static string User = "";
        public static string Password = "";
        public static string Port = "";
        public static string Database = "";
        public static void ReadDBConfig()
        {
            string cfgFile = @".\Config.json";
            string path = "";
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile)))
            {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile);
            }
            else
            {
                using (StreamWriter sw = File.AppendText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile)))
                {
                    sw.WriteLine("{");
                    sw.WriteLine("\"Database\":[{");
                    sw.WriteLine("\"Host\":\"localhost\",");
                    sw.WriteLine("\"User\":\"root\",");
                    sw.WriteLine("\"Password\":\"password\",");
                    sw.WriteLine("\"Port\":\"3306\",");
                    sw.WriteLine("\"Database\":\"test\"");
                    sw.WriteLine("}],"); 
                    sw.WriteLine("\"Discord\":[{");
                    sw.WriteLine("\"discordBotToken\":\"fdgaasgfag.dsg\",");
                    sw.WriteLine("\"Guild\":0000000000000000,");
                    sw.WriteLine("\"discordBotInfoChannel\":0000000000000000,");
                    sw.WriteLine("\"discordBotLogChannel\":0000000000000000,");
                    sw.WriteLine("\"discordBotDevChannel\":0000000000000000,");
                    sw.WriteLine("\"prefix\":\"!\"");
                    sw.WriteLine("}],");
                    sw.WriteLine("\"Debug\":\"true\"");
                    sw.WriteLine("}");
                }
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile);
            }
            WriteDBConfig(path);
        }
        internal static void WriteDBConfig(string path)
        {
            var json = System.IO.File.ReadAllText(path);
            var cfg = JObject.Parse(json);
            Host = Convert.ToString(cfg["Database"][0]["Host"]);
            User = Convert.ToString(cfg["Database"][0]["User"]);
            Password = Convert.ToString(cfg["Database"][0]["Password"]);
            Port = Convert.ToString(cfg["Database"][0]["Port"]);
            Database = Convert.ToString(cfg["Database"][0]["Database"]);
            Configs.Debug = Convert.ToBoolean(cfg["Debug"]);
            DiscordBot.BotConfiguration.discordBotToken = Convert.ToString(cfg["Discord"][0]["discordBotToken"]);
            DiscordBot.BotConfiguration.Guild = Convert.ToUInt64(cfg["Discord"][0]["Guild"]);
            DiscordBot.BotConfiguration.discordBotInfoChannel = Convert.ToUInt64(cfg["Discord"][0]["discordBotInfoChannel"]);
            DiscordBot.BotConfiguration.discordBotLogChannel = Convert.ToUInt64(cfg["Discord"][0]["discordBotLogChannel"]);
            DiscordBot.BotConfiguration.discordBotDevChannel = Convert.ToUInt64(cfg["Discord"][0]["discordBotDevChannel"]);
            DiscordBot.BotConfiguration.prefix = Convert.ToString(cfg["Discord"][0]["prefix"]);

        }
    }
}

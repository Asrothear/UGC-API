using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UGC_API.DiscordBot

{
    public class BotConfiguration
    {
        public partial class BotConfigurationData
        {
            public string DatabaseHost { get; set; }
            public int DatabasePort { get; set; }
            public string DatabaseUser { get; set; }
            public string DatabasePass { get; set; }
            public string DatabaseDb { get; set; }
            public string discordBotToken { get; set; }
            public ulong discordBotChannel { get; set; }
        }

        public static string discordBotToken { get; set; }
        public static ulong Guild { get; set; }
        public static ulong discordBotInfoChannel { get; set; }
        public static ulong discordBotLogChannel { get; set; }
        public static ulong discordBotDevChannel { get; set; }
        public static string prefix { get; set; }
    }
}

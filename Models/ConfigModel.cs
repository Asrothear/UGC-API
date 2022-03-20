using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Models
{
    class ConfigModel
    {
        public bool Debug { get; set; } = true;
        public DBConfig DB { get; set; } = new();
        public BotConfig Bot { get; set; } = new();
        public class DBConfig
        {
            public string Host { get; set; } = "localhost";
            public string User { get; set; } = "root";
            public string Password { get; set; } = "mystringpassword";
            public string Port { get; set; } = "3306";
            public string Database { get; set; } = "test";
        }
        public class BotConfig
        {
            public string Prefix { get; set; } = "!";
            public string Token { get; set; } = "";
            public ulong Guild { get; set; } = 000000000000;
            public ulong InfoChannel { get; set; } = 000000000000;
            public ulong LogChannel { get; set; } = 000000000000;
            public ulong DevChannel { get; set; } = 000000000000;
            public ulong RulesChannel { get; set; } = 000000000000;
            public ulong WelcomeChannel { get; set; } = 000000000000;
            public List<PermsConfig> Perms { get; set; } = new();
        }
        public class PermsConfig
        {
            public ulong Id { get; set; } = 0;
            public string Name { get; set; } = "";
            public int Level { get; set; } = 0;
        }
    }
}

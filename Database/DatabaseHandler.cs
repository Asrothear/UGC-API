using UGC_API.Functions;
using UGC_API.Database_Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UGC_API.Config;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Timers;
using UGC_API.Handler.v1_0;
using UGC_API.Service;
using System.Text.Json;

namespace UGC_API.Database
{
    internal class DatabaseHandler
    {
        internal static DBContext db = new();
        internal static void LoadData()
        {
            try
            {
                db.Database.EnsureCreated();
                Config_F.Configs = new List<DB_Config>(db.DB_Config);
                var temp = JsonSerializer.Deserialize<List<string>>(Config_F.Configs[0].systems_s);
                temp.Sort();
                Configs.Systems = temp.ToArray();
                Configs.Events = JsonSerializer.Deserialize<List<string>>(Config_F.Configs[0].events_s).ToArray();
                Configs.UpdateSystems = Config_F.Configs[0].update_systems;
                Configs.Plugin = new List<DB_Plugin>(db.Plugin);
                User._Users = new(db.DB_Users);
                VerifyToken._Verify_Token = new(db.Verify_Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                LoggingService.schreibeLogZeile(e.ToString());
            }
        }
        internal static bool CheckUsername(string Username)
        {
            return true;
        }
    }
}

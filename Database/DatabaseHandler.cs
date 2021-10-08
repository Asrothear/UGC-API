using UGC_API.Model_Functions;
using UGC_API.Database_Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UGC_API.Config;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace UGC_API.Database
{
    class DatabaseHandler
    {
        internal static void LoadConfig()
        {
            try
            {
                using (var db = new DBContext())
                {
                    Config_F.Configs = new List<DB_Config>(db.DB_Config);
                    Configs.Systems = Config_F.Configs[0].systems.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",");
                    Configs.Events =Config_F.Configs[0].events.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",");
                    Configs.UpdateSystems = Config_F.Configs[0].update_systems;
                    Systems.LoadFromDB(db);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
            }
        }
        internal static bool CheckUsername(string Username)
        {
            return true;
        }
    }
}

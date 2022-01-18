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
                Configs.Systems = Config_F.Configs[0].systems.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",");
                Configs.Events =Config_F.Configs[0].events.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",");
                Configs.UpdateSystems = Config_F.Configs[0].update_systems;
                Systems.LoadFromDB();
                Carriers.LoadFromDB();
                Markets.LoadFromDB();
                User._Users = new(db.DB_Users);
                VerifyToken._Verify_Token = new(db.Verify_Token);
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
        public static void OnUpdateDataCacheTimer(object sender, ElapsedEventArgs e)
        {
            Systems.LoadFromDB();
            Carriers.LoadFromDB();
            CarrierHandler.LoadCarrier(true);
            SystemHandler.LoadSystems(true);
        }
    }
}

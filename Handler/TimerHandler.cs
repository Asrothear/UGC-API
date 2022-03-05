using System.Collections.Generic;
using System.Timers;
using UGC_API.Config;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;

namespace UGC_API.Handler
{
    public class TimerHandler
    {
        internal static void Start()
        {
            Timer UpdateDataCacheTimer = new();
            Timer UpdateTickTimer = new();
            UpdateDataCacheTimer.Elapsed += new(OnUpdateDataCacheTimer);
            UpdateTickTimer.Elapsed += new(OnUpdateTickTimer);
            UpdateDataCacheTimer.Interval += 5 * (60 * 1000);
            UpdateTickTimer.Interval += 30 * (60 * 1000);
            UpdateDataCacheTimer.Enabled = true;
            UpdateDataCacheTimer.Enabled = true;
        }
        public static void OnUpdateDataCacheTimer(object sender, ElapsedEventArgs e)
        {
            Config_F.Configs = new List<DB_Config>(DatabaseHandler.db.DB_Config);
            Configs.Systems = Config_F.Configs[0].systems.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",");
            Configs.Events = Config_F.Configs[0].events.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",");
            Configs.UpdateSystems = Config_F.Configs[0].update_systems;
            VerifyToken._Verify_Token = new(DatabaseHandler.db.Verify_Token);
            v1_0.SystemHandler.LoadSystems(true);
            v1_0.CarrierHandler.LoadCarrier(true);
            v1_0.MarketHandler.LoadMarket(true);
        }
        public static void OnUpdateTickTimer(object sender, ElapsedEventArgs e)
        {
            var Tick = new Tick();
            Tick.GetTick();
        }
    }
}

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using UGC_API.Config;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;
using UGC_API.Handler.v1_0;
using UGC_API.Models.v1_0;
using UGC_API.Service;
using UGC_API.EDDN;

namespace UGC_API.Handler
{
    public class TaskHandler
    {
        private static int xx = 0;
        internal static async void Start()
        {
            Debug.WriteLine($"Execution: {xx}");
            xx++;
            await Task.Run(() => { Localisation.LoadLocalisation(true); });
            await Task.Run(() => { SystemHandler.LoadSystems(true); });
            await Task.Run(() => { CarrierHandler.LoadCarrier(true); });
            await Task.Run(() => { MarketHandler.LoadMarket(true); });
            await Task.Run(() => { ShedulerHandler.StateListUpdate(); LoggingService.schreibeLogZeile($"StateListUpdate geladen."); });
            await Task.Run(() => { ServiceHandler.LoadService(true); });
            await Task.Run(() => { MissionHandler.LoadMissions(true); });
            TimerHandler.Start();
            EDDNListener.listener();
        }
    }
    public class TimerHandler
    {
        internal static void Start()
        {
            Timer UpdateDataCacheTimer = new();
            Timer UpdateTickTimer = new();
            UpdateDataCacheTimer.Elapsed += new(OnUpdateDataCacheTimer);
            UpdateTickTimer.Elapsed += new(OnUpdateTickTimer);
            UpdateDataCacheTimer.Interval += 15 * (60 * 1000);
            UpdateTickTimer.Interval += 5 * (60 * 1000);
            UpdateDataCacheTimer.Enabled = true;
            UpdateDataCacheTimer.Enabled = true;
        }
        public static void OnUpdateDataCacheTimer(object sender = null, ElapsedEventArgs e = null)
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
    public class ShedulerHandler
    {

        private static bool updating = false;
        internal static void StateListUpdate()
        {
            if (updating) return;
            updating = true;
            SystemHandler.LoadSystems();
            List<string> Systems = new();
            var time = GetTime.DateNow();
            var tick = Tick.DateTimeTick.AddHours(3);
            if(time.Day != tick.Day)
            {
                Systems = new();
                Systems.Add("Alles Aktuell!");
                StateHandler.Systems_out = Systems;
                updating = false;
                return;
            }
            foreach (var CSystem in Configs.Systems)
            {
                //Hole alle Einträge aus dem aktuellen Monat
                SystemModel HSystem = SystemHandler._Systeme.Find(s => (s.Timestamp.Day == time.Day && s.Timestamp.Month == time.Month && s.last_update.Year == time.Year && s.System_Name == CSystem));
                //Filer Systeme
                if (HSystem == null)
                {
                    if (time.Day == tick.Day && time > tick)
                    {
                        Systems.Add(CSystem);
                    }
                    else
                    {
                        Systems.Add($"~{CSystem}~");
                    }
                }
                else if (time > tick && HSystem.last_update < tick)
                {
                    Systems.Add(CSystem);
                }
            }
            if (Systems.Count == 0)
            {
                Systems = new();
                Systems.Add("Alles Aktuell!");
            }
            StateHandler.Systems_out = Systems;
            updating = false;
        }
    }
}

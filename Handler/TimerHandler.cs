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
using System.Text.Json;

namespace UGC_API.Handler
{
    public class TaskHandler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static int xx = 0;
        internal static void Start()
        {
            logger.Info($"TaskHandler.Start Execution: {xx++}");
            Task.Run(() => { logger.Info($"Localisations werden geladen..."); Localisation.LoadLocalisation(true); });
            Task.Run(() => { logger.Info($"SystemHandler wird geladen..."); SystemHandler.LoadSystems(true); });
            Task.Run(() => { logger.Info($"CarrierHandler wird geladen..."); CarrierHandler.LoadCarrier(true); });
            Task.Run(() => { logger.Info($"MarketHandler wird geladen..."); MarketHandler.LoadMarket(true); });
            Task.Run(() => { logger.Info($"ServiceHandler wird geladen..."); ServiceHandler.LoadService(true); });
            Task.Run(() => { logger.Info($"MissionHandler wird geladen..."); MissionHandler.LoadMissions(true); });
            BGSOrderAPI.GetCurrentList();
        }
    }
    public class TimerHandler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        internal static void Start()
        {
            logger.Info($"TimerHandler geladen.");
            Timer UpdateDataCacheTimer = new();
            Timer UpdateTickTimer = new();
            Timer UpdateStateListTimer = new();
            UpdateDataCacheTimer.Elapsed += new(OnUpdateDataCacheTimer);
            UpdateTickTimer.Elapsed += new(OnUpdateTickTimer);
            UpdateStateListTimer.Elapsed += new(OnUpdateStateListTimer);
            UpdateDataCacheTimer.Interval += 15 * (60 * 1000);
            UpdateTickTimer.Interval += 10 * (60 * 1000);
            UpdateStateListTimer.Interval += 5 * (60 * 1000);
            UpdateDataCacheTimer.Enabled = true;
            UpdateTickTimer.Enabled = true;
            UpdateStateListTimer.Enabled = true;
        }
        public static void OnUpdateDataCacheTimer(object sender = null, ElapsedEventArgs e = null)
        {
            using (DBContext db = new())
            {
                Config_F.Configs = new List<DB_Config>(db.DB_Config);
                var temp = JsonSerializer.Deserialize<List<string>>(Config_F.Configs[0].systems_s);
                temp.Sort();
                Configs.Systems = temp.ToArray();
                Configs.Events = JsonSerializer.Deserialize<List<string>>(Config_F.Configs[0].events_s).ToArray();
                Configs.UpdateSystems = Config_F.Configs[0].update_systems;
                Configs.Plugin = new List<DB_Plugin>(db.Plugin);
                //User._Users = new(db.DB_Users);
                VerifyToken._Verify_Token = new(db.Verify_Token);
            }
        }
        public static void OnUpdateTickTimer(object sender, ElapsedEventArgs e)
        {
            var Tick = new Tick();
            Tick.GetTick();
        }
        public static void OnUpdateStateListTimer(object sender, ElapsedEventArgs e)
        {
            ShedulerHandler.StateListUpdate();
        }

    }
    public class ShedulerHandler
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static bool updating = false;
        internal static void StateListUpdate()
        {
            //LoggingService.schreibeLogZeile($"StateListUpdate - {updating}");
            if (updating) return;
            logger.Info($"StateListUpdate");
            updating = true;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            SystemHandler.LoadSystems();
            List<string> Systems = new();
            var time = GetTime.DateNow();
            var tick = Tick.DateTimeTick.AddHours(3);
            /*if(time.Day != tick.Day)
            {
                Systems = new();
                Systems.Add("Alles Aktuell!");
                StateHandler.Systems_out = Systems;
                updating = false;
                //LoggingService.schreibeLogZeile($"StateListUpdate ({StateHandler.Systems_out.Count}) Tick was yesterday Execution Time: {watch.ElapsedMilliseconds} ms");
                return;
            }*/
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
                else if (time.Day == tick.Day && time > tick && HSystem.last_update < tick)
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
            watch.Stop();
            logger.Info($"StateListUpdate ({StateHandler.Systems_out.Count}) Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}

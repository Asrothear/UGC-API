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
        private static int xx = 0;
        internal static async void Start()
        {
            LoggingService.schreibeLogZeile($"TaskHandler.Start Execution: {xx}");
            xx++;
            await Task.Run(() => { Localisation.LoadLocalisation(true); });
            await Task.Run(() => { SystemHandler.LoadSystems(true); });
            await Task.Run(() => { CarrierHandler.LoadCarrier(true); });
            await Task.Run(() => { MarketHandler.LoadMarket(true); });
            await Task.Run(() => { ShedulerHandler.StateListUpdate(); LoggingService.schreibeLogZeile($"StateListUpdate geladen."); });
            await Task.Run(() => { ServiceHandler.LoadService(true); });
            await Task.Run(() => { MissionHandler.LoadMissions(true); });
            TimerHandler.Start();
            //EDDNListener.listener();
        }
    }
    public class TimerHandler
    {
        internal static void Start()
        {
            LoggingService.schreibeLogZeile($"TimerHandler geladen.");
            Timer UpdateDataCacheTimer = new();
            Timer UpdateTickTimer = new();
            UpdateDataCacheTimer.Elapsed += new(OnUpdateDataCacheTimer);
            UpdateTickTimer.Elapsed += new(OnUpdateTickTimer);
            UpdateDataCacheTimer.Interval += 15 * (60 * 1000);
            UpdateTickTimer.Interval += 5 * (60 * 1000);
            UpdateDataCacheTimer.Enabled = true;
            UpdateTickTimer.Enabled = true;
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
            ShedulerHandler.StateListUpdate();
        }

    }
    public class ShedulerHandler
    {

        private static bool updating = false;
        internal static void StateListUpdate()
        {
            LoggingService.schreibeLogZeile($"StateListUpdate - {updating}");
            if (updating) return;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
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
                LoggingService.schreibeLogZeile($"StateListUpdate ({StateHandler.Systems_out.Count}) Tick was yesterday Execution Time: {watch.ElapsedMilliseconds} ms");
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
            watch.Stop();
            LoggingService.schreibeLogZeile($"StateListUpdate ({StateHandler.Systems_out.Count}) Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}

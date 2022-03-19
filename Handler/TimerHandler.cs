﻿using System.Collections.Generic;
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
            await Task.Run(() => { ShedulerHandler.StateListUpdate(); });
            TimerHandler.Start();
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
    public class ShedulerHandler
    {

        private static bool updating = false;
        internal static void StateListUpdate()
        {
            if (updating) return;
            updating = true;
            SystemHandler.LoadSystems();
            List<string> Systems = new();
            foreach (var CSystem in Configs.Systems)
            {
                //Hole alle Einträge aus dem aktuellen Monat
                SystemModel HSystem = SystemHandler._Systeme.FirstOrDefault(s => (s.last_update.Day == GetTime.DateNow().Day && s.last_update.Month == GetTime.DateNow().Month && s.last_update.Year == GetTime.DateNow().Year && s.System_Name == CSystem));
                //Filer Systeme
                if (HSystem == null)
                {
                    Systems.Add($"~{CSystem}~");
                }
                else if (HSystem.last_update < Tick.DateTimeTick)
                {
                    Systems.Add(CSystem);
                }
                if (Systems.Count == 0)
                {
                    Systems = new();
                    Systems.Add("Alles Aktuell!");
                }
                StateHandler.Systems_out = Systems;
            }
            updating = false;
        }
    }
}
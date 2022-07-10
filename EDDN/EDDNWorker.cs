using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UGC_API.Service;
using UGC_API.EDDN.Model;
using UGC_API.Config;
using UGC_API.Handler.v1_0;
using UGC_API.Functions;
using UGC_API.Models.v1_0;
using UGC_API.Database;
using UGC_API.Handler;

namespace UGC_API.EDDN
{
    internal class EDDNWorker
    {
        ulong lastsystem = 0;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        internal void WorkerThread(JObject resObjJson)
        {
            var InternalData = resObjJson["message"]?.Value<JObject>() ?? null;
            if (InternalData == null) return;
            SystemHandler.LoadSystems();
            if (InternalData.ContainsKey("event"))
            {
                // Has Envent, try to Parse Data
                switch (InternalData["event"].ToString())
                {
                    case "FSDJump":
                        var JumpData = System.Text.Json.JsonSerializer.Deserialize<EDDN_FSDJumpModel>(InternalData.ToString());
                        Task.Run(() => {
                            Systems.UpdateSystemData(JumpData);
                        });
                        bool ugs = false;
                        if (JumpData.Factions != null && Configs.Systems.Contains<string>(JumpData.StarSystem))
                        {
                            Task.Run(() => {
                                ShedulerHandler.StateListUpdate();
                            });
                            if (lastsystem != JumpData.SystemAddress)
                            {
                                ugs = true;
                                lastsystem = JumpData.SystemAddress;
                                JumpHandler(JumpData);
                                lastsystem = 0;
                            }
                        }
                        //LoggingService.schreibeEDDNLog($"EDDNWorker {JumpData.StarSystem} UGC:{ugs}");
                        break;
                }
            }
            else if (InternalData.ContainsKey("commodities"))
            {
                // Jobject has possible MarketData, try to Parse Data
                if (!MarketHandler.loaded) return;
                try {
                    var MarketData = System.Text.Json.JsonSerializer.Deserialize<EDDN_MarketModel>(resObjJson["message"].ToString());
                    EDDN_MarketHandler(MarketData);
                }catch(Exception ex) { };
                
            }
        }
        private void EDDN_MarketHandler(EDDN_MarketModel MarketData)
        {
            var create = false;
            var DBMarket = Markets._Markets.FirstOrDefault(c => c.MarketID == MarketData.marketId);
            if (DBMarket == null)
            {
                if (MarketHandler.LastMarketID == MarketData.marketId) return;
                DBMarket = new();
                create = true;
            }
            MarketHandler.LastMarketID = MarketData.marketId;
            DBMarket.MarketID = MarketData.marketId;
            DBMarket.StarSystem = MarketData.systemName;
            DBMarket.StationName = MarketData.stationName;
            DBMarket.Items = System.Text.Json.JsonSerializer.Serialize(MarketData.commodities);
            DBMarket.Last_Update = DateTime.Now;
            using (DBContext db = new())
            {
                if (create)
                {
                    DBMarket.StationType = "Unknown (EDDN)";
                    Markets._Markets.Add(DBMarket);
                    db.Market.Add(DBMarket);
                }
                else
                {
                    db.Market.Update(DBMarket);
                }
                db.SaveChanges();
                db.Dispose();
            }

        }
        private void JumpHandler(EDDN_FSDJumpModel JumpData)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            string[] t_arry = JumpData.timestamp.ToString("d").Split('.');
            int year = Convert.ToInt32(t_arry[2]) + 1286;
            var time = DateTime.Parse($"{year}-{t_arry[1]}-{t_arry[0]}");
            var API_System = SystemHandler._Systeme.FirstOrDefault(u => u.System_Name == JumpData.StarSystem && u.Timestamp == time);
            if (API_System == null) { CreateSystemDayEntry(JumpData, time); return; };
            var DB_System = Systems._Systeme.FirstOrDefault(db => db.System_Name == JumpData.StarSystem && db.Timestamp == time);
            if (DB_System == null) { return; };
            API_System.id = DB_System.id;
            API_System.last_update = DateTime.Now;
            API_System.User_ID = 0;
            API_System.System_ID = JumpData.SystemAddress;
            API_System.System_Name = JumpData.StarSystem;
            API_System.Factions = System.Text.Json.JsonSerializer.Deserialize<List<SystemModel.FactionsL>>(System.Text.Json.JsonSerializer.Serialize(JumpData.Factions));
            watch.Stop();
            logger.Info($"EDDNWorker-JumpHandler {JumpData.StarSystem} Execution Time: {watch.ElapsedMilliseconds} ms");
            UpdateSystem(API_System);
        }

        internal static void CreateSystemDayEntry(EDDN_FSDJumpModel JumpData, DateTime time)
        {
            SystemModel newSystemEntry = new SystemModel
            {
                Timestamp = time,
                last_update = DateTime.Now,
                System_ID = JumpData.SystemAddress,
                System_Name = JumpData.StarSystem
            };
            foreach (var factions in JumpData.Factions)
            {
                var faction = new SystemModel.FactionsL
                {
                    Name = factions.Name,
                    FactionState = factions.FactionState,
                    Government = factions.Government,
                    Influence = factions.Influence,
                    Allegiance = factions.Allegiance,
                    Happiness = factions.Happiness
                };
                if (factions.ActiveStates != null)
                {
                    foreach (var states in factions.ActiveStates)
                    {
                        var state = new SystemModel.FactionsL.ActiveStatesL
                        {
                            State = states.State
                        };
                        faction.ActiveStates.Add(state);
                    }
                }
                newSystemEntry.Factions.Add(faction);
            }
            SystemHandler._Systeme.Add(newSystemEntry);
            UpdateSystem(newSystemEntry, true);
            return;
        }
        public static void UpdateSystem(Models.v1_0.SystemModel SystemEntry, bool create = false)
        {
            var UpdateEntry = Systems._Systeme.FirstOrDefault(sy => sy.Timestamp == SystemEntry.Timestamp && sy.System_Name == SystemEntry.System_Name);
            if (UpdateEntry == null)
            {
                UpdateEntry = new();
                create = true;
            }

            UpdateEntry.Timestamp = SystemEntry.Timestamp;
            UpdateEntry.last_update = SystemEntry.last_update;
            UpdateEntry.User_ID = 0;
            UpdateEntry.System_ID = SystemEntry.System_ID;
            UpdateEntry.System_Name = SystemEntry.System_Name;
            UpdateEntry.Factions = System.Text.Json.JsonSerializer.Serialize(SystemEntry.Factions);


            if (SystemEntry == null)
            {
                return;
            }
            using (DBContext db = new())
            {
                if (create)
                {
                    Systems._Systeme.Add(UpdateEntry);
                    db.DB_Systemes.Add(UpdateEntry);
                }
                else
                {
                    db.DB_Systemes.Update(UpdateEntry);
                }
                try
                {
                    db.SaveChanges();
                }catch(Exception ex)
                {

                }
                db.Dispose();
            }
        }
    }
}
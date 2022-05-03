using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;
using UGC_API.Models.v1_0;
using UGC_API.Service;

namespace UGC_API.Handler.v1_0
{
    public class MarketHandler
    {
        public static List<Models.v1_0.Events.Market> _Markets = new();
        internal static ulong LastMarketID = 0;
        internal static void MarketEvent(string json, string @event, DB_User user)
        {
            LoadMarket();
            switch (@event)
            {
                case "Market":
                    Market(JsonSerializer.Deserialize<Models.v1_0.Events.Market>(json), user);
                    break;
                case "MarketBuy":
                    MarketBuy(JsonSerializer.Deserialize<Models.v1_0.Events.CarrierJump>(json));
                    break;
                case "MarketSell":
                    MarketSell(JsonSerializer.Deserialize<Models.v1_0.Events.CarrierJumpRequest>(json));
                    break;
                default:
                    break;
            }
        }

        internal static List<Models.v1_0.Events.Market> GetMarket(string name, ulong Id)
        {
            LoadMarket();
            var outs = _Markets.Where(m => m.MarketID == Id).ToList();
            if(outs.Count == 0)
            {
                outs = _Markets.Where(m => m.StationName.ToLower() == name.ToLower()).ToList();
                if (outs == null)
                {
                    return new List<Models.v1_0.Events.Market>();
                }
            }
            return outs;
        }
        internal static List<MarketSearchModel> FindWare(string Ware)
        {
            LoadMarket();
            List<MarketSearchModel> OBJ = new();
            List<Models.v1_0.Events.Market> CM = new();
            var find = Localisation._Localisations.FirstOrDefault(l => l.Name.ToLower() == Ware.ToLower() || l.de.ToLower() == Ware.ToLower() || l.en.ToLower() == Ware.ToLower());
            if (find == null) return OBJ;
            try
            {
                CM = _Markets.Where(u => u.Items.SingleOrDefault(m => m.Name == find.Name && m.BuyPrice > 0 && m.Stock > 0) != null).ToList();
            }
            catch (Exception e)
            {
                return OBJ;
            }
            foreach (var Data in CM)
            {
                var NewData = new MarketSearchModel
                {
                    MarketID = Data.MarketID,
                    Name = Data.StationName,
                    System = Data.StarSystem,
                    StationType = Data.StationType,
                    //market = Data.market.Where(i => i.Name == Ware).ToList()
                };
                Data.Items = Data.Items.Where(m => m.Name == find.Name && m.BuyPrice > 0 && m.Stock > 0).ToList();
                foreach(var Item in Data.Items)
                {
                    var newItem = new MarketModel
                    {
                        Name = Item.Name,
                        Name_de = Localisation.GetLocalisationString(Item.Name),
                        Name_en = Localisation.GetLocalisationString(Item.Name, true),
                        Category = Item.Category,
                        Category_de = Localisation.GetLocalisationString(Item.Category_Localised),
                        Category_en = Localisation.GetLocalisationString(Item.Category_Localised, true),
                        BuyPrice = Item.BuyPrice,
                        SellPrice = Item.SellPrice,
                        MeanPrice = Item.MeanPrice,
                        StockBracket = Item.StockBracket,
                        DemandBracket = Item.DemandBracket,
                        Stock = Item.Stock,
                        Demand = Item.Demand,
                        Consumer = Item.Consumer,
                        Producer = Item.Producer,
                        Rare = Item.Rare,

                    };
                    NewData.market.Add(newItem);
                }
                if(NewData.StationType != "FleetCarrier")
                {
                    NewData.DockingAccess = "Public";
                }
                else
                {
                    var carr = CarrierHandler._Carriers.FirstOrDefault(c => c.Callsign == NewData.Name);
                    NewData.DockingAccess = carr.DockingAccess;
                }
                OBJ.Add(NewData);
            }
            return OBJ;
        }
        private static void MarketSell(Models.v1_0.Events.CarrierJumpRequest carrierJumpRequest)
        {
            
        }

        private static void MarketBuy(Models.v1_0.Events.CarrierJump carrierJump)
        {
            
        }

        private static void Market(Models.v1_0.Events.Market market, DB_User user)
        {
            var ve = JsonSerializer.Serialize(market.Items);
            Localisation.Fetch(ve, user);
            UpdateMarket(market);
        }

        internal static void LoadMarket(bool force = false)
        {
            try
            {
                if (_Markets.Count != 0 && !force) return;
                _Markets = new();
                if (force || _Markets.Count == 0) Markets.LoadFromDB();
                LoggingService.schreibeLogZeile($"Pasring {Markets._Markets.Count} Markets");
                _Markets = ParseMarket(Markets._Markets);
                Service.LoggingService.schreibeLogZeile($"{_Markets.Count} Market´s geladen.");
            }catch(Exception ex) { Service.LoggingService.schreibeLogZeile(ex.Message); }
        }

        private static List<Models.v1_0.Events.Market> ParseMarket(List<DB_Market> db_markets)
        {
            List<Models.v1_0.Events.Market> API_Market = new();
            foreach(var market in db_markets)
            {
                var MarketData = new Models.v1_0.Events.Market
                {
                    MarketID = market.MarketID,
                    StarSystem = market.StarSystem,
                    StationName = market.StationName,
                    StationType = market.StationType,
                    Items = JsonSerializer.Deserialize<List<Models.v1_0.Events.Market.MarketItems>>(market.Items)
                };
                API_Market.Add(MarketData);
            }
            return API_Market;
        }
        internal static async void UpdateMarket(Models.v1_0.Events.Market MarketEntry)
        {
            var create = false;
            var DBMarket = Markets._Markets.FirstOrDefault(c => c.MarketID == MarketEntry.MarketID);
            if (DBMarket == null)
            {
                if (LastMarketID == MarketEntry.MarketID) return;
                DBMarket = new();
                create = true;
                return;
            }
            DBMarket.MarketID = MarketEntry.MarketID;
            LastMarketID = MarketEntry.MarketID;
            DBMarket.StarSystem = MarketEntry.StarSystem;
            DBMarket.StationName = MarketEntry.StationName;
            DBMarket.StationType = MarketEntry.StationType;
            //DBMarket.Items = JsonSerializer.Serialize(MarketEntry.Items);
            //DBMarket.Last_Update = DateTime.Now;
            using (DBContext db = new())
            {
                if (create)
                {
                    db.Market.Add(DBMarket);
                }
                else
                {
                    db.Market.Update(DBMarket);
                }
                db.SaveChanges();
                db.Dispose();
            }
            LoadMarket(true);
            LastMarketID = 0;
        }

        internal async static void UpdateAllMarket()
        {
            using (DBContext db = new())
            {
                foreach (var MAR in _Markets)
                {
                    var DBMarket = DatabaseHandler.db.Market.FirstOrDefault(c => c.MarketID == MAR.MarketID);
                    DBMarket.MarketID = MAR.MarketID;
                    DBMarket.StarSystem = MAR.StarSystem;
                    DBMarket.StationName = MAR.StationName;
                    DBMarket.StationType = MAR.StationType;
                    DBMarket.Items = JsonSerializer.Serialize(MAR.Items);
                    db.Market.Update(DBMarket);
                }
                db.SaveChanges();
                db.Dispose();
            }
            LoadMarket(true);
        }
    }
}

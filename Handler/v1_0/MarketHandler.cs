using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;

namespace UGC_API.Handler.v1_0
{
    public class MarketHandler
    {
        public static List<Models.v1_0.Events.Market> _Markets = new();
        internal static void MarketEvent(string json, string @event)
        {
            LoadMarket();
            switch (@event)
            {
                case "Market":
                    Market(JsonSerializer.Deserialize<Models.v1_0.Events.Market>(json));
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

        private static void MarketSell(Models.v1_0.Events.CarrierJumpRequest carrierJumpRequest)
        {
            throw new NotImplementedException();
        }

        private static void MarketBuy(Models.v1_0.Events.CarrierJump carrierJump)
        {
            throw new NotImplementedException();
        }

        private static void Market(Models.v1_0.Events.Market market)
        {
            UpdateMarket(market);
        }

        internal static void LoadMarket(bool force = false)
        {
            if (_Markets.Count != 0 && !force) return;
            _Markets = new();
            if (force) Markets.LoadFromDB();
            _Markets = ParseMarket(Markets._Markets);
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
        internal static void UpdateMarket(Models.v1_0.Events.Market MarketEntry)
        {
            var DBMarket = Markets._Markets.FirstOrDefault(c => c.MarketID == MarketEntry.MarketID);
            if (DBMarket == null) DBMarket = new();
            DBMarket.MarketID = MarketEntry.MarketID;
            DBMarket.StarSystem = MarketEntry.StarSystem;
            DBMarket.StationName = MarketEntry.StationName;
            DBMarket.StationType = MarketEntry.StationType;
            DBMarket.Items = JsonSerializer.Serialize(MarketEntry.Items);
            //DBMarket.Last_Update = DateTime.Now.ToString();
            DatabaseHandler.db.Market.Update(DBMarket);
            DatabaseHandler.db.SaveChanges();
        }

        internal static void UpdateAllMarket()
        {
            foreach (var MAR in _Markets)
            {
                var DBMarket = DatabaseHandler.db.Market.FirstOrDefault(c => c.MarketID == MAR.MarketID);
                DBMarket.MarketID = MAR.MarketID;
                DBMarket.StarSystem = MAR.StarSystem;
                DBMarket.StationName = MAR.StationName;
                DBMarket.StationType = MAR.StationType;
                DBMarket.Items = JsonSerializer.Serialize(MAR.Items);
                DatabaseHandler.db.Market.Update(DBMarket);
                DatabaseHandler.db.SaveChanges();
            }
            LoadMarket(true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Models.v1_0.Events
{
    public class Market
    {
        public ulong MarketID { get; set; }
        public string StarSystem { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public List<MarketItems> Items { get; set; }
        public class MarketItems
        {
            public int Id { get; set; }
            public ulong edId { get; set; }
            public string Name { get; set; }
            public bool Rare { get; set; }
            public ulong Stock { get; set; }
            public ulong Demand { get; set; }
            public ulong BuyPrice { get; set; }
            public bool Consumer { get; set; }
            public bool Producer { get; set; }
            public ulong MeanPrice { get; set; }
            public ulong SellPrice { get; set; }
            public ulong StockBracket { get; set; }
            public ulong DemandBracket { get; set; }
            public string Name_Localised { get; set; }
            public string Category_Localised { get; set; }
        }
    }
}

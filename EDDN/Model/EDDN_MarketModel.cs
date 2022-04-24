using System;
using System.Collections.Generic;

namespace UGC_API.EDDN.Model
{

    public class EDDN_MarketModel
    {
        public ulong marketId { get; set; }
        public string stationName { get; set; }
        public string systemName { get; set; }
        public DateTime timestamp { get; set; }
        public List<EDDN_CommoditiesModel> commodities { get; set; }

        public class EDDN_CommoditiesModel
        {
            public int buyPrice { get; set; }
            public int demand { get; set; }
            public int demandBracket { get; set; }
            public int meanPrice { get; set; }
            public string name { get; set; }
            public int sellPrice { get; set; }
            public int stock { get; set; }
            public int stockBracket { get; set; }
        }
    }
}

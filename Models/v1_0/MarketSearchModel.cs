using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
namespace UGC_API.Models.v1_0
{
    public class MarketModel
    {
        public string Name { get; set; }
        public string Name_de { get; set; }
        public string Name_en { get; set; }
        public string Category { get; set; }
        public string Category_de { get; set; }
        public string Category_en { get; set; }
        public ulong? BuyPrice { get; set; }
        public ulong? SellPrice { get; set; }
        public ulong? MeanPrice { get; set; }
        public ulong? StockBracket { get; set; }
        public ulong? DemandBracket { get; set; }
        public ulong? Stock { get; set; }
        public ulong? Demand { get; set; }
        public bool? Consumer { get; set; }
        public bool? Producer { get; set; }
        public bool? Rare { get; set; }
    }
    public class MarketSearchModel
    {
        public ulong MarketID { get; set; }
        public string Name { get; set; }
        public string System { get; set; }
        public string StationType { get; set; }
        public string DockingAccess { get; set; }
        public List<MarketModel> market { get; set; } = new();
    }
}

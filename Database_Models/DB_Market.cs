using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_Market
    {
        public int Id { get; set; }
        public ulong MarketID { get; set; }
        public string StarSystem { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public string Items { get; set; }        
    }
}

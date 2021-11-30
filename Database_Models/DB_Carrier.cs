using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_Carrier
    {
        public int id { get; set; }
        public long CarrierID { get; set; }
        public string Name { get; set; } = "";
        public string Callsign { get; set; } = "";
        public string System { get; set; }
        public string prev_System { get; set; }
        public string DockingAccess { get; set; }
        public string AllowNotorious { get; set; }
        public string FuelLevel { get; set; }
        public string JumpRangeCurr { get; set; }
        public string JumpRangeMax { get; set; }
        public string PendingDecommission { get; set; }
        public string SpaceUsage { get; set; }
        public string Finance { get; set; }
        public string Crew { get; set; }
        public string ShipPacks { get; set; }
        public string ModulePacks { get; set; }
        public string market { get; set; }
        public string Last_Update { get; set; }
    }
}

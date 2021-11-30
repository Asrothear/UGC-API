using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Models.v1_0.Events
{
    public class CarrierJump
    {
        public DateTime timestamp { get; set; }
        public string Event { get; set; }
        public bool Docked { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public long MarketID { get; set; }
        public string StationFaction { get; set; }
        public string StationGovernment { get; set; }
        public string StationGovernment_Localised { get; set; }
        public string[] StationServices { get; set; }
        public string StationEconomy { get; set; }
        public string StationEconomy_Localised { get; set; }
        public string StationEconomies { get; set; }
        public string StarSystem { get; set; }
        public ulong SystemAddress { get; set; }
        public double[] StarPos { get; set; }
        public string SystemAllegiance { get; set; }
        public string SystemEconomy { get; set; }
        public string SystemEconomy_Localised { get; set; }
        public string SystemSecondEconomy { get; set; }
        public string SystemSecondEconomy_Localised { get; set; }
        public string SystemGovernment { get; set; }
        public string SystemGovernment_Localised { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemSecurity_Localised { get; set; }
        public string Population { get; set; }
        public string Body { get; set; }
        public string BodyID { get; set; }
        public string BodyType { get; set; }
        public string SystemFaction { get; set; }
        public class StationFactionL
        {
            public string Name { get; set; }
        }
        public class StationEconomiesL
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public int Proportion { get; set; }
        }
        public class SystemFactionL
        {
            public string Name { get; set; }
        }

    }
}

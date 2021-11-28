using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Models.v1_0.Events
{
    public class Location
    {
        public DateTime Timestamp { get; set; }
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public double[] StarPos { get; set; }
        public string Body { get; set; }
        public int BodyID { get; set; }
        public string BodyType { get; set; }
        public double DistFromStarLS { get; set; }
        public bool Docked { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public long MarketID { get; set; }
        public FSDJump.SystemFactionL SystemFaction { get; set; }
        public string SystemAllegiance { get; set; }
        public string SystemEconomy { get; set; }
        public string SystemSecondEconomy { get; set; }
        public string SystemGovernment { get; set; }
        public string SystemSecurity { get; set; }
        public long Population { get; set; }
        public string Wanted { get; set; }
        public List<FSDJump.FactionsL> Factions { get; set; }
        public List<FSDJump.ConflictsL> Conflicts { get; set; }
        public string Powers { get; set; }
        public string PowerplayState { get; set; }
        public string StationFaction { get; set; }
        public string StationGovernment { get; set; }
        public string StationAllegiance { get; set; }
        public string StationServices { get; set; }
        public string StationEconomies { get; set; }
        public bool Taxi { get; set; }
        public bool Multicrew { get; set; }
        public bool InSRV { get; set; }
        public bool OnFoot { get; set; }
    }
}

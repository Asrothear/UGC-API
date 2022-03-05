using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Models.v1_0.Events
{
    public class FSDJump
    {
        public DateTime Timestamp { get; set; }
        public string Event { get; set; }
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
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
        public long Population { get; set; }
        public double JumpDist { get; set; }
        public double FuelUsed { get; set; }
        public double FuelLevel { get; set; }
        public List<FactionsL> Factions { get; set; }
        public SystemFactionL SystemFaction { get; set; }
        public List<ConflictsL> Conflicts { get; set; }

        public class FactionsL
        {
            public string Name { get; set; }
            public string FactionState { get; set; }
            public string Government { get; set; }
            public double Influence { get; set; }
            public string Allegiance { get; set; }
            public string Happiness { get; set; }
            public string Happiness_Localised { get; set; }
            public double MyReputation { get; set; }
            public List<ActiveStatesL> ActiveStates { get; set; }
        }
        public class SystemFactionL
        {
            public string Name { get; set; }
            public string FactionState { get; set; }
        }
        public class ConflictsL
        {
            public string WarType { get; set; }
            public CFaction Faction1 { get; set; }
            public CFaction Faction2 { get; set; }
        }
        public class CFaction
        {
            public string Name { get; set; }
            public string Stake { get; set; }
            public int WonDays { get; set; }
        }
        public class ActiveStatesL
        {
            public string State { get; set; }
        }
    }
}

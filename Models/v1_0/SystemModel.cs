using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Models.v1_0
{
    public class SystemModel
    {
        public int id { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime last_update { get; set; }
        public int User_ID { get; set; }
        public long System_ID { get; set; }
        public string System_Name { get; set; }
        public List<FactionsL> Factions { get; set; } = new();
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
            public List<ActiveStatesL> ActiveStates { get; set; } = new();
            public int BGSPoints { get; set; }
            public int Missions { get; set; }
            public int Explorer { get; set; }
            public int Voucheer { get; set; }
            public int Trade { get; set; }
            public class ActiveStatesL
            {
                public string State { get; set; }
            }
        }
    }
}
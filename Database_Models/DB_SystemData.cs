using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGC_API.Database_Models
{
    public class DB_SystemData
    {
        public int Id { get; set; }
        public string StarSystem { get; set; }
        public ulong SystemAddress { get; set; }
        public string StarPos { get; set; }
        public long Population { get; set; }
        public string SystemAllegiance { get; set; }
        public int? FactionsCount { get; set; } = 0;
        public string Faction_String { get; set; }
        [NotMapped]
        public List<FactionsModel> Factions { get; set; }
        public class FactionsModel
        {
            public string Name { get; set; }
            public string FactionState { get; set; }
            public string Government { get; set; }
            public double Influence { get; set; }
            public string Allegiance { get; set; }
            public string Happiness { get; set; }
            public string Happiness_Localised { get; set; }
            public double MyReputation { get; set; }
            public List<ActiveStatesModel> ActiveStates { get; set; }
            public class ActiveStatesModel
            {
                public string State { get; set; }
            }
        }
    }
}

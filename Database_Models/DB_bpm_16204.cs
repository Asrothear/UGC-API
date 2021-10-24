using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_bpm_16204
    {
        public int id { get; set; }
        public string Timestamp { get; set; }
        public string last_update { get; set; }
        public int User_ID { get; set; }
        public int System_ID { get; set; }
        public string Faction_Name { get; set; }
        public string Faction_State { get; set; }
        public string Faction_Government { get; set; }
        public string Faction_Influence { get; set; }
        public string Faction_Influence_change { get; set; }
        public string Faction_Allegiance { get; set; }
        public string Faction_Happiness { get; set; }
        public string Faction_ActiveState { get; set; }
        public string Faction_PendingState { get; set; }
        public string Faction_RecoveringState { get; set; }
        public string missions { get; set; }
        public string explorer { get; set; }
        public string voucheer { get; set; }
        public string trade { get; set; }
    }
}

using System.Collections.Generic;

namespace UGC_API.DiscordBot.DCModels
{
    public class SystemData
    {
        public int Id { get; set; } = 0;
        public string StarSystem { get; set; } = "";
        public ulong SystemAddress { get; set; } = 0;
        public double[] Coords { get; set; } = { 0,0,0 };
        public double DistanceToSol { get; set; } = 0;
        public long Population { get; set; } = 0;
        public string SystemAllegiance { get; set; } = "";
        public int FactionsCount { get; set; } = 0;
        public List<FactionsModel> Factions { get; set; } = new();
        public class FactionsModel
        {
            public string Name { get; set; } = "";
            public string FactionState { get; set; } = "";
            public string Government { get; set; } = "";
            public double Influence { get; set; } = 0;
            public string Allegiance { get; set; } = "";
            public string Happiness { get; set; } = "";
            public string Happiness_Localised { get; set; } = "";
            public double MyReputation { get; set; } = 0;
            public List<ActiveStatesModel> ActiveStates { get; set; } = new();
            public class ActiveStatesModel
            {
                public string State { get; set; } = "";
            }
        }
    }
}

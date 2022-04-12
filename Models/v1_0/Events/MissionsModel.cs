using System;
namespace UGC_API.Models.v1_0.Events
{
    public class MissionsModel
    {
        public int id { get; set; }
        public ulong MissionID { get; set; }
        public DateTime timestamp { get; set; }
        public string Name { get; set; }
        public string Event { get; set; }
        public int CMDr { get; set; }
        public string JSON { get; set; }
    }
}

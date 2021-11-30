using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Models.v1_0.Events
{
    public class CarrierJumpRequest
    {
        public DateTime timestamp { get; set; }
        public string Event { get; set; }
        public long CarrierID { get; set; }
        public string SystemName { get; set; }
        public string Body { get; set; }
        public ulong SystemAddress { get; set; }
        public int BodyID { get; set; }
    }
}

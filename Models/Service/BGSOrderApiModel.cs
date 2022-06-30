using System.Collections.Generic;

namespace UGC_API.Models.Service
{
    public class BGSOrderApiModel
    {
        public List<OrderList> order_list { get; set; }
        public ulong updated_at {get;set;}
        public string bgs_plugin_version { get; set; }
        public class OrderList
        {
            public ulong id { get; set; }
            public string starsystem { get; set; }
            public string factionname { get; set; }
            public string type { get; set; }
            public int priority { get; set; }
            public string text { get; set; }
        }
    }
}

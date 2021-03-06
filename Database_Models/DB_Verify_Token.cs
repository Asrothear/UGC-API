using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_Verify_Token
    {
        public int id { get; set; }
        public ulong? discord_id { get; set; }
        public string discord_name { get; set; }
        public string token { get; set; }
        public int used { get; set; }
        public int max_usage { get; set; }
        public DateTime created_time { get; set; }
        public DateTime used_time { get; set; }
    }
}

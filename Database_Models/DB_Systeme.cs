using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_Systeme
    {
        public int id { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime last_update { get; set; }
        public int User_ID { get; set; }
        public long System_ID { get; set; }
        public string System_Name { get; set; }
        public string Factions { get; set; }
    }
}

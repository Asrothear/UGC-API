using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_Log
    {
        public int ID { get; set; }
        public DateTime Timestamp { get; set; }
        public int User { get; set; }
        public string Event { get; set; }
        public string JSON { get; set; }
        public string version_plugin { get; set; }
    }
}

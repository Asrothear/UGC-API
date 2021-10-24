using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_User
    {
        public int id { get; set; }
        public string user { get; set; }
        public string uuid { get; set; }
        public string token { get; set; }
        public string last_pos { get; set; }
        public string system { get; set; }
        public string docked { get; set; }
        public string docked_faction { get; set; }
        public string last_docked { get; set; }
        public string last_docked_faction { get; set; }
        public DateTime last_data_insert { get; set; }
        public double? version_plugin { get; set; }
        public string branch { get; set; }
    }
}

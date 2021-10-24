using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_Config
    {
        public int id { get; set; }
        public string systems { get; set; }
        public string events { get; set; }
        public int update_systems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace UGC_API.Database_Models
{
    public class DB_Plugin
    {
        public int id { get; set; }
        public int force_url { get; set; }
        public int force_update { get; set; }
        public int min_version { get; set; }
        public double min_minor { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Database_Models
{
    public class DB_Config
    {
        public int id { get; set; }
        public string systems_s { get; set; }
        public string events_s { get; set; }
        [NotMapped]
        public string[] systems { get; set; }
        [NotMapped]
        public string[] events { get; set; }
        public int update_systems { get; set; }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace UGC_API.Database_Models
{
    public class DB_Localisation
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string de { get; set; }
        public string en { get; set; }
    }
}

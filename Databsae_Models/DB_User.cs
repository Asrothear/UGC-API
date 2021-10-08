using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGC_API.Databsae_Models
{
    public class DB_Users
    {
        public static int id { get; set; }
        public static string user { get; set; }
        public static string last_pos { get; set; }
        public static string system { get; set; }
        public static string docked { get; set; }
        public static string docked_faction { get; set; }
        public static string last_docked { get; set; }
        public static string last_docked_faction { get; set; }
        public static DateTime last_data_insert { get; set; }
        public static string version_plugin { get; set; }
        public static string branch { get; set; }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Models.v1_0;

namespace UGC_API.Functions
{
    public class Carriers
    {
        public static List<DB_Carrier> _Carriers = new();
        internal static void LoadFromDB()
        {
            _Carriers = new List<DB_Carrier>(DatabaseHandler.db.Carrier);
        }
    }
}

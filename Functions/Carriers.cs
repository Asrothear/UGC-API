using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Models.v1_0;

namespace UGC_API.Functions
{
    public class Carriers
    {
        private static int xx = 0;
        private static bool updating = false;
        public static List<DB_Carrier> _Carriers = new();
        internal static void LoadFromDB()
        {
            if (!updating)
            {
                Debug.WriteLine($"Carriers.LoadFromDB() Execution: {xx}");
                xx++;
                updating = true;
                using (DBContext db = new())
                {
                    _Carriers = new List<DB_Carrier>(db.Carrier);
                }
                updating = false;
            }
        }
    }
}

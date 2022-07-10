using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Service;

namespace UGC_API.Functions
{
    public class Markets
    {
        public static List<DB_Market> _Markets = new();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        internal static void LoadFromDB()
        {
            try {
            using (DBContext db = new())
            {
                _Markets = new List<DB_Market>(db.Market);
            }
            }catch(Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
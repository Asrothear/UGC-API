using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;

namespace UGC_API.Functions
{
    public class Systems
    {
        public static List<DB_Systeme> _Systeme = new();
        internal static void LoadFromDB(DBContext db)
        {
            _Systeme = new(db.DB_Systemes);
        }
    }
}
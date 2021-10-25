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
        public static List<DB_Systeme> _Systeme = new ();
        internal static void LoadFromDB(DBContext db)
        {
            _Systeme = new(db.DB_Systemes);
        }
        internal static int CountAllData()
        {
            return _Systeme.Count();
        }
        internal static dynamic GetSystem(string SystemName, string time)
        {
            var sys = _Systeme.Where(u => u.System_Name == SystemName).ToList();
            if (sys == null) return 0;
            var sys_out = sys.Where(i => i.Timestamp == time).ToList();
            if (sys_out == null) return 1; // WIP Create new Day for System !!
            return sys_out;
        } 
    }
}
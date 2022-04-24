using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;

namespace UGC_API.Functions
{
    public class Systems
    {
        public static List<DB_Systeme> _Systeme = new();
        public static List<DB_SystemData> _SystemData = new();
        internal static void LoadFromDB()
        {
            _Systeme = new(DatabaseHandler.db.DB_Systemes);
            _SystemData = new(DatabaseHandler.db.DB_SystemData);
        }
        internal static double[] GetSystemCoords(ulong SystemAddress)
        {
            var SystemByAddress = _SystemData.Find(x => x.systemAddress == SystemAddress);
            if (SystemByAddress == null)
            {
                return null;
            }
            string[] coord = SystemByAddress.starPos.Replace("[", "").Replace("]", "").Split(',');
            List<double> coords = new();
            foreach(var coor in coord)
            {
                coords.Add(double.Parse(coor, CultureInfo.InvariantCulture));
            }
            return coords.ToArray();
        }
        internal static void UpdateSystemData(string starSystem, ulong systemAddress, double[] starPos, long population)
        {
            var syst = _SystemData.Find(sys => sys.systemAddress == systemAddress);
            if (syst == null)
            {
                syst = new DB_SystemData
                {
                    starSystem = starSystem,
                    systemAddress = systemAddress,
                    starPos = JsonSerializer.Serialize(starPos),
                    population = population
                };
            }
            else
            {
                syst.starSystem = starSystem;
                syst.systemAddress = systemAddress;
                syst.starPos = JsonSerializer.Serialize(starPos);
                syst.population = population;
            }
            using(DBContext db = new())
            {
                db.DB_SystemData.Update(syst);
                try
                {
                    db.SaveChanges();
                }catch (Exception ex)
                {

                }
            }
        }
    }
}
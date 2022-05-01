using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.EDDN.Model;

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
            var SystemByAddress = _SystemData.Find(x => x.SystemAddress == SystemAddress);
            if (SystemByAddress == null)
            {
                return null;
            }
            string[] coord = SystemByAddress.StarPos.Replace("[", "").Replace("]", "").Split(',');
            List<double> coords = new();
            foreach(var coor in coord)
            {
                coords.Add(double.Parse(coor, CultureInfo.InvariantCulture));
            }
            return coords.ToArray();
        }
        internal static void UpdateSystemData(EDDN_FSDJumpModel Data)
        {
            var syst = _SystemData.Find(sys => sys.SystemAddress == Data.SystemAddress);
            if (syst == null)
            {
                syst = new DB_SystemData
                {
                    StarSystem = Data.StarSystem,
                    SystemAddress = Data.SystemAddress,
                    StarPos = JsonSerializer.Serialize(Data.StarPos),
                    Population = Data.Population,
                    SystemAllegiance = Data.SystemAllegiance,
                    Factions = JsonSerializer.Deserialize<List<DB_SystemData.FactionsModel>>(JsonSerializer.Serialize(Data.Factions)),
                };
                syst.FactionsCount = syst.Factions.Count;
                syst.Faction_String = JsonSerializer.Serialize(syst.Factions);
            }
            else
            {
                syst.StarSystem = Data.StarSystem;
                syst.SystemAddress = Data.SystemAddress;
                syst.StarPos = JsonSerializer.Serialize(Data.StarPos);
                syst.Population = Data.Population;
                syst.Factions = JsonSerializer.Deserialize<List<DB_SystemData.FactionsModel>>(JsonSerializer.Serialize(Data.Factions));
                syst.FactionsCount = syst.Factions.Count;
                syst.Faction_String = JsonSerializer.Serialize(syst.Factions);
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
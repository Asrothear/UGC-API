using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.EDDN;
using UGC_API.EDDN.Model;
using UGC_API.Handler;
using UGC_API.Service;

namespace UGC_API.Functions
{
    public class Systems
    {
        public static List<DB_Systeme> _Systeme = new();
        public static List<DB_SystemData> _SystemData = new();
        internal static void LoadFromDB()
        {
            using (DBContext db = new())
            {
                _Systeme = new(db.DB_Systemes);
                _SystemData = new(db.DB_SystemData);
            }
            LoggingService.schreibeLogZeile($"{_Systeme.Count} System´s geladen.");
            LoggingService.schreibeLogZeile($"{_SystemData.Count} SystemData geladen.");
            TimerHandler.Start();
            EDDNListener.listener();
            Task.Run(() => { LoggingService.schreibeLogZeile($"ShedulerHandler wird geladen..."); ShedulerHandler.StateListUpdate(); LoggingService.schreibeLogZeile($"StateListUpdate geladen."); });
            LoggingService.schreibeLogZeile($"{_SystemData.Count} Coordinaten werden geladen...");
            var xx = 0;
            /*foreach (var sysd in _SystemData)
            {
                GetSystemCoords(sysd.SystemAddress);
                xx++;
            }
            */
            LoggingService.schreibeLogZeile($"{xx} Coordinaten geladen.");
        }
        internal static void GetSystemCoords(ulong SystemAddress)
        {
            var SystemByAddress = _SystemData.Find(x => x.SystemAddress == SystemAddress);
            if (SystemByAddress == null)
            {
                return;
            }
            if(SystemByAddress.Coords != null && SystemByAddress.Coords.Length == 3) { return;}
            SystemByAddress.Coords = JsonSerializer.Deserialize<double[]>(SystemByAddress.StarPos);
            return;
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
                    Coords = Data.StarPos,
                    Population = Data.Population,
                    SystemAllegiance = Data.SystemAllegiance,
                    Factions = JsonSerializer.Deserialize<List<DB_SystemData.FactionsModel>>(JsonSerializer.Serialize(Data.Factions)),
                };
                syst.FactionsCount = syst.Factions?.Count ?? 0;
                syst.Faction_String = JsonSerializer.Serialize(syst.Factions);
                GetSystemCoords(syst.SystemAddress);
                _SystemData.Add(syst);
            }
            else
            {
                syst.StarSystem = Data.StarSystem;
                syst.SystemAddress = Data.SystemAddress;
                syst.StarPos = JsonSerializer.Serialize(Data.StarPos);
                syst.Coords = Data.StarPos;
                syst.Population = Data.Population;
                syst.Factions = JsonSerializer.Deserialize<List<DB_SystemData.FactionsModel>>(JsonSerializer.Serialize(Data.Factions));
                syst.FactionsCount = syst.Factions?.Count ?? 0;
                syst.Faction_String = JsonSerializer.Serialize(syst.Factions);
                syst.SystemAllegiance = Data.SystemAllegiance;
                GetSystemCoords(syst.SystemAddress);
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
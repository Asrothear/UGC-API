using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;
using UGC_API.Models.v1_0;
using UGC_API.Service;

namespace UGC_API.Handler.v1_0
{
    public class CarrierHandler
    {
        public static List<CarrierModel> _Carriers = new();
        internal static long LastCarrierID = 0;
        private static bool UpdateRuning = false;
        internal static void CarrierEvent(string json, string @event)
        {
            switch (@event)
            {
                case "CarrierStats":
                    CarrierStats(JsonSerializer.Deserialize<Models.v1_0.Events.CarrierStats>(json));
                    break;
                case "CarrierJump":
                    CarrierJump(JsonSerializer.Deserialize<Models.v1_0.Events.CarrierJump>(json));
                    break;
                case "CarrierJumpRequest":
                    CarrierJumpRequest(JsonSerializer.Deserialize<Models.v1_0.Events.CarrierJumpRequest>(json));
                    break;
                case "CarrierJumpCancelled":
                    CarrierJumpCancelled(JsonSerializer.Deserialize<Models.v1_0.Events.CarrierJumpCancelled>(json));
                    break;
                default:
                    break;
            }
        }

        private static void CarrierJumpCancelled(Models.v1_0.Events.CarrierJumpCancelled carrierJumpCancelled)
        {
            var Carrier = _Carriers.FirstOrDefault(c => c.CarrierID == carrierJumpCancelled.CarrierID);
            if (Carrier == null) Carrier = new();
            Carrier.CarrierID = carrierJumpCancelled.CarrierID;
            Carrier.System = Carrier.prev_System;
            Carrier.SystemAdress = Carrier.SystemAdress;
            Carrier.prev_System = "";
            Carrier.prev_SystemAdress = 0;
            UpdateCarrier(Carrier);
        }

        private static void CarrierJumpRequest(Models.v1_0.Events.CarrierJumpRequest carrierJumpRequest)
        {
            var Carrier = _Carriers.Find(c => c.CarrierID == carrierJumpRequest.CarrierID);
            if (Carrier == null) Carrier = new();
            var sys = Carrier.System;
            var sysa = Carrier.SystemAdress;
            Carrier.CarrierID = carrierJumpRequest.CarrierID;
            Carrier.prev_System = Carrier.System;
            Carrier.prev_SystemAdress = Carrier.SystemAdress;
            Carrier.System = carrierJumpRequest.SystemName;
            Carrier.SystemAdress = carrierJumpRequest.SystemAddress;
            UpdateCarrier(Carrier);            
            DiscordBot.Functions.AnnounceJump(Carrier, carrierJumpRequest, sys, sysa);
        }

        private static void CarrierJump(Models.v1_0.Events.CarrierJump carrierJump)
        {
            var Carrier = _Carriers.FirstOrDefault(c => c.CarrierID == carrierJump.MarketID);
            if (Carrier == null) Carrier = new();
            Carrier.CarrierID = carrierJump.MarketID;
            Carrier.prev_System = Carrier.System;
            Carrier.prev_SystemAdress = Carrier.SystemAdress;
            Carrier.System = carrierJump.StarSystem;
            Carrier.SystemAdress = carrierJump.SystemAddress;
            UpdateCarrier(Carrier);
        }

        private static void CarrierStats(Models.v1_0.Events.CarrierStats carrierStats)
        {
            var create = false;
            var Carrier = _Carriers.FirstOrDefault(c => c.CarrierID == carrierStats.CarrierID);
            if (Carrier == null)
            {
                if (LastCarrierID == carrierStats.CarrierID) return;
                Carrier = new();
                create = true;
            }
            Carrier.CarrierID = carrierStats.CarrierID;
            Carrier.Name = carrierStats.Name;
            Carrier.Callsign = carrierStats.Callsign;
            Carrier.DockingAccess = carrierStats.DockingAccess;
            Carrier.AllowNotorious = carrierStats.AllowNotorious;
            Carrier.FuelLevel = carrierStats.FuelLevel;
            Carrier.JumpRangeCurr = carrierStats.JumpRangeCurr;
            Carrier.JumpRangeMax = carrierStats.JumpRangeMax;
            Carrier.PendingDecommission = carrierStats.PendingDecommission;
            Carrier.SpaceUsage = JsonSerializer.Deserialize<CarrierModel.SpaceUsageModel>(JsonSerializer.Serialize(carrierStats.SpaceUsage));
            Carrier.Finance = JsonSerializer.Deserialize<CarrierModel.FinanceModel>(JsonSerializer.Serialize(carrierStats.Finance));
            Carrier.Crew = JsonSerializer.Deserialize<List<CarrierModel.CrewModel>>(JsonSerializer.Serialize(carrierStats.Crew));
            Carrier.ShipPacks = JsonSerializer.Deserialize<List<CarrierModel.ShipPacksModel>>(JsonSerializer.Serialize(carrierStats.ShipPacks));
            Carrier.ModulePacks = JsonSerializer.Deserialize<List<CarrierModel.ModulePacksModel>>(JsonSerializer.Serialize(carrierStats.ModulePacks));
            UpdateCarrier(Carrier, create);
            LastCarrierID = 0;
        }

        internal static void LoadCarrier(bool force = false)
        {
            if (!UpdateRuning) { 
                UpdateRuning = true;
                if (_Carriers.Count != 0 && !force) return;
                _Carriers = new();
                if (force) Carriers.LoadFromDB();
                _Carriers = ParseCarrier(Carriers._Carriers);
                UpdateRuning = false;
                Service.LoggingService.schreibeLogZeile($"{_Carriers.Count} Carrier´s geladen.");
            }
        }
        internal static CarrierModel GetCarrier(string CS)
        {
            var CM = _Carriers.FirstOrDefault(u => u.Callsign.ToLower() == CS.ToLower());
            return CM;
        }        
        public static List<CarrierModel> ParseCarrier(List<DB_Carrier> dB_Carriers)
        {
            List<CarrierModel> API_Carier = new();
            foreach (var DB_Carrier in dB_Carriers)
            {
                var CarrierData = new CarrierModel
                {
                    id = DB_Carrier.id,
                    CarrierID = DB_Carrier.CarrierID,
                    OwnerDC = DB_Carrier.OwnerDC,
                    Name = DB_Carrier.Name,
                    Callsign = DB_Carrier.Callsign,
                    System = DB_Carrier.System,
                    SystemAdress = DB_Carrier.SystemAdress,
                    prev_System = DB_Carrier.prev_System == "" ? null : DB_Carrier.prev_System,
                    prev_SystemAdress = DB_Carrier.prev_SystemAdress,
                    DockingAccess = DB_Carrier.DockingAccess,
                    AllowNotorious = (DB_Carrier.AllowNotorious != "1" ? false : true),
                    FuelLevel = double.Parse(DB_Carrier.FuelLevel, CultureInfo.InvariantCulture),
                    JumpRangeCurr = double.Parse(DB_Carrier.JumpRangeCurr, CultureInfo.InvariantCulture),
                    JumpRangeMax = double.Parse(DB_Carrier.JumpRangeMax, CultureInfo.InvariantCulture),
                    PendingDecommission = DB_Carrier.PendingDecommission != "1" ? false : true,
                    SpaceUsage = ConvertToSpaceUsage(DB_Carrier.SpaceUsage),
                    Finance = ConvertToFinance(DB_Carrier.Finance),
                    Crew = ConvertToCrew(DB_Carrier.Crew),
                    ShipPacks = ConvertToShipPacks(DB_Carrier.ShipPacks),
                    ModulePacks = ConvertToModulePacks(DB_Carrier.ModulePacks),
                    Last_Update = DateTime.Parse(DB_Carrier.Last_Update)
                };                
                API_Carier.Add(CarrierData);
            }
            return API_Carier;
        }
        private static CarrierModel.SpaceUsageModel ConvertToSpaceUsage(string DB_Carrier)
        {
            CarrierModel.SpaceUsageModel OBJ = new();
            if (String.IsNullOrEmpty(DB_Carrier) || DB_Carrier.Length <= 4)
            {
                return OBJ;
            }
            try
            {
                var _Obj = JObject.Parse(DB_Carrier.Replace("[", "").Replace("]", "").Replace("\\", ""));
                OBJ.TotalCapacity = _Obj["TotalCapacity"]?.Value<long?>() ?? 0;
                OBJ.Crew = _Obj["Crew"]?.Value<long?>() ?? 0;
                OBJ.Cargo = _Obj["Cargo"]?.Value<long?>() ?? 0;
                OBJ.CargoSpaceReserved = _Obj["CargoSpaceReserved"]?.Value<long?>() ?? 0;
                OBJ.ShipPacks = _Obj["ShipPacks"]?.Value<long?>() ?? 0;
                OBJ.ModulePacks = _Obj["ModulePacks"]?.Value<long?>() ?? 0;
                OBJ.FreeSpace = _Obj["FreeSpace"]?.Value<long?>() ?? 0;
            }catch(Exception e){
                OBJ = JsonSerializer.Deserialize<CarrierModel.SpaceUsageModel>(DB_Carrier);
            }
            return OBJ;
        }
        private static CarrierModel.FinanceModel ConvertToFinance(string DB_Carrier)
        {
            CarrierModel.FinanceModel OBJ = new();
            if (String.IsNullOrEmpty(DB_Carrier) || DB_Carrier.Length <= 4)
            {
                return OBJ;
            }
            try
            {
                var _Obj = JObject.Parse(DB_Carrier.Replace("[", "").Replace("]", "").Replace("\\", ""));
                OBJ.CarrierBalance = _Obj["CarrierBalance"]?.Value<long?>() != null ? _Obj["CarrierBalance"].Value<long>() : 0;
                OBJ.ReserveBalance = _Obj["ReserveBalance"]?.Value<long?>() != null ? _Obj["ReserveBalance"].Value<long>() : 0;
                OBJ.AvailableBalance = _Obj["AvailableBalance"]?.Value<long?>() != null ? _Obj["AvailableBalance"].Value<long>() : 0;
                OBJ.ReservePercent = _Obj["ReservePercent"]?.Value<long?>() != null ? _Obj["ReservePercent"].Value<long>() : 0;
                OBJ.TaxRate = _Obj["TaxRate"]?.Value<double?>() != null ? _Obj["TaxRate"].Value<double>() : 0;
            }
            catch(Exception e)
            {
                OBJ = JsonSerializer.Deserialize<CarrierModel.FinanceModel>(DB_Carrier);
            }
            return OBJ;
        }
        private static List<CarrierModel.CrewModel> ConvertToCrew(string DB_Carrier)
        {
            List<CarrierModel.CrewModel> OBJ = new();
            if (String.IsNullOrEmpty(DB_Carrier) || DB_Carrier.Length <= 4)
            {
                return OBJ;
            }
            try
            {
                var _Obj = JObject.Parse(DB_Carrier.Replace("[", "").Replace("]", "").Replace("\\", ""));
                foreach (var Data in _Obj)
                {
                    var NewData = new CarrierModel.CrewModel
                    {
                        CrewRole = Data.Value["CrewRole"]?.Value<string>() ?? null,
                        Activated = (Data.Value["Activated"]?.Value<string>() ?? null) != "1" ? false : true,
                        Enabled = (Data.Value["Enabled"]?.Value<string>() ?? null) != "1" ? false : true,
                        CrewName = Data.Value["CrewName"]?.Value<string>() ?? null,
                    };
                    OBJ.Add(NewData);
                }
            }catch(Exception e)
            {
                OBJ = JsonSerializer.Deserialize<List<CarrierModel.CrewModel>>(DB_Carrier);
            }
            return OBJ;
        }
        private static List<CarrierModel.ShipPacksModel> ConvertToShipPacks(string DB_Carrier)
        {
            List<CarrierModel.ShipPacksModel> OBJ = new();
            if (String.IsNullOrEmpty(DB_Carrier) || DB_Carrier.Length <= 4)
            {
                return OBJ;
            }
            try
            {
                var _Obj = JObject.Parse(DB_Carrier.Replace("[", "").Replace("]", "").Replace("\\", ""));
                foreach (var Data in _Obj)
                {
                    var NewData = new CarrierModel.ShipPacksModel
                    {
                        PackTheme = Data.Value["PackTheme"]?.Value<string>() ?? null,
                        PackTier = Data.Value["PackTier"]?.Value<long?>() ?? 0,
                    };
                    OBJ.Add(NewData);
                }
            }
            catch (Exception e)
            {
                OBJ = JsonSerializer.Deserialize<List<CarrierModel.ShipPacksModel>>(DB_Carrier);
            }
            return OBJ;
        }
        private static List<CarrierModel.ModulePacksModel> ConvertToModulePacks(string DB_Carrier)
        {
            List<CarrierModel.ModulePacksModel> OBJ = new();
            if (String.IsNullOrEmpty(DB_Carrier) || DB_Carrier.Length <= 4)
            {
                return OBJ;
            }
            try
            {
                var _Obj = JObject.Parse(DB_Carrier.Replace("[", "").Replace("]", "").Replace("\\", ""));
                foreach (var Data in _Obj)
                {
                    var NewData = new CarrierModel.ModulePacksModel
                    {
                        PackTheme = Data.Value["PackTheme"]?.Value<string>() ?? null,
                        PackTier = Data.Value["PackTier"]?.Value<long?>() ?? 0
                    };
                    OBJ.Add(NewData);
                }
            }
            catch (Exception e)
            {
                OBJ = JsonSerializer.Deserialize<List<CarrierModel.ModulePacksModel>>(DB_Carrier);
            }
            return OBJ;
        }
        internal static void UpdateCarrier(Models.v1_0.CarrierModel CarrierEntry, bool create = false)
        {
            var DBCarrier = Carriers._Carriers.FirstOrDefault(c => c.CarrierID == CarrierEntry.CarrierID);
            if (DBCarrier == null) DBCarrier = new();
            DBCarrier.CarrierID = CarrierEntry.CarrierID;
            DBCarrier.OwnerDC = CarrierEntry.OwnerDC;
            DBCarrier.Name = CarrierEntry.Name;
            DBCarrier.Callsign = CarrierEntry.Callsign;
            DBCarrier.System = CarrierEntry.System;
            DBCarrier.SystemAdress = CarrierEntry.SystemAdress;
            DBCarrier.prev_System = CarrierEntry.prev_System;
            DBCarrier.prev_SystemAdress = CarrierEntry.prev_SystemAdress;
            DBCarrier.DockingAccess = CarrierEntry.DockingAccess;
            DBCarrier.AllowNotorious = CarrierEntry.AllowNotorious.ToString();
            DBCarrier.FuelLevel = CarrierEntry.FuelLevel.ToString();
            DBCarrier.JumpRangeCurr = CarrierEntry.JumpRangeCurr.ToString();
            DBCarrier.JumpRangeMax = CarrierEntry.JumpRangeMax.ToString();
            DBCarrier.PendingDecommission = CarrierEntry.PendingDecommission.ToString();
            DBCarrier.SpaceUsage = JsonSerializer.Serialize(CarrierEntry.SpaceUsage);
            DBCarrier.Finance = JsonSerializer.Serialize(CarrierEntry.Finance);
            DBCarrier.Crew = JsonSerializer.Serialize(CarrierEntry.Crew);
            DBCarrier.ShipPacks = JsonSerializer.Serialize(CarrierEntry.ShipPacks);
            DBCarrier.ModulePacks = JsonSerializer.Serialize(CarrierEntry.ModulePacks);
            DBCarrier.Last_Update = DateTime.Now.ToString();
            using (DBContext db = new())
            {
                if (create)
                {
                    db.Carrier.Add(DBCarrier);
                }
                else
                {
                    db.Carrier.Update(DBCarrier);
                }
                db.SaveChanges();
                db.Dispose();
            }
            Task.Run(() => { ParseCarrier(Carriers._Carriers); });
        }

        internal static void UpdateAllCarrier()
        {
            foreach (var CAR in _Carriers)
            {
                var DBCarrier = DatabaseHandler.db.Carrier.FirstOrDefault(c => c.Callsign == CAR.Callsign);
                DBCarrier.CarrierID = CAR.CarrierID;
                DBCarrier.Name = CAR.Name;
                DBCarrier.Callsign = CAR.Callsign;
                DBCarrier.System = CAR.System;
                DBCarrier.SystemAdress = CAR.SystemAdress;
                DBCarrier.prev_System = CAR.prev_System;
                DBCarrier.prev_SystemAdress = CAR.prev_SystemAdress;
                DBCarrier.DockingAccess = CAR.DockingAccess;
                DBCarrier.AllowNotorious = CAR.AllowNotorious.ToString();
                DBCarrier.FuelLevel = CAR.FuelLevel.ToString();
                DBCarrier.JumpRangeCurr = CAR.JumpRangeCurr.ToString();
                DBCarrier.JumpRangeMax = CAR.JumpRangeMax.ToString();
                DBCarrier.PendingDecommission = CAR.PendingDecommission.ToString();
                DBCarrier.SpaceUsage = JsonSerializer.Serialize(CAR.SpaceUsage);
                DBCarrier.Finance = JsonSerializer.Serialize(CAR.Finance);
                DBCarrier.Crew = JsonSerializer.Serialize(CAR.Crew);
                DBCarrier.ShipPacks = JsonSerializer.Serialize(CAR.ShipPacks);
                DBCarrier.ModulePacks = JsonSerializer.Serialize(CAR.ModulePacks);
                DBCarrier.Last_Update = CAR.Last_Update.ToString();

                DatabaseHandler.db.Carrier.Update(DBCarrier);
                DatabaseHandler.db.SaveChangesAsync();

            }
            Task.Run(() => { ParseCarrier(Carriers._Carriers); });
        }
    }
}

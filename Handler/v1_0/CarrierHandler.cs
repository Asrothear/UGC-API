using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;
using UGC_API.Models.v1_0;

namespace UGC_API.Handler.v1_0
{
    public class CarrierHandler
    {
        public static List<CarrierModel> _Carriers = new();
        internal static void LoadCarrier(bool force = false)
        {
            if (_Carriers.Count != 0 && !force) return;
            _Carriers = new();
            if (force) Carriers.LoadFromDB(new DBContext());
            _Carriers = ParseCarrier(Carriers._Carriers);
        }

        internal static List<CarrierModel.MarketModel> GetCarrierMarket(string CS)
        {
            LoadCarrier();
            var CM = _Carriers.FirstOrDefault(u => u.Callsign == CS);
            return CM.market;
        }
        internal static CarrierModel GetCarrier(string CS)
        {
            LoadCarrier();
            var CM = _Carriers.FirstOrDefault(u => u.Callsign == CS);
            return CM;
        }
        internal static List<CarrierModel.MarketSearchModel> FindWare(string Ware)
        {
            LoadCarrier();
            List<CarrierModel.MarketSearchModel> OBJ = new();
            List <CarrierModel> CM = new();
            try
            {
                CM = _Carriers.Where(u => u.market.SingleOrDefault(m => m.Name == Ware && m.BuyPrice > 0 && m.Stock > 0) != null && u.DockingAccess != "none").ToList();
            }catch(Exception e){
                return OBJ;
            }
            foreach (var Data in CM)
            {
                var NewData = new CarrierModel.MarketSearchModel
                {
                    id = Data.id,
                    CarrierID = Data.CarrierID,
                    Name = Data.Name,
                    Callsign = Data.Callsign,
                    System = Data.System,
                    DockingAccess = Data.DockingAccess,
                    market = Data.market.Where(i => i.Name == Ware).ToList()
                };
                OBJ.Add(NewData);
            }
            return OBJ;
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
                    Name = DB_Carrier.Name,
                    Callsign = DB_Carrier.Callsign,
                    System = DB_Carrier.System,
                    prev_System = DB_Carrier.prev_System == "" ? null : DB_Carrier.prev_System,
                    DockingAccess = DB_Carrier.DockingAccess,
                    AllowNotorious = (DB_Carrier.AllowNotorious != "1" ? false : true),
                    FuelLevel = Convert.ToInt32(DB_Carrier.FuelLevel),
                    JumpRangeCurr = Convert.ToInt32(DB_Carrier.JumpRangeCurr),
                    JumpRangeMax = Convert.ToInt32(DB_Carrier.JumpRangeMax),
                    PendingDecommission = DB_Carrier.PendingDecommission == "" ? null : DB_Carrier.PendingDecommission,
                    SpaceUsage = ConvertToSpaceUsage(DB_Carrier.SpaceUsage),
                    Finance = ConvertToFinance(DB_Carrier.Finance),
                    Crew = ConvertToCrew(DB_Carrier.Crew),
                    ShipPacks = ConvertToShipPacks(DB_Carrier.ShipPacks),
                    ModulePacks = ConvertToModulePacks(DB_Carrier.ModulePacks),
                    market = ConvertToMarket(DB_Carrier.market),
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
                OBJ.CarrierBalance = _Obj["CarrierBalance"]?.Value<long?>() ?? 0;
                OBJ.ReserveBalance = _Obj["ReserveBalance"]?.Value<long?>() ?? 0;
                OBJ.AvailableBalance = _Obj["AvailableBalance"]?.Value<long?>() ?? 0;
                OBJ.ReservePercent = _Obj["ReservePercent"]?.Value<long?>() ?? 0;
                OBJ.TaxRate = _Obj["TaxRate"]?.Value<long?>() ?? 0;
            }catch(Exception e)
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
        private static List<CarrierModel.MarketModel> ConvertToMarket(string DB_Carrier)
        {
            List<CarrierModel.MarketModel> OBJ = new();
            if (String.IsNullOrEmpty(DB_Carrier) || DB_Carrier.Length <= 4)
            {
                return OBJ;
            }
            try
            {
                var _Obj = JObject.Parse(DB_Carrier.Replace("[", "").Replace("]", "").Replace("\\", ""));
                foreach (var Data in _Obj)
                {
                    if (Data.Key == "update") { return OBJ; }

                    var NewData = new CarrierModel.MarketModel
                    {
                        id = Data.Value["id"]?.Value<long?>() ?? 0,
                        Name = Data.Value["Name"]?.Value<string>() ?? null,
                        Name_Localised = Data.Value["Name_Localised"]?.Value<string>() ?? null,
                        Category = Data.Value["Category"]?.Value<string>() ?? null,
                        Category_Localised = Data.Value["Category_Localised"]?.Value<string>() ?? null,
                        BuyPrice = Data.Value["BuyPrice"]?.Value<long?>() ?? 0,
                        SellPrice = Data.Value["SellPrice"]?.Value<long?>() ?? 0,
                        MeanPrice = Data.Value["MeanPrice"]?.Value<long?>() ?? 0,
                        StockBracket = Data.Value["StockBracket"]?.Value<long?>() ?? 0,
                        DemandBracket = Data.Value["DemandBracket"]?.Value<long?>() ?? 0,
                        Stock = Data.Value["Stock"]?.Value<long?>() ?? 0,
                        Demand = Data.Value["Demand"]?.Value<long?>() ?? 0,
                        Consumer = Data.Value["Consumer"]?.Value<bool?>() ?? false,
                        Producer = Data.Value["Producer"]?.Value<bool?>() ?? false,
                        Rare = Data.Value["Rare"]?.Value<bool?>() ?? false,
                    };
                    OBJ.Add(NewData);
                }
            }catch(Exception e)
            {
                OBJ = JsonSerializer.Deserialize<List<CarrierModel.MarketModel>>(DB_Carrier);
            }
            return OBJ;
        }
        internal static void UpdateCarrier()
        {
            LoadCarrier();
            foreach (var CAR in _Carriers)
            {
                using (DBContext db = new DBContext())
                {
                    var DBCarrier = db.Carrier.FirstOrDefault(c => c.Callsign == CAR.Callsign);
                    DBCarrier.CarrierID = CAR.CarrierID;
                    DBCarrier.Name = CAR.Name;
                    DBCarrier.Callsign = CAR.Callsign;
                    DBCarrier.System = CAR.System;
                    DBCarrier.prev_System = CAR.prev_System;
                    DBCarrier.DockingAccess = CAR.DockingAccess;
                    DBCarrier.AllowNotorious = CAR.AllowNotorious.ToString();
                    DBCarrier.FuelLevel = CAR.FuelLevel.ToString();
                    DBCarrier.JumpRangeCurr = CAR.JumpRangeCurr.ToString();
                    DBCarrier.JumpRangeMax = CAR.JumpRangeMax.ToString();
                    DBCarrier.PendingDecommission = CAR.PendingDecommission;
                    DBCarrier.SpaceUsage = JsonSerializer.Serialize(CAR.SpaceUsage);
                    DBCarrier.Finance = JsonSerializer.Serialize(CAR.Finance);
                    DBCarrier.Crew = JsonSerializer.Serialize(CAR.Crew);
                    DBCarrier.ShipPacks = JsonSerializer.Serialize(CAR.ShipPacks);
                    DBCarrier.ModulePacks = JsonSerializer.Serialize(CAR.ModulePacks);
                    DBCarrier.market = JsonSerializer.Serialize(CAR.market);
                    DBCarrier.Last_Update = CAR.Last_Update.ToString();

                    db.Carrier.Update(DBCarrier);
                    db.SaveChanges();
                }
            }
            LoadCarrier(true);
        }
    }
}

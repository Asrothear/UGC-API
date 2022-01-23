﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;

namespace UGC_API.Models.v1_0
{

    public class CarrierModel
    {
        public static List<CarrierModel> _Carriers = new List<CarrierModel>();
        public int id { get; set; }
        public long CarrierID { get; set; }
        public string Name { get; set; } = "";
        public string Callsign { get; set; } = "";
        public string System { get; set; }
        public string prev_System { get; set; }
        public string DockingAccess { get; set; }
        public bool? AllowNotorious { get; set; }
        public double FuelLevel { get; set; }
        public double JumpRangeCurr { get; set; }
        public double JumpRangeMax { get; set; }
        public bool PendingDecommission { get; set; }
        public SpaceUsageModel SpaceUsage { get; set; } = new();
        public FinanceModel Finance { get; set; } = new();
        public List<CrewModel> Crew { get; set; } = new();
        public List<ShipPacksModel> ShipPacks { get; set; } = new();
        public List<ModulePacksModel> ModulePacks { get; set; } = new();
        public DateTime Last_Update { get; set; }

        public class SpaceUsageModel
        {
            public long TotalCapacity { get; set; }
            public long Crew { get; set; }
            public long Cargo { get; set; }
            public long CargoSpaceReserved { get; set; }
            public long ShipPacks { get; set; }
            public long ModulePacks { get; set; }
            public long FreeSpace { get; set; }
        }
        public class CrewModel
        {
            public string CrewRole { get; set; }
            public bool? Activated { get; set; }
            public bool? Enabled { get; set; }
            public string CrewName { get; set; }
        }
        public class FinanceModel
        {
            public long? CarrierBalance { get; set; }
            public long? ReserveBalance { get; set; }
            public long? AvailableBalance { get; set; }
            public long? ReservePercent { get; set; }
            public double? TaxRate { get; set; }
        }
        public class ModulePacksModel
        {
            public string PackTheme { get; set; }
            public long? PackTier { get; set; }
        }
        public class ShipPacksModel
        {
            public string PackTheme { get; set; }
            public long? PackTier { get; set; }
        }
    }
}

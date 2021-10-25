using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UGC_API.Config;
using UGC_API.Functions;

namespace UGC_API.Handler.v1_0
{
    public class JumpHandler
    {
        public static void FSDJump()
        {
            string system = QLSHandler.QLSData["StarSystem"]?.Value<string>() ?? null;
            if (!Configs.Systems.Contains<string>(system)) {
                return;
            };
            string time = DateTime.Now.ToString("d");
            string[] t_arry = time.Split('.');
            int year = Convert.ToInt32(t_arry[2]) + 1286;
            time = $"{year}-{t_arry[1]}-{t_arry[0]}";
            var DB_System = Systems.GetSystem(system, time);/*
            if (DB_System.timestamp != Configs.current.Timestring) SystemHandler.NewDay(system);

            foreach (var Data in QLS.json["Factions"])
            {
                SystemSingleData Faction = new SystemSingleData();
                Faction.Name = Data["Name"].ToString();
                Faction.State = Data["FactionState"].ToString();
                Faction.Government = Data["Government"].ToString();
                Faction.Influence = Data["Influence"].ToString();
                Faction.Allegiance = Data["Allegiance"].ToString();
                Faction.Happiness = Data["Happiness"].ToString();
                Faction.ActiveState = "";
                Faction.PendingState = "";
                Faction.RecoveringState = "";
                if (Data["ActiveStates"] != null)
                {
                    Faction.ActiveState = Data["ActiveStates"].ToString().Replace("\"", "").Replace("\n", "").Replace("\r", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("State:", "").Replace("{", "").Replace("}", "");
                }
                if (Data["PendingStates"] != null)
                {
                    Faction.PendingState = Data["PendingStates"].ToString().Replace("\"", "").Replace("\n", "").Replace("\r", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("State:", "").Replace("{", "").Replace("}", "");
                }
                if (Data["RecoveringStates"] != null)
                {
                    Faction.RecoveringState = Data["RecoveringStates"].ToString().Replace("\"", "").Replace("\n", "").Replace("\r", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("State:", "").Replace("{", "").Replace("}", "");
                }
                DBSystemHandler.UpdateFaction(Faction);
            }*/
        }
    }
}

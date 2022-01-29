using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using UGC_API.Config;
using UGC_API.Functions;
using UGC_API.Models.v1_0;
using UGC_API.Models.v1_0.Events;

namespace UGC_API.Handler.v1_0
{
    public class JumpHandler
    {
        internal static FSDJump JumpData = null;
        public static void FSDJump(Models.v1_0.Events.FSDJump fSDJump, JObject qLSData, DateTime timeStamp, Database_Models.DB_User user)
        {
            var ve = JsonSerializer.Serialize(fSDJump.Factions);
            Localisation.Fetch(ve, user);
            JumpData = fSDJump;
            JumpData.Timestamp = timeStamp;
            string system = qLSData["StarSystem"]?.Value<string>() ?? null;
            LocationHandler.UserSetLocation(user, JumpData.StarPos, JumpData.StarSystem);
            if (!Configs.Systems.Contains<string>(system))
            {
                return;
            };
            Systems.SetSystemData(fSDJump.StarSystem, fSDJump.SystemAddress, fSDJump.StarPos, fSDJump.Population);
            string[] t_arry = JumpData.Timestamp.ToString("d").Split('.');
            int year = Convert.ToInt32(t_arry[2]) + 1286;
            var time = DateTime.Parse($"{year}-{t_arry[1]}-{t_arry[0]}");
            var API_System = SystemHandler.GetSystem(user, system, time);
            if (API_System == null) { return; };
            var DB_System = Systems._Systeme.FirstOrDefault(db => db.System_Name == system && db.Timestamp == time);
            if (DB_System == null) { return; };
            API_System.id = DB_System.id;
            API_System.last_update = DateTime.Now;
            API_System.User_ID = user.id;
            API_System.System_ID = JumpData.SystemAddress;
            API_System.System_Name = JumpData.StarSystem;
            API_System.Factions = JsonSerializer.Deserialize<List<SystemModel.FactionsL>>(JsonSerializer.Serialize(JumpData.Factions));
            SystemHandler.UpdateSystem(user, API_System);
        }
    }
}

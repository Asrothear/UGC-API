using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UGC_API.Config;
using UGC_API.Database_Models;
using UGC_API.Functions;
using UGC_API.Models.v1_0.Events;

namespace UGC_API.Handler.v1_0
{
    internal class QLSHandler
    {
        internal static string Event { get; set; }
        internal static DateTime TimeStamp { get; set; }
        internal static JObject QLSData { get; set; }
        internal static DB_User user { get; set; }
        internal static void Startup(object s)
        {
            if (s == null) return;
            QLSData = JObject.Parse(s.ToString().Replace("&", "and").Replace("'", ""));
            string UUID = QLSData["ugc_token_v2"]["uuid"].ToString().Replace(@":", @"cd_").Replace(@"\\", @":").Replace("/", "").Replace("|", "_");
            string Token = QLSData["ugc_token_v2"]["token"].ToString();
            string verify = QLSData["ugc_token_v2"]?["verify"]?.Value<string>() ?? "";
            if ((!User.ExistUser(UUID)) && VerifyToken.ExistToken(verify)) User.CreateUserAccount(UUID, Token, verify);
            if (!User.CheckTokenHash(UUID, Token)) return;
            Event = QLSData["event"]?.Value<string>() ?? "";
            if (!Filter(Event)) return;
            user = User._Users.FirstOrDefault(u => u.uuid == UUID);
            if (user == null) return;
            user.user = QLSData["user"]?.Value<string>() ?? "";
            TimeStamp = QLSData["timestamp"].Value<DateTime>();
            user.version_plugin_major = QLSData["ugc_p_version"]?.Value<double?>() ?? 0;
            user.version_plugin_minor = QLSData["ugc_p_minor"]?.Value<int?>() ?? 0;
            user.branch = QLSData["ugc_p_branch"]?.Value<string>() ?? "";
            Run(s.ToString().Replace("&", "and").Replace("'", ""));
        }
        internal static bool Filter(string evt)
        {
            return Configs.Events?.Contains(evt) ?? false;
        }
        internal static void Run(string v)
        {
            var index = Event;
            if (Event.Contains("Carrier")) index = "Carrier";
            if (Event.Contains("Mission")) index = "Mission";
            switch (index)
            {
                case "FSDJump":
                    SystemHandler.LoadSystems();
                    DockingHandler.UnDocked();
                    JumpHandler.FSDJump(JsonSerializer.Deserialize<FSDJump>(v));
                    break;
                case "Location":
                    LocationHandler.UserSetLocation(JsonSerializer.Deserialize<Location>(v)?.StarPos, JsonSerializer.Deserialize<Location>(v)?.StarSystem);
                    break;
                case "Carrier":
                    CarrierHandler.UpdateCarrier();
                    break;
                case "Docked":
                    DockingHandler.Docked();
                    break;
                case "Undocked":
                    DockingHandler.UnDocked();
                    break;
                case "Mission":
                    MissionHandler.Init();
                    break;
                default:
                    BGSPointsHandler.Init();
                    break;
            }
        }
    }
}

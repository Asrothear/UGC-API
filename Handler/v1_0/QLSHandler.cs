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
        internal string Event { get; set; } = null;
        internal DateTime TimeStamp { get; set; }
        internal JObject QLSData { get; set; } = null;
        internal DB_User user { get; set; } = null;
        internal void Startup(object s)
        {
            if (s == null) return;
            QLSData = JObject.Parse(s.ToString().Replace("&", "and").Replace("'", ""));
            string UUID = QLSData["ugc_token_v2"]["uuid"].ToString().Replace(@":", @"cd_").Replace(@"\\", @":").Replace(@"\", @":").Replace("/", "").Replace("|", "_");
            string Token = QLSData["ugc_token_v2"]["token"].ToString();
            string verify = QLSData["ugc_token_v2"]?["verify"]?.Value<string>() ?? "";
            if ((!User.ExistUser(UUID)) && VerifyToken.ExistToken(verify)) User.CreateUserAccount(UUID, Token, verify);
            if (!User.CheckTokenHash(UUID, Token)) return;
            if (!Filter(QLSData["event"]?.Value<string>() ?? "")) return;
            Event = QLSData["event"]?.Value<string>() ?? "";
            user = User._Users.FirstOrDefault(u => u.uuid == UUID);
            if (user == null) return;
            user.user = QLSData["user"]?.Value<string>() ?? "";
            TimeStamp = QLSData["timestamp"]?.Value<DateTime>() ?? DateTime.Now;
            user.version_plugin_major = QLSData["ugc_p_version"]?.Value<double?>() ?? 0;
            user.version_plugin_minor = QLSData["ugc_p_minor"]?.Value<int?>() ?? 0;
            user.branch = QLSData["ugc_p_branch"]?.Value<string>() ?? "";
            Run(s.ToString().Replace("&", "and").Replace("'", ""));
        }
        internal bool Filter(string evt)
        {
            return Configs.Events?.Contains(evt) ?? false;
        }
        internal void Run(string v)
        {
            var index = Event;
            if (Event.Contains("Carrier")) index = "Carrier";
            if (Event.Contains("Mission")) index = "Mission";
            if (Event.Contains("Market")) index = "Market";
            LogHandler.Create(v, TimeStamp, user, Event);
            switch (index)
            {
                case "FSDJump":
                    SystemHandler.LoadSystems();
                    DockingHandler.UnDocked(user);
                    JumpHandler.FSDJump(JsonSerializer.Deserialize<FSDJump>(v), QLSData, TimeStamp, user);
                    break;
                case "Location":
                    LocationHandler.UserSetLocation(user, JsonSerializer.Deserialize<Location>(v)?.StarPos, JsonSerializer.Deserialize<Location>(v)?.StarSystem);
                    break;
                case "Carrier":
                    CarrierHandler.CarrierEvent(v, Event);
                    break;
                case "Docked":
                    DockingHandler.Docked(user, QLSData);
                    break;
                case "Undocked":
                    DockingHandler.UnDocked(user);
                    break;
                case "Mission":
                    MissionHandler.Init();
                    break;
                case "Market":
                    MarketHandler.MarketEvent(v, Event);
                    break;
                default:
                    BGSPointsHandler.Init();
                    break;
            }
        }
    }
}

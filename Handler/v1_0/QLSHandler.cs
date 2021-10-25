using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Config;
using UGC_API.Functions;

namespace UGC_API.Handler.v1_0
{
    internal class QLSHandler
    {
        internal static string UserName { get; set; }
        internal static string UUID { get; set; }
        internal static string Token { get; set; }
        internal static double? PVersion { get; set; }
        internal static int? PMinor { get; set; }
        internal static string PBranch { get; set; }
        internal static string Event { get; set; }
        internal static JObject QLSData { get; set; }
        internal static void Startup(object s)
        {
            QLSData = JObject.Parse(s.ToString().Replace("&", "and").Replace("'", ""));
            UUID = QLSData["ugc_token_v2"]["uuid"].ToString().Replace(@"\\", @":").Replace("/", "").Replace("|", "_");
            Token = QLSData["ugc_token_v2"]["token"].ToString();
            string verify = QLSData["ugc_token_v2"]?["verify"]?.Value<string>() ?? "";
            if ((!User.ExistUser(UUID)) && VerifyToken.ExistToken(verify)) User.CreateUserAccount(UUID, Token, verify);
            if (!User.CheckTokenHash(UUID, Token)) return;
            Event = QLSData["event"]?.Value<string>() ?? null;
            if (!Filter(Event)) return;
            UserName = QLSData["user"]?.Value<string>() ?? null;
            PVersion = QLSData["ugc_p_version"]?.Value<double?>() ?? null;
            PMinor = QLSData["ugc_p_minor"]?.Value<int?>() ?? null;
            PBranch = QLSData["ugc_p_branch"]?.Value<string>() ?? null;
            Run();
        }
        internal static bool Filter(string evt)
        {
            return Configs.Events?.Contains(evt) ?? false;
        }
        internal static void Run()
        {
            var index = Event;
            if (Event.Contains("Carrier")) index = "Carrier";
            if (Event.Contains("Mission")) index = "Mission";
            switch (index)
            {
                case "FSDJump":
                    DockingHandler.UnDocked();
                    JumpHandler.FSDJump();
                    break;
                case "Location":
                    LocationHandler.UserSetLocation();
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

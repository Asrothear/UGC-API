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
using UGC_API.Service;

namespace UGC_API.Handler.v1_0
{
    internal class QLSHandler
    {
        internal string Event { get; set; } = null;
        internal DateTime TimeStamp { get; set; }
        internal JObject QLSData { get; set; } = null;
        internal DB_User user { get; set; } = null;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        internal void Startup(object s)
        {
            if (s == null) { return; }
            QLSData = JObject.Parse(s.ToString());
            string UUID = User.CreateUUID(QLSData["ugc_token_v2"]["uuid"].ToString());
            if (UUID == "none" || UUID == "" || UUID == " " || UUID == null) {return; }
            string Token = QLSData["ugc_token_v2"]["token"].ToString();
            if (Token == "none" || Token == "" || Token == " " || Token == null) {return; }
            string verify = QLSData["ugc_token_v2"]?["verify"]?.Value<string>() ?? "";
            if ((!User.ExistUser(UUID)) && VerifyToken.ExistToken(verify)) User.CreateUserAccount(UUID, Token, verify);
            if (!User.CheckTokenHash(UUID, Token)) { return; }
            var Logg = new LogHandler();
            Event = QLSData["event"]?.Value<string>() ?? "";
            user = User.GetUser(UUID);
            if (user == null) return;
            if (!Filter(QLSData["event"]?.Value<string>() ?? ""))
            {
                return;
            }
            user.user = QLSData["user"]?.Value<string>() ?? "";
            TimeStamp = QLSData["timestamp"]?.Value<DateTime>() ?? DateTime.Now;
            user.version_plugin_major = QLSData["ugc_p_version"]?.Value<double?>() ?? 0;
            user.version_plugin_minor = QLSData["ugc_p_minor"]?.Value<int?>() ?? 0;
            user.branch = QLSData["ugc_p_branch"]?.Value<string>() ?? "";
            user.last_data_insert = GetTime.DateNow();
            logger.Info($"QLS {user.id}");
            string ss = s.ToString().Replace("&", "and").Replace("'", "");
            Logg.Create(ss, TimeStamp, user, Event);
            try
            {
                Run(ss);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                try
                {
                    LogErrorToDB.Add(ss, ex, TimeStamp, user, Event);
                }
                catch (Exception exx)
                {
                    logger.Error(exx);
                }
            }
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
            if (Event.Contains("Sell") && Event.Contains("Data")) index = "ExploData";
            switch (index)
            {
                case "LoadGame":
                    Localisation.SetUserLang(JsonSerializer.Deserialize<LoadGame>(v), user);
                    break;
                case "FSDJump":
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
                    MissionHandler.MissionEvent(v, Event, user);
                    break;
                case "Market":
                    MarketHandler.MarketEvent(v, Event, user);
                    break;
                case "ExploData":
                    ExplorerHandler.SellEvent(v, Event, user);
                    break;
                default:
                    BGSPointsHandler.Init();
                    break;
            }
        }
    }
}

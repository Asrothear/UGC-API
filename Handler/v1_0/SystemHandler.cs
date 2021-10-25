using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace UGC_WebAPI_v2.Controllers.qls
{
    public class SystemHandler
    {
        public static string SysID;
        public static async void NewDay(string system)
        {/*
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://elitebgs.app/api/ebgs/v5/systems?factionDetails=true&name={UrlEncode(system)}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);
            json = (JObject)json["docs"][0];
            SysID = json["eddb_id"].ToString();
            foreach(var Data in json["factions"])
            {
                SystemSingleData Faction = new SystemSingleData();
                Faction.Name = Data["name"].ToString();
                Faction.State = Data["faction_details"]["faction_presence"]["state"].ToString();
                Faction.Government = Data["faction_details"]["government"].ToString();
                Faction.Influence = Data["faction_details"]["faction_presence"]["influence"].ToString();
                Faction.Allegiance = Data["faction_details"]["allegiance"].ToString();
                Faction.Happiness = Data["faction_details"]["faction_presence"]["happiness"].ToString();
                Faction.ActiveState = "";
                Faction.PendingState = "";
                Faction.RecoveringState = "";
                if (Data["faction_details"]["faction_presence"]["active_states"] != null)
                {
                    Faction.ActiveState = Data["faction_details"]["faction_presence"]["active_states"].ToString().Replace("\"", "").Replace("\n", "").Replace("\r", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("State:", "").Replace("{", "").Replace("}", "");
                }
                if (Data["faction_details"]["faction_presence"]["pending_states"] != null)
                {
                    Faction.PendingState = Data["faction_details"]["faction_presence"]["pending_states"].ToString().Replace("\"", "").Replace("\n", "").Replace("\r", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("State:", "").Replace("{", "").Replace("}", "");
                }
                if (Data["faction_details"]["faction_presence"]["recovering_states"] != null)
                {
                    Faction.RecoveringState = Data["faction_details"]["faction_presence"]["recovering_states"].ToString().Replace("\"", "").Replace("\n", "").Replace("\r", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("State:", "").Replace("{", "").Replace("}", "");
                }
                DBSystemHandler.InstertFaction(Faction);
            }*/
        }

        static string UrlEncode(string url)
        {
            Dictionary<string, string> toBeEncoded = new Dictionary<string, string>() { { "%", "%25" }, { "!", "%21" }, { "#", "%23" }, { " ", "%20" },
            { "$", "%24" }, { "&", "%26" }, { "'", "%27" }, { "(", "%28" }, { ")", "%29" }, { "*", "%2A" }, { "+", "%2B" }, { ",", "%2C" },
            { "/", "%2F" }, { ":", "%3A" }, { ";", "%3B" }, { "=", "%3D" }, { "?", "%3F" }, { "@", "%40" }, { "[", "%5B" }, { "]", "%5D" } };
            Regex replaceRegex = new Regex(@"[%!# $&'()*+,/:;=?@\[\]]");
            MatchEvaluator matchEval = match => toBeEncoded[match.Value];
            string encoded = replaceRegex.Replace(url, matchEval);
            return encoded;
        }
    }

}
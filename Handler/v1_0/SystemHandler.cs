using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;
using UGC_API.Models.v1_0;

namespace UGC_API.Handler.v1_0
{
    public class SystemHandler
    {
        internal static List<SystemModel> _Systeme = new();
        internal static void LoadSystems(bool force = false)
        {
            if (_Systeme.Count != 0 && !force) return;
            _Systeme = new();
            if (force) Systems.LoadFromDB();
            _Systeme = ParseSystem(Systems._Systeme);
        }

        private static List<SystemModel> ParseSystem(List<DB_Systeme> db_Systeme)
        {
            List<SystemModel> API_Systeme = new();
            foreach (var DB_System in db_Systeme)
            {
                var SystemData = new SystemModel
                {
                    id = DB_System.id,
                    Timestamp = DB_System.Timestamp,
                    last_update = DB_System.last_update,
                    User_ID = DB_System.User_ID,
                    System_ID = DB_System.System_ID,
                    System_Name = DB_System.System_Name
                };
                API_Systeme.Add(SystemData);
            }
            return API_Systeme;
        }

        private static List<SystemModel.FactionsL> ConvertToFactions(string factions)
        {
            List <SystemModel.FactionsL> OBJ = new();
            if (string.IsNullOrEmpty(factions) || factions.Length <= 4) return OBJ;
            OBJ = JsonSerializer.Deserialize<List<SystemModel.FactionsL>>(factions);
            return OBJ;
        }

        internal static int CountAllData()
        {
            return _Systeme.Count();
        }
        internal static SystemModel GetSystem(DB_User user, string SystemName, DateTime time)
        {
            var sys = _Systeme.FirstOrDefault(u => u.System_Name == SystemName && u.Timestamp == time);
            if (sys == null) CreateSystemDayEntry(user, time);
            return sys;
        }
        internal static void CreateSystemDayEntry(DB_User user, DateTime time)
        {
            SystemModel newSystemEntry = new SystemModel
            {
                Timestamp = time,
                last_update = DateTime.Now,
                System_ID = JumpHandler.JumpData.SystemAddress,
                System_Name = JumpHandler.JumpData.StarSystem
            };
            foreach (var factions in JumpHandler.JumpData.Factions)
            {
                var faction = new SystemModel.FactionsL
                {
                    Name = factions.Name,
                    FactionState = factions.FactionState,
                    Government = factions.Government,
                    Influence = factions.Influence,
                    Allegiance = factions.Allegiance,
                    Happiness = factions.Happiness,
                    Happiness_Localised = factions.Happiness_Localised
                };
                if (factions.ActiveStates != null)
                {
                    foreach (var states in factions.ActiveStates)
                    {
                        var state = new SystemModel.FactionsL.ActiveStatesL
                        {
                            State = states.State
                        };
                        faction.ActiveStates.Add(state);
                    }
                }
                newSystemEntry.Factions.Add(faction);
            }
            _Systeme.Add(newSystemEntry);
            UpdateSystem(user, newSystemEntry);
            LoadSystems(true);
            return;
        }

        public static void UpdateSystem(DB_User user, Models.v1_0.SystemModel SystemEntry)
        {
            var UpdateEntry = Systems._Systeme.FirstOrDefault(sy => sy.Timestamp == SystemEntry.Timestamp && sy.System_Name == SystemEntry.System_Name);
            if (UpdateEntry == null) UpdateEntry = new();
            UpdateEntry.Timestamp = SystemEntry.Timestamp;
            UpdateEntry.last_update = SystemEntry.last_update;
            UpdateEntry.User_ID = user.id;
            UpdateEntry.System_ID = SystemEntry.System_ID;
            UpdateEntry.System_Name = SystemEntry.System_Name;
            UpdateEntry.Factions = JsonSerializer.Serialize(SystemEntry.Factions);
            
            if (SystemEntry == null)
            {
                return;
            }
            DatabaseHandler.db.DB_Systemes.Update(UpdateEntry);
            DatabaseHandler.db.SaveChanges();
        }


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
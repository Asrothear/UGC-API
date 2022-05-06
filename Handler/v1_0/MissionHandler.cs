
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using UGC_API.Database;
using UGC_API.Models.v1_0.Events;
using UGC_API.Service;

namespace UGC_API.Handler.v1_0
{
    public class MissionHandler
    {
        public static List<MissionsModel> _Missions = new();
        private static bool UpdateRuning;
        internal static void LoadMissions(bool force = false)
        {
            if (!UpdateRuning)
            {
                using (DBContext db = new())
                {
                    UpdateRuning = true;
                    if (_Missions.Count != 0 && !force) return;
                    _Missions = new();
                    if (force) _Missions = new List<MissionsModel>(db.Missions);
                    UpdateRuning = false;
                    Service.LoggingService.schreibeLogZeile($"{_Missions.Count} Mission´s geladen.");
                }
            }
        }
        public static void MissionEvent(string json, string @event, Database_Models.DB_User user)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            MissionsModel newMission = JsonSerializer.Deserialize<MissionsModel>(json);
            newMission.Event = @event;
            newMission.CMDr = user.id;
            var Data = JObject.Parse(json);
            Data.Remove("Name");
            Data.Remove("event");
            Data.Remove("ugc_token_v2");
            Data.Remove("user");
            Data.Remove("MissionID");
            Data.Remove("ugc_p_minor");
            Data.Remove("ugc_p_branch");
            Data.Remove("ugc_p_version");
            newMission.JSON = Data.ToString();
            MissionsModel oldMission = _Missions.Find(x => x.MissionID == newMission.MissionID && x.Event == newMission.Event);
            if (oldMission != null) return;
            _Missions.Add(newMission);
            Task.Run(() =>
            {
                try
                {
                    DatabaseHandler.db.Missions.Add(newMission);
                    DatabaseHandler.db.SaveChanges();
                }catch (Exception ex) { }
            });
            watch.Stop();
            LoggingService.schreibeLogZeile($"MissionHandler Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
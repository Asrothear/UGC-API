using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UGC_API.Database;
using UGC_API.Database_Models;

namespace UGC_API.Handler.v1_0
{
    public class LogHandler
    {
        public static void Create(string logtext)
        {
            var Data = JObject.Parse(logtext);
            Data.Remove("ugc_token_v2");
            DB_Log NewLog = new DB_Log
            {
                Timestamp = QLSHandler.TimeStamp,
                User = QLSHandler.user.id,
                Event = QLSHandler.Event,
                JSON = Data.ToString()                
            };
            var ss = String.Format("{0:0.0#}", QLSHandler.user.version_plugin_major);
            NewLog.version_plugin = $"{ss},{QLSHandler.user.version_plugin_minor} {QLSHandler.user.branch}";
            using (DBContext db = new DBContext())
            {
                db.DB_Log.Update(NewLog);
                db.SaveChanges();
            }
        }
    }
}
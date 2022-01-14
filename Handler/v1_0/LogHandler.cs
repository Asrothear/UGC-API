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
        public static void Create(string logtext, DateTime timeStamp, DB_User user, string @event)
        {
            var Data = JObject.Parse(logtext);
            Data.Remove("ugc_token_v2");
            DB_Log NewLog = new DB_Log
            {
                Timestamp = timeStamp,
                User = user.id,
                Event = @event,
                JSON = Data.ToString()                
            };
            var ss = String.Format("{0:0.0#}", user.version_plugin_major);
            NewLog.version_plugin = $"{ss},{user.version_plugin_minor} {user.branch}";
            DatabaseHandler.db.DB_Log.Update(NewLog);
            DatabaseHandler.db.SaveChanges();            
        }
    }
}
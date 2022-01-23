using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UGC_API.Database;
using UGC_API.Database_Models;

namespace UGC_API.Handler.v1_0
{
    public class LogHandler
    {
        public async void Create(string logtext, DateTime timeStamp, DB_User user, string @event)
        {
            Task.Delay(500).Wait();
            var Data = JObject.Parse(logtext);
            Data.Remove("ugc_token_v2");
            Data.Remove("user");
            DB_Log NewLog = new DB_Log
            {
                Timestamp = timeStamp,
                User = user.id,
                Event = @event,
                JSON = Data.ToString()                
            };
            var ss = String.Format("{0:0.0#}", user.version_plugin_major);
            NewLog.version_plugin = $"{ss},{user.version_plugin_minor} {user.branch}";
            using (DBContext db = new())
            {
                db.DB_Log.Add(NewLog);
                db.SaveChanges();
                db.Dispose();

            }
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Functions;

namespace UGC_API.Handler.v1_0
{
    public class QLSHandler
    {
        public static string Event { get; set; }
        public static void Startup(object s)
        {
            JObject data = JObject.Parse(s.ToString().Replace("&", "and").Replace("'", ""));
            var uuid = data["ugc_token_v2"]["uuid"].ToString().Replace(@"\\", @":").Replace("/", "").Replace("|", "_");
            var token = data["ugc_token_v2"]["token"].ToString();
            string verify = data["ugc_token_v2"]?["verify"]?.Value<string>() ?? null;
            if (verify != null) User.CreateUserAccount(uuid, token, verify);
            if (!User.CheckTokenHash(uuid, token)) return;
            if (!Filter(data["event"]?.Value<string>() ?? null)) return;
        }
        public static bool Filter(string evt)
        {
            return Config.Configs.Events?.Contains(evt) ?? false;
        }
    }
}

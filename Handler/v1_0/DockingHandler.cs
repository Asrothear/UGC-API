using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UGC_API.Functions;

namespace UGC_API.Handler.v1_0
{
    public class DockingHandler
    {
        public static void Docked(Database_Models.DB_User user, Newtonsoft.Json.Linq.JObject qLSData)
        {
            User.Docked(user.uuid, qLSData);
        }
        public static void UnDocked(Database_Models.DB_User user)
        {
            User.UnDocked(user.uuid);
        }
    }
}
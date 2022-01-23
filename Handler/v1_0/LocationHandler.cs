using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;
using UGC_API.Functions;
using UGC_API.Models.v1_0.Events;

namespace UGC_API.Handler.v1_0
{
    public class LocationHandler
    {
        public static void UserSetLocation(Database_Models.DB_User user, double[] starPos, string starSystem)
        {
            user.last_pos = JsonSerializer.Serialize(starPos);
            user.system = JsonSerializer.Serialize(starSystem);
            User.UpdateUser(user.uuid);
        }        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;
using UGC_API.Functions;

namespace UGC_API.Handler.v1_0
{
    public class LocationHandler
    {
        public static void UserSetLocation(double[] starPos, string starSystem)
        {
            QLSHandler.user.last_pos = JsonSerializer.Serialize(starPos);
            QLSHandler.user.system = JsonSerializer.Serialize(starSystem);
            User.UpdateUser(QLSHandler.user.uuid);
        }
    }
}
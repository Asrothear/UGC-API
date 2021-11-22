using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UGC_API.Functions;

namespace UGC_API.Handler.v1_0
{
    public class DockingHandler
    {
        public static void Docked()
        {
            User.Docked(QLSHandler.user.uuid, QLSHandler.QLSData);
        }
        public static void UnDocked()
        {
            User.UnDocked(QLSHandler.user.uuid);
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UGC_API.Config;
using UGC_API.Functions;
using UGC_API.Models.v1_0;

namespace UGC_API.Handler.v1_0
{
    public class StateHandler
    {
        private static bool advanced = false;
        internal static string[] state(StateModel stateModel)
        {
            if (!User.CheckTokenHash(stateModel.UUID, stateModel.Token) || !stateModel.Visible)
            {
                advanced = false;
            }
            List<string> Systems_out = new();
            foreach (var sys in Configs.Systems)
            {
                Systems_out.Add(sys);
            }
            SystemHandler.LoadSystems();
            //Liste aller aktuellen systeme
            List<SystemModel> Systems = SystemHandler._Systeme.Where(s => s.last_update.Month == DateTime.Now.Month && s.last_update.Day == DateTime.Now.Day).ToList();

            //Alle Systeme sind Aktuell
            if (Systems.Count == Systems_out.Count)
            {
                Systems_out = new();
                Systems_out.Add("Alles Aktuell!");
                return Systems_out.ToArray();
            }
            //Entferne Systeme aus der Liste die Aktuell sind

            foreach(var sys in Systems)
            {
                Systems_out.Remove(sys.System_Name);
            }

            return Systems_out.ToArray();
        }
        internal static string[] returnn()
        {
            return new string[0];
        }
    }
}

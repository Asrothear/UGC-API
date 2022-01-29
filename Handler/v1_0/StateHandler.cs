using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UGC_API.Config;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;
using UGC_API.Models.v1_0;
using UGC_API.Service;

namespace UGC_API.Handler.v1_0
{
    public class StateHandler
    {
        private static bool advanced = false;
        internal static string[] state(StateModel stateModel)
        {
            DB_User user = new();
            List<string> Systems_out = new();
            using (DBContext db = new())
            {
                if (Configs.Plugin == null) Configs.Plugin = new List<DB_Plugin>(db.Plugin);
            }
            if (stateModel.UUID != null || stateModel.Token != null)
            {
                if (!User.CheckTokenHash(stateModel.UUID, stateModel.Token))
                {
                    Systems_out.Add("!! CMDr-Daten Unbekannt !!");
                    return Systems_out.ToArray();
                }
                else
                {
                    user = User.GetUser(stateModel.UUID);
                    if (user != null)
                    {
                        advanced = true;
                    }
                }
            }
            if(stateModel.Version < Configs.Plugin.First().min_version)
            {
                Systems_out.Add("!! Plugin Outdated !!");
                return Systems_out.ToArray();
            }
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
            //Array.Sort(b.ToArray(), c);
            if (advanced)
            {
                //[41.9375,-155.6875,16.3125]
                /*
                var pos_array = user.last_pos.Replace("[","").Replace("]","").Split(',');
                double u_x = Convert.ToDouble(pos_array[0]);
                double u_y = Convert.ToDouble(pos_array[1]);
                double u_z = Convert.ToDouble(pos_array[2]);
                foreach(var System in Systems_out)
                {
                    //System
                */
                return Systems_out.ToArray();
            }
            else
            {
                return Systems_out.ToArray();
            }
        }
        internal static string[] returnn()
        {
            return new string[0];
        }
    }
}

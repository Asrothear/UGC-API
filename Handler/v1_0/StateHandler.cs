using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        internal static List<string> Systems_out = new();
        public class SystemDistance
        {
            public string Name { get; set; }
            public double Distance { get; set; }
        }
        private bool advanced = false;
        internal string[] state(StateModel stateModel)
        {
            DB_User user = new();
            List<double> Pos_out = new();
            using (DBContext db = new())
            {
                if (Configs.Plugin == null) Configs.Plugin = new List<DB_Plugin>(db.Plugin);
            }
            if (!string.IsNullOrWhiteSpace(stateModel.UUID)|| !string.IsNullOrWhiteSpace(stateModel.Token))
            {
                if (!User.CheckTokenHash(stateModel.UUID, stateModel.Token))
                {
                    List<string> no = new();
                    no.Add("!! CMDr-Daten Unbekannt !!");
                    return no.ToArray();
                }
                else
                {
                    user = User.GetUser(stateModel.UUID);
                    logger.Info($"State-API {user.id}");
                    if (user != null && stateModel.Visible)
                    {
                        advanced = true;
                    }
                    else
                    {
                        advanced = false;
                    }
                }
            }
            if(stateModel.Version < Configs.Plugin.First().min_version)
            {
                List<string> no = new();
                no.Add("!! Plugin Outdated !!");
                return no.ToArray();
            }            
            if (stateModel.onlyBGS)
            {
                List<string> Filter = new();
                foreach (string Sys in Systems_out)
                {
                    if (Sys.Contains("~")) continue;
                    Filter.Add(Sys);
                }
                if (Filter.Count == 0)
                {
                    List<string> ss = new();
                    ss.Add("Alles Aktuell!");
                    return ss.ToArray();
                }
                Systems_out = Filter;
            }
            //Berechne Distanz zum CMDr
            if (advanced)
            {
                string[] pos_array = null;
                try
                {
                    pos_array = user.last_pos.Replace("[", "").Replace("]", "").Split(',');
                }catch(Exception e)
                {
                    return Systems_out.ToArray();
                }
                if (pos_array.Length != 3) return Systems_out.ToArray();
                double u_x = double.Parse(pos_array[0], CultureInfo.InvariantCulture);
                double u_y = double.Parse(pos_array[1], CultureInfo.InvariantCulture);
                double u_z = double.Parse(pos_array[2], CultureInfo.InvariantCulture);
                List<SystemDistance> _syst = new();
                foreach (var System in Systems_out)
                {
                    DB_SystemData SystemData = Systems._SystemData.FirstOrDefault(sy => sy.StarSystem.ToLower() == System.Replace("~", "").ToLower());
                    if (SystemData == null) continue;
                    SystemDistance _systData = new();
                    var star_pos = SystemData.StarPos.Replace("[", "").Replace("]", "").Split(',');
                    double s_x = double.Parse(star_pos[0], CultureInfo.InvariantCulture);
                    double s_y = double.Parse(star_pos[1], CultureInfo.InvariantCulture);
                    double s_z = double.Parse(star_pos[2], CultureInfo.InvariantCulture);
                    var dist = Math.Round(Math.Sqrt(Math.Pow(u_x - s_x, 2) + Math.Pow(u_y - s_y, 2) + Math.Pow(u_z - s_z, 2)), 2);
                    _systData.Name = $"{System} : {dist} ly";
                    _systData.Distance = dist;
                    _syst.Add(_systData);
                }
                //Ausgabe der Erweiterten System-Liste *EOE
                var sortet = _syst.OrderBy(y => y.Distance).ToList().Select(r => r.Name).ToArray();
                if (sortet.Length < 1)
                {
                    List<string> list = new List<string>();
                    list.Add("Alles Aktuell!");
                    sortet = list.ToArray();
                };
                return sortet;
            }
            else
            {
                //Ausgabe der Normalen-Systemliste *EOE
                return Systems_out.ToArray();
            }
        }
    }
}

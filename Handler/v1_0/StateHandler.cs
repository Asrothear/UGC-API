﻿using Newtonsoft.Json.Linq;
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
        public class SystemDistance
        {
            public string Name { get; set; }
            public double Distance { get; set; }
        }
        private bool advanced = false;
        internal string[] state(StateModel stateModel)
        {
            DB_User user = new();
            List<string> Systems_out = new();
            List<double> Pos_out = new();
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
            List<SystemModel> HSystems = SystemHandler._Systeme.Where(s => s.last_update.Month == DateTime.Now.Month && s.last_update.Day == DateTime.Now.Day).ToList();

            //Alle Systeme sind Aktuell
            if (HSystems.Count == Systems_out.Count)
            {
                Systems_out = new();
                Systems_out.Add("Alles Aktuell!");
                return Systems_out.ToArray();
            }
            //Entferne Systeme aus der Liste die Aktuell sind

            foreach(var sys in HSystems)
            {
                Systems_out.Remove(sys.System_Name);
            }
            //Array.Sort(b.ToArray(), c);
            if (advanced)
            {
                string[] pos_array = null;
                //[41.9375,-155.6875,16.3125]
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
                    DB_SystemData SystemData = Systems._SystemData.FirstOrDefault(sy => sy.starSystem.ToLower() == System.ToLower());
                    if (SystemData == null) continue;
                    SystemDistance _systData = new();
                    var star_pos = SystemData.starPos.Replace("[", "").Replace("]", "").Split(',');
                    double s_x = double.Parse(star_pos[0], CultureInfo.InvariantCulture);
                    double s_y = double.Parse(star_pos[1], CultureInfo.InvariantCulture);
                    double s_z = double.Parse(star_pos[2], CultureInfo.InvariantCulture);
                    var dist = Math.Round(Math.Sqrt(Math.Pow(u_x - s_x, 2) + Math.Pow(u_y - s_y, 2) + Math.Pow(u_z - s_z, 2)), 2);
                    _systData.Name = $"{System} : {dist} ly";
                    _systData.Distance = dist;
                    _syst.Add(_systData);
                }
                return _syst.OrderBy(y => y.Distance).ToList().Select(r => r.Name).ToArray();
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
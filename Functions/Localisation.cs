using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;

namespace UGC_API.Functions
{
    public class Localisation
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static List<DB_Localisation> _Localisations = new();

        internal static void LoadFromDB()
        {
            using (DBContext db = new())
            {
                _Localisations = new List<DB_Localisation>(db.Localisation);
            }
        }
        internal static void LoadLocalisation(bool force = false)
        {
            if (_Localisations.Count != 0 && !force) return;
            if (force) LoadFromDB();
            logger.Info($"{_Localisations.Count} Localisation´s geladen.");
        }
        internal static void SetUserLang(Models.v1_0.Events.LoadGame loadGame, Database_Models.DB_User user)
        {
            switch (loadGame.language)
            {
                case "English/UK":
                    user.Language = "en";
                    break;
                case "German/DE":
                    user.Language = "de";
                    break;
            }            
            User.UpdateUser(user.uuid);
        }

        internal static void Fetch(string v, Database_Models.DB_User user)
        {
            if (string.IsNullOrWhiteSpace(user.Language)) return;
            LoadLocalisation();
            v = v.Replace("\"", "'");
            if (v.Contains("_Localised"))
            {
                var j2 = JArray.Parse(v);
                foreach (var j3 in j2)
                {
                    var j = JObject.FromObject(j3.ToObject<JObject>());
                    foreach (var item in j)
                    {
                        if (item.Key.Contains("_Localised"))
                        {
                            var filter = item.Key.Replace("_Localised", "");
                            var find = j[filter].ToString();
                            var exist = _Localisations.FirstOrDefault(l => l.Name == find);
                            if (exist == null)
                            {                                
                                var newLocalisation = new DB_Localisation
                                {
                                    Name = find,
                                    de = "",
                                    en = "",
                                };
                                setlocalisation(newLocalisation, item.Value.ToString(), user);
                                _Localisations.Add(newLocalisation);
                                exist = newLocalisation;
                            }
                            else
                            {
                                setlocalisation(exist, item.Value.ToString(), user);
                            }
                            using (DBContext db = new())
                            {
                                db.Localisation.Update(exist);
                                db.SaveChanges();
                                db.Dispose();
                            }
                        }
                    }
                }
            }
        }
        internal static string GetLocalisationString(string Name, bool en = false) 
        {
            LoadLocalisation();
            var find = _Localisations.FirstOrDefault(l => l.Name == Name);
            if (find == null) return null;
            if (!en)
            {
                return find.de;
            }
            else
            {
                return find.en;
            }
        }
        private static DB_Localisation setlocalisation(DB_Localisation loc,string value, Database_Models.DB_User user)
        {
            if (user.Language == "de")
            {
                loc.de = value;
            }
            else if (user.Language == "en")
            {
                loc.en = value;
            }
            return loc;
        }
    }
}

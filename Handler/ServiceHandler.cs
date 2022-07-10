using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Functions;
using UGC_API.Models.v1_0;

namespace UGC_API.Handler
{
    /// <summary>
    /// Functions to verify Thrid-Party Services such as Anweisungs-Plugin etc
    /// </summary>
    public class ServiceHandler
    {
        static List<DB_Service> _Sevice = new();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        internal static void LoadFromDB()
        {
            _Sevice = new List<DB_Service>(DatabaseHandler.db.Service);
        }
        internal static void LoadService(bool force = false)
        {
            if (_Sevice.Count != 0 && !force) return;
            if (force) LoadFromDB();
            logger.Info($"{_Sevice.Count} Sevice´s geladen.");
        }
        internal static DB_Service AddService(string Name)
        {
            var find = _Sevice.FirstOrDefault(x => x.name == Name);
            if (find != null) return find;
            var token = CryptHandler.HashPasword(Name);
            var newService = new DB_Service {
                name = Name,
                token = token,
                active = true,
                blocked = "[]"
            };
            _Sevice.Add(newService);
            DatabaseHandler.db.Service.Add(newService);
            Task.Run(() =>
            {
                try
                {
                    DatabaseHandler.db.SaveChanges();
                }catch(Exception ee)
                {
                    Debug.WriteLine(ee.ToString());
                }
            });
            return newService;
        }

        internal static int GetServiceID(string token)
        {
            var Serv = _Sevice.FirstOrDefault(x => x.token == token);
            if (Serv != null) return Serv.id;
            return 0;
        }

        internal static bool VerifyService(string token)
        {
            var Serv = _Sevice.FirstOrDefault(x => x.token == token);
            if (Serv != null) return true;
            return false;
        }
    }
}

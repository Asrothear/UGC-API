using UGC_API.Database;
using UGC_API.Database_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using UGC_API.Service;

namespace UGC_API.Functions
{
    class User
    {
        public static List<DB_User> _Users = new();
        public static string CreateUUID(string UUID)
        {
            if (UUID == null) return null;
            return UUID.Replace(@":", @"cd_").Replace(@"\\", @":").Replace(@"\", @":").Replace("/", "").Replace("|", "_").Replace("::",":");
        }
        public static void CreateUserAccount(string uuid, string token, string verify)
        {
            if (!VerifyToken.IsUsed(verify)) return;
            VerifyToken.TakeToken(verify);
            var UserData = new DB_User
            {
                uuid = uuid,
                token = CryptHandler.HashPasword(token),
            };
            _Users.Add(UserData);
            try
            {
                using (DBContext db = new())
                {
                    db.DB_Users.Update(UserData);                    
                    db.SaveChanges();
                    db.Dispose();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        public static DB_User GetUser (string UUID)
        {
            var user = _Users.FirstOrDefault(u => u.uuid == UUID);
            if (user == null) return null;
            return user;
        }
        public static bool CheckTokenHash(string uuid, string token)
        {
            var us = _Users.FirstOrDefault(u => u.uuid == uuid);

            if (us == null)
            {
                return false;
            }
            return CryptHandler.CheckPassword(token, us.token);
        }
        public static bool ExistUser(string uuid)
        {
            var us = _Users.FirstOrDefault(u => u.uuid == uuid);

            if (us == null)
            {
                return false;
            }
            return true;
        }
        public static void Docked(string uuid, JObject qls)
        {
            var us = _Users.FirstOrDefault(u => u.uuid == uuid);

            if (us == null)
            {
                return;
            }
            us.last_docked = us.docked;
            us.last_docked_faction = us.docked_faction;
            us.docked = qls["StationName"].ToString();
            us.docked_faction = qls["StationFaction"]["Name"].ToString();
            UpdateUser(uuid);
            return;
        }
        public static void UnDocked(string uuid)
        {
            var us = _Users.FirstOrDefault(u => u.uuid == uuid);

            if (us == null)
            {
                return;
            }
            us.last_docked = us.docked;
            us.last_docked_faction = us.docked_faction;
            us.docked = "";
            us.docked_faction = "";
            UpdateUser(uuid);
            return;
        }
        public static void UpdateUser(string uuid)
        {
            var us = _Users.FirstOrDefault(u => u.uuid == uuid);
            if (us == null)
            {
                return;
            }
            using (DBContext db = new())
            {
                db.DB_Users.Update(us);
                db.SaveChanges();
                db.Dispose();
            }
        }
    }
}

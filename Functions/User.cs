using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UGC_API.Models;

namespace UGC_API.Functions
{
    class User
    {
        public static List<DB_User> _Users = new();
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

                using (DBContext db = new DBContext())
                {
                    db.DB_Users.Update(UserData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
        
    }
}

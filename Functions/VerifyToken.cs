using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Service;

namespace UGC_API.Functions
{
    public class VerifyToken
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static List<DB_Verify_Token> _Verify_Token = new();
        public static void AddToken(string Token, ulong DCid, string DCname)
        {
            var TokenData = new DB_Verify_Token
            {
                token = Token,
                discord_id = DCid,
                discord_name = DCname,
                used = 0,
                max_usage = 2,
                created_time = DateTime.Now,
            };
            _Verify_Token.Add(TokenData);
            try
            {
                using (DBContext db = new())
                {
                    db.Verify_Token.Update(TokenData);
                    db.SaveChanges();
                    db.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                logger.Error(e);
            }
        }
        public static bool ExistToken(string Token)
        {
            var tk = _Verify_Token.FirstOrDefault(u => u.token == Token);

            if (tk != null)
            {
                return true;
            }
            return false;
        }
        public static bool ExistTokenDC(ulong DCid)
        {
            var tk = _Verify_Token.FirstOrDefault(u => u.discord_id == DCid);

            if (tk != null)
            {
                return true;
            }
            return false;
        }
        public static string GetToken(ulong DCid)
        {
            var tk = _Verify_Token.FirstOrDefault(u => u.discord_id == DCid);

            if (tk != null)
            {
                return tk.token;
            }
            return null;
        }
        public static bool IsUsed(string token)
        {
            var us = _Verify_Token.FirstOrDefault(u => u.token == token);

            if (us == null)
            {
                return false;
            }
            if(us.used < us.max_usage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void TakeToken(string token)
        {
            var us = _Verify_Token.FirstOrDefault(u => u.token == token);

            if ((us == null) || (us.used >= us.max_usage))
            {
                return;
            }
            us.used ++;
            us.used_time = DateTime.Now;
            using (DBContext db = new())
            {
                db.Verify_Token.Update(us);
                db.SaveChanges();
                db.Dispose();
            }
            return;
        }
    }
}

using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UGC_API.Model_Functions
{
    class User
    {
        public static List<User> Account = new List<User>();
        public static void CreateUserAccount(string username, string email, string password)
        {
            /*var UserData = new User
            {
                Name = username,
                EMail = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };*/

            try
            {
                //Account.Add(UserData);

                using (DBContext db = new DBContext())
                {
                    //db.Accounts.Add(UserData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static bool ExistUserName(string UserName)
        {
            /*var us = Account.FirstOrDefault(u => u.Name == UserName);

            if (us != null)
            {
                return true;
            }
            */
            return false;
        }
        public static string GetPassword(string UserName)
        {
            /*var us = Account.FirstOrDefault(u => u.Name == UserName);

            if (us != null)
            {
                return us.Password;
            }
            */
            return "!JSDs!JI!";
        }
    }
}

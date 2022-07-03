using System;
using UGC_API.Database;
using UGC_API.Database_Models;

namespace UGC_API.Service
{
    public class LogErrorToDB
    {
        public static void Add(string Json, Exception ex, DateTime timeStamp, DB_User user, string @event)
        {
            DB_Error newError = new DB_Error
            {
                Message = ex.Message,
                Error = ex.ToString(),
                Json = Json,
                TimeStamp = timeStamp,
                User = user.id,
                Event = @event
                
            };
            using (DBContext db = new())
            {
                db.DB_Error.Add(newError);
                db.SaveChanges();
                db.Dispose();

            }
        }
    }
}

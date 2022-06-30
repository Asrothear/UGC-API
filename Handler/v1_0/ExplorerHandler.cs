using System;
using System.Text.Json;
using UGC_API.Database;
using UGC_API.Database_Models;
using UGC_API.Service;

namespace UGC_API.Handler.v1_0
{
    public class ExplorerHandler
    {
        internal static void SellEvent(string v, string @event, DB_User user)
        {
            DB_Explorer newData = new();
            newData = JsonSerializer.Deserialize<DB_Explorer>(v);
            newData.User = user.id;
            newData.Timestamp = GetTime.DateNow();
            newData.DataType = "Systems";
            if(newData.BioData != null)
            {
                foreach (var bioData in newData.BioData)
                {
                    newData.TotalEarnings += bioData.Value;
                }
                newData.DataType = "BioData";
            }
            using (DBContext db = new())
            {
                db.DB_Explorer.Add(newData);
                db.SaveChanges();
            }
        }
    }
}

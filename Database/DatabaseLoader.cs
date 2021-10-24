using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using UGC_API.Config;

namespace UGC_API.Database
{
    class DatabaseLoader
    {
        public static void LoadDatabase()
        {
            DatabaseConfig.ReadDBConfig();
            DatabaseHandler.LoadData();
            System.Timers.Timer UpdateDataCacheTimer = new System.Timers.Timer();
            UpdateDataCacheTimer.Elapsed += new ElapsedEventHandler(DatabaseHandler.OnUpdateDataCacheTimer);
            UpdateDataCacheTimer.Interval += 5*(60*1000);
            UpdateDataCacheTimer.Enabled = true;
        }
    }
}

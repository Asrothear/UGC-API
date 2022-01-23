using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using UGC_API.Config;
using UGC_API.Service;

namespace UGC_API.Database
{
    class DatabaseLoader
    {
        public static void LoadDatabase()
        {
            Configs.ReadConfig();
            DatabaseHandler.LoadData();
            Timer UpdateDataCacheTimer = new();
            UpdateDataCacheTimer.Elapsed += new(DatabaseHandler.OnUpdateDataCacheTimer);
            UpdateDataCacheTimer.Interval += 5*(60*1000);
            UpdateDataCacheTimer.Enabled = true;
        }
    }
}

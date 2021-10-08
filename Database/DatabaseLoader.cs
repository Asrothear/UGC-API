using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UGC_API.Config;

namespace UGC_API.Database
{
    class DatabaseLoader
    {
        public static void LoadDatabase()
        {
            DatabaseConfig.ReadDBConfig();
            DatabaseHandler.LoadConfig();
        }
    }
}

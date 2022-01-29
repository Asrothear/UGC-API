using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Config;
using UGC_API.Database;
using UGC_API.Database_Models;

namespace UGC_API.Controllers.v1_0
{

    [ApiVersionNeutral]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PluginControll : Controller
    {
        [HttpGet]
        [ApiVersionNeutral]
        public Controll get()
        {
            Controll Plugin = new();
            using (DBContext db = new())
            {
                Configs.Plugin = new List<DB_Plugin>(db.Plugin);
                Plugin.force_url = Configs.Plugin.First().force_url;
                Plugin.force_update = Configs.Plugin.First().force_update;
            }
            return Plugin;
        }
    }
    public class Controll
    {
        public int force_url { get; set; }
        public int force_update { get; set; }
    }
}

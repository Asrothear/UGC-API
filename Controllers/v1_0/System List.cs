using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace UGC_API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class System_List : ControllerBase
    {
        // GET: api/<System_List>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Config.Configs.Systems;
        }
        [HttpGet("{syst}")]
        [MapToApiVersion("1.0")]
        public string Find(string syst) 
        {
            string system_find = syst;
            foreach (var sys in Config.Configs.Systems)
            {
                if (sys == system_find) return sys;
            }
            return  "Kein System gefunden";
        }
    }
}

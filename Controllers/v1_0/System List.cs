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
    }
}

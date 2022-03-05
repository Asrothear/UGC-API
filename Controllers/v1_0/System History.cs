using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Handler.v1_0;
using UGC_API.Models.v1_0;

namespace UGC_API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class System_History : Controller
    {
        // GET: System_History
        [HttpGet("{Name}")]
        public List<SystemModel> Index(string Name, [FromHeader] string token)
        {
            SystemHandler.LoadSystems();
            var sys = SystemHandler._Systeme.Where(sys => sys.System_Name == Name).ToList();
            if (sys == null) return new List<SystemModel>();
            return sys;
        }
    }
}

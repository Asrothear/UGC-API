using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UGC_API.Handler.v1_0;
using UGC_API.Models.v1_0;
using UGC_API.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UGC_API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class State : ControllerBase
    {
        // GET api/<ValuesController>/5
        [HttpGet]
        [MapToApiVersion("1.0")]
        public string[] Get([FromHeader] string version, [FromHeader] string br, [FromHeader] string branch, [FromHeader] string cmdr, [FromHeader] string uuid, [FromHeader] string token)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            StateModel stateModel = new StateModel
            {
                UUID = Functions.User.CreateUUID(uuid),
                Token = token,
                Visible = cmdr != "True" ? false : true,
                Version = Convert.ToDouble(version),
                Minor = Convert.ToInt32(br),
                Branch = branch
            };
            var StateHandler = new StateHandler();
            var ous = StateHandler.state(stateModel);
            watch.Stop();
            LoggingService.schreibeLogZeile($"StateHandler Execution Time: {watch.ElapsedMilliseconds} ms - {cmdr}\n{Functions.User._Users.Count}");
            return ous;
        }
    }
}

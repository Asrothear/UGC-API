using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UGC_API.Handler;
using UGC_API.Handler.v1_0;
using UGC_API.Models.v1_0.Events;

namespace UGC_API.Controllers.v1_0
{

    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Missions : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public List<MissionsModel> Get([FromHeader] string ServieID)
        {
            if (!ServiceHandler.VerifyService(ServieID))return new List<MissionsModel>();
            return MissionHandler._Missions;
        }
    }
}

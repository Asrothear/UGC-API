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
    public class Market : ControllerBase
    {
        [HttpGet("{Name}")]
        [MapToApiVersion("1.0")]
        public List<Models.v1_0.Events.Market> Get(string Name)
        {
            var Out = MarketHandler.GetMarket(Name);
            return Out;
        }
    }
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MarketFindItem : ControllerBase
    {
        [HttpGet("{Ware}")]
        [MapToApiVersion("1.0")]
        public List<MarketSearchModel> Get(string Ware)
        {
            List<MarketSearchModel> Out = MarketHandler.FindWare(Ware);
            return Out;
        }
    }
}

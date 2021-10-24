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
    public class CarrierMarket : ControllerBase
    {
        [HttpGet("{CallSign}")]
        [MapToApiVersion("1.0")]
        public List<CarrierModel.MarketModel> Get(string CallSign)
        {
            var Out = CarrierHandler.GetCarrierMarket(CallSign);
            return Out;
        }
    }
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CarrierFindItem : ControllerBase
    {
        [HttpGet("{Ware}")]
        [MapToApiVersion("1.0")]
        public List<CarrierModel.MarketSearchModel> Get(string Ware)
        {
            var Out = CarrierHandler.FindWare(Ware);
            return Out;
        }
    }
}

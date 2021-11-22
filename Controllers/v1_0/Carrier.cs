using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Functions;
using UGC_API.Handler.v1_0;
using UGC_API.Models.v1_0;


namespace UGC_API.Controllers.v1_0
{
    /// <summary>
    /// Lists all UGC Carrier.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Carrier : ControllerBase
    {
        // GET api/<Carrier>/
        [HttpGet]
        [MapToApiVersion("1.0")]
        public List<CarrierModel> Get()
        {
            CarrierHandler.LoadCarrier();
            return CarrierHandler._Carriers;
        }
        [HttpGet("{CallSign}")]
        [MapToApiVersion("1.0")]
        public CarrierModel Get(string CallSign)
        {            
            return CarrierHandler.GetCarrier(CallSign);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Models.v1_0;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UGC_API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Carrier : ControllerBase
    {
        // GET api/<Carrier>/5
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public CarrierModel Get(int id)
        {
            CarrierModel träger = new CarrierModel();
            return träger;
        }
    }
}

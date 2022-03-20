using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Functions;
using UGC_API.Service;
using UGC_API.Models.v1_0;
using UGC_API.Handler;

namespace UGC_API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class auth : ControllerBase
    {
        // GET api/<auth>/5
        [HttpGet]
        [MapToApiVersion("1.0")]
        public AuthModel Get([FromHeader] string token, [FromHeader] string service)
        {
            AuthModel auth = new();
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(service))
            {
                return auth = new();
            }
            if(ServiceHandler.VerifyService(service))
            {
                if (VerifyToken.ExistToken(token))
                {
                    auth = new AuthModel
                    {
                        id = ServiceHandler.GetServiceID(service),
                        response = new AuthModel.Response
                        {
                            Valid = true,
                            Cmdr = true,
                            Blocked = false,                            
                        },
                    };
                }
            }
            return auth;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGC_API.Database_Models;

namespace UGC_API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class Localisation : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public List<DB_Localisation> Get()
        {
            Functions.Localisation.LoadLocalisation();
            return Functions.Localisation._Localisations;
        }
    }
}

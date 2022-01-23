using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
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
    public class Tick : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]        
        public string[] Get()
        {
            List<TickModel> tick = new();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://elitebgs.app/api/ebgs/v5/ticks");
                tick = JsonSerializer.Deserialize<List<TickModel>>(json);
            }
            
            string[] outs = {$"{GetTime.DateNow(tick.ElementAt(0).time)}" };
            return outs;
        }
    }
}

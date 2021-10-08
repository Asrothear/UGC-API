namespace UGC_API.Controllers.v1_0.QLS
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UGC_API.Models.v1_0;

    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class QLS_Example : ControllerBase
    {
        /// <summary>
        /// Gets the list of all QLS.
        /// </summary>
        /// <returns>The list of QLS.</returns>
        // GET: api/<QLS_Example>
        [HttpGet]
        [MapToApiVersion("1.0")]
        public int Get()
        {
            return Model_Functions.Systems.CountAllData();
        }

        // GET api/<QLS_Example>/5
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Creates an QLS request.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Employee
        ///     {        
        ///       "firstName": "Mike",
        ///       "lastName": "Andrew",
        ///       "emailId": "Mike.Andrew@gmail.com"        
        ///     }
        /// </remarks>
        /// <param name="value"></param>     
        /// <returns>A newly created QLS request.</returns>
        /// <response code="201">Returns the newly created QLS request.</response>
        /// <response code="400">If the QLS request. is null</response>  
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public void Post([FromBody] test value)
        {
            Console.WriteLine(value.ToString());
        }

        // PUT api/<QLS_Example>/5
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QLS_Example>/5
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public void Delete(int id)
        {
        }
    }
}

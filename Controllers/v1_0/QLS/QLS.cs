using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UGC_API.Handler.v1_0;

namespace UGC_API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class QLS : ControllerBase
    {
        /// <summary>
        /// Creates an QLS request.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     Post api/qls
        ///     {
        ///             "timestamp": "2021-10-01T17:35:12Z",
        ///             "event": "Docked",
        ///             "StationName": "Jensen Gateway",
        ///             "StationType": "Ocellus",
        ///             "StarSystem": "64 Ceti",
        ///             "SystemAddress": 800751339875,
        ///             "MarketID": 3223182848,
        ///             "StationFaction": {"Name": "The Wild Bunch", "FactionState": "Boom"},
        ///             "StationGovernment": "$government_Corporate;",
        ///             "StationGovernment_Localised": "Konzernpolitik",
        ///             "StationServices": ["dock", "autodock", "commodities", "contacts", "exploration", "missions", "outfitting", "crewlounge", "rearm", "refuel", "repair", "shipyard",
        ///                                 "tuning", "engineer", "missionsgenerated", "flightcontroller", "stationoperations", "powerplay", "searchrescue", "materialtrader", "stationMenu",
        ///                                 "shop", "voucherredemption"],
        ///             "StationEconomy": "$economy_Industrial;",
        ///             "StationEconomy_Localised": "Industrie",
        ///             "StationEconomies": [{"Name": "$economy_Industrial;", "Name_Localised": "Industrie", "Proportion": 0.77}, {"Name": "$economy_Extraction;", "Name_Localised": "Abbau",
        ///                                 "Proportion": 0.23}],
        ///             "DistFromStarLS": 474.464033,
        ///             "user": "John Doe",
        ///             "ugc_p_version": 2.1,
        ///             "ugc_p_minor": "5", 
        ///             "ugc_p_branch": "rel", 
        ///             "data_system": "64 Ceti",
        ///             "ugc_token_v2": [{"uuid": "58240B00-D7DA-11DD-8862-704D7B68A607", "token":"G4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ"}]}
        /// </remarks>
        /// /// <param name="value"></param>
        /// <returns>A newly created QLS request.</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        public void Post([FromBody] object value)
        {
            var s = JObject.Parse(value.ToString());
            Thread thread = new Thread(new ParameterizedThreadStart(QLSHandler.Startup));
            thread.Start(s);
        }
    }
}

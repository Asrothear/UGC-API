using Microsoft.AspNetCore.Mvc;
using System;
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
        public List<MissionsModel> Get([FromHeader] string ServieID, DateTime? Start, DateTime? End, ulong MissionID, string MissionName, string Type)
        {
            List<MissionsModel> _list = new List<MissionsModel>();
            if (!ServiceHandler.VerifyService(ServieID))return _list;
            _list = new List<MissionsModel>(MissionHandler._Missions);
            if (Start is not null){
                if (End is not null)
                {
                    _list = _list.FindAll(x => x.timestamp >= Start && x.timestamp <= End);
                }
                else
                {
                    _list = _list.FindAll(x => x.timestamp == Start);
                }                
            }
            if (MissionID > 0)
            {
                _list = _list.FindAll(x => x.MissionID == MissionID);
            }
            if (!string.IsNullOrWhiteSpace(MissionName))
            {
                _list = _list.FindAll(x => x.Name == MissionName);
            }
            if (!string.IsNullOrWhiteSpace(Type))
            {
                _list = _list.FindAll(x => x.Event == Type);
            }
            return _list;
        }
    }
}

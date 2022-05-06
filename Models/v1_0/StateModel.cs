using System;
namespace UGC_API.Models.v1_0
{
    public class StateModel
    {
        //Get([FromHeader] string version, [FromHeader] string br, [FromHeader] string branch, [FromHeader] string cmdr, [FromHeader] string uuid, [FromHeader] string token)
        public string UUID { get; set; }
        public string Token { get; set; }
        public bool Visible { get; set; }
        public bool onlyBGS { get; set; }
        public double Version { get; set; }
        public int Minor { get; set; }
        public string Branch { get; set; }

    }
}

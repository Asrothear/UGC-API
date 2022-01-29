using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UGC_API.EDDN
{
    internal class EDDNWorker
    {
        internal void WorkerThread(JObject resObjJson)
        {
            var InternalData = resObjJson["message"]?.Value<JObject>() ?? null;
            if (InternalData == null) return;
            if (InternalData.ContainsKey("event"))
            {
                // Has Envent, try to Parse Data
                switch (InternalData["event"].ToString())
                {
                    case "FSDJump":
                        break;

                }
            }
            else if (InternalData.ToString().Contains(""))
            {
                // Jobject has possible MArket Data, try to Parse Data
            }
        }
    }
}
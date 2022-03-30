
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UGC_API.Handler.v1_0
{
    public class MissionHandler
    {
        public static void MissionEvent(string json, string @event)
        {
            switch (@event)
            {
                case "MissionAccepted":
                    MissionAccepted();
                    break;
                case "MissionCompleted":
                    MissionCompleted();
                    break;
                case "MissionAbandoned":
                    MissionAbandoned();
                    break;
                case "MissionFailed":
                    MissionFailed();
                    break;
                default:
                    return;
            }

            static void MissionAccepted()
            {

            }
            static void MissionCompleted()
            {

            }
            static void MissionAbandoned()
            {

            }
            static void MissionFailed()
            {

            }
        }
    }
}
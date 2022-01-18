using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using Ionic.Zlib;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UGC_API.Service;

namespace UGC_API.Service
{
    public static class Eddn_Main
    {
        public async static void eddn_listener()
        {
            await Task.Run(new Action(() =>
            {
                Debug.Print("LISTENING EDDN");

                var utf8 = new UTF8Encoding();

                using (var client = new SubscriberSocket())
                {
                    client.Options.ReceiveHighWatermark = 1000;
                    client.Connect("tcp://eddn.edcd.io:9500");
                    client.SubscribeToAnyTopic();
                    while (true)
                    {
                        try
                        {
                            var bytes = client.ReceiveFrameBytes();
                            var uncompressed = ZlibStream.UncompressBuffer(bytes);
                            var result = utf8.GetString(uncompressed);
                            JObject resObjJson = JObject.Parse(result);
                        }
                        catch (Exception ex)
                        {
                            Debug.Print(ex.ToString());
                        }
                    }
                }
            }));
        }
    }
}
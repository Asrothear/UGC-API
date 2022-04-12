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
using UGC_API.Service;

namespace UGC_API.Functions
{
    public class Tick
    {
        public static string[] AktualTick { get; set; } = { "BGS-Timeout" };
        public static DateTime DateTimeTick { get; set; } = GetTime.DateNow();
        internal async void GetTick()
        {
            List<Models.v1_0.TickModel> tick = new();
            try
            {
                using HttpClient Client = new HttpClient();
                Client.Timeout = TimeSpan.FromSeconds(10);
                var response = await Client.GetAsync("https://elitebgs.app/api/ebgs/v5/ticks");
                var json = await response.Content.ReadAsStringAsync();
                tick = JsonSerializer.Deserialize<List<Models.v1_0.TickModel>>(json);
                DateTimeTick = GetTime.DateNow(tick.ElementAt(0).time);
                AktualTick[0] = $"{DateTimeTick}\n(~{DateTimeTick.AddHours(3)}(+3h)~)";
            }
            catch (Exception ex)
            {

            }
        }
    }
}

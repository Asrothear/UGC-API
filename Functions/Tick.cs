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
using UGC_API.Models.v1_0;
using UGC_API.Service;

namespace UGC_API.Functions
{
    public class Tick
    {
        public static string[] AktualTick { get; set; } = { "BGS-Timeout" };
        public static DateTime DateTimeTick { get; set; } = GetTime.DateNow();
        internal static List<TickModel> APITick { get; set; } = new();
        internal static bool Override = false;
        internal static int OverrideHours = 0;
        internal static int OverrideDay = 0;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        internal async void GetTick()
        {
            try
            {
                using HttpClient Client = new HttpClient();
                Client.Timeout = TimeSpan.FromSeconds(10);
                var response = await Client.GetAsync("https://elitebgs.app/api/ebgs/v5/ticks");
                var json = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<List<TickModel>>(json);
                if (!Override)
                {
                    APITick = temp;
                }
                else
                {
                    if (GetTime.DateNow(temp.ElementAt(0).time).Day > GetTime.DateNow().Day)
                    {
                        OverrideTick();
                        logger.Info($"TICK-OVERRIDE OverrideHours:{OverrideHours}, OverrideDay:{OverrideDay} : {GetTime.DateNow(APITick.ElementAt(0).time)}->{DateTimeTick}");
                    }
                }
                SetTick(APITick);
            }
            catch (Exception ex)
            {
                logger.Error($"GetTick-ERROR: Last Tick:{AktualTick[0]}\n{ex.Message}");
            }
        }
        internal void SetTick(List<TickModel> tick)
        {
            DateTimeTick = GetTime.DateNow(tick.ElementAt(0).time);
            if (Override) {
                DateTimeTick = DateTimeTick.AddHours(OverrideHours).AddDays(OverrideDay);
                logger.Info($"TICK-OVERRIDE OverrideHours:{OverrideHours}, OverrideDay:{OverrideDay} : {GetTime.DateNow(tick.ElementAt(0).time)}->{DateTimeTick}");
            }
            if (Override)
            {
                AktualTick[0] = $"*{DateTimeTick}*\n*(~{DateTimeTick.AddHours(3)}(+3h)~)*";
            }
            else
            {
                AktualTick[0] = $"{DateTimeTick}\n(~{DateTimeTick.AddHours(3)}(+3h)~)";
            }
            logger.Info($"GetTick-Success: {AktualTick[0]}");
        }
        /// <summary>
        /// Override Tick
        /// </summary>
        /// <param name="x">OverrideHours Int Hours to Add</param>
        /// <param name="y">OverrideDay Int Days to Add</param>
        /// <param name="z">Override Bool Toogle Override</param>
        internal static void OverrideTick(int x=0, int y=0, bool z=false)
        {
            var tt = new Tick();
            if (Override && !z)
            {
                Override = false;
                tt.GetTick();
                return;
            }
            Override = z;
            OverrideHours = x;
            OverrideDay = y;
            tt.GetTick();
        }
    }
}

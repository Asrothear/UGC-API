using System;
using System.Collections.Generic;
using System.Text;

namespace UGC_API.Service
{
    class GetTime
    {
        public static DateTime DateNow(DateTime? vars = null)
        {
            DateTime inpu;
            if(vars == null)
            {
                inpu = DateTime.UtcNow;
            }
            else
            {
                inpu = vars.Value;
            }
            try
            {//windows
                return TimeZoneInfo.ConvertTimeFromUtc(inpu, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
            } catch(Exception e)
            {//Linux
                return TimeZoneInfo.ConvertTimeFromUtc(inpu, TimeZoneInfo.FindSystemTimeZoneById("CET"));
            }
        }
    }
    
}

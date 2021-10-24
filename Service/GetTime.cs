using System;
using System.Collections.Generic;
using System.Text;

namespace UGC_API.Service
{
    class GetTime
    {
        public static DateTime DateNow()
        {
            try
            {//Linux
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("CET"));
            } catch(Exception e)
            {//windows
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
            }
        }
    }
    
}

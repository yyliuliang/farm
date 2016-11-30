using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GoldenFarm
{
    public static class Extensions
    {
        public static int ToTimestamp(this DateTime datetime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(datetime - startTime).TotalSeconds;
        }



        public static string UserIP(this HttpRequestBase request)
        {
            string userIP = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (userIP == null || userIP == "")
            {
                userIP = request.ServerVariables["REMOTE_ADDR"];
            }
            return userIP;
        }
    }
}

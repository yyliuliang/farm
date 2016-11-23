using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenFarm
{
    public static class Extensions
    {
        public static int ToTimestamp(this DateTime datetime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(datetime - startTime).TotalSeconds;
        }
    }
}

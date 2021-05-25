using Camino.Core.Constants;
using System;

namespace Camino.Core.Utils
{
    public static class DateTimeUtil
    {
        public static string ToDateHourMinusFormat(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormatConst.DateHourMinusFormat);
        }

        public static string ToDateHourMinusFormat(this DateTimeOffset dateTime)
        {
            return dateTime.ToString(DateTimeFormatConst.DateHourMinusFormat);
        }
    }
}

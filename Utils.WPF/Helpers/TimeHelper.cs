using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.WPF.Helpers
{
    public static class TimeHelper
    {
        public static string DisplayUTCTime(this DateTime date)
        {
            var localTime = date.ToLocalTime();

            return localTime.ToString("dd/MM HH:mm");
        }
    }
}

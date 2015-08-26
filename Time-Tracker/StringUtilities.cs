using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Tracker
{
    class StringUtilities
    {
        internal static string formatTimeText(int minutes)
        {
            string answer = "";
            if (minutes >= 60)
            {
                int hours = minutes / 60;
                minutes = minutes % 60;
                if (hours >= 8)
                {
                    int workDays = hours / 8;
                    hours = hours % 8;
                    answer += workDays + "d ";
                }
                if (hours > 0)
                    answer += hours + "h ";
            }
            if (minutes > 0)
                answer += minutes + "m ";
            return answer.Trim();
        }

        internal static int minutesSinceMidnight(string time)
        {
            int minutes = 0;
            if (time.Contains("PM"))
                minutes = 720;
            string[] data = time.Split(':');
            if (!data[0].Equals("12"))
                minutes += int.Parse(data[0]) * 60;
            minutes += int.Parse(data[1].Substring(0, 2));
            return minutes;
        }

        internal static string timeFromMinutes(int minutes)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMinutes(minutes);
            return dt.ToString("h:mm tt");
        }
    }
}

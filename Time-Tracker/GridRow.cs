using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Tracker
{
    class GridRow
    {
        int time;
        string key;
        string desc;
        public GridRow(string time, string key, string desc)
        {
            this.time = StringUtilities.minutesSinceMidnight(time);
            this.key = key;
            this.desc = desc;
        }
        public GridRow(string[] data)
        {
            if (data.Length != 3)
                throw new ArgumentException();
            time = StringUtilities.minutesSinceMidnight(data[0]);
            key = data[1];
            desc = data[2];
        }
        internal string[] ToArray()
        {
            return new string[] { StringUtilities.timeFromMinutes(time), key, desc };
        }
        internal int TimeInMinutes()
        {
            return time;
        }

        public string Key {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }
        public string Description
        {
            get
            {
                return desc;
            }
            set
            {
                desc = value;
            }
        }
        public string Time
        {
            get
            {
                return StringUtilities.timeFromMinutes(time);
            }
            set
            {
                time = StringUtilities.minutesSinceMidnight(value);
            }
        }
        public int TimeMinutes
        {
            get
            {
                return time;
            }
        }
    }
}

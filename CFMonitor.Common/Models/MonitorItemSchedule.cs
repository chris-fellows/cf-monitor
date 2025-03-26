using CFMonitor.Enums;
using CFUtilities.Utilities;
using System.Xml.Serialization;

namespace CFMonitor.Models
{
    [XmlType("MonitorItemSchedule")]
    public class MonitorItemSchedule
    {        
        [XmlAttribute("ScheduleType")]
        public ScheduleTypes ScheduleType { get; set; }

        [XmlAttribute("Times")]
        public string Times { get; set; } = String.Empty;

        public MonitorItemSchedule()
        {
     
        }

        public DateTime? CheckIsTime(DateTime time, DateTime timeLastChecked)
        {            
            switch (ScheduleType)
            {
                case ScheduleTypes.FixedInterval:

                    var interval = GetFixedInterval(Times);
                    var isTime = (timeLastChecked.Add(interval) <= time);

                    if (isTime)
                    {
                        // Return the value to set TimeLastChecked
                        var currentTimeLastChecked = timeLastChecked;
                        while (currentTimeLastChecked < time)
                        {
                            currentTimeLastChecked = currentTimeLastChecked.Add(interval);
                        }
                        currentTimeLastChecked = currentTimeLastChecked.Subtract(interval);

                        return currentTimeLastChecked;
                    }                                        
                    break;

                case ScheduleTypes.FixedTimes:
                    // TODO: Implement this
                    // Format [Times]|[DaysOfWeek]  E.g. "13:00,18:30|Mon,Tue

                    var elements = Times.Split('|');
                    var times = elements[0].Split(',');
                    var daysOfWeek = elements[1].Split(',');

                    break;
            }

            return null;
        }
        
        private static TimeSpan GetFixedInterval(string times)
        {
            int value = (int)NumericUtilities.GetNumberFromString(times);
            if (times.EndsWith("ms"))
            {
                return TimeSpan.FromMilliseconds(value);
            }
            else if (times.EndsWith("sec"))
            {
                return TimeSpan.FromSeconds(value);
            }
            else if (times.EndsWith("min"))
            {
                return TimeSpan.FromMinutes(value);
            }
            else if (times.EndsWith("hour"))
            {
                return TimeSpan.FromHours(value);
            }

            return TimeSpan.Zero;
        }
    }
}

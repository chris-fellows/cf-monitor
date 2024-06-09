using System;
using CFMonitor.Enums;
using System.Xml.Serialization;
using CFUtilities;

namespace CFMonitor.Models
{
    [XmlType("MonitorItemSchedule")]
    public class MonitorItemSchedule
    {
        [XmlAttribute("TimeLastChecked")]
        public DateTime TimeLastChecked { get; set; }

        [XmlAttribute("ScheduleType")]
        public ScheduleTypes ScheduleType { get; set; }

        [XmlAttribute("Times")]
        public string Times { get; set; }

        public MonitorItemSchedule()
        {
            TimeLastChecked = DateTime.MinValue;
        }

        public bool IsTime(DateTime time)
        {
            bool isTime = false;
            switch (ScheduleType)
            {
                case ScheduleTypes.FixedInterval:

                    int value = (int)NumericUtilities.GetNumberFromString(Times);
                    if (Times.EndsWith("ms"))
                    {                        
                        isTime = (TimeLastChecked.AddMilliseconds(value) <= time);
                    }
                    else if (Times.EndsWith("sec"))
                    {
                        isTime = (TimeLastChecked.AddSeconds(value) <= time);
                    }
                    else if (Times.EndsWith("min"))
                    {                        
                        isTime = (TimeLastChecked.AddMinutes(value) <= time);
                    }
                    else if (Times.EndsWith("hour"))
                    {
                        isTime = (TimeLastChecked.AddHours(value) <= time);
                    }
                    break;
            }
            return isTime;
        }        
    }
}

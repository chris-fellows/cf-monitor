using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor
{
    public enum ConditionOperators : byte
    {
        Equals = 0,
        NotEquals = 1,
        InList = 2,
        NotInList = 3,
        Between = 4,
        LessThan = 5,
        LessThanOrEqualTo = 6,
        MoreThan = 7,
        MoreThanOrEqualTo = 8
    }

    public enum ScheduleTypes : byte
    {
        FixedInterval = 0,       // E.g. Every N secs
        FixedTimes = 1           // E.g. At 12:00 & 18:00 every Mon, Wed & Fri
    } 
}

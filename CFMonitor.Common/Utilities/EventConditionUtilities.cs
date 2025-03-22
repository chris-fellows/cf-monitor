using CFMonitor.Enums;
using CFMonitor.Models;
using System.Text;

namespace CFMonitor.Utilities
{
    public static class EventConditionUtilities
    {
        /// <summary>
        /// Returns display summary for event item. Typically used for display in UI.
        /// 
        /// E.g. "HTTP status code not in (200, 205)"
        /// </summary>
        /// <param name="eventItem"></param>
        /// <param name="systemValueTypes"></param>
        /// <returns></returns>
        public static string GetDisplaySummary(EventItem eventItem, List<SystemValueType> systemValueTypes)
        {
            var eventConditionSystemValueType = systemValueTypes.First(svt => svt.ValueType == eventItem.EventCondition.SourceValueType);

            var text = new StringBuilder($"{eventConditionSystemValueType.Name}");

            switch(eventItem.EventCondition.Operator)
            {
                case ConditionOperators.Between:
                    text.Append($" between {eventItem.EventCondition.Values[0]} and {eventItem.EventCondition.Values[1]}");
                    break;
                case ConditionOperators.Equals:
                    text.Append($" equals {eventItem.EventCondition.Values[0]}");
                    break;
                case ConditionOperators.InList:
                    text.Append(" in (");
                    foreach(var value in eventItem.EventCondition.Values)
                    {
                        if (value != eventItem.EventCondition.Values[0]) text.Append(", ");
                        text.Append(value.ToString());
                    }
                    text.Append(")");
                    break;
                case ConditionOperators.LessThan:
                    text.Append($" < {eventItem.EventCondition.Values[0]}");
                    break;
                case ConditionOperators.LessThanOrEqualTo:
                    text.Append($" <= {eventItem.EventCondition.Values[0]}");
                    break;
                case ConditionOperators.MoreThan:
                    text.Append($" > {eventItem.EventCondition.Values[0]}");
                    break;
                case ConditionOperators.MoreThanOrEqualTo:
                    text.Append($" >= {eventItem.EventCondition.Values[0]}");
                    break;
                case ConditionOperators.NotEquals:
                    text.Append($" not {eventItem.EventCondition.Values[0]}");
                    break;
                case ConditionOperators.NotInList:
                    text.Append(" not in (");
                    foreach (var value in eventItem.EventCondition.Values)
                    {
                        if (value != eventItem.EventCondition.Values[0]) text.Append(", ");
                        text.Append(value.ToString());
                    }
                    text.Append(")");
                    break;
            }
    
            return text.ToString();
        }
    }
}

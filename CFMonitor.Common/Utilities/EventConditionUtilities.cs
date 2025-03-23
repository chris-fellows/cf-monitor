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
        /// E.g. "NTP time inside tolerance = No"
        /// 
        /// TODO: We could clean this up and make the descriptions more readable.
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
                    text.Append($" between {GetDisplayValue(eventItem.EventCondition.Values[0], eventConditionSystemValueType.DataType)} and " +
                        $"{GetDisplayValue(eventItem.EventCondition.Values[1], eventConditionSystemValueType.DataType)}");
                    break;
                case ConditionOperators.Equals:
                    text.Append($" = {GetDisplayValue(eventItem.EventCondition.Values[0], eventConditionSystemValueType.DataType)}");
                    break;
                case ConditionOperators.InList:
                    text.Append(" in (");
                    foreach(var value in eventItem.EventCondition.Values)
                    {
                        if (value != eventItem.EventCondition.Values[0]) text.Append(", ");
                        text.Append(GetDisplayValue(value, eventConditionSystemValueType.DataType));
                    }
                    text.Append(")");
                    break;
                case ConditionOperators.LessThan:
                    text.Append($" < {GetDisplayValue(eventItem.EventCondition.Values[0], eventConditionSystemValueType.DataType)}");
                    break;
                case ConditionOperators.LessThanOrEqualTo:
                    text.Append($" <= {GetDisplayValue(eventItem.EventCondition.Values[0], eventConditionSystemValueType.DataType)}");
                    break;
                case ConditionOperators.MoreThan:
                    text.Append($" > {GetDisplayValue(eventItem.EventCondition.Values[0], eventConditionSystemValueType.DataType)}");
                    break;
                case ConditionOperators.MoreThanOrEqualTo:
                    text.Append($" >= {GetDisplayValue(eventItem.EventCondition.Values[0], eventConditionSystemValueType.DataType)}");
                    break;
                case ConditionOperators.NotEquals:
                    text.Append($" not {GetDisplayValue(eventItem.EventCondition.Values[0], eventConditionSystemValueType.DataType)}");
                    break;
                case ConditionOperators.NotInList:
                    text.Append(" not in (");
                    foreach (var value in eventItem.EventCondition.Values)
                    {
                        if (value != eventItem.EventCondition.Values[0]) text.Append(", ");
                        text.Append(GetDisplayValue(value, eventConditionSystemValueType.DataType));
                    }
                    text.Append(")");
                    break;
            }
    
            return text.ToString();
        }

        /// <summary>
        /// Gets value for display
        /// </summary>
        /// <param name="value"></param>
        /// <param name="systemValueDataType"></param>
        /// <returns></returns>
        private static string GetDisplayValue(object value, SystemValueDataTypes systemValueDataType)
        {
            if (value == null) return "";

            return systemValueDataType switch
            {
                SystemValueDataTypes.Boolean => (bool)value ? "Yes" : "No",
                _ => value.ToString()
            };
        }
    }
}

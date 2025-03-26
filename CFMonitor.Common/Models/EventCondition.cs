using CFMonitor.Enums;
using System.Xml.Serialization;

namespace CFMonitor.Models
{
    /// <summary>
    /// Condition for an event to have happened
    /// </summary>
    [XmlType("EventCondition")]
    public class EventCondition
    {        
        [XmlAttribute("SourceValueType")]        
        public SystemValueTypes SourceValueType { get; set; }

        [XmlAttribute("Operator")]
        public ConditionOperators Operator { get; set; } = ConditionOperators.Equals;

        /// <summary>
        /// Type name for values
        /// </summary>
        [XmlAttribute("ValueTypeName")]
        public string ValueTypeName { get; set; } = typeof(String).FullName;    // Default

        /// <summary>
        /// Value(s) for checking
        /// </summary>
        [XmlArray("Values")]
        [XmlArrayItem("Value")]
        public List<string> Values { get; set; } = new();

        /// <summary>
        /// Whether value is valid for event condition
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValid(object value)
        {
            bool isValid = false;

            if (value == null) return false;

            // Cast value strings to typed value
            Type valueType = Type.GetType(ValueTypeName);
            List<object> valuesTyped = Values.Select(stringValue =>
            {
                return Convert.ChangeType(stringValue, valueType);
            }).ToList();            
            
            switch (Operator)
            {
                case ConditionOperators.Equals:
                    isValid = value.Equals(valuesTyped[0]);
                    break;
                case ConditionOperators.InList:
                    isValid = valuesTyped.Contains(value);
                    break;
                case ConditionOperators.Between:
                    isValid = Convert.ToDouble(value) >= Convert.ToDouble(valuesTyped[0]) &&
                            Convert.ToDouble(value) <= Convert.ToDouble(valuesTyped[1]);
                    break;
                case ConditionOperators.LessThan:
                    isValid = Convert.ToDouble(value) < Convert.ToDouble(valuesTyped[0]);
                    break;
                case ConditionOperators.LessThanOrEqualTo:
                    isValid = Convert.ToDouble(value) <= Convert.ToDouble(valuesTyped[0]);
                    break;
                case ConditionOperators.MoreThan:
                    isValid = Convert.ToDouble(value) > Convert.ToDouble(valuesTyped[0]);
                    break;
                case ConditionOperators.MoreThanOrEqualTo:
                    isValid = Convert.ToDouble(value) >= Convert.ToDouble(valuesTyped[0]);
                    break;
                case ConditionOperators.NotEquals:
                    isValid = !value.Equals(valuesTyped[0]);
                    break;
                case ConditionOperators.NotInList:
                    isValid = !valuesTyped.Contains(value);
                    break;
            }
            return isValid;
        }
    }
}

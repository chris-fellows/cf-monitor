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
        /// Value(s) for checking
        /// </summary>
        [XmlArray("Values")]
        [XmlArrayItem("Value")]
        public List<object> Values = new List<object>();

        /// <summary>
        /// Whether value is valid for event condition
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValid(object value)
        {
            bool result = false;

            //TODO: Handle all operators
            switch (Operator)
            {
                case ConditionOperators.Equals:
                    result = value.Equals(Values[0]);
                    break;
                case ConditionOperators.InList:
                    result = Values.Contains(value);
                    break;
                case ConditionOperators.Between:
                    //result = value >= Values[0] && value <= Values[1];
                case ConditionOperators.LessThan:
                    //return value < Values[0];
                    break;
                case ConditionOperators.LessThanOrEqualTo:
                    //return value <= Values[0];
                    break;
                case ConditionOperators.MoreThan:
                    //return value > Values[0];
                    break;
                case ConditionOperators.MoreThanOrEqualTo:
                    //return value >= Values[0];
                    break;
                case ConditionOperators.NotEquals:
                    result = !value.Equals(Values[0]);
                    break;
                case ConditionOperators.NotInList:
                    result = !Values.Contains(value);
                    break;
            }
            return result;
        }
    }
}

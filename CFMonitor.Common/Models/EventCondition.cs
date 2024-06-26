﻿using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models
{
    /// <summary>
    /// Condition for an event to have happened
    /// </summary>
    [XmlType("EventCondition")]
    public class EventCondition
    {        
        [XmlAttribute("Source")]
        //public string Source { get; set; }
        public EventConditionSource Source { get; set; }

        [XmlAttribute("Operator")]
        public ConditionOperators Operator { get; set; }

        [XmlArray("Values")]
        [XmlArrayItem("Value")]
        public List<object> Values = new List<object>();

        /// <summary>
        /// Evaluates in input value against this condition
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Whether input value is valid for condition</returns>
        public bool Evaluate(object value)
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

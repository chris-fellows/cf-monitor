using CFMonitor.Enums;
using CFMonitor.Models;
using System.Net;
using System.Net.NetworkInformation;

namespace CFMonitor.Utilities
{
    public static class SystemValueDataTypeUtilities
    {       
        /// <summary>
        /// Gets allowed values for system value type. Only returns values if there's a finite list. For booleans
        /// then we return an empty list because that gets special handling (E.g. HTML checkbox) rather than a 
        /// dropdown list.
        /// </summary>
        /// <param name="systemValueType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static List<NameAndValue> GetAllowedValues(SystemValueTypes systemValueType, Type valueType)
        {
            // Check more complex types
            switch(valueType)
            {
                case Type _ when valueType == typeof(System.Net.HttpStatusCode):
                    var statusCodes = Enum.GetValues(typeof(System.Net.HttpStatusCode)).Cast<HttpStatusCode>().ToList();
                    return statusCodes.Select(statusCodes =>
                    {
                        return new NameAndValue() { Name = statusCodes.ToString(), Value = statusCodes.ToString() };
                    }).ToList();                                      
                case Type _ when valueType == typeof(System.Net.NetworkInformation.IPStatus):
                    var ipStatuses = Enum.GetValues(typeof(System.Net.NetworkInformation.IPStatus)).Cast<IPStatus>().ToList();
                    return ipStatuses.Select(ipStatus =>
                    {
                        return new NameAndValue() { Name = ipStatus.ToString(), Value = ipStatus.ToString() };
                    }).ToList();                   
            }

            // Check system value type enum
            switch(systemValueType)
            {
                case SystemValueTypes.MIP_URLMethod:
                    return new List<NameAndValue>()
                    {
                        new NameAndValue() { Name = "DELETE", Value = "DELETE" },
                        new NameAndValue() { Name = "GET", Value = "DELETE" },
                        new NameAndValue() { Name = "PATCH", Value = "DELETE" },
                        new NameAndValue() { Name = "POST", Value = "DELETE" },
                        new NameAndValue() { Name = "PUT", Value = "DELETE" },
                    };
                case SystemValueTypes.MIP_TimeServerType:
                    return new List<NameAndValue>()
                    {
                        new NameAndValue() { Name = "NIST", Value= "NIST"},
                        new NameAndValue() { Name = "NTP", Value= "NTP"},
                        new NameAndValue() { Name = "HTTP", Value= "HTTP"}
                    };                   
                default:
                    return new();
            }
        }
    }
}

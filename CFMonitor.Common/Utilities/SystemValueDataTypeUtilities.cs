using CFMonitor.Enums;

namespace CFMonitor.Utilities
{
    public static class SystemValueDataTypeUtilities
    {
        /// <summary>
        /// Returns allowed values for data type. Returns emppty list if not a finite list
        /// </summary>
        /// <param name="systemValueDataType"></param>
        /// <returns></returns>
        public static List<string> GetAllowedValues(SystemValueDataTypes systemValueDataType)
        {
            return systemValueDataType switch
            {
                SystemValueDataTypes.HttpMethod => new List<string>() { "DELETE", "GET", "PATCH", "POST", "PUT" },
                SystemValueDataTypes.HttpStatusCode => Enum.GetValues(typeof(System.Net.HttpStatusCode)).Cast<int>().Select(i => i.ToString()).ToList(),
                SystemValueDataTypes.PingReplyStatus => Enum.GetValues(typeof(System.Net.NetworkInformation.IPStatus)).Cast<string>().ToList(),
                _ => new List<string>()
            };
        }
    }
}

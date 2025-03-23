namespace CFMonitor.Models
{
    /// <summary>
    /// System value.
    /// 
    /// It allows the system to store lots of different values without having to add new properties to classes.
    /// </summary>
    public class SystemValue
    {
        /// <summary>
        /// System value type
        /// </summary>
        public string TypeId { get; set; } = String.Empty;

        /// <summary>
        /// Value. E.g. Monitor Item Id, User Id etc
        /// </summary>
        public string Value { get; set; } = String.Empty;
    }
}

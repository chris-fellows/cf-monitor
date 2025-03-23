
namespace CFMonitor.Models
{
    /// <summary>
    /// Filter on system values
    /// </summary>
    public class SystemValueFilter
    {    
        /// <summary>
        /// System value type
        /// </summary>
        public string TypeId { get; set; } = String.Empty;

        /// <summary>
        /// System values
        /// </summary>
        public List<string> Values { get; set; } = new();
    }
}


namespace CFMonitor.Models
{
    /// <summary>
    ///System task parameter
    /// </summary>
    public class SystemTaskParameter
    {        
        public string Id { get; set; } = String.Empty;
        
        public string SystemValueTypeId { get; set; } = String.Empty;
        
        public string Value { get; set; } = String.Empty;

        public SystemValue ToSystemValue()
        {            
            return new SystemValue()
            {
                TypeId = SystemValueTypeId,
                Value = Value
            };
        }
    }
}

namespace CFMonitor.Models
{
    public class SystemTaskJob
    {        
        public string Id { get; set; } = String.Empty;

        public string StatusId { get; set; } = String.Empty;

        public string TypeId { get; set; } = String.Empty;

        public string? Error { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; }

        /// <summary>
        /// Filter on parameters.
        /// </summary>
        public List<SystemTaskParameter> Parameters { get; set; } = new();
    }
}

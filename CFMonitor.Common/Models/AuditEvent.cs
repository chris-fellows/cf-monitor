namespace CFMonitor.Models
{
    /// <summary>
    /// Audit event
    /// </summary>
    public class AuditEvent
    {
        /// <summary>
        /// Unique
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Audit event type
        /// </summary>
        public string TypeId { get; set; } = String.Empty;

        /// <summary>
        /// Parameters
        /// </summary>
        public List<AuditEventParameter> Parameters { get; set; } = new();

        /// <summary>
        /// Created user
        /// </summary>
        public string CreatedUserId { get; set; } = String.Empty;

        /// <summary>
        /// Created date 
        /// </summary>
        public DateTimeOffset CreatedDateTime { get; set; } 
    }
}

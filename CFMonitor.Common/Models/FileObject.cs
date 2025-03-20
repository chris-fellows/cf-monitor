namespace CFMonitor.Models
{
    /// <summary>
    /// File used for monitoring. E.g. SQL script, PowerShell script etc.
    /// </summary>
    public class FileObject
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public byte[] Content { get; set; } = new byte[0];
    }
}

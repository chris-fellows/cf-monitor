using CFMonitor.Enums;

namespace CFMonitor.Models
{
    public class ActionItemType
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public ActionerTypes ActionerType { get; set; }

        public string Color { get; set; } = String.Empty;

        public string ImageSource { get; set; } = String.Empty;
    }
}

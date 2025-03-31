using CFMonitor.Models;

namespace CFMonitor.UI.Models
{
    public class ParameterItem
    {
        public string Id { get; set; } = String.Empty;

        public SystemValueType SystemValueType { get; set; } = new();

        public string Name { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;
        
        public bool ValueBoolean
        {
            get { return Convert.ToBoolean(Value); }
            set { Value = value.ToString(); }            
        }
        
        public Decimal ValueDecimal
        {
            get { return Convert.ToDecimal(Value); }
            set { Value = value.ToString(); }
        }

        public Int64 ValueInteger
        {
            get { return Convert.ToInt64(Value); }
            set { Value = value.ToString(); }
        }

        public Int64 MinValueInteger { get; set; }

        public Int64 MaxValueInteger { get; set; }
    }
}

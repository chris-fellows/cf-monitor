//namespace CFMonitor.Models
//{
//    /// <summary>
//    /// Stores value as string and supports casting to original value
//    /// </summary>
//    public class ValueAsString
//    {
//        public string TypeName { get; set; } = String.Empty;

//        public string? Value { get; set; } = String.Empty;

//        public void SetValue(object value)
//        {            
//            TypeName = value.GetType().FullName;
//            Value = value.ToString();
//        }

//        public object? GetValue()
//        {
//            return Convert.ChangeType(Value, Type.GetType(TypeName));
//        }
//    }
//}

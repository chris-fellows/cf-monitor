using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// System value display service. Typically used for providing system values in displayable format rather
    /// than display Ids etc.
    /// </summary>
    public interface ISystemValueDisplayService
    {
        /// <summary>
        /// Returns display items (List of label and display value)
        /// 
        /// E.g. System value for User Id could return user name or other details
        /// </summary>
        /// <param name="systemValue"></param>
        /// <returns>List of [Label, DisplayValue]</returns>
        Task<List<string[]>> GetDisplayItemsAsync(SystemValue systemValue);
    }
}

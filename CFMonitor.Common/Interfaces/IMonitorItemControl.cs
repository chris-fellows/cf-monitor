using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for control that can view/edit monitor item
    /// </summary>
    public interface IMonitorItemControl
    {
        /// <summary>
        /// Sets the model to display
        /// </summary>
        /// <param name="monitorItem"></param>
        void ModelToView(MonitorItem monitorItem);

        /// <summary>
        /// Validates currrent model changes
        /// </summary>
        /// <returns></returns>
        List<string> Validate();

        /// <summary>
        /// Applies changes to model
        /// </summary>
        /// <param name="monitorItem"></param>
        void ViewToModel(MonitorItem monitorItem);
    }
}

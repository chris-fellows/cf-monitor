using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.SystemTask
{
    /// <summary>
    /// Interface to system task
    /// </summary>
    public interface ISystemTask
    {
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Executes system task
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}

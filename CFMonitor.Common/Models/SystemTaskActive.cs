using CFMonitor.SystemTask;

namespace CFMonitor.Models
{
    /// <summary>
    /// Details of active system task
    /// </summary>
    public class SystemTaskActive
    {
        public ISystemTask SystemTask { get; internal set;  }

        public Task Task { get; internal set; }

        public CancellationTokenSource CancellationTokenSource { get; internal set; }

        public SystemTaskActive(ISystemTask systemTask, Task task, CancellationTokenSource cancellationTokenSource)
        {
            SystemTask = systemTask;
            Task = task;
            CancellationTokenSource = cancellationTokenSource;
        }
    }
}

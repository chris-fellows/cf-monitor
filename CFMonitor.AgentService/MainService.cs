using System.ServiceProcess;

namespace CFMonitor.AgentService
{
    public partial class MainService : ServiceBase
    {
        private MonitorItemManager _monitorItemManager;

        public MainService()
        {
            InitializeComponent();           
        }

        protected override void OnStart(string[] args)
        {
            _monitorItemManager.Start();
        }

        protected override void OnStop()
        {
            _monitorItemManager.Stop();
        }      
    }
}

using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace CFMonitor.AgentService
{
    public partial class Service1 : ServiceBase
    {
        private MonitorItemManager _monitorItemManager;

        public Service1()
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

using CFMonitor.Interfaces;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFMonitor.Controls
{
    public partial class MonitorRunProcessControl : UserControl, IMonitorItemControl
    {
        public MonitorRunProcessControl()
        {
            InitializeComponent();
        }

        public void ModelToView(MonitorItem monitorItem)
        {
            var monitorRunProcess = (MonitorRunProcess)monitorItem;

            monitorItemControl1.ModelToView(monitorRunProcess);
        }

        public List<string> Validate()
        {
            return new List<string>();
        }

        public void ViewToModel(MonitorItem monitorItem)
        {
            var monitorRunProcess = (MonitorRunProcess)monitorItem;
        }
    }
}

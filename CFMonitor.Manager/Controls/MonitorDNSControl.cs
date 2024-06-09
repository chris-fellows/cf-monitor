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
    public partial class MonitorDNSControl : UserControl, IMonitorItemControl
    {
        public MonitorDNSControl()
        {
            InitializeComponent();
        }

        public void ModelToView(MonitorItem monitorItem)
        {
            var monitorDNS = (MonitorDNS)monitorItem;

            monitorItemControl1.ModelToView(monitorDNS);
        }

        public List<string> Validate()
        {
            return new List<string>();
        }

        public void ViewToModel(MonitorItem monitorItem)
        {
            var monitorDNS = (MonitorDNS)monitorItem;
        }
    }
}

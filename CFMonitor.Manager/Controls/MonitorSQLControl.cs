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
    public partial class MonitorSQLControl : UserControl, IMonitorItemControl
    {
        public MonitorSQLControl()
        {
            InitializeComponent();
        }

        public void ModelToView(MonitorItem monitorItem)
        {
            var monitorSQL = (MonitorSQL)monitorItem;

            monitorItemControl1.ModelToView(monitorSQL);
        }

        public List<string> Validate()
        {
            return new List<string>();
        }

        public void ViewToModel(MonitorItem monitorItem)
        {
            var monitorSQL = (MonitorSQL)monitorItem;
        }
    }
}

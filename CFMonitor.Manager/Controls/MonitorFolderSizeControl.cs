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
    public partial class MonitorFolderSizeControl : UserControl, IMonitorItemControl
    {
        public MonitorFolderSizeControl()
        {
            InitializeComponent();
        }

        public void ModelToView(MonitorItem monitorItem)
        {
            var monitorFolderSize = (MonitorFolderSize)monitorItem;

            monitorItemControl1.ModelToView(monitorFolderSize);
        }

        public List<string> Validate()
        {
            return new List<string>();
        }

        public void ViewToModel(MonitorItem monitorItem)
        {
            var monitorFolderSize = (MonitorFolderSize)monitorItem;
        }
    }
}

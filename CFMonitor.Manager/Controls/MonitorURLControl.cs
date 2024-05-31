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
    public partial class MonitorURLControl : UserControl
    {
        public MonitorURLControl()
        {
            InitializeComponent();
        }

        public void ModelToView(MonitorURL monitorURL)
        {
            monitorItemControl1.ModelToView(monitorURL);
        }
    }
}

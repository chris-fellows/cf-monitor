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
    public partial class MonitorURLControl : UserControl, IMonitorItemControl
    {
        //private MonitorURL _monitorURL;

        public MonitorURLControl()
        {
            InitializeComponent();
        }

        public void ModelToView(MonitorItem monitorItem)
        {
            var monitorURL = (MonitorURL)monitorItem;

            monitorItemControl1.ModelToView(monitorURL);

            txtMethod.Text = monitorURL.Method;
            txtURL.Text = monitorURL.URL;
            txtUsername.Text = monitorURL.UserName;
            txtPassword.Text = monitorURL.Password;

            dgvHeaders.Rows.Clear();
            dgvHeaders.Columns.Clear();
            var columnIndex = dgvHeaders.Columns.Add("Name", "Name");
            columnIndex = dgvHeaders.Columns.Add("Value", "Value");

            foreach (var header in monitorURL.Headers)
            {
                using (var row = new DataGridViewRow())
                {
                    using (var cell = new DataGridViewTextBoxCell())
                    {
                        cell.Value = header.Name;
                        row.Cells.Add(cell);
                    }

                    using (var cell = new DataGridViewTextBoxCell())
                    {
                        cell.Value = header.Value;
                        row.Cells.Add(cell);
                    }
                }
            }
        }

        public List<string> Validate()
        {
            return new List<string>();
        }      

        public void ViewToModel(MonitorItem monitorItem)
        {
            var monitorURL = (MonitorURL)monitorItem;
        }
    }
}

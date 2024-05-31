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
    public partial class MonitorItemControl : UserControl
    {
        public MonitorItemControl()
        {
            InitializeComponent();
        }

        public void ModelToView(MonitorItem monitorItem)
        {
            txtName.Text = monitorItem.Name;
            chkEnabled.Checked = monitorItem.Enabled;

            dgvEvents.Rows.Clear();

            var columnIndex = dgvEvents.Columns.Add("Event", "Event");
            columnIndex = dgvEvents.Columns.Add("Summary", "Summary");
            columnIndex = dgvEvents.Columns.Add("Edit", "Edit");

            foreach(var @event in monitorItem.EventItems)
            {
                var row = new DataGridViewRow();

                using (var cell = new DataGridViewTextBoxCell())
                {
                    cell.Value = @event.EventCondition.Source;
                    row.Cells.Add(cell);
                }

                using (var cell = new DataGridViewTextBoxCell())
                {
                    cell.Value = "";
                    row.Cells.Add(cell);
                }

                using (var cell = new DataGridViewButtonCell())
                {
                    cell.Value = "Edit";
                    row.Cells.Add(cell);
                }


                dgvEvents.Rows.Add(row);
            }

        }
    }
}

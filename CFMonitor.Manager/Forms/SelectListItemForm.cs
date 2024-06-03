using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CFMonitor.Forms
{
    /// <summary>
    /// Form for selecting item from list and clicking "OK"
    /// </summary>
    public partial class SelectListItemForm : Form
    {
        public SelectListItemForm()
        {
            InitializeComponent();
        }

        public SelectListItemForm(string title, string label, List<string> names)
        {
            InitializeComponent();

            this.Text = title;
            lblLabel.Text = label;
            cbItems.DataSource = names;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string SelectedItem
        {
            get { return cbItems.SelectedItem.ToString(); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

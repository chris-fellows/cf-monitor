using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFMonitor
{
    public partial class MainForm : Form
    {
        private Manager _manager = null;

        public MainForm()
        {
            InitializeComponent();

            //CreateTestMonitorItems();

            this.Visible = false;
            _manager = new Manager(Factory.GetCheckerList(), Factory.GetActionerList(), Factory.GetDefaultMonitorItemRepository());
            _manager.Start();
        }

        private void CreateTestMonitorItems()
        {
            IMonitorItemRepository monitorItemRepository = Factory.GetDefaultMonitorItemRepository();
            MonitorItemFactory.Create(monitorItemRepository);

        }
    }
}

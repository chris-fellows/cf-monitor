using CFMonitor.Controls;
using CFMonitor.Forms;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.MonitorItems;
using CFMonitor.Services;
using System;
using System.CodeDom;
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
    /// <summary>
    /// Main form. Displays list of monitor items which can be modified
    /// </summary>
    public partial class MainForm : Form
    {
        private IMonitorItemService _monitorItemService;
        private IMonitorItemTypeService _monitorItemTypeService;

        private enum MyTreeNodeType
        {
            Unknown = 0,
            MonitorItem = 1
        }

        public MainForm(IMonitorItemService monitorItemService, IMonitorItemTypeService monitorItemTypeService)
        {
            InitializeComponent();

            _monitorItemService = monitorItemService;
            _monitorItemTypeService = monitorItemTypeService;

            //CreateTestMonitorItems();
            this.Visible = true;
            DisplayMonitorItems();
        }

        private static MyTreeNodeType GetMyTreeNodeType(TreeNode treeNode)
        {
            if (treeNode.Name.StartsWith("MonitorItem.")) return MyTreeNodeType.MonitorItem;

            return MyTreeNodeType.Unknown;
        }

        //private void CreateTestMonitorItems()
        //{
        //    IMonitorItemService monitorItemService = Factory.GetDefaultMonitorItemRepository();
        //    MonitorItemFactory.Create(monitorItemRepository);

        //}

        private void DisplayMonitorItems()
        {          
            tvwMonitorItem.Nodes.Clear();

            // Get monitor items
            var monitorItems = _monitorItemService.GetAll().OrderBy(mi => mi.Name).ToList();

            // Display monitor items
            foreach(var monitorItem in monitorItems)
            {
                var node = tvwMonitorItem.Nodes.Add($"MonitorItem.{monitorItem.Name}", monitorItem.Name);
                node.Tag = monitorItem;
            }
        }

        private void DisplayMonitorItemControl(MonitorItem monitorItem)
        {
            // Clear existing controls
            splitContainer1.Panel2.Controls.Clear();
            
            if (monitorItem.GetType() == typeof(MonitorURL))
            {
                var control = new MonitorURLControl();
                control.ModelToView(monitorItem);
                splitContainer1.Panel2.Controls.Add(control);
            }
        }

        private void tvwMonitorItem_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Get node type
            var nodeType = GetMyTreeNodeType(e.Node);

            splitContainer1.Panel2.Controls.Clear();

            // Display control for node
            switch(nodeType)
            {
                case MyTreeNodeType.MonitorItem:
                    DisplayMonitorItemControl((MonitorItem)e.Node.Tag);
                    break;
            }
        }

        private void tsbNewMonitorItem_Click(object sender, EventArgs e)
        {
            var monitorItemTypes = _monitorItemTypeService.GetAll();

            SelectListItemForm form = new SelectListItemForm("New Monitor Item", "Item", monitorItemTypes.Select(mi => mi.Name).ToList());
            if (form.ShowDialog() == DialogResult.OK)
            {
                MonitorItemType monitorItemType = monitorItemTypes.First(mit => mit.Name == form.SelectedItem);

                // Create empty monitor item
                var monitorItem = monitorItemType.CreateMonitorItem();                
                _monitorItemService.Add(monitorItem);

                // Refresh
                DisplayMonitorItems();
            }
        }

        private void createTestItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to create test items?", "Test Items", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Create
                var monitorItems = MonitorItemTestFactory.Create();
                monitorItems.ForEach(monitorItem => _monitorItemService.Add(monitorItem));

                // Refresh
                DisplayMonitorItems();
            }
        }
    }
}

﻿namespace CFMonitor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNewMonitorItem = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwMonitorItem = new System.Windows.Forms.TreeView();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.createTestItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 568);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1161, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewMonitorItem,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1161, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNewMonitorItem
            // 
            this.tsbNewMonitorItem.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewMonitorItem.Image")));
            this.tsbNewMonitorItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewMonitorItem.Name = "tsbNewMonitorItem";
            this.tsbNewMonitorItem.Size = new System.Drawing.Size(78, 22);
            this.tsbNewMonitorItem.Text = "New item";
            this.tsbNewMonitorItem.Click += new System.EventHandler(this.tsbNewMonitorItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvwMonitorItem);
            this.splitContainer1.Size = new System.Drawing.Size(1161, 543);
            this.splitContainer1.SplitterDistance = 387;
            this.splitContainer1.TabIndex = 2;
            // 
            // tvwMonitorItem
            // 
            this.tvwMonitorItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwMonitorItem.Location = new System.Drawing.Point(0, 0);
            this.tvwMonitorItem.Name = "tvwMonitorItem";
            this.tvwMonitorItem.Size = new System.Drawing.Size(387, 543);
            this.tvwMonitorItem.TabIndex = 0;
            this.tvwMonitorItem.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwMonitorItem_AfterSelect);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createTestItemsToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(73, 22);
            this.toolStripDropDownButton1.Text = "Testing";
            // 
            // createTestItemsToolStripMenuItem
            // 
            this.createTestItemsToolStripMenuItem.Name = "createTestItemsToolStripMenuItem";
            this.createTestItemsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createTestItemsToolStripMenuItem.Text = "Create test items";
            this.createTestItemsToolStripMenuItem.Click += new System.EventHandler(this.createTestItemsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 590);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "Monitor Manager";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvwMonitorItem;
        private System.Windows.Forms.ToolStripButton tsbNewMonitorItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem createTestItemsToolStripMenuItem;
    }
}


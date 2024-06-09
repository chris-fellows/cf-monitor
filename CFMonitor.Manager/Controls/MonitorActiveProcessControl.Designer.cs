namespace CFMonitor.Controls
{
    partial class MonitorActiveProcessControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.monitorItemControl1 = new CFMonitor.Controls.MonitorItemControl();
            this.SuspendLayout();
            // 
            // monitorItemControl1
            // 
            this.monitorItemControl1.Location = new System.Drawing.Point(29, 13);
            this.monitorItemControl1.Name = "monitorItemControl1";
            this.monitorItemControl1.Size = new System.Drawing.Size(494, 203);
            this.monitorItemControl1.TabIndex = 0;
            // 
            // MonitorActiveProcessControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.monitorItemControl1);
            this.Name = "MonitorActiveProcessControl";
            this.Size = new System.Drawing.Size(719, 505);
            this.ResumeLayout(false);

        }

        #endregion

        private MonitorItemControl monitorItemControl1;
    }
}

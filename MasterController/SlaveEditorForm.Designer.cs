namespace MasterController
{
    partial class SlaveEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button buttonOK;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            textBoxName = new System.Windows.Forms.TextBox();
            textBoxIp = new System.Windows.Forms.TextBox();
            textBoxPort = new System.Windows.Forms.TextBox();
            buttonOK = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // textBoxName
            // 
            textBoxName.Location = new System.Drawing.Point(12, 12);
            textBoxName.Name = "textBoxName";
            textBoxName.PlaceholderText = "Name";
            textBoxName.Size = new System.Drawing.Size(260, 23);
            textBoxName.TabIndex = 0;
            // 
            // textBoxIp
            // 
            textBoxIp.Location = new System.Drawing.Point(12, 41);
            textBoxIp.Name = "textBoxIp";
            textBoxIp.PlaceholderText = "IP Address";
            textBoxIp.Size = new System.Drawing.Size(260, 23);
            textBoxIp.TabIndex = 1;
            // 
            // textBoxPort
            // 
            textBoxPort.Location = new System.Drawing.Point(12, 70);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.PlaceholderText = "Port";
            textBoxPort.Size = new System.Drawing.Size(260, 23);
            textBoxPort.TabIndex = 2;
            // 
            // buttonOK
            // 
            buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonOK.ForeColor = System.Drawing.Color.White;
            buttonOK.Location = new System.Drawing.Point(197, 99);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new System.Drawing.Size(75, 23);
            buttonOK.TabIndex = 4;
            buttonOK.Text = "OK";
            buttonOK.Click += buttonOK_Click;
            // 
            // SlaveEditorForm
            // 
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            ClientSize = new System.Drawing.Size(331, 154);
            Controls.Add(textBoxName);
            Controls.Add(textBoxIp);
            Controls.Add(textBoxPort);
            Controls.Add(buttonOK);
            Name = "SlaveEditorForm";
            Text = "Add/Edit Slave";
            Controls.SetChildIndex(buttonOK, 0);
            Controls.SetChildIndex(textBoxPort, 0);
            Controls.SetChildIndex(textBoxIp, 0);
            Controls.SetChildIndex(textBoxName, 0);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

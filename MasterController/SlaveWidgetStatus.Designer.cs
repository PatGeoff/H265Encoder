namespace MasterController
{
    partial class SlaveWidgetStatus
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            checkBox1 = new System.Windows.Forms.CheckBox();
            lbl_BladeName = new System.Windows.Forms.Label();
            lbl_bladeNetStat = new System.Windows.Forms.Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            tableLayoutPanel1.Controls.Add(checkBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(lbl_BladeName, 1, 0);
            tableLayoutPanel1.Controls.Add(lbl_bladeNetStat, 2, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(344, 28);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            checkBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBox1.Location = new System.Drawing.Point(3, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(15, 22);
            checkBox1.TabIndex = 0;
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // lbl_BladeName
            // 
            lbl_BladeName.AutoSize = true;
            lbl_BladeName.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_BladeName.ForeColor = System.Drawing.Color.White;
            lbl_BladeName.Location = new System.Drawing.Point(24, 0);
            lbl_BladeName.Name = "lbl_BladeName";
            lbl_BladeName.Size = new System.Drawing.Size(65, 28);
            lbl_BladeName.TabIndex = 1;
            lbl_BladeName.Text = "Name";
            lbl_BladeName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_bladeNetStat
            // 
            lbl_bladeNetStat.AutoSize = true;
            lbl_bladeNetStat.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_bladeNetStat.ForeColor = System.Drawing.Color.White;
            lbl_bladeNetStat.Location = new System.Drawing.Point(95, 0);
            lbl_bladeNetStat.Name = "lbl_bladeNetStat";
            lbl_bladeNetStat.Size = new System.Drawing.Size(85, 28);
            lbl_bladeNetStat.TabIndex = 2;
            lbl_bladeNetStat.Text = "Status";
            lbl_bladeNetStat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SlaveWidgetStatus
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            Controls.Add(tableLayoutPanel1);
            Name = "SlaveWidgetStatus";
            Size = new System.Drawing.Size(344, 28);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lbl_BladeName;
        private System.Windows.Forms.Label lbl_bladeNetStat;
    }
}

namespace MasterController
{
    partial class SlaveWidget
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlaveWidget));
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            lbl_bladeName = new System.Windows.Forms.Label();
            lbl_IpPort = new System.Windows.Forms.Label();
            lbl_bladeNetStat = new System.Windows.Forms.Label();
            btn_deleteBlade = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tableLayoutPanel1.Size = new System.Drawing.Size(382, 31);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(lbl_bladeName, 0, 0);
            tableLayoutPanel2.Controls.Add(lbl_IpPort, 1, 0);
            tableLayoutPanel2.Controls.Add(lbl_bladeNetStat, 2, 0);
            tableLayoutPanel2.Controls.Add(btn_deleteBlade, 4, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(376, 25);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lbl_bladeName
            // 
            lbl_bladeName.AutoSize = true;
            lbl_bladeName.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_bladeName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lbl_bladeName.ForeColor = System.Drawing.Color.White;
            lbl_bladeName.Location = new System.Drawing.Point(3, 0);
            lbl_bladeName.Name = "lbl_bladeName";
            lbl_bladeName.Size = new System.Drawing.Size(78, 25);
            lbl_bladeName.TabIndex = 0;
            lbl_bladeName.Text = "Blade";
            lbl_bladeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_IpPort
            // 
            lbl_IpPort.AutoSize = true;
            lbl_IpPort.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_IpPort.ForeColor = System.Drawing.Color.White;
            lbl_IpPort.Location = new System.Drawing.Point(87, 0);
            lbl_IpPort.Name = "lbl_IpPort";
            lbl_IpPort.Size = new System.Drawing.Size(153, 25);
            lbl_IpPort.TabIndex = 1;
            lbl_IpPort.Text = "IP:port";
            lbl_IpPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_bladeNetStat
            // 
            lbl_bladeNetStat.AutoSize = true;
            lbl_bladeNetStat.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_bladeNetStat.ForeColor = System.Drawing.Color.White;
            lbl_bladeNetStat.Location = new System.Drawing.Point(246, 0);
            lbl_bladeNetStat.Name = "lbl_bladeNetStat";
            lbl_bladeNetStat.Size = new System.Drawing.Size(92, 25);
            lbl_bladeNetStat.TabIndex = 2;
            lbl_bladeNetStat.Text = "Status";
            lbl_bladeNetStat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_deleteBlade
            // 
            btn_deleteBlade.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_deleteBlade.BackgroundImage");
            btn_deleteBlade.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            btn_deleteBlade.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_deleteBlade.FlatAppearance.BorderSize = 0;
            btn_deleteBlade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_deleteBlade.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btn_deleteBlade.ForeColor = System.Drawing.Color.DarkGray;
            btn_deleteBlade.Location = new System.Drawing.Point(362, 3);
            btn_deleteBlade.Name = "btn_deleteBlade";
            btn_deleteBlade.Size = new System.Drawing.Size(11, 19);
            btn_deleteBlade.TabIndex = 4;
            btn_deleteBlade.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            btn_deleteBlade.UseVisualStyleBackColor = true;
            btn_deleteBlade.Click += btn_deleteBlade_Click;
            // 
            // SlaveWidget
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(tableLayoutPanel1);
            Name = "SlaveWidget";
            Size = new System.Drawing.Size(382, 31);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lbl_bladeName;
        private System.Windows.Forms.Label lbl_IpPort;
        private System.Windows.Forms.Label lbl_bladeNetStat;
        private System.Windows.Forms.Button btn_deleteBlade;
    }
}

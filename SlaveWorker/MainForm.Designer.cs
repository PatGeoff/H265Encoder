namespace SlaveWorker
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBox_Output;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            textBox_Output = new System.Windows.Forms.TextBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            btn_ClearText = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // textBox_Output
            // 
            textBox_Output.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            textBox_Output.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox_Output.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_Output.ForeColor = System.Drawing.Color.White;
            textBox_Output.Location = new System.Drawing.Point(3, 2);
            textBox_Output.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            textBox_Output.Multiline = true;
            textBox_Output.Name = "textBox_Output";
            textBox_Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox_Output.Size = new System.Drawing.Size(694, 298);
            textBox_Output.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(textBox_Output, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanel1.Size = new System.Drawing.Size(700, 338);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(btn_ClearText, 0, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 305);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(694, 30);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // btn_ClearText
            // 
            btn_ClearText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_ClearText.Location = new System.Drawing.Point(3, 3);
            btn_ClearText.Name = "btn_ClearText";
            btn_ClearText.Size = new System.Drawing.Size(74, 23);
            btn_ClearText.TabIndex = 0;
            btn_ClearText.Text = "Clear";
            btn_ClearText.UseMnemonic = false;
            btn_ClearText.UseVisualStyleBackColor = true;
            btn_ClearText.Click += btn_ClearText_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            ClientSize = new System.Drawing.Size(700, 338);
            Controls.Add(tableLayoutPanel1);
            ForeColor = System.Drawing.Color.White;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "MainForm";
            Text = "Slave Worker";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_ClearText;
    }
}

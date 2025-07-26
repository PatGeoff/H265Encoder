namespace MasterController
{
    partial class FFMPEGEditor
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            button2 = new System.Windows.Forms.Button();
            comboBox1 = new System.Windows.Forms.ComboBox();
            btn_ok = new System.Windows.Forms.Button();
            textBox_ffmpegCommand = new System.Windows.Forms.TextBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(textBox_ffmpegCommand, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.1550388F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.84496F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            tableLayoutPanel2.Controls.Add(button2, 1, 0);
            tableLayoutPanel2.Controls.Add(comboBox1, 0, 0);
            tableLayoutPanel2.Controls.Add(btn_ok, 2, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 412);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new System.Drawing.Size(794, 35);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // button2
            // 
            button2.Dock = System.Windows.Forms.DockStyle.Fill;
            button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button2.Location = new System.Drawing.Point(133, 3);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(509, 29);
            button2.TabIndex = 1;
            button2.Text = "Sauvegarder le preset";
            button2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBox1.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            comboBox1.ForeColor = System.Drawing.Color.White;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(3, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(124, 23);
            comboBox1.TabIndex = 2;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // btn_ok
            // 
            btn_ok.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_ok.Location = new System.Drawing.Point(648, 3);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new System.Drawing.Size(143, 29);
            btn_ok.TabIndex = 0;
            btn_ok.Text = "OK";
            btn_ok.UseVisualStyleBackColor = true;
            btn_ok.Click += btn_ok_Click;
            // 
            // textBox_ffmpegCommand
            // 
            textBox_ffmpegCommand.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            textBox_ffmpegCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_ffmpegCommand.ForeColor = System.Drawing.Color.White;
            textBox_ffmpegCommand.Location = new System.Drawing.Point(3, 3);
            textBox_ffmpegCommand.Multiline = true;
            textBox_ffmpegCommand.Name = "textBox_ffmpegCommand";
            textBox_ffmpegCommand.Size = new System.Drawing.Size(794, 403);
            textBox_ffmpegCommand.TabIndex = 2;
            // 
            // FFMPEGEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            ForeColor = System.Drawing.Color.White;
            Name = "FFMPEGEditor";
            Text = "Éditier la commande FFMPEG";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_ffmpegCommand;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
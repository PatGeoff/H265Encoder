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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlaveWidgetStatus));
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            lbl_ffFeedback = new System.Windows.Forms.Label();
            textBox_EndFrame = new System.Windows.Forms.TextBox();
            checkBox_Utiliser = new System.Windows.Forms.CheckBox();
            lbl_BladeName = new System.Windows.Forms.Label();
            lbl_bladeNetStat = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            textBox_StartFrame = new System.Windows.Forms.TextBox();
            btn_StartSlaveJob = new System.Windows.Forms.Button();
            btn_EditFFmpegCommand = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.Controls.Add(lbl_ffFeedback, 8, 0);
            tableLayoutPanel1.Controls.Add(textBox_EndFrame, 6, 0);
            tableLayoutPanel1.Controls.Add(checkBox_Utiliser, 0, 0);
            tableLayoutPanel1.Controls.Add(lbl_BladeName, 1, 0);
            tableLayoutPanel1.Controls.Add(lbl_bladeNetStat, 2, 0);
            tableLayoutPanel1.Controls.Add(label1, 3, 0);
            tableLayoutPanel1.Controls.Add(label2, 5, 0);
            tableLayoutPanel1.Controls.Add(textBox_StartFrame, 4, 0);
            tableLayoutPanel1.Controls.Add(btn_StartSlaveJob, 9, 0);
            tableLayoutPanel1.Controls.Add(btn_EditFFmpegCommand, 7, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1114, 31);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lbl_ffFeedback
            // 
            lbl_ffFeedback.AutoSize = true;
            lbl_ffFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_ffFeedback.ForeColor = System.Drawing.Color.White;
            lbl_ffFeedback.Location = new System.Drawing.Point(786, 1);
            lbl_ffFeedback.Name = "lbl_ffFeedback";
            lbl_ffFeedback.Size = new System.Drawing.Size(223, 29);
            lbl_ffFeedback.TabIndex = 9;
            lbl_ffFeedback.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_EndFrame
            // 
            textBox_EndFrame.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            textBox_EndFrame.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox_EndFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_EndFrame.ForeColor = System.Drawing.Color.White;
            textBox_EndFrame.Location = new System.Drawing.Point(531, 4);
            textBox_EndFrame.Multiline = true;
            textBox_EndFrame.Name = "textBox_EndFrame";
            textBox_EndFrame.Size = new System.Drawing.Size(94, 23);
            textBox_EndFrame.TabIndex = 6;
            textBox_EndFrame.Text = "0";
            textBox_EndFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            textBox_EndFrame.MouseLeave += textBox_EndFrame_TextChanged;
            // 
            // checkBox_Utiliser
            // 
            checkBox_Utiliser.AutoSize = true;
            checkBox_Utiliser.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            checkBox_Utiliser.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBox_Utiliser.Location = new System.Drawing.Point(4, 4);
            checkBox_Utiliser.Name = "checkBox_Utiliser";
            checkBox_Utiliser.Size = new System.Drawing.Size(15, 23);
            checkBox_Utiliser.TabIndex = 0;
            checkBox_Utiliser.UseVisualStyleBackColor = true;
            checkBox_Utiliser.CheckedChanged += checkBox_Utiliser_CheckedChanged;
            // 
            // lbl_BladeName
            // 
            lbl_BladeName.AutoSize = true;
            lbl_BladeName.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_BladeName.ForeColor = System.Drawing.Color.White;
            lbl_BladeName.Location = new System.Drawing.Point(26, 1);
            lbl_BladeName.Name = "lbl_BladeName";
            lbl_BladeName.Size = new System.Drawing.Size(94, 29);
            lbl_BladeName.TabIndex = 1;
            lbl_BladeName.Text = "Name";
            lbl_BladeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_bladeNetStat
            // 
            lbl_bladeNetStat.AutoSize = true;
            lbl_bladeNetStat.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_bladeNetStat.ForeColor = System.Drawing.Color.White;
            lbl_bladeNetStat.Location = new System.Drawing.Point(127, 1);
            lbl_bladeNetStat.Name = "lbl_bladeNetStat";
            lbl_bladeNetStat.Size = new System.Drawing.Size(94, 29);
            lbl_bladeNetStat.TabIndex = 2;
            lbl_bladeNetStat.Text = "Status";
            lbl_bladeNetStat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(228, 1);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(94, 29);
            label1.TabIndex = 3;
            label1.Text = "Frame départ";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(430, 1);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(94, 29);
            label2.TabIndex = 4;
            label2.Text = "Frame Fin";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_StartFrame
            // 
            textBox_StartFrame.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            textBox_StartFrame.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox_StartFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_StartFrame.ForeColor = System.Drawing.Color.White;
            textBox_StartFrame.Location = new System.Drawing.Point(329, 4);
            textBox_StartFrame.Multiline = true;
            textBox_StartFrame.Name = "textBox_StartFrame";
            textBox_StartFrame.Size = new System.Drawing.Size(94, 23);
            textBox_StartFrame.TabIndex = 5;
            textBox_StartFrame.Text = "0";
            textBox_StartFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            textBox_StartFrame.MouseLeave += textBox_EndFrame_TextChanged;
            // 
            // btn_StartSlaveJob
            // 
            btn_StartSlaveJob.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_StartSlaveJob.FlatAppearance.BorderSize = 0;
            btn_StartSlaveJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_StartSlaveJob.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btn_StartSlaveJob.ForeColor = System.Drawing.Color.White;
            btn_StartSlaveJob.Location = new System.Drawing.Point(1016, 4);
            btn_StartSlaveJob.Name = "btn_StartSlaveJob";
            btn_StartSlaveJob.Size = new System.Drawing.Size(94, 23);
            btn_StartSlaveJob.TabIndex = 7;
            btn_StartSlaveJob.Text = "Lancer";
            btn_StartSlaveJob.UseVisualStyleBackColor = true;
            btn_StartSlaveJob.Click += btn_StartSlaveJob_Click;
            // 
            // btn_EditFFmpegCommand
            // 
            btn_EditFFmpegCommand.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_EditFFmpegCommand.BackgroundImage");
            btn_EditFFmpegCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            btn_EditFFmpegCommand.FlatAppearance.BorderSize = 0;
            btn_EditFFmpegCommand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_EditFFmpegCommand.ForeColor = System.Drawing.Color.White;
            btn_EditFFmpegCommand.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            btn_EditFFmpegCommand.Location = new System.Drawing.Point(632, 4);
            btn_EditFFmpegCommand.Name = "btn_EditFFmpegCommand";
            btn_EditFFmpegCommand.Size = new System.Drawing.Size(147, 23);
            btn_EditFFmpegCommand.TabIndex = 8;
            btn_EditFFmpegCommand.Text = "commandes ffmpeg";
            btn_EditFFmpegCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            btn_EditFFmpegCommand.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            btn_EditFFmpegCommand.UseVisualStyleBackColor = true;
            btn_EditFFmpegCommand.Click += btn_EditFFmpegCommand_Click;
            // 
            // SlaveWidgetStatus
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            Controls.Add(tableLayoutPanel1);
            Name = "SlaveWidgetStatus";
            Size = new System.Drawing.Size(1114, 31);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox_Utiliser;
        private System.Windows.Forms.Label lbl_BladeName;
        private System.Windows.Forms.Label lbl_bladeNetStat;
        private System.Windows.Forms.TextBox textBox_EndFrame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_StartFrame;
        private System.Windows.Forms.Button btn_StartSlaveJob;
        private System.Windows.Forms.Button btn_EditFFmpegCommand;
        private System.Windows.Forms.Label lbl_ffFeedback;
    }
}

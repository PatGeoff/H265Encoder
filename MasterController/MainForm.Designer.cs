namespace MasterController
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panel1 = new System.Windows.Forms.Panel();
            button3 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            panel2 = new System.Windows.Forms.Panel();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            button5 = new System.Windows.Forms.Button();
            Panel_Encodage = new System.Windows.Forms.Panel();
            borderlessTabControl1 = new BorderlessTabControl();
            tabPage_Encode = new System.Windows.Forms.TabPage();
            listBox2 = new System.Windows.Forms.ListBox();
            flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            lbl_imageNameFormatted = new System.Windows.Forms.Label();
            btn_LoadSourceImages = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            lbl_SourceImagePath = new System.Windows.Forms.Label();
            tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            textBox_autoSplitNbr = new System.Windows.Forms.TextBox();
            textBox_NbrImages = new System.Windows.Forms.TextBox();
            button7 = new System.Windows.Forms.Button();
            btn_autoSplit = new System.Windows.Forms.Button();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            lbl_DestinationPath = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            btn_ChooseDestination = new System.Windows.Forms.Button();
            flowLayoutPanel_SlaveStatus = new System.Windows.Forms.FlowLayoutPanel();
            tabPage_Concat = new System.Windows.Forms.TabPage();
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            tabPage_Réglages = new System.Windows.Forms.TabPage();
            flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            btn_FFMPEGPath = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            textBox_FFMPEGpath = new System.Windows.Forms.TextBox();
            tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            lbl_ffmpegPresetPath = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            btn_ffPresetsPath = new System.Windows.Forms.Button();
            comboBox_ffPresets = new System.Windows.Forms.ComboBox();
            btn_RefreshPing = new System.Windows.Forms.Button();
            listBox1 = new System.Windows.Forms.ListBox();
            panel3 = new System.Windows.Forms.Panel();
            textBox_messageToSend = new System.Windows.Forms.TextBox();
            btn_SendMessage = new System.Windows.Forms.Button();
            button6 = new System.Windows.Forms.Button();
            flowLayoutPanel_SlaveList = new System.Windows.Forms.FlowLayoutPanel();
            toolTip_Editffmpeg = new System.Windows.Forms.ToolTip(components);
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            Panel_Encodage.SuspendLayout();
            borderlessTabControl1.SuspendLayout();
            tabPage_Encode.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tabPage_Concat.SuspendLayout();
            tabPage_Réglages.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(button5);
            panel1.Dock = System.Windows.Forms.DockStyle.Left;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(200, 794);
            panel1.TabIndex = 0;
            // 
            // button3
            // 
            button3.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            button3.Dock = System.Windows.Forms.DockStyle.Top;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            button3.ForeColor = System.Drawing.Color.LightGray;
            button3.Location = new System.Drawing.Point(0, 279);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(200, 69);
            button3.TabIndex = 3;
            button3.Text = "Réglages";
            button3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            button2.Dock = System.Windows.Forms.DockStyle.Top;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            button2.ForeColor = System.Drawing.Color.LightGray;
            button2.Location = new System.Drawing.Point(0, 210);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(200, 69);
            button2.TabIndex = 2;
            button2.Text = "Concaténation";
            button2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            button1.Dock = System.Windows.Forms.DockStyle.Top;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            button1.ForeColor = System.Drawing.Color.LightGray;
            button1.Location = new System.Drawing.Point(0, 141);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(200, 69);
            button1.TabIndex = 1;
            button1.Text = "Encodage";
            button1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button4
            // 
            button4.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            button4.Dock = System.Windows.Forms.DockStyle.Top;
            button4.Enabled = false;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button4.ForeColor = System.Drawing.Color.Gray;
            button4.Location = new System.Drawing.Point(0, 118);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(200, 23);
            button4.TabIndex = 4;
            button4.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(pictureBox1);
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 18);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(200, 100);
            panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(12, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(180, 90);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button5
            // 
            button5.Dock = System.Windows.Forms.DockStyle.Top;
            button5.Enabled = false;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button5.Location = new System.Drawing.Point(0, 0);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(200, 18);
            button5.TabIndex = 5;
            button5.UseVisualStyleBackColor = true;
            // 
            // Panel_Encodage
            // 
            Panel_Encodage.Controls.Add(borderlessTabControl1);
            Panel_Encodage.Dock = System.Windows.Forms.DockStyle.Fill;
            Panel_Encodage.Location = new System.Drawing.Point(200, 0);
            Panel_Encodage.Name = "Panel_Encodage";
            Panel_Encodage.Size = new System.Drawing.Size(1144, 794);
            Panel_Encodage.TabIndex = 1;
            // 
            // borderlessTabControl1
            // 
            borderlessTabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            borderlessTabControl1.Controls.Add(tabPage_Encode);
            borderlessTabControl1.Controls.Add(tabPage_Concat);
            borderlessTabControl1.Controls.Add(tabPage_Réglages);
            borderlessTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            borderlessTabControl1.ItemSize = new System.Drawing.Size(0, 1);
            borderlessTabControl1.Location = new System.Drawing.Point(0, 0);
            borderlessTabControl1.Name = "borderlessTabControl1";
            borderlessTabControl1.SelectedIndex = 0;
            borderlessTabControl1.Size = new System.Drawing.Size(1144, 794);
            borderlessTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            borderlessTabControl1.TabIndex = 0;
            // 
            // tabPage_Encode
            // 
            tabPage_Encode.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            tabPage_Encode.Controls.Add(listBox2);
            tabPage_Encode.Controls.Add(flowLayoutPanel3);
            tabPage_Encode.Controls.Add(flowLayoutPanel_SlaveStatus);
            tabPage_Encode.Location = new System.Drawing.Point(4, 5);
            tabPage_Encode.Name = "tabPage_Encode";
            tabPage_Encode.Padding = new System.Windows.Forms.Padding(3);
            tabPage_Encode.Size = new System.Drawing.Size(1136, 785);
            tabPage_Encode.TabIndex = 0;
            tabPage_Encode.Text = "tabPage1";
            // 
            // listBox2
            // 
            listBox2.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            listBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            listBox2.ForeColor = System.Drawing.Color.White;
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new System.Drawing.Point(6, 545);
            listBox2.Name = "listBox2";
            listBox2.Size = new System.Drawing.Size(1122, 227);
            listBox2.TabIndex = 10;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.AutoScroll = true;
            flowLayoutPanel3.Controls.Add(tableLayoutPanel2);
            flowLayoutPanel3.Controls.Add(tableLayoutPanel5);
            flowLayoutPanel3.Controls.Add(tableLayoutPanel4);
            flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel3.Location = new System.Drawing.Point(6, 18);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new System.Drawing.Size(1122, 118);
            flowLayoutPanel3.TabIndex = 9;
            flowLayoutPanel3.WrapContents = false;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 319F));
            tableLayoutPanel2.Controls.Add(lbl_imageNameFormatted, 3, 0);
            tableLayoutPanel2.Controls.Add(btn_LoadSourceImages, 0, 0);
            tableLayoutPanel2.Controls.Add(label3, 1, 0);
            tableLayoutPanel2.Controls.Add(lbl_SourceImagePath, 2, 0);
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(1115, 31);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lbl_imageNameFormatted
            // 
            lbl_imageNameFormatted.AutoSize = true;
            lbl_imageNameFormatted.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_imageNameFormatted.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbl_imageNameFormatted.ForeColor = System.Drawing.Color.White;
            lbl_imageNameFormatted.Location = new System.Drawing.Point(798, 1);
            lbl_imageNameFormatted.Name = "lbl_imageNameFormatted";
            lbl_imageNameFormatted.Size = new System.Drawing.Size(313, 29);
            lbl_imageNameFormatted.TabIndex = 3;
            lbl_imageNameFormatted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl_imageNameFormatted.UseMnemonic = false;
            // 
            // btn_LoadSourceImages
            // 
            btn_LoadSourceImages.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_LoadSourceImages.BackgroundImage");
            btn_LoadSourceImages.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            btn_LoadSourceImages.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_LoadSourceImages.FlatAppearance.BorderSize = 0;
            btn_LoadSourceImages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_LoadSourceImages.Location = new System.Drawing.Point(4, 4);
            btn_LoadSourceImages.Name = "btn_LoadSourceImages";
            btn_LoadSourceImages.Size = new System.Drawing.Size(25, 23);
            btn_LoadSourceImages.TabIndex = 0;
            btn_LoadSourceImages.UseVisualStyleBackColor = true;
            btn_LoadSourceImages.Click += btn_LoadSourceImages_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Fill;
            label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.ForeColor = System.Drawing.Color.White;
            label3.Location = new System.Drawing.Point(36, 1);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(87, 29);
            label3.TabIndex = 1;
            label3.Text = "Images source";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SourceImagePath
            // 
            lbl_SourceImagePath.AutoSize = true;
            lbl_SourceImagePath.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_SourceImagePath.ForeColor = System.Drawing.Color.White;
            lbl_SourceImagePath.Location = new System.Drawing.Point(130, 1);
            lbl_SourceImagePath.Name = "lbl_SourceImagePath";
            lbl_SourceImagePath.Size = new System.Drawing.Size(661, 29);
            lbl_SourceImagePath.TabIndex = 2;
            lbl_SourceImagePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel5.ColumnCount = 5;
            tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 179F));
            tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 982F));
            tableLayoutPanel5.Controls.Add(textBox_autoSplitNbr, 3, 0);
            tableLayoutPanel5.Controls.Add(textBox_NbrImages, 1, 0);
            tableLayoutPanel5.Controls.Add(button7, 0, 0);
            tableLayoutPanel5.Controls.Add(btn_autoSplit, 2, 0);
            tableLayoutPanel5.Location = new System.Drawing.Point(3, 40);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new System.Drawing.Size(1115, 31);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // textBox_autoSplitNbr
            // 
            textBox_autoSplitNbr.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            textBox_autoSplitNbr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox_autoSplitNbr.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_autoSplitNbr.ForeColor = System.Drawing.Color.White;
            textBox_autoSplitNbr.Location = new System.Drawing.Point(372, 4);
            textBox_autoSplitNbr.Name = "textBox_autoSplitNbr";
            textBox_autoSplitNbr.Size = new System.Drawing.Size(59, 23);
            textBox_autoSplitNbr.TabIndex = 6;
            textBox_autoSplitNbr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            textBox_autoSplitNbr.WordWrap = false;
            // 
            // textBox_NbrImages
            // 
            textBox_NbrImages.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            textBox_NbrImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox_NbrImages.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_NbrImages.ForeColor = System.Drawing.Color.White;
            textBox_NbrImages.Location = new System.Drawing.Point(184, 4);
            textBox_NbrImages.Name = "textBox_NbrImages";
            textBox_NbrImages.Size = new System.Drawing.Size(93, 23);
            textBox_NbrImages.TabIndex = 3;
            textBox_NbrImages.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            textBox_NbrImages.WordWrap = false;
            // 
            // button7
            // 
            button7.Dock = System.Windows.Forms.DockStyle.Fill;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button7.ForeColor = System.Drawing.Color.White;
            button7.Location = new System.Drawing.Point(4, 4);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(173, 23);
            button7.TabIndex = 4;
            button7.Text = "Calculer nombre d'images";
            button7.UseVisualStyleBackColor = true;
            button7.Click += btn_LoadSourceImages_Click;
            // 
            // btn_autoSplit
            // 
            btn_autoSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_autoSplit.FlatAppearance.BorderSize = 0;
            btn_autoSplit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_autoSplit.ForeColor = System.Drawing.Color.White;
            btn_autoSplit.Location = new System.Drawing.Point(284, 4);
            btn_autoSplit.Name = "btn_autoSplit";
            btn_autoSplit.Size = new System.Drawing.Size(81, 23);
            btn_autoSplit.TabIndex = 5;
            btn_autoSplit.Text = "Auto Split";
            btn_autoSplit.UseVisualStyleBackColor = true;
            btn_autoSplit.Click += btn_autoSplit_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(lbl_DestinationPath, 2, 0);
            tableLayoutPanel4.Controls.Add(label5, 1, 0);
            tableLayoutPanel4.Controls.Add(btn_ChooseDestination, 0, 0);
            tableLayoutPanel4.Location = new System.Drawing.Point(3, 77);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new System.Drawing.Size(1115, 31);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // lbl_DestinationPath
            // 
            lbl_DestinationPath.AutoSize = true;
            lbl_DestinationPath.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_DestinationPath.ForeColor = System.Drawing.Color.White;
            lbl_DestinationPath.Location = new System.Drawing.Point(130, 1);
            lbl_DestinationPath.Name = "lbl_DestinationPath";
            lbl_DestinationPath.Size = new System.Drawing.Size(981, 29);
            lbl_DestinationPath.TabIndex = 3;
            lbl_DestinationPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = System.Windows.Forms.DockStyle.Fill;
            label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(36, 1);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(87, 29);
            label5.TabIndex = 2;
            label5.Text = "Destination";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_ChooseDestination
            // 
            btn_ChooseDestination.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_ChooseDestination.BackgroundImage");
            btn_ChooseDestination.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            btn_ChooseDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_ChooseDestination.FlatAppearance.BorderSize = 0;
            btn_ChooseDestination.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_ChooseDestination.Location = new System.Drawing.Point(4, 4);
            btn_ChooseDestination.Name = "btn_ChooseDestination";
            btn_ChooseDestination.Size = new System.Drawing.Size(25, 23);
            btn_ChooseDestination.TabIndex = 1;
            btn_ChooseDestination.UseVisualStyleBackColor = true;
            btn_ChooseDestination.Click += btn_ChooseDestination_Click;
            // 
            // flowLayoutPanel_SlaveStatus
            // 
            flowLayoutPanel_SlaveStatus.AutoScroll = true;
            flowLayoutPanel_SlaveStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            flowLayoutPanel_SlaveStatus.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel_SlaveStatus.Location = new System.Drawing.Point(6, 162);
            flowLayoutPanel_SlaveStatus.Name = "flowLayoutPanel_SlaveStatus";
            flowLayoutPanel_SlaveStatus.Size = new System.Drawing.Size(1122, 377);
            flowLayoutPanel_SlaveStatus.TabIndex = 0;
            flowLayoutPanel_SlaveStatus.WrapContents = false;
            // 
            // tabPage_Concat
            // 
            tabPage_Concat.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            tabPage_Concat.Controls.Add(flowLayoutPanel2);
            tabPage_Concat.Location = new System.Drawing.Point(4, 5);
            tabPage_Concat.Name = "tabPage_Concat";
            tabPage_Concat.Padding = new System.Windows.Forms.Padding(3);
            tabPage_Concat.Size = new System.Drawing.Size(1136, 785);
            tabPage_Concat.TabIndex = 1;
            tabPage_Concat.Text = "tabPage2";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            flowLayoutPanel2.Location = new System.Drawing.Point(84, 74);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(584, 368);
            flowLayoutPanel2.TabIndex = 1;
            // 
            // tabPage_Réglages
            // 
            tabPage_Réglages.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            tabPage_Réglages.Controls.Add(flowLayoutPanel4);
            tabPage_Réglages.Controls.Add(btn_RefreshPing);
            tabPage_Réglages.Controls.Add(listBox1);
            tabPage_Réglages.Controls.Add(panel3);
            tabPage_Réglages.Controls.Add(button6);
            tabPage_Réglages.Controls.Add(flowLayoutPanel_SlaveList);
            tabPage_Réglages.Location = new System.Drawing.Point(4, 5);
            tabPage_Réglages.Name = "tabPage_Réglages";
            tabPage_Réglages.Padding = new System.Windows.Forms.Padding(3);
            tabPage_Réglages.Size = new System.Drawing.Size(1136, 785);
            tabPage_Réglages.TabIndex = 2;
            tabPage_Réglages.Text = "tabPage3";
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Controls.Add(tableLayoutPanel1);
            flowLayoutPanel4.Controls.Add(tableLayoutPanel6);
            flowLayoutPanel4.Location = new System.Drawing.Point(402, 7);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new System.Drawing.Size(726, 101);
            flowLayoutPanel4.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 139F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 341F));
            tableLayoutPanel1.Controls.Add(btn_FFMPEGPath, 0, 0);
            tableLayoutPanel1.Controls.Add(label1, 1, 0);
            tableLayoutPanel1.Controls.Add(textBox_FFMPEGpath, 2, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(723, 33);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_FFMPEGPath
            // 
            btn_FFMPEGPath.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_FFMPEGPath.BackgroundImage");
            btn_FFMPEGPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            btn_FFMPEGPath.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_FFMPEGPath.FlatAppearance.BorderSize = 0;
            btn_FFMPEGPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_FFMPEGPath.Location = new System.Drawing.Point(4, 4);
            btn_FFMPEGPath.Name = "btn_FFMPEGPath";
            btn_FFMPEGPath.Size = new System.Drawing.Size(29, 25);
            btn_FFMPEGPath.TabIndex = 0;
            btn_FFMPEGPath.UseVisualStyleBackColor = true;
            btn_FFMPEGPath.Click += btn_FFMPEGPath_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(40, 1);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(133, 31);
            label1.TabIndex = 1;
            label1.Text = "Chemin vers FFMPEG";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_FFMPEGpath
            // 
            textBox_FFMPEGpath.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            textBox_FFMPEGpath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox_FFMPEGpath.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_FFMPEGpath.ForeColor = System.Drawing.Color.White;
            textBox_FFMPEGpath.Location = new System.Drawing.Point(180, 4);
            textBox_FFMPEGpath.Multiline = true;
            textBox_FFMPEGpath.Name = "textBox_FFMPEGpath";
            textBox_FFMPEGpath.Size = new System.Drawing.Size(539, 25);
            textBox_FFMPEGpath.TabIndex = 2;
            textBox_FFMPEGpath.Text = "\"C:\\d7software\\utilities\\FFMPEG.new\\ffmpeg.exe\"";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel6.ColumnCount = 4;
            tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel6.Controls.Add(lbl_ffmpegPresetPath, 2, 0);
            tableLayoutPanel6.Controls.Add(label2, 1, 0);
            tableLayoutPanel6.Controls.Add(btn_ffPresetsPath, 0, 0);
            tableLayoutPanel6.Controls.Add(comboBox_ffPresets, 3, 0);
            tableLayoutPanel6.Location = new System.Drawing.Point(3, 42);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new System.Drawing.Size(723, 31);
            tableLayoutPanel6.TabIndex = 5;
            // 
            // lbl_ffmpegPresetPath
            // 
            lbl_ffmpegPresetPath.AutoSize = true;
            lbl_ffmpegPresetPath.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl_ffmpegPresetPath.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbl_ffmpegPresetPath.ForeColor = System.Drawing.Color.White;
            lbl_ffmpegPresetPath.Location = new System.Drawing.Point(180, 1);
            lbl_ffmpegPresetPath.Name = "lbl_ffmpegPresetPath";
            lbl_ffmpegPresetPath.Size = new System.Drawing.Size(438, 29);
            lbl_ffmpegPresetPath.TabIndex = 5;
            lbl_ffmpegPresetPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(42, 1);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(131, 29);
            label2.TabIndex = 4;
            label2.Text = "Presets FFMPEG";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_ffPresetsPath
            // 
            btn_ffPresetsPath.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_ffPresetsPath.BackgroundImage");
            btn_ffPresetsPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            btn_ffPresetsPath.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_ffPresetsPath.FlatAppearance.BorderSize = 0;
            btn_ffPresetsPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_ffPresetsPath.Location = new System.Drawing.Point(4, 4);
            btn_ffPresetsPath.Name = "btn_ffPresetsPath";
            btn_ffPresetsPath.Size = new System.Drawing.Size(31, 23);
            btn_ffPresetsPath.TabIndex = 3;
            btn_ffPresetsPath.UseVisualStyleBackColor = true;
            btn_ffPresetsPath.Click += btn_ffPresetsPath_Click;
            // 
            // comboBox_ffPresets
            // 
            comboBox_ffPresets.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            comboBox_ffPresets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            comboBox_ffPresets.ForeColor = System.Drawing.Color.White;
            comboBox_ffPresets.FormattingEnabled = true;
            comboBox_ffPresets.Location = new System.Drawing.Point(625, 4);
            comboBox_ffPresets.Name = "comboBox_ffPresets";
            comboBox_ffPresets.Size = new System.Drawing.Size(94, 23);
            comboBox_ffPresets.TabIndex = 2;
            comboBox_ffPresets.Text = "Preset";
            comboBox_ffPresets.SelectedIndexChanged += comboBox_ffPresets_SelectedIndexChanged;
            // 
            // btn_RefreshPing
            // 
            btn_RefreshPing.Location = new System.Drawing.Point(179, 349);
            btn_RefreshPing.Name = "btn_RefreshPing";
            btn_RefreshPing.Size = new System.Drawing.Size(217, 23);
            btn_RefreshPing.TabIndex = 6;
            btn_RefreshPing.Text = "Rafraichir la liste";
            btn_RefreshPing.UseVisualStyleBackColor = true;
            btn_RefreshPing.Click += btn_RefreshPing_Click;
            // 
            // listBox1
            // 
            listBox1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            listBox1.ForeColor = System.Drawing.Color.White;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new System.Drawing.Point(5, 565);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(391, 214);
            listBox1.TabIndex = 5;
            // 
            // panel3
            // 
            panel3.Controls.Add(textBox_messageToSend);
            panel3.Controls.Add(btn_SendMessage);
            panel3.Location = new System.Drawing.Point(6, 510);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(390, 49);
            panel3.TabIndex = 4;
            // 
            // textBox_messageToSend
            // 
            textBox_messageToSend.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            textBox_messageToSend.Dock = System.Windows.Forms.DockStyle.Top;
            textBox_messageToSend.ForeColor = System.Drawing.Color.White;
            textBox_messageToSend.Location = new System.Drawing.Point(0, 23);
            textBox_messageToSend.Name = "textBox_messageToSend";
            textBox_messageToSend.Size = new System.Drawing.Size(390, 23);
            textBox_messageToSend.TabIndex = 3;
            // 
            // btn_SendMessage
            // 
            btn_SendMessage.Dock = System.Windows.Forms.DockStyle.Top;
            btn_SendMessage.Location = new System.Drawing.Point(0, 0);
            btn_SendMessage.Name = "btn_SendMessage";
            btn_SendMessage.Size = new System.Drawing.Size(390, 23);
            btn_SendMessage.TabIndex = 4;
            btn_SendMessage.Text = "Envoyer le message";
            btn_SendMessage.UseVisualStyleBackColor = true;
            btn_SendMessage.Click += btn_SendMessage_Click;
            // 
            // button6
            // 
            button6.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button6.ForeColor = System.Drawing.Color.White;
            button6.Location = new System.Drawing.Point(6, 349);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(167, 23);
            button6.TabIndex = 2;
            button6.Text = "Ajouter";
            button6.UseVisualStyleBackColor = false;
            button6.Click += buttonAdd_Click;
            // 
            // flowLayoutPanel_SlaveList
            // 
            flowLayoutPanel_SlaveList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            flowLayoutPanel_SlaveList.Location = new System.Drawing.Point(6, 8);
            flowLayoutPanel_SlaveList.Name = "flowLayoutPanel_SlaveList";
            flowLayoutPanel_SlaveList.Size = new System.Drawing.Size(390, 335);
            flowLayoutPanel_SlaveList.TabIndex = 1;
            // 
            // MainForm
            // 
            BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            ClientSize = new System.Drawing.Size(1344, 794);
            Controls.Add(Panel_Encodage);
            Controls.Add(panel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "Master Controller";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            Panel_Encodage.ResumeLayout(false);
            borderlessTabControl1.ResumeLayout(false);
            tabPage_Encode.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tabPage_Concat.ResumeLayout(false);
            tabPage_Réglages.ResumeLayout(false);
            flowLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel Panel_Encodage;
        private BorderlessTabControl borderlessTabControl1;
        private System.Windows.Forms.TabPage tabPage_Encode;
        private System.Windows.Forms.TabPage tabPage_Concat;
        private System.Windows.Forms.TabPage tabPage_Réglages;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_SlaveStatus;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_SlaveList;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox_messageToSend;
        private System.Windows.Forms.Button btn_SendMessage;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_RefreshPing;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_FFMPEGPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_LoadSourceImages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_SourceImagePath;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lbl_DestinationPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_ChooseDestination;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox textBox_NbrImages;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ToolTip toolTip_Editffmpeg;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btn_autoSplit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btn_ffPresetsPath;
        private System.Windows.Forms.ComboBox comboBox_ffPresets;
        private System.Windows.Forms.TextBox textBox_autoSplitNbr;
        private System.Windows.Forms.TextBox textBox_FFMPEGpath;
        private System.Windows.Forms.Label lbl_ffmpegPresetPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_imageNameFormatted;
    }
}

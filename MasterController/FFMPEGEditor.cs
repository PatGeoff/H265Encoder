using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterController
{
    public partial class FFMPEGEditor : Form
    {
        private SlaveConfig slave;
        private AppSettings settings;
        public FFMPEGEditor(SlaveConfig _slave, AppSettings _settings)
        {
            InitializeComponent();
            this.slave = _slave;
            this.settings = _settings;          


            textBox_ffmpegCommand.Text = slave.FFMPEGCommand;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}

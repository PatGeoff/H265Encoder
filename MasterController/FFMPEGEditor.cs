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

            if (string.IsNullOrEmpty(slave.FFMPEGCommand))
            {
                var sb = new StringBuilder();
                sb.Append("start /affinity FFFFFFFFFFFFFFFF \"FFmpeg\" ");
                sb.AppendFormat("\"{0}\" -hide_banner -r 60 ", settings.FFMPEGPath);

                // Add start_number and frames:v
                sb.AppendFormat("-start_number {0} ", slave.StartFrame);
                sb.AppendFormat("-i \"{0}\" ", Path.Combine(settings.SourceImagePath, settings.ImageName));
                sb.AppendFormat("-frames:v {0} ", slave.FrameCount);

                sb.Append("-b:v 100M -maxrate 100M -minrate 100M -bufsize 100M ");
                sb.Append("-vf scale=5948:5948 -c:v libx265 -pix_fmt yuv420p10le -profile:v main10 ");
                sb.Append("-threads 72 -x265-params \"pools=2:frame-threads=16:wpp=1\" -y ");
                sb.AppendFormat("\"{0}\"", Path.Combine(settings.DestinationPath, settings.DestinationName));


                slave.FFMPEGCommand = sb.ToString();
            }


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

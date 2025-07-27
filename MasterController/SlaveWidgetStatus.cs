using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterController
{
    public partial class SlaveWidgetStatus : UserControl
    {
        public event EventHandler DeleteRequested;
        private SlaveConfig _slave;
        private AppSettings _settings;

        public SlaveWidgetStatus(SlaveConfig slave, AppSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            _slave = slave;
            _slave.PropertyChanged += Config_PropertyChanged;
            this.AutoSize = false;
            this.Dock = DockStyle.None;
            this.Name = slave.Name;
            SetFFMPEGCommand();
            UpdateDisplay();
        }
        public void SetFFMPEGCommand()
        {

            string nomImage = _settings.DestinationName;

            if (nomImage.Contains("."))
            {
                string[] noms = nomImage.Split('.');
                nomImage = $"{noms[0]}_{_slave.Part}.{noms[1]}";
            }


            string inputPath = Path.Combine(
     _settings.SourceImagePath.TrimEnd('\\'),
     _settings.ImageName.TrimStart('\\')
 );

            var sb = new StringBuilder();
            sb.AppendFormat("\"{0}\" -r {1} ", _settings.FFMPEGPath, _settings.FFframeRate);
            sb.AppendFormat("-start_number {0} ", _slave.StartFrame);
            sb.AppendFormat("-i \"{0}\" ", inputPath);
            sb.AppendFormat("-frames:v {0} ", _slave.FrameCount);

            sb.AppendFormat("-b:v {0}M -maxrate {0}M -minrate {0}M -bufsize {0}M ", _settings.FFBitrate);
            sb.AppendFormat("-vf scale=5948:5948 -c:v libx265 -pix_fmt {0} -profile:v {1} ", _settings.FFFormat, _settings.FFProfile);
            sb.Append("-threads 72 -x265-params \"pools=2:frame-threads=16:wpp=1\" -y ");
            sb.AppendFormat("\"{0}\"", Path.Combine(_settings.DestinationPath, nomImage));


            _slave.FFMPEGCommand = sb.ToString();

        }


        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update only the relevant part of the UI
            if (e.PropertyName == nameof(SlaveConfig.NetworkStatus) || e.PropertyName == nameof(SlaveConfig.Utiliser) || e.PropertyName == nameof(SlaveConfig.StartFrame) || e.PropertyName == nameof(SlaveConfig.EndFrame))
            {
                UpdateDisplay();
            }
        }

        public bool UtiliserChecked
        {
            get => checkBox_Utiliser.Checked;
            set => checkBox_Utiliser.Checked = value;
        }


        public void UpdateDisplay()
        {
            lbl_BladeName.Text = _slave.Name;
            lbl_bladeNetStat.Text = _slave.NetworkStatus ? "Online" : "Offline";
            lbl_bladeNetStat.ForeColor = _slave.NetworkStatus ? Color.Green : Color.Red;
            textBox_StartFrame.Text = _slave.StartFrame.ToString();
            textBox_EndFrame.Text = _slave.EndFrame.ToString();
        }


        private void btn_deleteBlade_Click(object sender, EventArgs e)
        {
            this.Parent?.Controls.Remove(this);
            DeleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private void checkBox_Utiliser_CheckedChanged(object sender, EventArgs e)
        {
            if (_slave.NetworkStatus)
            {
                _slave.Utiliser = checkBox_Utiliser.Checked;
            }
            else
            {
                checkBox_Utiliser.Checked = false;
            }

        }

        private void btn_StartSlaveJob_Click(object sender, EventArgs e)
        {            
            Task.Run(() => StartSlaveJob());
        }

        private async void StartSlaveJob()
        {
            
            SetFFMPEGCommand();
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    try
                    {
                        await client.ConnectAsync(_slave.Ip, _slave.Port);
                        //MessageBox.Show("Connection successful!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    using (NetworkStream stream = client.GetStream())
                    {
                        // Send the command
                        byte[] commandBytes = Encoding.UTF8.GetBytes("StartJob " + _slave.FFMPEGCommand + "\n");
                        //byte[] commandBytes = Encoding.UTF8.GetBytes("Allo\n");
                        await stream.WriteAsync(commandBytes, 0, commandBytes.Length);

                        // Read the response
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            string line;
                            string lastLine = string.Empty;

                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                lastLine = line;
                                if (lastLine.Contains("frame"))
                                {
                                    string ligne = lastLine.Split("fps")[0];
                                    lastLine = ligne + " / " + _slave.FrameCount; 

                                }
                               
                                    lbl_ffFeedback.Invoke((MethodInvoker)(() => lbl_ffFeedback.Text = lastLine));
                                
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with slave: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btn_EditFFmpegCommand_Click(object sender, EventArgs e)
        {
            SetFFMPEGCommand();
            var editFFMPEGForm = new FFMPEGEditor(_slave, _settings);
            editFFMPEGForm.Show();
        }

        private void textBox_StartFrame_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _slave.StartFrame = int.Parse(textBox_StartFrame.Text);
                _slave.FrameCount = _slave.EndFrame - _slave.StartFrame;
                SetFFMPEGCommand();
            }
            catch (Exception)
            {
                MessageBox.Show("Svp entrer un nombre valide");
            }

        }

        private void textBox_EndFrame_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _slave.EndFrame = int.Parse(textBox_EndFrame.Text);
                _slave.FrameCount = _slave.EndFrame - _slave.StartFrame;
                SetFFMPEGCommand();
            }
            catch (Exception)
            {
                MessageBox.Show("Svp entrer un nombre valide");
            }

        }
    }
}

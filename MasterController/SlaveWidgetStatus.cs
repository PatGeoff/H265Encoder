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
            UpdateDisplay();
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
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(_slave.Ip, _slave.Port);
                    using (NetworkStream stream = client.GetStream())
                    {
                        // Send the command
                        byte[] commandBytes = Encoding.UTF8.GetBytes(_slave.FFMPEGCommand + "\n");
                        await stream.WriteAsync(commandBytes, 0, commandBytes.Length);

                        // Read the response
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            string line;
                            string lastLine = string.Empty;

                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                lastLine = line;
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
            var editFFMPEGForm = new FFMPEGEditor(_slave, _settings);
            editFFMPEGForm.Show();
        }

       
    }
}

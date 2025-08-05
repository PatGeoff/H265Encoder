using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterController
{
    public partial class SlaveWidgetStatus : UserControl
    {
        public event EventHandler DeleteRequested;
        public SlaveConfig _slave;
        private AppSettings _settings;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isJobRunning = false;


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

        public void SetSuffixNbr()
        {
            if (checkBox_Utiliser.Checked)
            {
                string nomImage = _settings.DestinationName;
                string extension = Path.GetExtension(nomImage);
                string baseName = Path.GetFileNameWithoutExtension(nomImage);
                string destinationFolder = _settings.DestinationPath;
                string[] existingFiles = Array.Empty<string>();
                try
                {
                    existingFiles = Directory.GetFiles(destinationFolder, $"{baseName}_*.{extension.TrimStart('.')}");
                }
                catch (Exception)
                {

                }


                int maxIndex = -1;

                if (existingFiles.Length != 0)
                {
                    foreach (var file in existingFiles)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string suffix = fileName.Substring(baseName.Length + 1);
                        if (int.TryParse(suffix, out int index))
                        {
                            if (index > maxIndex)
                                maxIndex = index;
                        }
                    }
                }
                int indexInList = MainForm.slaveWidgetStatusList.FindIndex(widget => widget.UtiliserChecked == true);               
                int nextIndex = maxIndex + 1 + indexInList;

                textBox_SeqNbr.Text = nextIndex.ToString();
            }
        }

        public void SetFFMPEGCommand()
        {
            SetSuffixNbr();
            string nomImage = _settings.DestinationName;
            string extension = Path.GetExtension(nomImage);
            string baseName = Path.GetFileNameWithoutExtension(nomImage);
            string destinationFolder = _settings.DestinationPath;
            string index = textBox_SeqNbr.Text;

            string finalName = $"{baseName}_{index}{extension}";

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
            sb.AppendFormat("-vf scale={0} -c:v libx265 {1} ", _settings.FFResolution, _settings.FFProfile);
            sb.Append("-threads 72 -x265-params \"pools=2:frame-threads=16:wpp=1\" -y ");
            sb.AppendFormat("\"{0}\"", Path.Combine(destinationFolder, finalName));
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
            if (checkBox_Utiliser.Checked)
            {
                lbl_BladeName.ForeColor = Color.White;
            }
            else
            {
                lbl_BladeName.ForeColor = Color.DarkRed;
            }

        }

        private void btn_StartSlaveJob_Click(object sender, EventArgs e)
        {
            if (_isJobRunning)
            {
                CancelSlaveJob();
                btn_StartSlaveJob.Text = "Lancer";
            }
            else
            {
                StartSlaveJob();
                btn_StartSlaveJob.Text = "Arrêter";
            }
        }

        public void StartSlaveJob()
        {
            if (_isJobRunning)
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            _isJobRunning = true;

            Task.Run(() => StartSlaveJob(_cancellationTokenSource.Token));
        }

        public void CancelSlaveJob()
        {
            if (_isJobRunning && _cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _isJobRunning = false;
            }
        }


        public async void StartSlaveJob(CancellationToken token)
        {
            SetFFMPEGCommand();
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(_slave.Ip, _slave.Port);

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] commandBytes = Encoding.UTF8.GetBytes("StartJob " + _slave.FFMPEGCommand + "\n");
                        await stream.WriteAsync(commandBytes, 0, commandBytes.Length, token);

                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            string line;
                            string lastLine = string.Empty;

                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    break;
                                }

                                lastLine = line;
                                if (lastLine.Contains("frame"))
                                {
                                    string ligne = lastLine.Split("fps")[0];
                                    lastLine = ligne + " / " + _slave.FrameCount;
                                }
                                //if (lastLine.Contains("perdue."))
                                //{
                                //    string ligne = lastLine.Split("perdue.")[1];
                                //    lastLine = ligne;
                                //}

                                lbl_ffFeedback.Invoke((MethodInvoker)(() => lbl_ffFeedback.Text = lastLine));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions if needed
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    btn_StartSlaveJob.Invoke((MethodInvoker)(() => btn_StartSlaveJob.Text = "Lancer"));
                    _isJobRunning = false;
                }
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

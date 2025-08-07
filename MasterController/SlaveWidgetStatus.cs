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
        private MainForm _mainForm;
        public int startFrame, endFrame, frameCount;
        private bool utiliser;
        public string nomImage, extension, baseName, destinationFolder, finalName, inputPath, index, ffpath, ffres, ffprofile;
        private int ffrate, ffbitrate;

        public SlaveWidgetStatus(SlaveConfig slave, AppSettings settings, MainForm mainForm)
        {
            InitializeComponent();
            _settings = settings;
            _slave = slave;
            _slave.PropertyChanged += Config_PropertyChanged;
            _settings.PropertyChanged += Settings_PropertyChanged;
            _mainForm = mainForm;
            this.AutoSize = false;
            this.Dock = DockStyle.None;
            this.Name = slave.Name;
            Task.Run(() => SetFFMPEGCommand());
            UpdateDisplay();
        }

        public void SetSuffixNbr(string[] existingFiles, int maxIndex, int indexInList)
        {
            if (checkBox_Utiliser.Checked)
            {
                int nextIndex = maxIndex + 1 + indexInList;
                index = nextIndex.ToString("D2");
                textBox_SeqNbr.Text = index;
                SetVariables();
            }
        }


        public void SetVariables()
        {
            ffpath = _settings.FFMPEGPath;
            ffrate = _settings.FFframeRate;
            ffbitrate = _settings.FFBitrate;
            ffres = _settings.FFResolution;
            ffprofile = _settings.FFProfile;

            nomImage = _settings.DestinationName;
            extension = Path.GetExtension(nomImage);
            baseName = Path.GetFileNameWithoutExtension(nomImage);
            destinationFolder = _settings.DestinationPath;
            index = textBox_SeqNbr.Text;

            finalName = $"{baseName}_{index}{extension}";

            inputPath = Path.Combine(
               _settings.SourceImagePath.TrimEnd('\\'),
               _settings.ImageName.TrimStart('\\')
           );

        }

        public async Task SetFFMPEGCommand()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("\"{0}\" -r {1} ", ffpath, ffrate);
            sb.AppendFormat("-start_number {0} ", startFrame);
            sb.AppendFormat("-i \"{0}\" ", inputPath);
            sb.AppendFormat("-frames:v {0} ", frameCount);
            sb.AppendFormat("-b:v {0}M -maxrate {0}M -minrate {0}M -bufsize {0}M ", ffbitrate);
            sb.AppendFormat("-vf scale={0} -c:v libx265 {1} ", ffres , ffprofile);
            sb.Append("-threads 72 -x265-params \"pools=2:frame-threads=16:wpp=1\" -y ");
            sb.AppendFormat("\"{0}\"", Path.Combine(destinationFolder, finalName));
            _slave.FFMPEGCommand = sb.ToString();

            _mainForm.SetButtonColor();
        }

        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update only the relevant part of the UI
            if (e.PropertyName == nameof(SlaveConfig.NetworkStatus) || e.PropertyName == nameof(startFrame) || e.PropertyName == nameof(endFrame))
            {
                UpdateDisplay();
                SetVariables();
            }

        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_settings.DestinationPath) || e.PropertyName == nameof(_settings.DestinationName) || e.PropertyName == nameof(_settings.SourceImagePath) || e.PropertyName == nameof(_settings.ImageName) || e.PropertyName == nameof(_settings.FFMPEGPath) || e.PropertyName == nameof(_settings.FFframeRate) || e.PropertyName == nameof(_settings.FFBitrate) || e.PropertyName == nameof(_settings.FFResolution))
            {
                SetVariables();
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
            textBox_StartFrame.Text = startFrame.ToString();
            textBox_EndFrame.Text = endFrame.ToString();
            Task.Delay(100);
        }

        private void checkBox_Utiliser_CheckedChanged(object sender, EventArgs e)
        {
            if (_slave.NetworkStatus)
            {
                utiliser = checkBox_Utiliser.Checked;

            }
            else
            {
                checkBox_Utiliser.Checked = false;

            }
            SetNameColor();
            _mainForm.UpdateNumberBlades();
            if (!checkBox_Utiliser.Checked)
            {
                startFrame = 0;
                endFrame = 0;
                textBox_SeqNbr.Text = "-";
                UpdateDisplay();
            }

        }

        public void SetNameColor()
        {
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
                btn_StartSlaveJob.Text = "Lancer";
            }
        }

        public async void StartSlaveJob(CancellationToken token)
        {
            Task.Run(() => SetFFMPEGCommand());
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
                                    lastLine = ligne + " / " + frameCount;
                                }

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

        public void ResetButtonText()
        {
            btn_StartSlaveJob.Text = "Lancer";
        }

        private void btn_EditFFmpegCommand_Click(object sender, EventArgs e)
        {
            SetVariables();
            Task.Run(() => SetFFMPEGCommand());
            var editFFMPEGForm = new FFMPEGEditor(_slave, _settings);
            editFFMPEGForm.Show();
        }

        private void textBox_StartFrame_TextChanged(object sender, EventArgs e)
        {
            try
            {
                startFrame = int.Parse(textBox_StartFrame.Text);
                frameCount = endFrame - startFrame;
                Task.Run(() => SetFFMPEGCommand());
            }
            catch (Exception)
            {
                MessageBox.Show("Svp entrer un nombre valide");
            }

        }

        public void ClearFFConsole()
        {
            lbl_ffFeedback.Text = string.Empty;
        }

        private void textBox_EndFrame_TextChanged(object sender, EventArgs e)
        {
            try
            {
                endFrame = int.Parse(textBox_EndFrame.Text);
                frameCount = endFrame - startFrame;
                Task.Run(() => SetFFMPEGCommand());
            }
            catch (Exception)
            {
                MessageBox.Show("Svp entrer un nombre valide");
            }

        }
    }
}

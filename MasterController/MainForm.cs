using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MasterController
{
    public partial class MainForm : Form
    {
        private List<SlaveConfig> slaves = new();
        private string configPath = "config.json";

        private AppSettings settings;

        private Dictionary<string, SlaveWidget> widgetMap = new();
        private Dictionary<string, SlaveWidgetStatus> widgetStatusMap = new();
        private Timer refreshTimer;

        public MainForm()
        {
            InitializeComponent();
            settings = new AppSettings(configPath);
            LoadSettings();
            LoadSlaves();
            InitializeWidgets();
            StartRefreshTimer();
        }


        private void InitializeWidgets()
        {
            flowLayoutPanel_SlaveList.Controls.Clear();
            flowLayoutPanel_SlaveStatus.Controls.Clear();
            foreach (var slave in slaves.OrderBy(s => s.Name))
            {

                var widget = new SlaveWidget(slave);
                widget.WidgetActionTriggered += Widget_Deleted;
                widget.DeleteRequested += (s, e) =>
               {
                   flowLayoutPanel_SlaveList.Controls.Remove(widget);
                   slaves.RemoveAll(sl => sl.Name == slave.Name);
                   SaveSlaves();
               };

                widgetMap[slave.Name] = widget;
                flowLayoutPanel_SlaveList.Controls.Add(widget);
            }
            foreach (var slave in slaves.OrderBy(s => s.Name))
            {
                var widget = new SlaveWidgetStatus(slave);
                flowLayoutPanel_SlaveStatus.Controls.Add(widget);
            }
            Task.Run(() => RefreshSlaveStatusesAsync());
            foreach (var slave in slaves)
            {
                slave.Utiliser = slave.NetworkStatus;
                var name = slave.Name;

                var controlToUpdate = flowLayoutPanel_SlaveStatus.Controls
                    .OfType<SlaveWidgetStatus>()
                    .FirstOrDefault(c => c.Name == name);

                if (controlToUpdate != null)
                {
                    controlToUpdate.UtiliserChecked = slave.Utiliser;
                }
            }

        }

        private void Widget_Deleted(object sender, EventArgs e)
        {

            if (sender is SlaveWidget widget)
            {
                var name = widget.Name;

                // Find the matching control in flowLayoutPanel_SlaveStatus
                var controlToRemove = flowLayoutPanel_SlaveStatus.Controls
                    .OfType<Control>()
                    .FirstOrDefault(c => c.Name == name);

                if (controlToRemove != null)
                {
                    flowLayoutPanel_SlaveStatus.Controls.Remove(controlToRemove);
                    controlToRemove.Dispose(); // Optional: free resources
                }
                else
                {
                    MessageBox.Show("aucun Control avec le nom " + name);
                }

            }

        }

        private void StartRefreshTimer()
        {
            refreshTimer = new Timer { Interval = 5000 }; // 5 seconds
            refreshTimer.Tick += async (s, e) => await RefreshSlaveStatusesAsync();
            refreshTimer.Start();
        }

        private async Task RefreshSlaveStatusesAsync()
        {
            var tasks = slaves.Select(async slave =>
            {
                bool online = await Task.Run(() => IsSocketConnected(slave.Ip, slave.Port));
                slave.NetworkStatus = online;
            });

            await Task.WhenAll(tasks);

            foreach (SlaveWidget widget in flowLayoutPanel_SlaveList.Controls)
            {
                widget.UpdateDisplay(); // uses the bound SlaveConfig
            }
        }

        private bool IsSocketConnected(string ip, int port)
        {
            try
            {
                using TcpClient client = new();
                var result = client.BeginConnect(ip, port, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(500));
                return success && client.Connected;
            }
            catch
            {
                return false;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new SlaveEditorForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                slaves.Add(form.Slave);
                SaveSlaves();
                InitializeWidgets();
            }
        }

        private async void btn_SendMessage_Click(object sender, EventArgs e)
        {
            await SendMessageAsync();
        }

        private async Task SendMessageAsync()
        {
            listBox1.Items.Clear();
            string message = textBox_messageToSend.Text;
            byte[] data = Encoding.UTF8.GetBytes(message);

            var tasks = slaves.Select(async slave =>
            {
                try
                {
                    using TcpClient client = new();
                    await client.ConnectAsync(slave.Ip, slave.Port);
                    using NetworkStream stream = client.GetStream();
                    await stream.WriteAsync(data, 0, data.Length);

                    Invoke(() => listBox1.Items.Add($"Message envoyé à {slave.Ip}:{slave.Port}"));
                }
                catch (Exception ex)
                {
                    Invoke(() => listBox1.Items.Add($"Échec d'envoi à {slave.Name} ({slave.Ip}:{slave.Port}): {ex.Message}"));
                }
            });

            await Task.WhenAll(tasks);
        }


        private void btn_RefreshPing_Click(object sender, EventArgs e)
        {
            Task.Run(() => RefreshSlaveStatusesAsync());

        }

        private void button1_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 0;

        private void button2_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 1;

        private void button3_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 2;

        private async void btn_LoadSourceImages_Click(object sender, EventArgs e)
        {

            using OpenFileDialog openFileDialog = new();
            openFileDialog.Title = "Sélectionner une image";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullPath = openFileDialog.FileName;
                string folderPath = Path.GetDirectoryName(fullPath);
                string fileName = Path.GetFileName(fullPath);

                settings.SourceImagePath = folderPath;
                settings.ImageName = fileName;

                lbl_SourceImagePath.Text = folderPath;
                settings.Save();
                label_NbrImages.Text = "Calcul en cours";
                Task.Run(async () => await CalculateImageNumber(folderPath, fileName));

            }

        }

        private async Task CalculateImageNumber(string folderPath, string fileName)
        {


            int count = await Task.Run(() =>
            {
                var match = Regex.Match(fileName, @"^(.*?)[._]\d+\.(\w+)$");

                if (match.Success)
                {
                    string baseName = match.Groups[1].Value;
                    string extension = match.Groups[2].Value;

                    string pattern = $@"^{Regex.Escape(baseName)}[._]\d+\.{Regex.Escape(extension)}$";

                    return Directory.GetFiles(folderPath)
                        .Count(f => Regex.IsMatch(Path.GetFileName(f), pattern, RegexOptions.IgnoreCase));
                }
                else
                {
                    return -1;
                }
            });

            // Mise à jour UI sur le thread principal
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateUI(count);
                }));
            }
            else
            {
                UpdateUI(count);
            }
        }

        private void UpdateUI(int count)
        {
            if (count >= 0)
            {
                label_NbrImages.Text = $"Found {count} matching image(s).";
            }
            else
            {
                label_NbrImages.Text = "Filename does not match the expected pattern.";
            }
        }


        private void btn_ChooseDestination_Click(object sender, EventArgs e)
        {

            using FolderBrowserDialog folderDialog = new();
            folderDialog.Description = "Sélectionner un dossier";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                settings.DestinationPath = folderDialog.SelectedPath;

                settings.Save();
                lbl_DestinationPath.Text = folderDialog.SelectedPath;
            }

        }



        public AppSettings LoadSettings()
        {
            settings.Load();

            if (settings != null)
            {
                if (!string.IsNullOrEmpty(settings.SourceImagePath))
                    lbl_SourceImagePath.Text = settings.SourceImagePath;

                if (!string.IsNullOrEmpty(settings.DestinationPath))
                    lbl_DestinationPath.Text = settings.DestinationPath;

                if (!string.IsNullOrEmpty(settings.FFMPEGPath))
                    lbl_FFMPEGPath.Text = settings.FFMPEGPath;
            }

            return settings;
        }

        private void btn_FFMPEGPath_Click(object sender, EventArgs e)
        {

            using OpenFileDialog openFileDialog = new();
            openFileDialog.Title = "Sélectionner le fichier FFMPEG";
            openFileDialog.Filter = "Executable Files|*.exe|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string ffmpegPath = openFileDialog.FileName;
                settings.FFMPEGPath = ffmpegPath;
                lbl_FFMPEGPath.Text = ffmpegPath;
                settings.Save();
            }

        }

        private void tabPage_Encode_Click(object sender, EventArgs e)
        {

        }

        private void btn_AutoAssign_Click(object sender, EventArgs e)
        {

            var slaveForm = new SlaveSpreadForm(slaves);
            slaveForm.Show();

        }
    }

    public class AppSettings
    {
        [JsonIgnore]
        private string _filePath;

        public AppSettings()
        {

        }

        public AppSettings(string filepath)
        {
            _filePath = filepath;
        }

        public string DestinationPath { get; set; }
        public string SourceImagePath { get; set; }
        public string ImageName { get; set; }
        public string FFMPEGPath { get; set; }

        public void Save()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void Load()
        {
            string json = File.ReadAllText(_filePath);
            var loaded = JsonSerializer.Deserialize<AppSettings>(json);

            if (loaded != null)
            {
                DestinationPath = loaded.DestinationPath;
                SourceImagePath = loaded.SourceImagePath;
                ImageName = loaded.ImageName;
                FFMPEGPath = loaded.FFMPEGPath;
            }
        }

    }


    public class SlaveConfig : INotifyPropertyChanged  // Mes deux widgets (SlaveWidgetStatus et SlaveWidget) sont liés à un objet qui les notifient quand la valeur de NetworkStatus et Utiliser change.
    {
        public event PropertyChangedEventHandler PropertyChanged; 

        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        private bool _networkStatus;
        public bool NetworkStatus
        {
            get => _networkStatus;
            set
            {
                if (_networkStatus != value)
                {
                    _networkStatus = value;
                    OnPropertyChanged(nameof(NetworkStatus));
                }
            }
        }

        private bool _utiliser;
        public bool Utiliser
        {
            get => _utiliser;
            set
            {
                if (_utiliser != value)
                {
                    _utiliser = value;
                    OnPropertyChanged(nameof(Utiliser));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }  




    public class BorderlessTabControl : TabControl
    {
        public BorderlessTabControl()
        {
            Appearance = TabAppearance.FlatButtons;
            ItemSize = new Size(0, 1);
            SizeMode = TabSizeMode.Fixed;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode)
            {
                m.Result = (IntPtr)1;
                return;
            }
            base.WndProc(ref m);
        }
    }
}

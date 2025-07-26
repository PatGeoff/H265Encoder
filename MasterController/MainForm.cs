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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MasterController
{
    public partial class MainForm : Form
    {
        public static List<SlaveConfig> slaves = new();
        private string configPath = "config.json";
        public string ffmpegCommmand;

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
            LoadFFPresets();
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
                var widget = new SlaveWidgetStatus(slave, settings);
                flowLayoutPanel_SlaveStatus.Controls.Add(widget);
            }
            Task.Run(() => RefreshSlaveStatusesAsync());
           


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
            var selectedBlades = slaves.OrderBy(s => s.Name).Where(blade => blade.Utiliser).ToList();
            textBox_autoSplitNbr.Text = selectedBlades.Count.ToString();
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

        public async Task SendMessageAsync()
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

        private void button1_Click(object sender, EventArgs e)
        {
            borderlessTabControl1.SelectedIndex = 0;
            radioButtons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            borderlessTabControl1.SelectedIndex = 1;
            radioButtons();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            borderlessTabControl1.SelectedIndex = 2;
            radioButtons();
        }

        private void radioButtons()
        {
            Color selectedColor = Color.FromArgb(40, 40, 40);
            Color unselectedColor = Color.FromArgb(30, 30, 30);

            switch (borderlessTabControl1.SelectedIndex)
            {
                case 0:
                    button1.BackColor = selectedColor;
                    button2.BackColor = unselectedColor;
                    button3.BackColor = unselectedColor;
                    break;
                case 1:
                    button1.BackColor = unselectedColor;
                    button2.BackColor = selectedColor;
                    button3.BackColor = unselectedColor;
                    break;
                case 2:
                    button1.BackColor = unselectedColor;
                    button2.BackColor = unselectedColor;
                    button3.BackColor = selectedColor;
                    break;
            }
        }

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


                var match = Regex.Match(fileName, @"^(.*?)[._]\d+\.(\w+)$");
                if (match.Success)
                {
                    string baseName = match.Groups[1].Value;
                    string extension = match.Groups[2].Value;

                    string ffmpegPattern = $"{baseName}_%08d.{extension}";
                    settings.ImageName = ffmpegPattern;
                    lbl_imageNameFormatted.Text = ffmpegPattern;
                }
                else
                {
                    MessageBox.Show("Le nom de fichier ne correspond pas au format attendu (ex: image_00000001.png).");
                    return;
                }



                lbl_SourceImagePath.Text = folderPath;


                settings.Save();
                textBox_NbrImages.Text = "Calcul en cours";
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
                textBox_NbrImages.Text = $"{count}";
            }
            else
            {
                textBox_NbrImages.Text = "Erreur";
            }
        }

        private void btn_ChooseDestination_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new();
            folderDialog.Description = "Sélectionner un dossier de destination";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderDialog.SelectedPath;

                // Utiliser une boîte de saisie simple pour le nom du fichier
                string fileName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Entrez le nom du fichier (sans extension) :",
                    "Nom du fichier",
                    "config");

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    // Nettoyer le nom et ajouter l'extension si nécessaire
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                    string fullFileName = fileName + ".mp4";

                    // Stocker dans AppSettings
                    settings.DestinationPath = folderPath;
                    settings.DestinationName = fullFileName;
                    settings.Save();

                    lbl_DestinationPath.Text = Path.Combine(folderPath, fullFileName);
                }
            }
        }

        public AppSettings LoadSettings()
        {
            settings.Load();

            if (settings != null)
            {
                if (!string.IsNullOrEmpty(settings.SourceImagePath))
                {
                    lbl_SourceImagePath.Text = settings.SourceImagePath;
                }

                if (!string.IsNullOrEmpty(settings.DestinationPath))
                {
                    if (!string.IsNullOrEmpty(settings.DestinationName))
                    {
                        lbl_DestinationPath.Text = settings.DestinationPath + "\\" + settings.DestinationName;
                    }
                    else { lbl_DestinationPath.Text = settings.DestinationPath; }

                }

                if (!string.IsNullOrEmpty(settings.FFMPEGPath))
                    textBox_FFMPEGpath.Text = settings.FFMPEGPath;
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
                textBox_FFMPEGpath.Text = ffmpegPath;
                settings.Save();
            }

        }

        private void LoadFFPresets()
        {

            string presetsFolder = settings.FFMPEGPresetsPath;
            if (comboBox_ffPresets.Items.Count > 0)
            {
                comboBox_ffPresets.SelectedItem = settings.FFMPEGPreset;
            }

            if (Directory.Exists(presetsFolder))
            {
                string[] files = Directory.GetFiles(presetsFolder);

                comboBox_ffPresets.Items.Clear();

                foreach (string file in files)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    comboBox_ffPresets.Items.Add(fileNameWithoutExtension);
                }
                lbl_ffmpegPresetPath.Text = presetsFolder;

            }
            else
            {
                FFMPEGSetPresetPath();
            }
        }

        private void btn_AutoAssign_Click(object sender, EventArgs e)
        {

            //var slaveForm = new SlaveSpreadForm(slaves);
            //slaveForm.Show();

        }

        private void btn_ffPresetsPath_Click(object sender, EventArgs e)
        {
            FFMPEGSetPresetPath();
        }

        private void FFMPEGSetPresetPath()
        {
            using FolderBrowserDialog folderDialog = new();
            folderDialog.Description = "Sélectionner le dossier des presets FFMPEG";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                settings.FFMPEGPresetsPath = folderDialog.SelectedPath;
                settings.Save(); // Sauvegarde dans le fichier config
                lbl_ffmpegPresetPath.Text = folderDialog.SelectedPath;
            }
        }

        private void comboBox_ffPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.FFMPEGPreset = comboBox_ffPresets.SelectedItem.ToString();
        }

        private void btn_autoSplit_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox_NbrImages.Text, out int totalImages))
            {
                MessageBox.Show("Entrer le nombre d'images au total à splitter ou\nsélectionnez le dossier d'images pour un calcul automatique");
                return;
            }

            var selectedBlades = slaves.OrderBy(s => s.Name).Where(blade => blade.Utiliser).ToList();
            int bladeCount = selectedBlades.Count;

            
            if (bladeCount == 0)
            {
                MessageBox.Show("No blades selected.");
                return;
            }

            int baseCount = totalImages / bladeCount;
            int remainder = totalImages % bladeCount;

            int currentStart = 0;

            for (int i = 0; i < bladeCount; i++)
            {
                int imagesForThisBlade = baseCount + (i == bladeCount - 1 ? remainder : 0);
                int currentEnd = currentStart + imagesForThisBlade - 1;

                selectedBlades[i].StartFrame = currentStart;
                selectedBlades[i].EndFrame = currentEnd;

                currentStart = currentEnd + 1;
            }
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
        public string DestinationName { get; set; }
        public string SourceImagePath { get; set; }
        public string ImageName { get; set; }
        public string FFMPEGPath { get; set; }
        public string FFMPEGPresetsPath {  get; set; }
        public string FFMPEGPreset {  get; set; }
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
                DestinationName = loaded.DestinationName;
                ImageName = loaded.ImageName;
                FFMPEGPath = loaded.FFMPEGPath;
                FFMPEGPreset = loaded.FFMPEGPreset;
                FFMPEGPresetsPath = loaded.FFMPEGPresetsPath;                
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

        public int _startFrame { get; set; }

        public int StartFrame
        {
            get => _startFrame;
            set
            {
                if (_startFrame != value)
                {
                    _startFrame = value;
                    OnPropertyChanged(nameof(StartFrame));
                }
            }
        }

        public int _endFrame { get; set; }
        public int EndFrame
        {
            get => _endFrame;
            set
            {
                if (_endFrame != value)
                {
                    _endFrame = value;
                    OnPropertyChanged(nameof(EndFrame));
                }
            }
        }

        public int _frameCount { get; set; }
        public int FrameCount
        {
            get => _frameCount;
            set
            {
                if (_frameCount != value)
                {
                    _frameCount = value;
                    OnPropertyChanged(nameof(FrameCount));
                }
            }
        }

        public string FFMPEGCommand { get; set; }

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

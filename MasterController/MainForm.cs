using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MasterController
{
    public partial class MainForm : Form
    {
        public static List<SlaveConfig> slaves = new();
        public static List<SlaveWidgetStatus> slaveWidgetStatusList = new();
        private List<string> ConcatList = new List<string>();

        private string configPath = "config.json";
        public string slavePath = "slaves_config.json";
        public string ffmpegCommmand;
        private Process ffmpegProcess;


        private AppSettings settings;

        private readonly DateTime _startTime = DateTime.Now;


        //private Dictionary<string, SlaveWidget> widgetMap = new();
        // private Dictionary<string, SlaveWidgetStatus> widgetStatusMap = new();
        private System.Windows.Forms.Timer refreshTimer;

        public MainForm()
        {

            InitializeComponent();
            settings = new AppSettings(configPath);
            LoadSettings();
            LoadSlaves();
            LoadFFPresets();
            InitializeWidgets();
            StartRefreshTimer();
            SetDropDownMenus();
            SetUnselectBoxes();
            SetCheckedBoxes();
            UpdateNumberBlades();
        }


        public void InitializeWidgets()
        {
            flowLayoutPanel_SlaveList.Controls.Clear();
            flowLayoutPanel_SlaveStatus.Controls.Clear();

            foreach (var slave in slaves.OrderBy(s => s.Name))
            {

                var widget = new SlaveWidget(slave, this);
                widget.WidgetActionTriggered += Widget_Deleted;
                widget.DeleteRequested += (s, e) =>
               {
                   flowLayoutPanel_SlaveList.Controls.Remove(widget);
                   slaves.RemoveAll(sl => sl.Name == slave.Name);
                   SaveSlaves();

               };

                // widgetMap[slave.Name] = widget;
                flowLayoutPanel_SlaveList.Controls.Add(widget);
            }
            foreach (var slave in slaves.OrderBy(s => s.Name))
            {
                Debug.WriteLine(slave.Instances);
                for (var i = 0; i < slave.Instances; i++)
                {
                    var widget = new SlaveWidgetStatus(slave, settings, this);
                    flowLayoutPanel_SlaveStatus.Controls.Add(widget);
                    slaveWidgetStatusList.Add(widget);
                }
            }

            Task.Run(() => RefreshSlaveStatusesAsync());
        }

        public void SetCheckedBoxes()
        {
            foreach (SlaveWidgetStatus slave in slaveWidgetStatusList)
            {
                var name = slave.Name;
                slave.UtiliserChecked = slave._slave.NetworkStatus;
                slave.SetNameColor();
                slave.UpdateDisplay();
            }
        }
        public void AddSlaveInstances()
        {
            flowLayoutPanel_SlaveStatus.Controls.Clear();
            slaveWidgetStatusList.Clear();
            foreach (var slave in slaves.OrderBy(s => s.Name))
            {
                for (var i = 0; i < slave.Instances; i++)
                {
                    var widget = new SlaveWidgetStatus(slave, settings, this);
                    flowLayoutPanel_SlaveStatus.Controls.Add(widget);
                    slaveWidgetStatusList.Add(widget);
                    widget.UtiliserChecked = true;
                }
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
            refreshTimer = new System.Windows.Forms.Timer { Interval = 5000 }; // 5 seconds
            refreshTimer.Tick += async (s, e) => await RefreshSlaveStatusesAsync();
            refreshTimer.Start();
        }

        private void SetUnselectBoxes()
        {
            comboBox_FFBitrate.SelectionStart = 0;
            comboBox_FFBitrate.SelectionLength = 0;
            comboBox_FFProfile.SelectionStart = 0;
            comboBox_FFProfile.SelectionLength = 0;
            comboBox_FFFramerate.SelectionStart = 0;
            comboBox_FFFramerate.SelectionLength = 0;
            comboBox_FFRes.SelectionStart = 0;
            comboBox_FFRes.SelectionLength = 0;
        }

        public async Task RefreshSlaveStatusesAsync()
        {
            var tasks = slaves.Select(async slave =>
            {
                bool online = await IsSocketConnectedAsync(slave.Ip, slave.Port);

                slave.NetworkStatus = online;
            });

            await Task.WhenAll(tasks);


            foreach (SlaveWidgetStatus slave in slaveWidgetStatusList)
            {
                var name = slave.Name;
                if (!slave._slave.NetworkStatus)
                { slave.UtiliserChecked = false; }
            }

            UpdateNumberBlades();

            foreach (SlaveWidgetStatus widget in slaveWidgetStatusList)
            {
                widget.UpdateDisplay();
            }
            foreach (SlaveWidget widget in flowLayoutPanel_SlaveList.Controls)
            {
                widget.UpdateDisplay(); // uses the bound SlaveConfig
            }

        }

        public void UpdateNumberBlades()
        {
            var selectedBlades = slaveWidgetStatusList.OrderBy(s => s.Name).Where(blade => blade.UtiliserChecked).ToList();
            textBox_autoSplitNbr.Text = selectedBlades.Count.ToString();
        }

        private async Task<bool> IsSocketConnectedAsync(string ip, int port)
        {
            try
            {
                using TcpClient client = new();
                var connectTask = client.ConnectAsync(ip, port);
                var timeoutTask = Task.Delay(500);

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                return completedTask == connectTask && client.Connected;
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

        private void SetDropDownMenus()
        {
            comboBox_FFBitrate.SelectedIndex = 2;
            comboBox_FFProfile.SelectedIndex = 1;
            comboBox_FFFramerate.SelectedIndex = 0;
            comboBox_FFRes.SelectedIndex = 3;

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

                var match = Regex.Match(fileName, @"^(.*?)([_\.])?(\d+)\.(\w+)$");

                if (match.Success)
                {
                    string baseName = match.Groups[1].Value;
                    string separator = match.Groups[2].Success ? match.Groups[2].Value : "_"; // par défaut "_"
                    string numberPart = match.Groups[3].Value;
                    string extension = match.Groups[4].Value;

                    int digitCount = numberPart.Length;
                    string ffmpegPattern = $"{baseName}{separator}%0{digitCount}d.{extension}";
                    settings.ImageName = ffmpegPattern;
                    lbl_imageNameFormatted.Text = ffmpegPattern;
                }

                else
                {
                    MessageBox.Show("Le nom de fichier ne correspond pas au format attendu (ex: image_00001.jpg).");
                    return;
                }

                lbl_SourceImagePath.Text = folderPath;
                settings.Save();
                textBox_NbrImages.ForeColor = Color.Red;
                textBox_NbrImages.Text = "Calcul";

                // Trouver le premier frame et le dernier frame
                await Task.Run(() =>
                {

                    var regex = new Regex(@"(\d+)\.(\w+)$", RegexOptions.IgnoreCase);

                    var files = Directory.GetFiles(folderPath, "*.*")
                                         .Where(f => regex.IsMatch(Path.GetFileName(f)))
                                         .ToList();

                    var frameNumbers = files
                        .Select(f => regex.Match(Path.GetFileName(f)))
                        .Where(m => m.Success)
                        .Select(m => int.Parse(m.Groups[1].Value))
                        .ToList();


                    if (frameNumbers.Count > 0)
                    {
                        int minFrame = frameNumbers.Min();
                        int maxFrame = frameNumbers.Max();

                        Invoke(() =>
                        {
                            textBox_premierFrame.Text = minFrame.ToString();
                            textBox_dernierFrame.Text = maxFrame.ToString();
                        });
                    }
                    else
                    {
                        Invoke(() =>
                        {
                            MessageBox.Show("Aucune image valide trouvée dans le dossier.");
                            textBox_premierFrame.Text = "-";
                            textBox_dernierFrame.Text = "-";
                        });
                    }
                });

                await Task.Run(async () => await CalculateImageNumber(folderPath, fileName));

                foreach (var widget in slaveWidgetStatusList)
                {
                    widget.ClearFFConsole();
                    widget.ResetButtonText();
                }
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
                    textBox_NbrImages.ForeColor = Color.White;
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

                    textBox_DestinationPath.Text = Path.Combine(folderPath, fullFileName);
                    textBox_ConcatInputVideoPath.Text = textBox_DestinationPath.Text;
                }
            }
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

            //string presetsFolder = settings.FFMPEGPresetsPath;
            //if (comboBox_ffPresets.Items.Count > 0)
            //{
            //    comboBox_ffPresets.SelectedItem = settings.FFMPEGPreset;
            //}

            //if (Directory.Exists(presetsFolder))
            //{
            //    string[] files = Directory.GetFiles(presetsFolder);

            //    comboBox_ffPresets.Items.Clear();

            //    foreach (string file in files)
            //    {
            //        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            //        comboBox_ffPresets.Items.Add(fileNameWithoutExtension);
            //    }

            //}
            //else
            //{
            //    FFMPEGSetPresetPath();
            //}
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
            }
        }

        private void comboBox_ffPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            //settings.FFMPEGPreset = comboBox_ffPresets.SelectedItem.ToString();
        }

        public void SetButtonColor()
        {
            btn_autoSplit.BackColor = Color.FromArgb(255, 40, 40, 40);
            btn_autoSplit.Text = "Auto Split";
        }

        //private void btn_autoSplit_Click(object sender, EventArgs e)
        //{
        //    btn_autoSplit.Text = "Calcul";
        //    btn_autoSplit.BackColor = Color.DarkRed;

        //    if (!int.TryParse(textBox_premierFrame.Text, out int firstFrame) ||
        //        !int.TryParse(textBox_dernierFrame.Text, out int lastFrame))
        //    {
        //        MessageBox.Show("Veuillez entrer des valeurs valides pour le premier et le dernier frame.");
        //        return;
        //    }

        //    int totalImages = lastFrame - firstFrame + 1;

        //    var selectedWidgets = slaveWidgetStatusList.Where(w => w.UtiliserChecked).ToList();
        //    int bladeCount = selectedWidgets.Count;

        //    if (bladeCount == 0)
        //    {
        //        MessageBox.Show("Aucune blade sélectionnée.");
        //        return;
        //    }

        //    int baseCount = totalImages / bladeCount;
        //    int remainder = totalImages % bladeCount;
        //    int currentStart = firstFrame;

        //    for (int i = 0; i < selectedWidgets.Count; i++)
        //    {
        //        var widget = selectedWidgets[i];
        //        int extra = remainder > 0 ? 1 : 0;
        //        int imagesForThisBlade = baseCount + extra;
        //        if (remainder > 0) remainder--;

        //        int currentEnd = currentStart + imagesForThisBlade - 1;

        //        widget.startFrame = currentStart;
        //        widget.endFrame = currentEnd;
        //        widget.frameCount = imagesForThisBlade;
        //        widget._slave.Part = i;
        //        currentStart = currentEnd + 1;
        //    }

        //   // GetSuffixNbr();
        //}

        //private void btn_autoSplit_Click(object sender, EventArgs e)
        //{
        //    btn_autoSplit.Text = "Calcul";
        //    btn_autoSplit.BackColor = Color.DarkRed;

        //    if (!int.TryParse(textBox_premierFrame.Text, out int firstFrame) ||
        //        !int.TryParse(textBox_dernierFrame.Text, out int lastFrame))
        //    {
        //        MessageBox.Show("Veuillez entrer des valeurs valides pour le premier et le dernier frame.");
        //        return;
        //    }

        //    int totalImages = lastFrame - firstFrame + 1;
        //    var selectedWidgets = slaveWidgetStatusList.Where(w => w.UtiliserChecked).ToArray();
        //    int bladeCount = selectedWidgets.Length;

        //    if (bladeCount == 0)
        //    {
        //        MessageBox.Show("Aucune blade sélectionnée.");
        //        return;
        //    }

        //    int baseCount = totalImages / bladeCount;
        //    int remainder = totalImages % bladeCount;
        //    int currentStart = firstFrame;

        //    for (int i = 0; i < bladeCount; i++)
        //    {
        //        int imagesForThisBlade = baseCount + (remainder-- > 0 ? 1 : 0);
        //        int currentEnd = currentStart + imagesForThisBlade - 1;

        //        var widget = selectedWidgets[i];
        //        widget.startFrame = currentStart;
        //        widget.endFrame = currentEnd;
        //        widget.frameCount = imagesForThisBlade;
        //        widget._slave.Part = i;

        //        currentStart = currentEnd + 1;
        //    }

        //    // Optional: GetSuffixNbr();
        //}

        private void btn_autoSplit_Click(object sender, EventArgs e)
        {
            btn_autoSplit.Text = "Calcul";
            btn_autoSplit.BackColor = Color.DarkRed;
            if (!int.TryParse(textBox_premierFrame.Text, out int firstFrame) ||
               !int.TryParse(textBox_dernierFrame.Text, out int lastFrame))
            {
                MessageBox.Show("Veuillez entrer des valeurs valides pour le premier et le dernier frame.");
                return;
            }
             Task.Run(()  => performSplitCalculations(firstFrame, lastFrame));

            GetSuffixNbr();
        }

        private async Task performSplitCalculations(int firstFrame, int lastFrame)
        {
                      

            int totalImages = lastFrame - firstFrame + 1;
            var selectedWidgets = slaveWidgetStatusList.Where(w => w.UtiliserChecked).ToArray();
            int bladeCount = selectedWidgets.Length;

            if (bladeCount == 0)
            {
                MessageBox.Show("Aucune blade sélectionnée.");
                return;
            }

            int baseCount = totalImages / bladeCount;
            int remainder = totalImages % bladeCount;
            int currentStart = firstFrame;

            for (int i = 0; i < bladeCount; i++)
            {
                int imagesForThisBlade = baseCount + (remainder-- > 0 ? 1 : 0);
                int currentEnd = currentStart + imagesForThisBlade - 1;

                var widget = selectedWidgets[i];
                widget.startFrame = currentStart;
                widget.endFrame = currentEnd;
                widget.frameCount = imagesForThisBlade;
                widget._slave.Part = i;

                currentStart = currentEnd + 1;
            }
        }

        private void GetSuffixNbr()
        {
            string nomImage = settings.DestinationName;
            string extension = Path.GetExtension(nomImage);
            string baseName = Path.GetFileNameWithoutExtension(nomImage);
            string destinationFolder = settings.DestinationPath;

            string[] existingFiles = Array.Empty<string>();
            try
            {
                existingFiles = Directory.GetFiles(destinationFolder, $"{baseName}_*.{extension.TrimStart('.')}");
            }
            catch (Exception) { }

            int maxIndex = -1;
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

            // Ensuite, dans ta boucle :
            int indexInList = 0;
            foreach (var widget in slaveWidgetStatusList.Where(blade => blade.UtiliserChecked))
            {
                widget.SetSuffixNbr(existingFiles, maxIndex, indexInList);
                indexInList++;
            }
            Task.Delay(400);

        }

        private void SetFFMPEGPath()
        {
            string path = textBox_FFMPEGpath.Text;

            if (Uri.TryCreate(path, UriKind.Absolute, out Uri uriResult) && uriResult.IsUnc)
            {
                settings.FFMPEGPath = path;
                settings.Save();
            }
            else
            {
                MessageBox.Show("Le chemin spécifié n'est pas un chemin UNC valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox_FFMPEGpath_MouseLeave(object sender, EventArgs e)
        {
            SetFFMPEGPath();
        }

        private void textBox_FFMPEGpath_KeyDown(object sender, KeyEventArgs e)
        {
            SetFFMPEGPath();
        }

        private void textBox_DestinationPath_KeyDown(object sender, KeyEventArgs e)
        {
            SaveOutputDestinationPath();
        }

        private void textBox_DestinationPath_Leave(object sender, EventArgs e)
        {
            SaveOutputDestinationPath();
        }

        private void SaveOutputDestinationPath()
        {
            settings.DestinationPath = Path.GetFullPath(textBox_DestinationPath.Text);
            settings.Save();
        }

        private void btn_ConcatOutPath_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new();
            folderDialog.Description = "Sélectionner un dossier de destination";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderDialog.SelectedPath;

                // Prompt for filename
                string fileName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Entrez le nom du vidéo (sans extension) :",
                    "Nom du fichier",
                    "config");

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                    string fullFileName = fileName + ".mp4";
                    string fullPath = Path.Combine(folderPath, fullFileName);

                    // Check if file exists
                    if (File.Exists(fullPath))
                    {
                        DialogResult result = MessageBox.Show(
                            $"Le fichier \"{fullFileName}\" existe déjà dans ce dossier.\nVoulez-vous l'écraser ?",
                            "Fichier existant",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result != DialogResult.Yes)
                        {
                            return; // User chose not to overwrite
                        }
                    }

                    textBox_ConcatOutputVideoPath.Text = fullPath;
                }
            }
        }

        private async void btn_StartConcat_Click(object sender, EventArgs e)
        {
            string ffmpegPath = settings.FFMPEGPath;
            string outputPath = textBox_ConcatOutputVideoPath.Text;
            string folderPath = textBox_ConcatInputVideoPath.Text;

            if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(outputPath) || ConcatList.Count == 0)
            {
                MessageBox.Show("Please make sure all paths are set and videos are selected.");
                return;
            }

            // Create temporary file list for FFmpeg
            string tempListPath = Path.Combine(Path.GetTempPath(), "ffmpeg_concat_list.txt");
            using (StreamWriter writer = new StreamWriter(tempListPath))
            {
                foreach (string file in ConcatList)
                {
                    writer.WriteLine($"file '{file.Replace("'", "'\\''")}'");
                }
            }

            // Prepare FFmpeg processqui
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-y -f concat -safe 0 -i \"{tempListPath}\" -c copy \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            textBox_FFmpegConcatOutput.Clear();

            void AppendLine(string line)
            {
                if (textBox_FFmpegConcatOutput.InvokeRequired)
                {
                    textBox_FFmpegConcatOutput.Invoke((MethodInvoker)(() =>
                    {
                        textBox_FFmpegConcatOutput.AppendText(line + Environment.NewLine);
                        textBox_FFmpegConcatOutput.SelectionStart = textBox_FFmpegConcatOutput.Text.Length;
                        textBox_FFmpegConcatOutput.ScrollToCaret();
                    }));
                }
                else
                {
                    textBox_FFmpegConcatOutput.AppendText(line + Environment.NewLine);
                    textBox_FFmpegConcatOutput.SelectionStart = textBox_FFmpegConcatOutput.Text.Length;
                    textBox_FFmpegConcatOutput.ScrollToCaret();
                }
            }

            AppendLine("Starting FFmpeg...");

            ffmpegProcess = new Process { StartInfo = psi };

            ffmpegProcess.OutputDataReceived += (s, ev) =>
            {
                if (ev.Data != null) AppendLine(ev.Data);
            };

            ffmpegProcess.ErrorDataReceived += (s, ev) =>
            {
                if (ev.Data != null) AppendLine(ev.Data);
            };

            ffmpegProcess.Start();
            ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();

            await ffmpegProcess.WaitForExitAsync();

            AppendLine("FFmpeg process completed.");

            try
            {
                const int maxRetries = 5;
                const int delayMs = 500;

                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        if (File.Exists(tempListPath))
                        {
                            File.Delete(tempListPath);
                        }
                        break; // Success
                    }
                    catch (IOException)
                    {
                        await Task.Delay(delayMs); // Wait and retry
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLine($"Failed to delete temp file: {ex.Message}");
            }

        }

        private void btn_selectVideos_Click(object sender, EventArgs e)
        {


            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a video file";
                openFileDialog.Filter = "Video Files (*.mp4;*.avi;*.mov)|*.mp4;*.avi;*.mov|All Files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Display selected file path in the TextBox
                    string selectedFilePath = openFileDialog.FileName;
                    textBox_ConcatInputVideoPath.Text = Path.GetDirectoryName(selectedFilePath);

                    // Get all files in the same folder
                    string folderPath = Path.GetDirectoryName(selectedFilePath);
                    string[] allFiles = Directory.GetFiles(folderPath);

                    // Store in list and display in ListBox
                    ConcatList.Clear();
                    ConcatList.AddRange(allFiles);

                    listBox_ConcatSource.Items.Clear();
                    listBox_ConcatSource.Items.AddRange(ConcatList.ToArray());
                }
            }


        }

        private void btn_StopFFmpegProcess_Click(object sender, EventArgs e)
        {
            if (ffmpegProcess != null && !ffmpegProcess.HasExited)
            {
                try
                {
                    ffmpegProcess.Kill();
                    textBox_FFmpegConcatOutput.AppendText("FFmpeg process was terminated." + Environment.NewLine);

                    ffmpegProcess.WaitForExit();

                    string tempListPath = Path.Combine(Path.GetTempPath(), "ffmpeg_concat_list.txt");

                    // Retry deletion with delay
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            if (File.Exists(tempListPath))
                            {
                                File.Delete(tempListPath);
                                textBox_FFmpegConcatOutput.AppendText("Temporary file deleted." + Environment.NewLine);
                            }
                            break;
                        }
                        catch (IOException)
                        {
                            Thread.Sleep(500); // Wait half a second before retrying
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error stopping FFmpeg: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("FFmpeg is not running.");
            }
        }

        private void comboBox_FFBitrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.FFBitrate = int.Parse(comboBox_FFBitrate.SelectedItem.ToString());
        }

        private void comboBox_FFFramerate_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.FFframeRate = int.Parse(comboBox_FFFramerate.SelectedItem.ToString());
        }

        private void comboBox_FFRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.FFResolution = comboBox_FFRes.SelectedItem.ToString();
        }

        private void comboBox_FFDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_FFProfile.SelectedIndex)
            {
                case 0: // 4:2:0 - 8 bits

                    settings.FFProfile = "-pix_fmt yuv420p -profile:v main";
                    break;

                case 1: // 4:2:0 - 10 bits
                    settings.FFProfile = "-pix_fmt yuv420p10le -profile:v main10";
                    break;

                case 2: // 4:2:2 - 8 bits
                    comboBox_FFProfile.SelectedIndex = 1;
                    break;
                //    settings.FFProfile = "-pix_fmt yuv422p -profile:v main422";
                //    break;

                case 3: // 4:2:2 - 10 bits
                    comboBox_FFProfile.SelectedIndex = 1;
                    break;
                //    settings.FFProfile = "-pix_fmt yuv422p10le -profile:v main422-10";
                //    break;

                case 4: // 4:4:4 - 8 bits
                    comboBox_FFProfile.SelectedIndex = 1;
                    break;
                //    settings.FFProfile = "-pix_fmt yuv444p -profile:v main444";
                //    break;

                case 5: // 4:4:4 - 10 bits
                    comboBox_FFProfile.SelectedIndex = 1;
                    break;
                //    settings.FFProfile = "-pix_fmt yuv444p10le -profile:v main444-10";
                //    break;
                default:

                    break;

            }
        }
        public AppSettings LoadSettings()
        {

            if (!File.Exists(configPath))
            {
                File.WriteAllText(configPath, "{}"); // Creates an empty JSON object
            }


            settings.Load();

            if (settings != null)
            {
                //if (!string.IsNullOrEmpty(settings.SourceImagePath))
                //{
                //    lbl_SourceImagePath.Text = settings.SourceImagePath;
                //}

                if (!string.IsNullOrEmpty(settings.DestinationPath))
                {
                    if (!string.IsNullOrEmpty(settings.DestinationName))
                    {
                        textBox_DestinationPath.Text = settings.DestinationPath + "\\" + settings.DestinationName;
                    }
                    else { textBox_DestinationPath.Text = settings.DestinationPath; }

                }

                if (!string.IsNullOrEmpty(settings.FFMPEGPath))
                    textBox_FFMPEGpath.Text = settings.FFMPEGPath;
            }

            return settings;
        }

        private void btn_SeeFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string folderPath = settings.DestinationPath;
                if (Directory.Exists(folderPath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", folderPath);
                }
                else
                {
                    MessageBox.Show("Le dossier spécifié est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ouverture du dossier : " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btn_StartAll_Click(object sender, EventArgs e)
        {
            foreach (var widget in slaveWidgetStatusList)
            {
                if (widget.UtiliserChecked)
                {
                    widget.StartSlaveJob();
                }
                Task.Delay(1500);
            }
        }

        private void btn_StopAll_Click(object sender, EventArgs e)
        {
            foreach (var widget in slaveWidgetStatusList)
            {
                if (widget.UtiliserChecked)
                {
                    widget.CancelSlaveJob();
                    widget.ResetButtonText();
                }
            }
        }

    }

    public class AppSettings : INotifyPropertyChanged
    {
        [JsonIgnore]
        private string _filePath;

        public event PropertyChangedEventHandler PropertyChanged;
        public AppSettings()
        {

        }

        public AppSettings(string filepath)
        {
            _filePath = filepath;
        }

        private string _destinationPath;
        public string DestinationPath
        {
            get => _destinationPath;
            set
            {
                if (_destinationPath != value)
                {
                    _destinationPath = value;
                    OnPropertyChanged(nameof(DestinationPath));
                }
            }
        }
        private string _destinationName;
        public string DestinationName
        {
            get => _destinationName;
            set
            {
                if (_destinationName != value)
                {
                    _destinationName = value;
                    OnPropertyChanged(nameof(DestinationName));
                }
            }
        }
        private string _sourceImagePath;
        public string SourceImagePath
        {
            get => _sourceImagePath;
            set
            {
                if (_sourceImagePath != value)
                {
                    _sourceImagePath = value;
                    OnPropertyChanged(nameof(SourceImagePath));
                }
            }
        }
        private string _imageName;
        public string ImageName
        {
            get => _imageName;
            set
            {
                if (_imageName != value)
                {
                    _imageName = value;
                    OnPropertyChanged(nameof(ImageName));
                }
            }
        }
        private string _ffmpegPath;
        public string FFMPEGPath
        {
            get => _ffmpegPath;
            set
            {
                if (_ffmpegPath != value)
                {
                    _ffmpegPath = value;
                    OnPropertyChanged(nameof(FFMPEGPath));
                }
            }
        }
        public string FFMPEGPresetsPath { get; set; }
        public string FFMPEGPreset { get; set; }

        private int _ffFrameRate;
        public int FFframeRate
        {
            get => _ffFrameRate;
            set
            {
                if (_ffFrameRate != value)
                {
                    _ffFrameRate = value;
                    OnPropertyChanged(nameof(FFframeRate));
                }
            }
        }

        private int _ffBitrate;
        public int FFBitrate
        {
            get => _ffBitrate;
            set
            {
                if (_ffBitrate != value)
                {
                    _ffBitrate = value;
                    OnPropertyChanged(nameof(FFBitrate));
                }
            }
        }
        private string _ffResolution;
        public string FFResolution
        {
            get => _ffResolution;
            set
            {
                if (_ffResolution != value)
                {
                    _ffResolution = value;
                    OnPropertyChanged(nameof(FFResolution));
                }
            }
        }
        public string FFFormat { get; set; }
        public string FFProfile { get; set; }
        public string FFChroma { get; set; }

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
                FFframeRate = 30;
                FFBitrate = 60;
                FFResolution = "6144:6144";
                FFProfile = "-pix_fmt yuv420p10le -profile:v main10";
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class SlaveConfig : INotifyPropertyChanged  // Mes deux widgets (SlaveWidgetStatus et SlaveWidget) sont liés à un objet qui les notifient quand la valeur de NetworkStatus et Utiliser change.
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SlaveConfig()
        {
            StartFrame = 0;
            EndFrame = 0;
            FrameCount = 0;
        }

        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public int Instances { get; set; }

        public int Part { get; set; }

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterController
{
    public partial class MainForm : Form
    {
        private List<SlaveConfig> slaves = new();
        private Dictionary<string, SlaveWidget> widgetMap = new();
        private Dictionary<string, SlaveWidgetStatus> widgetStatusMap = new();
        private Timer refreshTimer;

        public MainForm()
        {
            InitializeComponent();
            LoadSlaves();
            InitializeWidgets();
            StartRefreshTimer();
        }


        private void InitializeWidgets()
        {
            flowLayoutPanel_SlaveList.Controls.Clear();
            FLP_SlaveStatus.Controls.Clear();
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
                widget.DeleteRequested += (s, e) =>
                {
                    flowLayoutPanel_SlaveList.Controls.Remove(widget);
                    slaves.RemoveAll(sl => sl.Name == slave.Name);                    
                    SaveSlaves();
                };
                
                widgetStatusMap[slave.Name] = widget;
                FLP_SlaveStatus.Controls.Add(widget);
            }
            Task.Run(() => RefreshSlaveStatusesAsync());


        }

        private void Widget_Deleted(object sender, EventArgs e)
        {
            //InitializeWidgets();
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

       


        private void btn_SendMessage_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string message = textBox_messageToSend.Text;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

            foreach (var slave in slaves)
            {
                try
                {
                    using TcpClient client = new();
                    client.Connect(slave.Ip, slave.Port);
                    using NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);

                    listBox1.Items.Add($"Message envoyé à {slave.Ip}:{slave.Port}");
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add($"Échec d'envoi à {slave.Name} ({slave.Ip}:{slave.Port}): {ex.Message}");
                }
            }
        }

        private void btn_RefreshPing_Click(object sender, EventArgs e)
        {
            Task.Run(() => RefreshSlaveStatusesAsync());

        }

        private void button1_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 0;

        private void button2_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 1;

        private void button3_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 2;

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

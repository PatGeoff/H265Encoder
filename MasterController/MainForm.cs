using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Windows.Forms;

namespace MasterController
{
    public partial class MainForm : Form
    {
        private List<SlaveConfig> slaves = new();
        private Dictionary<string, SlaveWidget> widgetMap = new();
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
            widgetMap.Clear();

            foreach (var slave in slaves.OrderBy(s => s.Name))
            {
                var widget = new SlaveWidget
                {
                    BladeName = slave.Name,
                    IpPort = $"{slave.Ip}:{slave.Port}",
                    NetworkStatus = slave.Connected ? "Online" : "Offline"

                };



                widget.DeleteRequested += (s, e) =>
               {
                   flowLayoutPanel_SlaveList.Controls.Remove(widget);
                   slaves.RemoveAll(sl => sl.Name == widget.BladeName);
                   SaveSlaves();
               };

                widgetMap[slave.Name] = widget;
                flowLayoutPanel_SlaveList.Controls.Add(widget);
            }
        }

        private void StartRefreshTimer()
        {
            refreshTimer = new Timer { Interval = 5000 }; // 5 secondes
            refreshTimer.Tick += (s, e) => RefreshSlaveStatuses();
            refreshTimer.Start();
        }

        private void RefreshSlaveStatuses()
        {
            foreach (var slave in slaves)
            {
                bool online = IsSocketConnected(slave.Ip, slave.Port);
                slave.NetworkStatus = online;
                               

            }
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
            RefreshSlaveStatuses();
        }

        private void button1_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 0;
        private void button2_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 1;
        private void button3_Click(object sender, EventArgs e) => borderlessTabControl1.SelectedIndex = 2;


    }

    public class SlaveConfig
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool NetworkStatus { get; set; }
        public bool Utiliser { get; set; }
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

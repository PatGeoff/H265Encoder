using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MasterController
{
    public partial class SlaveWidget : UserControl
    {
        public event EventHandler DeleteRequested;
        public event EventHandler WidgetActionTriggered;
        private SlaveConfig _slave;
        private string texte;
        private MainForm _mainForm;

        public SlaveWidget(SlaveConfig slave, MainForm mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _slave = slave;
            _slave.PropertyChanged += Config_PropertyChanged;
            this.AutoSize = false;
            this.Dock = DockStyle.None;
            this.Name = slave.Name;
            texte = _slave.Instances.ToString();
            int inst = _slave.Instances;
            if (inst == 0)
            {
                _slave.Instances = 1;
                inst = 1;
            }
            textBox_Instances.Text = inst.ToString();
        }


        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update only the relevant part of the UI
            if (e.PropertyName == nameof(SlaveConfig.NetworkStatus))
            {
                UpdateDisplay();
            }
        }

        public void UpdateDisplay()
        {
            lbl_bladeName.Text = _slave.Name;
            lbl_IpPort.Text = $"{_slave.Ip}:{_slave.Port}";
            lbl_bladeNetStat.Text = _slave.NetworkStatus ? "Online" : "Offline";
            lbl_bladeNetStat.ForeColor = _slave.NetworkStatus ? Color.Green : Color.Red;
        }


        private void btn_deleteBlade_Click(object sender, EventArgs e)
        {

            this.Parent?.Controls.Remove(this);
            DeleteRequested?.Invoke(this, EventArgs.Empty);
            WidgetActionTriggered?.Invoke(this, EventArgs.Empty);
        }


        private void textBox_Instances_Click(object sender, EventArgs e)
        {
            texte = textBox_Instances.Text;
        }

        private void textBox_Instances_Leave(object sender, EventArgs e)
        {
            if (textBox_Instances.Text != texte && int.TryParse(textBox_Instances.Text, out int value))
            {
                texte = textBox_Instances.Text;
                _slave.Instances = int.Parse(textBox_Instances.Text);
                _mainForm.SaveSlaves();
                _mainForm.AddSlaveInstances();
            }
        }

        private void btn_killFFMPEG_Click(object sender, EventArgs e)
        {
            Task.Run(() => SendMessage("KillFFMPEG"));
        }

        private void btn_killMe_Click(object sender, EventArgs e)
        {
            Task.Run(() => SendMessage("KillMe"));
        }

        public async Task SendMessage(string message)
        {
            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(_slave.Ip, _slave.Port);

                using (NetworkStream stream = client.GetStream())
                {
                    byte[] commandBytes = Encoding.UTF8.GetBytes(message + "\n");
                    await stream.WriteAsync(commandBytes, 0, commandBytes.Length);
                }
            }
        }

       
    }
}

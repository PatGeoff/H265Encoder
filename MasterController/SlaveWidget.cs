using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public SlaveWidget(SlaveConfig slave)
        {
            InitializeComponent();
            _slave = slave;
            _slave.PropertyChanged += Config_PropertyChanged;
            this.AutoSize = false;
            this.Dock = DockStyle.None;
        }


        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update only the relevant part of the UI
            if (e.PropertyName == nameof(SlaveConfig.NetworkStatus) || e.PropertyName == nameof(SlaveConfig.Utiliser))
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
            WidgetActionTriggered?.Invoke(this, EventArgs.Empty); // pour pouvoir exécuter SaveSlaves() dans MainForm. 
        }
    }
}

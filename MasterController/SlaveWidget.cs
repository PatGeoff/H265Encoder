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
        private SlaveConfig _slave;

        public SlaveWidget()
        {
            InitializeComponent();
            this.AutoSize = false;
            this.Dock = DockStyle.None;
        }
      

        public void BindToSlave(SlaveConfig slave)
        {
            _slave = slave;
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            lbl_bladeName.Text = _slave.Name;
            lbl_IpPort.Text = $"{_slave.Ip}:{_slave.Port}";
            lbl_bladeNetStat.Text = _slave.Connected ? "Online" : "Offline";
            lbl_bladeNetStat.ForeColor = _slave.Connected ? Color.Green : Color.Red;
        }

        public SlaveConfig GetSlave() => _slave;



        private void btn_deleteBlade_Click(object sender, EventArgs e)
        {
            this.Parent?.Controls.Remove(this);
            DeleteRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}

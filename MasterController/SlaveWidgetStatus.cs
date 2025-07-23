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
    public partial class SlaveWidgetStatus : UserControl
    {
        private SlaveConfig _slave;
        public SlaveWidgetStatus()
        {
            InitializeComponent();
        }


        public void BindToSlave(SlaveConfig slave)
        {
            _slave = slave;
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            //lbl_bladeName.Text = _slave.Name;
            //lbl_IpPort.Text = $"{_slave.Ip}:{_slave.Port}";
            //lbl_bladeNetStat.Text = _slave.Connected ? "Online" : "Offline";
            //lbl_bladeNetStat.ForeColor = _slave.Connected ? Color.Green : Color.Red;
        }

        public SlaveConfig GetSlave() => _slave;


    }
}

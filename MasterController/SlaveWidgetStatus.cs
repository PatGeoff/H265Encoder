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
        public event EventHandler DeleteRequested;
        private SlaveConfig _slave;

        public SlaveWidgetStatus(SlaveConfig slave)
        {
            InitializeComponent();
            _slave = slave;
            _slave.PropertyChanged += Config_PropertyChanged;
            this.AutoSize = false;
            this.Dock = DockStyle.None;
            this.Name = slave.Name;
            UpdateDisplay();
        }


        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update only the relevant part of the UI
            if (e.PropertyName == nameof(SlaveConfig.NetworkStatus) || e.PropertyName == nameof(SlaveConfig.Utiliser))
            {
                UpdateDisplay();
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
        }


        private void btn_deleteBlade_Click(object sender, EventArgs e)
        {
            this.Parent?.Controls.Remove(this);
            DeleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private void checkBox_Utiliser_CheckedChanged(object sender, EventArgs e)
        {
            if (_slave.NetworkStatus)
            {
                _slave.Utiliser = checkBox_Utiliser.Checked;
            }
            else
            {
                checkBox_Utiliser.Checked = false;
            }
           
        }

       
    }
}

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
    public partial class SlaveSpreadForm : Form
    {
        private List<SlaveConfig> slaves;

        public SlaveSpreadForm(List<SlaveConfig> slaves)
        {
            InitializeComponent();
            this.slaves = slaves;
            LoadSlaves();
        }

        private void LoadSlaves()
        {
            foreach (var slave in slaves)
            {
                // Traitement ici
            }
        }
    }

}

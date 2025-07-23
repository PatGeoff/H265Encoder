using System;
using System.Windows.Forms;

namespace MasterController
{
    public partial class SlaveEditorForm : Form
    {
        public SlaveConfig Slave { get; private set; }

        public SlaveEditorForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Slave = new SlaveConfig
            {
                Name = textBoxName.Text,
                Ip = textBoxIp.Text,
                Port = int.Parse(textBoxPort.Text)
            };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

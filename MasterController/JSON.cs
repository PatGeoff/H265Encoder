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
        private void LoadSlaves()
        {
            try
            {
                if (File.Exists("slaves_config.json"))
                {
                    string json = File.ReadAllText("slaves_config.json");
                    slaves = JsonSerializer.Deserialize<List<SlaveConfig>>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading slave config: " + ex.Message);
            }
        }

        public void SaveSlaves()
        {
            try
            {
                string json = JsonSerializer.Serialize(slaves, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("slaves_config.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving slave config: " + ex.Message);
            }
        }             

    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlaveWorker
{
    public partial class MainForm : Form
    {
        private TcpListener listener;

        public MainForm()
        {
            InitializeComponent();
            StartListening();
        }

        private void StartListening()
        {
            Task.Run(() =>
            {
                try
                {
                    listener = new TcpListener(IPAddress.Any, 9000);
                    listener.Start();
                    AppendText("Listening on port 9000...");

                    while (true)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        NetworkStream stream = client.GetStream();
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        string command = reader.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(command))
                        {
                            AppendText("Received command: " + command);
                        }
                        //ExecuteFFmpeg(command, stream);
                        client.Close();
                    }
                }
                catch (Exception ex)
                {
                    AppendText("Error: " + ex.Message);
                }
            });
        }

        private void ExecuteFFmpeg(string command, NetworkStream stream)
        {
            Task.Run(() =>
            {
                try
                {
                    Process ffmpeg = new Process();
                    ffmpeg.StartInfo.FileName = "ffmpeg";
                    ffmpeg.StartInfo.Arguments = command;
                    ffmpeg.StartInfo.RedirectStandardError = true;
                    ffmpeg.StartInfo.UseShellExecute = false;
                    ffmpeg.StartInfo.CreateNoWindow = true;

                    ffmpeg.ErrorDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            AppendText(e.Data);
                            byte[] data = Encoding.UTF8.GetBytes(e.Data + "\n");
                            stream.Write(data, 0, data.Length);
                        }
                    };

                    ffmpeg.Start();
                    ffmpeg.BeginErrorReadLine();
                    ffmpeg.WaitForExit();
                }
                catch (Exception ex)
                {
                    AppendText("Execution error: " + ex.Message);
                }
            });
        }

        private void AppendText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendText), text);
            }
            else
            {
                textBoxOutput.AppendText(text + Environment.NewLine);
            }
        }
    }
}

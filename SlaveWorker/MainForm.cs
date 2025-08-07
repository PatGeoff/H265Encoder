using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlaveWorker
{
    public partial class MainForm : Form
    {
        private TcpListener listener;
        private string commande;
        private Process currentProcess;
        private readonly List<NetworkStream> connectedClients = new();
        private readonly object clientLock = new();


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
                        string command = reader.ReadLine();

                        // Send response back to client
                        byte[] responseBytes = Encoding.UTF8.GetBytes("Blade a reçu la commande " + command + "\n");
                        stream.Write(responseBytes, 0, responseBytes.Length);
                        stream.Flush();


                        if (!string.IsNullOrWhiteSpace(command))
                        {

                            if (command.Contains("StartJob"))
                            {
                                commande = command.Split("StartJob")[1];
                                ExecuteFFmpeg(commande, stream);
                            }
                            if (command.Contains("KillFFMPEG"))
                            {
                                KillFFmpeg();
                            }
                            if (command.Contains("KillMe"))
                            {
                                Environment.Exit(0);
                            }
                        }


                        //client.Close();
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
                    var match = Regex.Match(command.Trim(), "^\"([^\"]+)\"\\s+(.*)");
                    if (!match.Success)
                        throw new InvalidOperationException("Invalid command format. Expected quoted executable path.");

                    string executable = match.Groups[1].Value;
                    string arguments = match.Groups[2].Value;

                    currentProcess = new Process();
                    currentProcess.StartInfo.FileName = executable;
                    currentProcess.StartInfo.Arguments = arguments;
                    currentProcess.StartInfo.RedirectStandardError = true;
                    currentProcess.StartInfo.RedirectStandardOutput = true;
                    currentProcess.StartInfo.UseShellExecute = false;
                    currentProcess.StartInfo.CreateNoWindow = true;

                    currentProcess.ErrorDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            AppendText(e.Data);
                            byte[] data = Encoding.UTF8.GetBytes(e.Data + "\n");
                            if (stream.CanWrite)
                                stream.Write(data, 0, data.Length);
                        }
                    };

                    currentProcess.OutputDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            AppendText(e.Data);
                            byte[] data = Encoding.UTF8.GetBytes(e.Data + "\n");
                            if (stream.CanWrite)
                                stream.Write(data, 0, data.Length);
                        }
                    };

                    currentProcess.Start();
                    currentProcess.BeginErrorReadLine();
                    currentProcess.BeginOutputReadLine();


                    // Surveiller la connexion
                    while (!currentProcess.HasExited)
                    {
                        if (!IsClientConnected(stream))
                        {
                            AppendText("Connexion perdue. Arrêt du processus FFmpeg.");
                            //byte[] data = Encoding.UTF8.GetBytes("Arrêt du processus FFmpeg");
                            //stream.Write(data);
                            try { currentProcess.Kill(); } catch { }
                            break;
                        }
                        Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = "Execution error: " + ex.Message;
                    AppendText(errorMsg);
                    byte[] data = Encoding.UTF8.GetBytes(errorMsg + "\n");
                    if (stream.CanWrite)
                        stream.Write(data, 0, data.Length);
                }
            });
        }

        private bool IsClientConnected(NetworkStream stream)
        {
            try
            {
                if (stream == null || !stream.CanRead)
                    return false;

                return !(stream.Socket.Poll(1, SelectMode.SelectRead) && stream.Socket.Available == 0);
            }
            catch
            {
                return false;
            }
        }

        private void AppendText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendText), text);
            }
            else
            {
                textBox_Output.AppendText(text + Environment.NewLine);
            }
        }

        private void btn_ClearText_Click(object sender, EventArgs e)
        {
            textBox_Output.Clear();
        }


        public static void KillFFmpeg()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("ffmpeg");
                foreach (Process proc in processes)
                {
                    //Console.WriteLine($"Killing process: {proc.ProcessName} (ID: {proc.Id})");
                    proc.Kill();
                    proc.WaitForExit();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }


}


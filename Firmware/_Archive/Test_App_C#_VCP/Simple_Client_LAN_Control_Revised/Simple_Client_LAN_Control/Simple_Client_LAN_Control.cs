using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Simple_Client_LAN_Control
{
    public partial class Simple_Client_LAN_Control : UserControl
    {
        private int com_Counter = 0;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private bool is_Connected = false;
        private bool is_Data_Received = false;
        private int maxRetries = 5; // Limit the number of retries
        private int retryCount = 0;

        private string iPAddress;
        public string IPAddress
        {
            get { return iPAddress; }
            set { iPAddress = value; }
        }

        private int port;
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private byte[] rx_Data = new byte[64];
        public byte[] RX_Data
        {
            get { return rx_Data; }
            set { rx_Data = value; }
        }

        private int rx_Byte_Count = 64;
        public int RX_Byte_Count
        {
            get { return rx_Byte_Count; }
            set
            {
                rx_Byte_Count = value;
                rx_Data = new byte[rx_Byte_Count];
            }
        }

        // Define a delegate for updating the UI safely
        public delegate void UpdateReceivedDataDelegate(string data);
        public UpdateReceivedDataDelegate UpdateReceivedDataCallback;
        public event EventHandler Recieved_Data;

        private void Fire_Recieved_Data()
        {
            if (Recieved_Data != null)
            {
                Recieved_Data(this, null);
            }
        }

        public Simple_Client_LAN_Control()
        {
            InitializeComponent();
        }

        public async Task ConnectAsync()
        {
            while (!is_Connected && retryCount < maxRetries)
            {
                try
                {
                    Console.WriteLine($"Attempting to connect to {iPAddress}:{port}...");
                    tcpClient = new TcpClient();
                    await tcpClient.ConnectAsync(iPAddress, port);
                    stream = tcpClient.GetStream();
                    is_Connected = true;
                    retryCount = 0; // Reset retry count upon successful connection
                    Console.WriteLine($"Connected to {iPAddress}:{port}");
                    _ = ReadDataAsync(); // Start reading data once connected
                    break;
                }
                catch (Exception ex)
                {
                    is_Connected = false;
                    retryCount++;
                    LogConnectionError($"Failed to connect. Retrying in 10 seconds... ({retryCount}/{maxRetries})");
                    Console.WriteLine($"Connection failed: {ex.Message}");
                    await Task.Delay(10000); // Wait for 10 seconds before retrying
                }
            }

            if (!is_Connected && retryCount >= maxRetries)
            {
                Console.WriteLine("Max retries reached, starting retry timer...");
                timer_Connection_Retry.Start();
            }
        }

        private async Task ReadDataAsync()
        {
            var tempBuf = new byte[256];      // read chunk
            var frameBuf = new List<byte>();  // accumulator

            try
            {
                Console.WriteLine("Started reading data...");
                while (is_Connected)
                {
                    int n = await stream.ReadAsync(tempBuf, 0, tempBuf.Length);
                    Console.WriteLine($"{n} Bytes Ready to be Read.");
                    if (n == 0)
                    {
                        // disconnected
                        Console.WriteLine("Disconnected from server.");
                        is_Connected = false;
                        break;
                    }
                    
                    // append to accumulator
                    frameBuf.AddRange(tempBuf.Take(n));

                    // try to pull out all full frames
                    while (frameBuf.Count >= RX_Byte_Count)
                    {
                        Console.WriteLine("Reading Bytes.");
                        // look for a candidate frame start at index 0..(Count-64)
                        int idx = -1;
                        for (int i = 0; i <= frameBuf.Count - RX_Byte_Count; i++)
                        {
                            Console.WriteLine($"Reading Byte{i}.");
                            // sync bytes at [2]==0x55 && [3]==0x55
                            //if (frameBuf[i + 2] == 0x55 && frameBuf[i + 3] == 0x55)
                            //{
                            // checksum: sum[0..62] & 0xFF == frame[63]
                            int sum = 0;
                                for (int j = 0; j < RX_Byte_Count - 1; j++)
                                    sum += frameBuf[i + j];
                                if ((byte)sum == frameBuf[i + RX_Byte_Count - 1])
                                {
                                    idx = i;
                                    break;
                                }
                            //}
                        }

                        if (idx < 0)
                        {
                            // no valid frame header found yet; drop everything before Count-64
                            frameBuf.RemoveRange(0, frameBuf.Count - (RX_Byte_Count - 1));
                            break;
                        }

                        // extract that frame
                        var frame = frameBuf.Skip(idx).Take(RX_Byte_Count).ToArray();
                        // copy into RX_Data
                        Array.Copy(frame, RX_Data, RX_Data.Length);
                        Console.WriteLine($"Received data: {BitConverter.ToString(frame)}");
                        // signal
                        Fire_Recieved_Data();
                        is_Data_Received = true;

                        // remove up through end of that frame
                        frameBuf.RemoveRange(0, idx + RX_Byte_Count);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Read error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Closing stream and client...");
                stream?.Close();
                tcpClient?.Close();
                await ConnectAsync();
            }
        }

        public async Task Send_Data(byte[] buffer)
        {
            try
            {
                if (is_Connected)
                {
                    Console.WriteLine($"Sending data: {BitConverter.ToString(buffer)}");
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send error: {ex.Message}");
                is_Connected = false;
                stream?.Close();
                tcpClient?.Close();
                // Attempt to reconnect
                await ConnectAsync();
            }
        }

        private void Copy_To_Rx_Data(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                rx_Data[i] = buffer[i];
            }
        }

        private void button_LAN_MouseUp_1(object sender, MouseEventArgs e)
        {
            // Check if the left mouse button was released and the CTRL key is pressed
            if (e.Button == MouseButtons.Left && (Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (textBox_Rx_Data.Visible)
                {
                    textBox_Rx_Data.Visible = false;
                    this.Size = new System.Drawing.Size(32, 32);
                }
                else
                {
                    textBox_Rx_Data.Visible = true;
                    this.Size = new System.Drawing.Size(360, 180);
                }
            }
        }

        private void Simple_Client_LAN_Control_Load(object sender, EventArgs e)
        {
            //this.Size = new System.Drawing.Size(32, 32);
        }

        private void timer_LED_Tick(object sender, EventArgs e)
        {
            if (is_Connected)
            {
                if (is_Data_Received)
                {
                    is_Data_Received = false;
                    label_Lan_Status.ForeColor = label_Lan_Status.ForeColor == Color.Black ? Color.LimeGreen : Color.Black;
                }
                else
                {
                    label_Lan_Status.ForeColor = Color.Gray;
                }
            }
            else
            {
                label_Lan_Status.ForeColor = Color.Tomato;
            }
        }

        private void LogConnectionError(string message)
        {
            // Log the message to a TextBox or a log file instead of showing a MessageBox
            Console.WriteLine($"{DateTime.Now}: {message}");
        }

        private void timer_Connection_Retry_Tick(object sender, EventArgs e)
        {
            timer_Connection_Retry.Stop();
            retryCount = 0;
            Task.Run(() => ConnectAsync());
        }
    }
}

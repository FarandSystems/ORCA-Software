using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace Simple_VCP_Control
{
    public partial class Simple_VCP_Control : UserControl
    {
        string[] port_Names;       


        public enum Connection_Status_Enum
        {
            Disconnected,
            Connected,
            Ready_To_Retry,
            Connecting,
            Port_Opened
        }

        Connection_Status_Enum connection_Status = Connection_Status_Enum.Disconnected;

        bool is_Minimised = true;
        public bool Is_Minimised
        {
            get
            {
                return is_Minimised;
            }
            set
            {
                is_Minimised = value;
                Change_Size();
            }
        }

        bool is_Port_Open = false;
        public bool Is_Port_Open
        {
            get
            {
                return is_Port_Open;
            }            
        }

        int rx_Byte_Count = 8;
        public int Rx_Byte_Count
        {
            get
            {
                return rx_Byte_Count;
            }
            set
            {
                rx_Byte_Count = value;
                byte[] rx_Bytes_Raw = new byte[rx_Byte_Count];
                byte[] rx_Bytes = new byte[rx_Byte_Count];


                serial_Port.ReceivedBytesThreshold = value;
            }
        }

        byte[] rx_Bytes_Raw = new byte[8];

        byte[] rx_Bytes = new byte[8];
        public byte[] Rx_Bytes
        {
            get
            {
                return rx_Bytes;
            }
            set
            {
                rx_Bytes = value;
            
            }
        }


        int baud_Rate = 9600;
        public int Baud_Rate
        {
            get
            {
                return baud_Rate;
            }
            set
            {
                baud_Rate = value;
                serial_Port.BaudRate = value;

                
            }
        }

        // delegate is used to write to a UI control from a non-UI thread 
        private delegate void SetTextDeleg(byte[] dataBytes);

        public event EventHandler Received_Data_Ready;

        public Simple_VCP_Control()
        {
            InitializeComponent();
            Update_Port_List();
        }

        private void Update_Port_List()
        {
            port_Names = SerialPort.GetPortNames();

            for (int i = 0; i < port_Names.Length; i++)
            {
                comboBox_Available_Ports.Items.Add(port_Names[i]);
            }

            if (port_Names.Length > 1)
            {
                comboBox_Available_Ports.Text = comboBox_Available_Ports.Items[1].ToString();
            }
            else
            {
                comboBox_Available_Ports.Text = comboBox_Available_Ports.Items[0].ToString();
            }
        }

        private void panel_COM_LED_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                Change_Size();
            }
            else
            {
                if (serial_Port.IsOpen == true)
                {
                    serial_Port.Close(); 
                }
                Start_Connection();
            }
        }

        private void Change_Size()
        {
            if (is_Minimised == true)
            {
                is_Minimised = false;

                this.Width = 128;
                this.Height = 28;
                toolTip1.SetToolTip(panel_COM_LED, "Press Ctrl + Click to Maximise.");
            }
            else
            {
                is_Minimised = true;
                this.Width = 480;
                this.Height = 160;
                toolTip1.SetToolTip(panel_COM_LED, "Press Ctrl + Click to Mainimise.");
                Update_LED_Color();
            }

            panel1.Width = this.Width - 2;
            panel1.Height = this.Height - 2;
        }

        private void Update_LED_Color()
        {
            switch (connection_Status)
            {
                case Connection_Status_Enum.Connected:
                    panel_COM_LED.BackColor = Color.LimeGreen;
                    break;
                case Connection_Status_Enum.Ready_To_Retry:
                    panel_COM_LED.BackColor = Color.Gold;
                    break;
                case Connection_Status_Enum.Disconnected:
                    panel_COM_LED.BackColor = Color.Tomato;
                    break;
                case Connection_Status_Enum.Port_Opened:
                    panel_COM_LED.BackColor = Color.MediumTurquoise;
                    break;
            }
        }

        private void Simple_VCP_Control_Load(object sender, EventArgs e)
        {
            Change_Size();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(20);
            rx_Bytes_Raw = new byte[serial_Port.ReceivedBytesThreshold];
            if (serial_Port.IsOpen == true && serial_Port.BytesToRead > 0) // Note When Receiveing multiple of 64 bytes from STM32F7, a Zero Length Packet should be sent after sending the data into USB interface
            {// This will generate an interrupt in C# while there is no bytes received by the serial port!, so it is necessary to check if the BytesToRead property is non-zero
                serial_Port.Read(rx_Bytes_Raw, 0, rx_Bytes_Raw.Length);
                // Invokes the delegate on the UI thread, and sends the receivedBytes that was received to the invoked method. 
                // ---- The "ShowReceivedData" method will be executed on the UI thread which allows populating of the textbox. 
                this.BeginInvoke(new SetTextDeleg(Read_Received_Data), new object[] { rx_Bytes_Raw });
            }
        }

        public void Start_Connection()
        {
            try
            {
                serial_Port.ReceivedBytesThreshold = Rx_Byte_Count;
                serial_Port.BaudRate = 9600;
                serial_Port.DataBits = 8;
                serial_Port.Parity = Parity.None;
                serial_Port.StopBits = StopBits.One;
                serial_Port.ReadBufferSize = 16;
                serial_Port.DataReceived += SerialPort_DataReceived;
                serial_Port.PortName = comboBox_Available_Ports.Text;
                serial_Port.Open();
                label_Status.Text = serial_Port.PortName + " opened!";
                connection_Status = Connection_Status_Enum.Port_Opened;
                Update_LED_Color();
                is_Port_Open = true;
            }
            catch
            {
                MessageBox.Show("Can not open serial port " + serial_Port.PortName);
                is_Port_Open = false;
            }
        }

        private void Read_Received_Data(byte[] receivedBytes_Raw)
        {


            rx_Bytes = Decode_0x1A(receivedBytes_Raw);
            // Test CheckSum


            if (checkBox_Rx_Bytes.Checked == true)
            {
                Show_Received_Bytes(rx_Bytes);
            }

            // Toggle COM LED
            if (panel_COM_LED.BackColor == Color.DimGray)
            {
                panel_COM_LED.BackColor = Color.LimeGreen;
            }
            else
            {
                panel_COM_LED.BackColor = Color.DimGray;
            }

            // Fire Rx Dta Ready event
            if (Received_Data_Ready != null)
            {
                Received_Data_Ready(null, null);
            }
        }    

        private byte[] Decode_0x1A(byte[] codedBytes)
        {
            byte[] decodedBytes = new byte[codedBytes.Length];



            // Copy entire array into decodedBytes
            for (int i = 0; i < codedBytes.Length; i++)
            {
                decodedBytes[i] = codedBytes[i];
            }

            int dataPackCount = codedBytes.Length / 8;

            // Decode decodedBytes
            for (int dataPack = 0; dataPack < dataPackCount; dataPack++) // there are 32*16 = 512 data bytes is each sector
            {
                byte codeByte = decodedBytes[dataPack * 8]; // read Code Byte for byte0 - 6			
                byte code_Nibble = (byte)(0xF0 & codeByte); // codeByte: abc0 defg  code_Nibble:abc0 0000
                code_Nibble = (byte)(code_Nibble >> 1);     // codeByte: abc0 defg  code_Nibble:0abc 0000

                codeByte = (byte)(code_Nibble | (0x0F & codeByte)); //codeByte =  0abc 0000 | 0000 defg = 0abcdefg

                // check  Code Byte for channel 0 - 6
                for (int index = 0; index < 7; index++)
                {
                    if (((codeByte >> index) & 0x01) == 1)
                    { // Replace byte with 0x1A
                        decodedBytes[8 * dataPack + index + 1] = 0x1A;
                    }

                }
            }
            return decodedBytes;
        }
      
        private void Show_Received_Bytes(byte[] bytesToShow)
        {
            string s = "";

            for (int i = 0; i < bytesToShow.Length; i++)
            {
                s += Convert_To_Hex_Format(bytesToShow[i]) + ", ";
            }

            textBox_Rx_Bytes.Text = s;

        }

        private string Convert_To_Hex_Format(int n)
        {
            string s;
            s = Convert.ToString(n, 16);
            if (s.Length < 2)
            {
                s = "0" + s;
            }
            s = "0x" + s.ToUpper();
            return s;
        }

        public void Send_Data(byte[] tx_Bytes)
        {
            serial_Port.Write(tx_Bytes, 0, tx_Bytes.Length);
        }
    }

}

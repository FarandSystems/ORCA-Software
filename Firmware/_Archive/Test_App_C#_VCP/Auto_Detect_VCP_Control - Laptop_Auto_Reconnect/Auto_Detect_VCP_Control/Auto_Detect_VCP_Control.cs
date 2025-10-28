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

namespace Auto_Detect_VCP_Control
{
    public partial class Auto_Detect_VCP_Control : UserControl
    {
        // ---- ADDED: Timer-related fields ----
        private System.Windows.Forms.Timer reconnectTimer;
        private int reconnectAttempts = 0;
        private int maxReconnectAttempts = 15;  // Or whatever you prefer
        private int reconnectInterval = 2000;  // in milliseconds (3 seconds)
        // -------------------------------------

        public Auto_Detect_VCP_Control()
        {
            InitializeComponent();
            //Update_Port_List();
            // ---- ADDED: Initialize the reconnect timer ----
            reconnectTimer = new System.Windows.Forms.Timer();
            reconnectTimer.Interval = reconnectInterval;
            reconnectTimer.Tick += ReconnectTimer_Tick;
            // ------------------------------------------------


        }


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

        public enum System_State_Enum
        {
            GET_COMPORT_LIST,
            OPEN_COMX,
            WAIT,
            SEND_COMMAND_TO_COMX,
            WAIT_FOR_RESPONSE_UNTILL_TIMEOUT,
            GO_TO_NEXT_COM,
            CHECK_RECEIVED_CODE,
            COMX_IS_CONNECTED,
            NORMAL_OPERATION,
            ERROR_STATE
        }

        System_State_Enum system_State_Enum = System_State_Enum.GET_COMPORT_LIST;


        bool is_Minimised = false;
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

        int rx_Byte_Count = 56;
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

        byte[] rx_Bytes_Raw = new byte[56];


        byte[] rx_Bytes = new byte[56];
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

        int communication_Response = 0;
        public int Communication_Response
        {
            get
            {
                return communication_Response;
            }
            set
            {
                communication_Response = value;

            }
        }

        int communication_Response_Byte_Index = 0;
        public int Communication_Response_Byte_Index
        {
            get
            {
                return communication_Response_Byte_Index;
            }
            set
            {
                communication_Response_Byte_Index = value;

            }
        }


        int start_Communication_Byte_Index = 0;
        public int Start_Communication_Byte_Index
        {
            get
            {
                return start_Communication_Byte_Index;
            }
            set
            {
                start_Communication_Byte_Index = value;

            }
        }

        int start_Communication_Byte = 0;
        public int Start_Communication_Byte
        {
            get
            {
                return start_Communication_Byte;
            }
            set
            {
                start_Communication_Byte = value;

            }
        }

        bool start_VCP_Connection = false;
        public bool Start_VCP_Connection
        {
            get
            {
                return start_VCP_Connection;
            }
            set
            {
                start_VCP_Connection = value;
                if (start_VCP_Connection == true)
                {
                    timer_State_Machine.Start();
                }


            }
        }

        bool close_Serialport = false;
        public bool Close_Serialport
        {
            get
            {
                return close_Serialport;
            }
            set
            {
                close_Serialport = value;
                if (close_Serialport == true)
                {
                    serial_Port.Close();
                }


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

        int counter = 0;

        byte[] command_Bytes = new byte[128];
        public byte[] Command_Bytes
        {
            get
            {
                return command_Bytes;
            }
            set
            {
                command_Bytes = value;


            }
        }

        int command_Byte_Count = 56;
        public int Command_Byte_Count
        {
            get
            {
                return command_Byte_Count;
            }
            set
            {
                command_Byte_Count = value;
                byte[] command_Bytes = new byte[command_Byte_Count];
            }
        }


        bool right_COM_Is_Chosen = false;

        bool introduction_Command_Received_Flag = false;

        int port_Counter = 0;
        //mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
        public event EventHandler Connection_Ready;
        public event EventHandler Connection_Failed;

        private void Fire_Connection_Ready()
        {
            if (Connection_Ready != null)
            {
                Connection_Ready(this, null);
            }
        }
        private void Fire_Connection_Failed()
        {
            if (Connection_Failed != null)
            {
                Connection_Failed(this, null);
            }
        }
        //mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm

        // delegate is used to write to a UI control from a non-UI thread 
        private delegate void SetTextDeleg(byte[] dataBytes);

        public event EventHandler Received_Data_Ready;
        public event EventHandler Normal_Operation_Starts;

        private void Update_Auto_Detect_State_Machine()
        {
            switch (system_State_Enum)
            {
                case System_State_Enum.GET_COMPORT_LIST:
                    port_Names = SerialPort.GetPortNames();
                    if (port_Names.Length != 0)
                    {
                        for (int i = 0; i < port_Names.Length; i++)
                        {
                            comboBox_Available_Ports.Items.Add(port_Names[i]);
                        }
                        system_State_Enum = System_State_Enum.OPEN_COMX;
                    }

                    //for (int i = 0; i < port_Names.Length; i++)
                    //{
                    //    serial_Port.PortName = port_Names[i];
                    //    serial_Port.Open();
                    //    serial_Port.Close();
                    //}

                    

                    break;

                case System_State_Enum.OPEN_COMX:

                    comboBox_Available_Ports.Text = port_Names[port_Counter];
                    serial_Port.ReceivedBytesThreshold = Rx_Byte_Count;
                    serial_Port.BaudRate = 9600;
                    serial_Port.DataBits = 8;
                    serial_Port.Parity = Parity.None;
                    serial_Port.StopBits = StopBits.One;
                    //serial_Port.ReadBufferSize = 16;
                    serial_Port.DataReceived += SerialPort_DataReceived;
                    serial_Port.PortName = port_Names[port_Counter];
                    try
                    {
                        serial_Port.Open();
                        label_Status.Text = serial_Port.PortName + " opened!";
                        connection_Status = Connection_Status_Enum.Port_Opened;
                        Update_LED_Color();
                        is_Port_Open = true;
                        system_State_Enum = System_State_Enum.WAIT;

                    }
                    catch (Exception ex)
                    {
                        
                        system_State_Enum = System_State_Enum.GO_TO_NEXT_COM;
                    }
                    break;

                case System_State_Enum.WAIT:
                    counter++;
                    if (counter == 4)
                    {
                        counter = 0;
                        system_State_Enum = System_State_Enum.SEND_COMMAND_TO_COMX;
                    }
                    break;

                case System_State_Enum.SEND_COMMAND_TO_COMX:
                    Send_Start_Communication_Command();
                    system_State_Enum = System_State_Enum.WAIT_FOR_RESPONSE_UNTILL_TIMEOUT;
                    break;

                case System_State_Enum.WAIT_FOR_RESPONSE_UNTILL_TIMEOUT:
                    counter++;
                    if (counter == 10)
                    {
                        counter = 0;
                        system_State_Enum = System_State_Enum.GO_TO_NEXT_COM;
                    }

                    if (introduction_Command_Received_Flag == true)
                    {
                        //button_Start.BackColor = Color.Red;
                        introduction_Command_Received_Flag = false;
                        system_State_Enum = System_State_Enum.CHECK_RECEIVED_CODE;
                    }
                    break;

                case System_State_Enum.GO_TO_NEXT_COM:
                    serial_Port.Close();
                    port_Counter++;
                    if (port_Counter >= port_Names.Length)
                    {
                        label_Status.Text = "COM Not Foumd!";
                        label_Status.ForeColor = Color.Red;

                        comboBox_Available_Ports.Text = "COM Not Foumd!";
                        system_State_Enum = System_State_Enum.ERROR_STATE;
                    }
                    else
                    {
                        system_State_Enum = System_State_Enum.OPEN_COMX;
                    }

                    break;

                case System_State_Enum.CHECK_RECEIVED_CODE:
                    if (right_COM_Is_Chosen == true)
                    {
                        right_COM_Is_Chosen = false;
                        system_State_Enum = System_State_Enum.COMX_IS_CONNECTED;
                    }
                    else
                    {
                        system_State_Enum = System_State_Enum.GO_TO_NEXT_COM;
                    }
                    break;

                case System_State_Enum.COMX_IS_CONNECTED:
                    system_State_Enum = System_State_Enum.NORMAL_OPERATION;
                    break;

                case System_State_Enum.NORMAL_OPERATION:

                    Start_Normal_Operation();
                    break;

                case System_State_Enum.ERROR_STATE:

                    break;


            }
        }

        private void Start_Normal_Operation()
        {
            if (Normal_Operation_Starts != null)
            {
                Normal_Operation_Starts(this, null);
            }
            timer_State_Machine.Stop();
        }
        //private void Update_Port_List()
        //{
        //    port_Names = SerialPort.GetPortNames();

        //    for (int i = 0; i < port_Names.Length; i++)
        //    {
        //        comboBox_Available_Ports.Items.Add(port_Names[i]);
        //    }

        //    if (port_Names.Length > 1)
        //    {
        //        comboBox_Available_Ports.Text = comboBox_Available_Ports.Items[1].ToString();
        //    }
        //    else
        //    {
        //        comboBox_Available_Ports.Text = comboBox_Available_Ports.Items[0].ToString();
        //    }
        //}

        private void Send_Start_Communication_Command()
        {
            Clear_All_Buffers();
            //command_Bytes[start_Communication_Byte_Index] = (byte)start_Communication_Byte;
            //command_Bytes[start_Communication_Byte_Index + 1] = (byte)start_Communication_Byte;
            //command_Bytes[15] = Calculate_CheckSum(command_Bytes);
            Send_Data(command_Bytes);
        }

        byte Calculate_CheckSum(byte[] data_Bytes)
        {
            byte cs = 0;

            for (int k = 0; k < data_Bytes.Length - 1; k++)
            {
                cs += data_Bytes[k];
            }
            return cs;
        }

        private void Clear_All_Buffers()
        {
            for (int i = 0; i < command_Bytes.Length; i++)
            {
                command_Bytes[i] = 0;
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
                Reconnect();
            }
        }

        private void Change_Size()
        {
            if (is_Minimised == true)
            {
                is_Minimised = false;

                this.Width = 15;
                this.Height = 15;
                toolTip1.SetToolTip(label_Connection, "Press Ctrl + Click to Maximise.");
            }
            else
            {
                is_Minimised = true;
                this.Width = 480;
                this.Height = 160;
                toolTip1.SetToolTip(label_Connection, "Press Ctrl + Click to Mainimise.");
                Update_LED_Color();
            }

            //panel1.Width = this.Width - 2;
            //panel1.Height = this.Height - 2;
        }

        private void Update_LED_Color()
        {
            switch (connection_Status)
            {
                case Connection_Status_Enum.Connected:
                    label_Connection.ForeColor = Color.LimeGreen;
                    break;
                case Connection_Status_Enum.Ready_To_Retry:
                    label_Connection.ForeColor = Color.Gold;
                    break;
                case Connection_Status_Enum.Disconnected:
                    label_Connection.ForeColor = Color.Tomato;
                    break;
                case Connection_Status_Enum.Port_Opened:
                    label_Connection.ForeColor = Color.MediumTurquoise;
                    break;
            }
        }

        private void VCP_Control_Load(object sender, EventArgs e)
        {
            Change_Size();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //introduction_Command_Received_Flag = true;
            //Thread.Sleep(20);
            //rx_Bytes_Raw = new byte[serial_Port.ReceivedBytesThreshold];
            //if (serial_Port.IsOpen == true && serial_Port.BytesToRead > 0) // Note When Receiveing multiple of 64 bytes from STM32F7, a Zero Length Packet should be sent after sending the data into USB interface
            //{// This will generate an interrupt in C# while there is no bytes received by the serial port!, so it is necessary to check if the BytesToRead property is non-zero
            //    serial_Port.Read(rx_Bytes_Raw, 0, rx_Bytes_Raw.Length);
            //    // Invokes the delegate on the UI thread, and sends the receivedBytes that was received to the invoked method. 
            //    // ---- The "ShowReceivedData" method will be executed on the UI thread which allows populating of the textbox. 
            //    this.BeginInvoke(new SetTextDeleg(Read_Received_Data), new object[] { rx_Bytes_Raw });
            //}

            try
            {
                introduction_Command_Received_Flag = true;
                Thread.Sleep(20);
                rx_Bytes_Raw = new byte[serial_Port.ReceivedBytesThreshold];
                if (serial_Port.IsOpen == true && serial_Port.BytesToRead > 0)
                {
                    serial_Port.Read(rx_Bytes_Raw, 0, rx_Bytes_Raw.Length);
                    this.BeginInvoke(new SetTextDeleg(Read_Received_Data), new object[] { rx_Bytes_Raw });
                }
                else if (!serial_Port.IsOpen)
                {
                    Exception ex = new Exception();
                    HandlePortError(ex);
                }
            }
            catch (Exception ex)
            {
                // If we get an exception here, for instance if the device was unplugged,
                // we can attempt to reconnect.
                HandlePortError(ex);
            }

        }

        public void Reconnect()
        {
            try
            {
                if (comboBox_Available_Ports.Items.Count == 0)
                {
                    // No COM ports found at all
                    connection_Status = Connection_Status_Enum.Disconnected;
                    Update_LED_Color();
                    label_Status.Text = "No COM ports available.";
                    return;
                }

                serial_Port.ReceivedBytesThreshold = rx_Byte_Count;
                serial_Port.BaudRate = 9600;
                serial_Port.DataBits = 8;
                serial_Port.Parity = Parity.None;
                serial_Port.StopBits = StopBits.One;
                //serial_Port.ReadBufferSize = 16;
                serial_Port.DataReceived += SerialPort_DataReceived;
                serial_Port.PortName = comboBox_Available_Ports.Text;

                // Attempt to open the port
                serial_Port.Open();
                connection_Status = Connection_Status_Enum.Port_Opened;
                Update_LED_Color();
                is_Port_Open = true;
                //label_Status.Text = serial_Port.PortName + " opened!";
                Fire_Connection_Ready();
                label_Status.Text = "Connected!";

                // If opened, reset reconnect attempts just in case
                reconnectAttempts = 0;
            }
            catch (Exception ex)
            {
                // On error, start the reconnect timer
                is_Port_Open = false;
                HandlePortError(ex);
            }

        }

        private void Read_Received_Data(byte[] receivedBytes_Raw)
        {
            rx_Bytes = Decode_0x1A(receivedBytes_Raw);
            // Test CheckSum

            //if (rx_Bytes[communication_Response_Byte_Index] == (byte)communication_Response && rx_Bytes[communication_Response_Byte_Index + 1] == (byte)communication_Response)
            //{
                right_COM_Is_Chosen = true;
            //}


            if (checkBox_Rx_Bytes.Checked == true)
            {
                Show_Received_Bytes(rx_Bytes);
            }

            // Toggle COM LED
            if (label_Connection.ForeColor == Color.DimGray)
            {
                label_Connection.ForeColor = Color.LimeGreen;
            }
            else
            {
                label_Connection.ForeColor = Color.DimGray;
            }

            // Fire Rx Data Ready event
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
            //if (close_Serialport == false)
            //{
            //    serial_Port.Write(tx_Bytes, 0, tx_Bytes.Length);
            //}

            try
            {
                if (serial_Port.IsOpen)
                {
                    serial_Port.Write(tx_Bytes, 0, tx_Bytes.Length);

                }
                else
                {
                    Exception ex = new Exception();
                    HandlePortError(ex);
                }
                //Console.WriteLine("Sent");
            }
            catch (Exception ex)
            {
                HandlePortError(ex);
            }

        }

        private void timer_State_Machine_Tick(object sender, EventArgs e)
        {
            Update_Auto_Detect_State_Machine();
            label_State.Text = system_State_Enum.ToString();
            //Console.WriteLine(system_State_Enum.ToString());
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            timer_State_Machine.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        public void Close_Port()
        {
            serial_Port.Close();
        }

        private void panel_COM_LED_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // ---- ADDED: Handle errors by scheduling reconnect attempts ----
        private void HandlePortError(Exception ex)
        {
            // If the port is open, close it, because it may be in a fault state
            if (serial_Port.IsOpen)
            {
                try
                {

                    serial_Port.Close();
                }
                catch
                {
                    textBox_Rx_Bytes.Text = ex.Message;
                }
            }
            Fire_Connection_Failed();
            connection_Status = Connection_Status_Enum.Disconnected;
            Update_LED_Color();
            //label_Status.Text = "Port error: " + ex.Message;
            is_Port_Open = false;

            // Start the reconnect process
            StartReconnectTimer();
        }
        private void StartReconnectTimer()
        {
            reconnectAttempts = 0;
            reconnectTimer.Start();
        }

        private void ReconnectTimer_Tick(object sender, EventArgs e)
        {
            if (!serial_Port.IsOpen)
            {
                label_Status.Text = "Reconnect...";
                if (reconnectAttempts < maxReconnectAttempts)
                {
                    reconnectAttempts++;
                    //label_Status.Text = $"Trying to reconnect... (attempt {reconnectAttempts}/{maxReconnectAttempts})";
                    // Try to re-open the port
                    Reconnect();
                }
                else
                {
                    // Stop trying after max attempts
                    reconnectTimer.Stop();
                    //label_Status.Text = "Max reconnect attempts reached. Stopped trying.";
                }
            }
            else
            {
                // If for some reason the port is now open, we can stop the timer
                reconnectTimer.Stop();
                //label_Status.Text = "Connected.";
            }

            // ---------------------------------------------------------------

        }

        private void label_Connection_Click(object sender, EventArgs e)
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
                Reconnect();
            }
        }
    }
}


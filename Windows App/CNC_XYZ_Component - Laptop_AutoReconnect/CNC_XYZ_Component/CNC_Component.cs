using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;

namespace CNC_XYZ_Component
{
    public partial class CNC_Component : UserControl
    {
        int com_Counter = 0;
        byte[] command_Bytes = new byte[32];
        byte[] rx_Bytes = new byte[56];
        bool command_Is_Ready = false;
        double x_Start_Position;
        double y_Start_Position;
        double z_Start_Position;
        int x_Auto_Step_Counter;
        int y_Auto_Step_Counter;
        int z_Auto_Step_Counter;
        int first_Priority_Step_Counter;
        int second_Priority_Step_Counter;
        int third_Priority_Step_Counter;
        int time_Counter = 0;
        int photo_Count = 0;
        int photo_Counter = 0;

        int x_Auto_Step_mm = 0;
        int y_Auto_Step_mm = 0;
        int z_Auto_Step_mm = 0;

        int x_Auto_Step_Count = 0;
        int y_Auto_Step_Count = 0;
        int z_Auto_Step_Count = 0;

        bool is_Auto_Step_Running = false;
        bool is_Auto_Stepping_Paused = false;
        bool is_Auto_Stepping_Canceled = false;

        bool is_Moving_image = false;
        public event EventHandler Take_Photo;

        public event EventHandler Show_Help_Image;

        bool start_Connection = false;
        public bool Start_Connection
        {
            get
            {
                return start_Connection;
            }
            set
            {
                start_Connection = value;
                if (start_Connection == true)
                {
                    vcp.Start_VCP_Connection = true;
                }

            }
        }
        public enum Position_Status
        {
            Backward,
            Idle,
            Forward
        }

        Position_Status x_Position_Status = Position_Status.Idle;
        public Position_Status X_Position_Status
        {
            get { return x_Position_Status; }
            set
            {
                x_Position_Status = value;
            }
        }

        Position_Status y_Position_Status = Position_Status.Idle;
        public Position_Status Y_Position_Status
        {
            get { return y_Position_Status; }
            set
            {
                y_Position_Status = value;
            }
        }



        Position_Status z_Position_Status = Position_Status.Idle;
        public Position_Status Z_Position_Status
        {
            get { return z_Position_Status; }
            set
            {
                z_Position_Status = value;
            }
        }

        string project_Path;
        public string Project_Path
        {
            get { return project_Path; }
            set { project_Path = value; }
        }

        string images_Path = "Farand Systems\\PDIA_Images";
        public string Images_Path
        {
            get { return images_Path; }
            set { images_Path = value; }
        }

        Bitmap captured_Image;
        public Bitmap Captured_Image
        {
            get { return captured_Image; }
            set { captured_Image = value; }
        }

        string image_File_Name;
        public string Image_File_Name
        {
            get { return image_File_Name; }
            set { image_File_Name = value; }
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
                    timer1.Stop();
                    vcp.Close_Serialport = true;
                    
                }
            }
        }

        bool more_Settings = false;
        public bool More_Settings
        {
            get
            {
                return more_Settings;
            }
            set
            {
                more_Settings = value;
                //if(more_Settings == true)
                //{
                //    X.More_Settings = true; 
                //    Y.More_Settings = true;
                //    Z.More_Settings = true;
                //    //groupBox_XYZ.Visible = true;
                //    groupBox_Auto_Stepping.Visible = true;
                //}
                //else
                //{
                //    X.More_Settings = false;
                //    Y.More_Settings = false;
                //    Z.More_Settings = false;
                //    //groupBox_XYZ.Visible = false;
                //    groupBox_Auto_Stepping.Visible = false;

                //}


            }
        }

        private double x_Current_Position;
        public double X_Current_Position
        {
            get { return x_Current_Position; }
            set { x_Current_Position = value; }
        }

        private double y_Current_Position;
        public double Y_Current_Position
        {
            get { return y_Current_Position; }
            set { y_Current_Position = value; }
        }

        private double z_Current_Position;
        public double Z_Current_Position
        {
            get { return z_Current_Position; }
            set { z_Current_Position = value; }
        }


        public enum Auto_Position_Enum
        {
            STEP_WAIT = 0,
            PHOTOGRAPH = 1,
            WAIT_AFTER_TAKING_PICTURE = 2,
            WAIT = 3,
            STEP = 4,
            CONTINUE = 5,
            PAUSED = 6,
            CANCELED = 7

        }
        Auto_Position_Enum auto_Positioning_System_State = Auto_Position_Enum.STEP_WAIT;

        public enum Controller_State_Enum
        {
            START_UP_STATE = 0,
            GO_HOME_STATE = 1,
            GO_TO_CENTER_POSITION = 2,
            GO_TO_USER_POSITION_STATE = 3,
            IDLE_STATE = 4,
            ERROR_STATE = 5,
            GET_OUT_OF_LIMIT_RANGE = 6,
            RETREAT_FROM_HOME_STATE = 7,
            FIND_HOME_STATE = 8,
        }
        Controller_State_Enum x_Controller_State;
        Controller_State_Enum y_Controller_State;
        Controller_State_Enum z_Controller_State;

        public event EventHandler Connection_Succeed;
        public event EventHandler CNC_Connection_Ready;
        public event EventHandler CNC_Connection_Failed;
        public event EventHandler New_Image_Captured;
        public event EventHandler Positions_Statuses_Changed;
        private void Fire_New_Image_Captured()
        {
            if (New_Image_Captured != null)
            {
                New_Image_Captured(this, null);
            }
        }

        private void Fire_Positions_Statuses_Changed()
        {
            if (Positions_Statuses_Changed != null)
            {
                Positions_Statuses_Changed(this, null);
            }
        }

        public CNC_Component()
        {
            InitializeComponent();
        }

        private void CNC_Component_Load(object sender, EventArgs e)
        {
            Initialize_Virtual_COM_Port();
            Initialize_Axis_Component();
            textBox_Info.Visible = false;
            textBox_Rx_Data.Visible = false;
            //
            //ChangeFontSizeOnly(this, 3f); // Adjust font size to 12 for all controls without resizing components
            //ResizeForm();

        }
        private void ResizeForm()
        {
            float scaleFactorX = (float)1366 / 1920;
            float scaleFactorY = (float)768 / 1080;

            foreach (Control control in this.Controls)
            {
                control.Scale(new SizeF(scaleFactorX, scaleFactorY));
            }
        }

        private void ChangeFontSizeOnly(Control control, float newSize)
        {
            //// Set the font size for the current control without resizing the component
            //control.Font = new Font(control.Font.FontFamily, newSize, control.Font.Style);

            //// Recursively apply to all child controls
            //foreach (Control childControl in control.Controls)
            //{
            //    // If AutoSize is enabled, temporarily disable it to prevent resizing
            //    bool originalAutoSize = childControl.AutoSize;
            //    childControl.AutoSize = false;

            //    ChangeFontSizeOnly(childControl, newSize);

            //    // Restore original AutoSize setting
            //    childControl.AutoSize = originalAutoSize;
            //}
        }



        private void Initialize_Axis_Component()
        {
            X.Axis_Name = "X";
            X.Command_Address = 0x01;
            X.Mm_Per_Rotation = 4;
            X.Stepper_Motor_Resolution = 400;
            X.Axis_Course = 200;

            Y.Axis_Name = "Y";
            Y.Command_Address = 0xA1;
            Y.Mm_Per_Rotation = 5;
            Y.Stepper_Motor_Resolution = 400;

            Z.Axis_Name = "Z";
            Z.Command_Address = 0xB1;
            Z.Mm_Per_Rotation = 4;
            Z.Stepper_Motor_Resolution = 400;
        }

        private void Initialize_Virtual_COM_Port()
        {
            vcp.Is_Minimised = true;
            
            vcp.Rx_Byte_Count = 56;
            vcp.Baud_Rate = 9600;
            vcp.Communication_Response = 0x55;
            vcp.Communication_Response_Byte_Index = 51;
            vcp.Start_Communication_Byte = 0x55;
            vcp.Start_Communication_Byte_Index = 27;

            vcp.Connection_Failed += Vcp_Connection_Failed;
            vcp.Connection_Ready += Vcp_Connection_Ready;

            vcp.Received_Data_Ready += Vcp_Received_Data_Ready;
            vcp.Normal_Operation_Starts += Vcp_Normal_Operation_Starts;
            //vcp.Start_Connection();
        }

        private void Vcp_Connection_Ready(object sender, EventArgs e)
        {
            if(CNC_Connection_Ready != null)
            {
                CNC_Connection_Ready(this,null);
            }

        }

        private void Vcp_Connection_Failed(object sender, EventArgs e)
        {
            if(CNC_Connection_Failed != null)
            {
                CNC_Connection_Failed(this,null);
            }
        }

        private void Vcp_Normal_Operation_Starts(object sender, EventArgs e)
        {
            timer1.Start();
            if(Connection_Succeed != null)
            {
                Connection_Succeed(this, null);
            }
            
        }

        private void Vcp_Received_Data_Ready(object sender, EventArgs e)
        {
            Byte checkSum = 0;
            for (int i = 0; i < vcp.Rx_Bytes.Length - 1; i++)
            {
                checkSum += vcp.Rx_Bytes[i];
            }

            Array.Copy(vcp.Rx_Bytes, rx_Bytes, vcp.Rx_Bytes.Length);
               
            Show_Received_Bytes(rx_Bytes);
            Show_Current_Position();
            Update_XYZ_Property();
            Show_Information();
            Show_Errors();
            Update_State_Machine();

            label_x.Text = x_Auto_Step_Counter.ToString();
            label_y.Text = y_Auto_Step_Counter.ToString();
            label_z.Text = z_Auto_Step_Counter.ToString();
        }
        private void Step()
        {
            Find_Axis_Priority();

            if (x_Auto_Step_Counter == 0 && y_Auto_Step_Counter == 0 && z_Auto_Step_Counter == 0)
            {
                is_Auto_Step_Running = false;
                pictureBox_Auto_Stepping.Image = Properties.Resources.Stepping_Run_Highlighted;
            }

            Clear_All_Buffers();
            // Send Command Template
            double x_Dest = x_Start_Position + x_Auto_Step_mm * x_Auto_Step_Counter;
            int x_int = (int)(100 * x_Dest);
            byte x_LLB = (byte)((x_int & 0x000000FF) >> 0);
            byte x_LHB = (byte)((x_int & 0x0000FF00) >> 8);
            byte x_HLB = (byte)((x_int & 0x00FF0000) >> 16);
            byte x_HHB = (byte)((x_int & 0xFF000000) >> 24);

            double y_Dest = y_Start_Position + y_Auto_Step_mm * y_Auto_Step_Counter;
            int y_int = (int)(100 * y_Dest);
            byte y_LLB = (byte)((y_int & 0x000000FF) >> 0);
            byte y_LHB = (byte)((y_int & 0x0000FF00) >> 8);
            byte y_HLB = (byte)((y_int & 0x00FF0000) >> 16);
            byte y_HHB = (byte)((y_int & 0xFF000000) >> 24);

            double z_Dest = z_Start_Position + z_Auto_Step_mm * z_Auto_Step_Counter;
            int z_int = (int)(100 * z_Dest);
            byte z_LLB = (byte)((z_int & 0x000000FF) >> 0);
            byte z_LHB = (byte)((z_int & 0x0000FF00) >> 8);
            byte z_HLB = (byte)((z_int & 0x00FF0000) >> 16);
            byte z_HHB = (byte)((z_int & 0xFF000000) >> 24);

            command_Bytes[0] = 0xC2;
            command_Bytes[1] = x_LLB;
            command_Bytes[2] = x_LHB;
            command_Bytes[3] = x_HLB;
            command_Bytes[4] = x_HHB;
            command_Bytes[5] = y_LLB;
            command_Bytes[6] = y_LHB;
            command_Bytes[7] = y_HLB;
            command_Bytes[8] = y_HHB;
            command_Bytes[9] = z_LLB;
            command_Bytes[10] = z_LHB;
            command_Bytes[11] = z_HLB;
            command_Bytes[12] = z_HHB;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            command_Is_Ready = true;

        }

        private void Show_Errors()
        {
            string str_x = "";

            if ((rx_Bytes[9] & 0b00000001) != 0)
            {
                str_x += "Go to user position Error!,\r\n";
            }

            if ((rx_Bytes[9] & 0b00000010) != 0)
            {
                str_x += "Go to center position Error!,\r\n";
            }

            if ((rx_Bytes[9] & 0b00000100) != 0)
            {
                str_x += "Opto-Interrupter Error!,\r\n";
            }

            if ((rx_Bytes[9] & 0b00001000) != 0)
            {
                str_x += "Out of Rangr Error!,\r\n";
                str_x += "Please clear Error!,\r\n";
            }


            X.Errors = str_x;
            /////////////////////////////////////////////////

            string str_y = "";

            if ((rx_Bytes[10] & 0b00000001) != 0)
            {
                str_y += "Go to user position Error!,\r\n";
            }

            if ((rx_Bytes[10] & 0b00000010) != 0)
            {
                str_y += "Go to center position Error!,\r\n";
            }

            if ((rx_Bytes[10] & 0b00000100) != 0)
            {
                str_y += "Opto-Interrupter Error!,\r\n";
            }

            if ((rx_Bytes[10] & 0b00001000) != 0)
            {
                str_y += "Out of Rangr Error!,\r\n";
            }


            Y.Errors = str_y;
            ////////////////////////////////////////////////////

            string str_z = "";

            if ((rx_Bytes[11] & 0b00000001) != 0)
            {
                str_z += "Go to user position Error!,\r\n";
            }

            if ((rx_Bytes[11] & 0b00000010) != 0)
            {
                str_z += "Go to center position Error!,\r\n";
            }

            if ((rx_Bytes[11] & 0b00000100) != 0)
            {
                str_z += "Opto-Interrupter Error!,\r\n";
            }

            if ((rx_Bytes[11] & 0b00001000) != 0)
            {
                str_z += "Out of Rangr Error!,\r\n";
            }

            Z.Errors = str_z;

        }

        private Controller_State_Enum Get_Controller_Enum(byte b)
        {
            Controller_State_Enum state = Controller_State_Enum.IDLE_STATE;
            switch (b)
            {
                case 0:
                    state = Controller_State_Enum.START_UP_STATE;
                    break;

                case 1:
                    state = Controller_State_Enum.GO_HOME_STATE;
                    break;

                case 2:
                    state = Controller_State_Enum.GO_TO_CENTER_POSITION;
                    break;

                case 3:
                    state = Controller_State_Enum.GO_TO_USER_POSITION_STATE;
                    break;

                case 4:
                    state = Controller_State_Enum.IDLE_STATE;
                    break;

                case 5:
                    state = Controller_State_Enum.ERROR_STATE;
                    break;

                case 6:
                    state = Controller_State_Enum.GET_OUT_OF_LIMIT_RANGE;
                    break;

                case 7:
                    state = Controller_State_Enum.RETREAT_FROM_HOME_STATE;
                    break;

                case 8:
                    state = Controller_State_Enum.FIND_HOME_STATE;
                    break;
            }
            return state;
        }

        private void Show_Information()
        {
            string s = "";

            Byte checkSum = (byte)(rx_Bytes[12] + rx_Bytes[13] + rx_Bytes[14]);

            if (rx_Bytes[15] == checkSum)
            {
                x_Controller_State = Get_Controller_Enum(rx_Bytes[12]);
                y_Controller_State = Get_Controller_Enum(rx_Bytes[13]);
                z_Controller_State = Get_Controller_Enum(rx_Bytes[14]);

                s += " X Controller State = " + x_Controller_State.ToString() + "\r\n";
                s += " Y Controller State = " + y_Controller_State.ToString() + "\r\n";
                s += " Z Controller State = " + z_Controller_State.ToString() + "\r\n";

                textBox_Info.Text = s;

            }
        }

        private void Show_Current_Position()
        {
            X.Current_Position = 0.01 * (Int32)((rx_Bytes[20] << 24) + (rx_Bytes[19] << 16) + (rx_Bytes[18] << 8) + rx_Bytes[17]);
            Y.Current_Position = 0.01 * (Int32)((rx_Bytes[25] << 24) + (rx_Bytes[23] << 16) + (rx_Bytes[22] << 8) + rx_Bytes[21]);
            Z.Current_Position = 0.01 * (Int32)((rx_Bytes[29] << 24) + (rx_Bytes[28] << 16) + (rx_Bytes[27] << 8) + rx_Bytes[26]);
        }

        private void Update_XYZ_Property()
        {
            //// X
            if (x_Current_Position > X.Current_Position) // X Moving Forward
            {
                x_Position_Status = Position_Status.Backward;
                
            }
            else if (x_Current_Position < X.Current_Position) // X Moving Forward
            {
                x_Position_Status = Position_Status.Forward;
            }
            else // X IDLE
            {
                x_Position_Status = Position_Status.Idle;
            }
            //// Y
            if (y_Current_Position > Y.Current_Position) // Y Moving Forward
            {
                y_Position_Status = Position_Status.Backward;
            }
            else if (y_Current_Position < Y.Current_Position) // Y Moving Forward
            {
                y_Position_Status = Position_Status.Forward;
            }
            else // Y IDLE
            {
                y_Position_Status = Position_Status.Idle;
            }
            //// Z
            if (z_Current_Position > Z.Current_Position) // Z Moving Forward
            {
                z_Position_Status = Position_Status.Backward;
            }
            else if (z_Current_Position < Z.Current_Position) // Z Moving Forward
            {
                z_Position_Status = Position_Status.Forward;
            }
            else // Z IDLE
            {
                z_Position_Status = Position_Status.Idle;
            }
            //// Updating
            x_Current_Position = X.Current_Position;
            y_Current_Position = Y.Current_Position;
            z_Current_Position = Z.Current_Position;
            Fire_Positions_Statuses_Changed();
        }

        private void Show_Received_Bytes(byte[] bytesToShow)
        {
            string s = "";

            for (int i = 0; i < bytesToShow.Length; i++)
            {
                s += Convert_To_Hex_Format(bytesToShow[i]) + ", ";
            }

            textBox_Rx_Data.Text = s;

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            //com_Counter++;
            //if (X.Command_Is_Ready == true)
            //{
            //    X.Command_Is_Ready = false;
            //    Array.Copy(X.Command_Bytes, command_Bytes, X.Command_Bytes.Length);
            //}
            //else if (Y.Command_Is_Ready == true)
            //{
            //    Y.Command_Is_Ready = false;
            //    Array.Copy(Y.Command_Bytes, command_Bytes, Y.Command_Bytes.Length);
            //}
            //else if (Z.Command_Is_Ready == true)
            //{
            //    Z.Command_Is_Ready = false;
            //    Array.Copy(Z.Command_Bytes, command_Bytes, Z.Command_Bytes.Length);
            //}
            //else if (command_Is_Ready == true)
            //{
            //    command_Is_Ready = false;
            //}
            //else
            //{
            //    Clear_All_Buffers();

            //    command_Bytes[0] = 0x01;
            //    command_Bytes[29] = 0xAA;
            //    command_Bytes[30] = 0xAA;
            //    command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            //}

            //if (close_Serialport == false)
            //{
            //    vcp.Send_Data(command_Bytes);
            //}


        }

        public void Send_Data()
        {
            //try
            //{
                if (close_Serialport == false)
                {
                    // com_Counter++;
                    if (X.Command_Is_Ready == true)
                    {
                        X.Command_Is_Ready = false;
                        Array.Copy(X.Command_Bytes, command_Bytes, X.Command_Bytes.Length);
                    }
                    else if (Y.Command_Is_Ready == true)
                    {
                        Y.Command_Is_Ready = false;
                        Array.Copy(Y.Command_Bytes, command_Bytes, Y.Command_Bytes.Length);
                    }
                    else if (Z.Command_Is_Ready == true)
                    {
                        Z.Command_Is_Ready = false;
                        Array.Copy(Z.Command_Bytes, command_Bytes, Z.Command_Bytes.Length);
                    }
                    else if (command_Is_Ready == true)
                    {
                        command_Is_Ready = false;
                    }
                    else
                    {
                        Clear_All_Buffers();

                        command_Bytes[0] = 0x01;
                        command_Bytes[29] = 0xAA;
                        command_Bytes[30] = 0xAA;
                        command_Bytes[31] = Calculate_CheckSum(command_Bytes);

                    }

                    if (close_Serialport == false)
                    {
                        vcp.Send_Data(command_Bytes);
                    }
                }

            //}
            //catch (Exception ex)
            //{
               // Application.Exit();
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

        }
        private byte Calculate_CheckSum(byte[] data_Bytes)
        {
            int cs = 0;
            for (int i = 0; i < data_Bytes.Length - 1; i++)
            {
                cs = cs + data_Bytes[i];
            }
            return (byte)cs;
        }

        private void button_Emergency_Stop_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();
            // Send Command Template
            command_Bytes[0] = 0xC7;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            is_Auto_Stepping_Canceled = true;


            command_Is_Ready = true;


        }

        private void Clear_All_Buffers()
        {
            for (int i = 0; i < command_Bytes.Length; i++)
            {
                command_Bytes[i] = 0x00;
            }
        }

        private void button_XYZ_Go_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();
            // Send Command Template
            int x_int = (int)(100 * numericUpDown_X_General.Value);
            byte x_LLB = (byte)((x_int & 0x000000FF) >> 0);
            byte x_LHB = (byte)((x_int & 0x0000FF00) >> 8);
            byte x_HLB = (byte)((x_int & 0x00FF0000) >> 16);
            byte x_HHB = (byte)((x_int & 0xFF000000) >> 24);

            int y_int = (int)(100 * numericUpDown_Y_General.Value);
            byte y_LLB = (byte)((y_int & 0x000000FF) >> 0);
            byte y_LHB = (byte)((y_int & 0x0000FF00) >> 8);
            byte y_HLB = (byte)((y_int & 0x00FF0000) >> 16);
            byte y_HHB = (byte)((y_int & 0xFF000000) >> 24);

            int z_int = (int)(100 * numericUpDown_Z_General.Value);
            byte z_LLB = (byte)((z_int & 0x000000FF) >> 0);
            byte z_LHB = (byte)((z_int & 0x0000FF00) >> 8);
            byte z_HLB = (byte)((z_int & 0x00FF0000) >> 16);
            byte z_HHB = (byte)((z_int & 0xFF000000) >> 24);

            command_Bytes[0] = 0xC2;
            command_Bytes[1] = x_LLB;
            command_Bytes[2] = x_LHB;
            command_Bytes[3] = x_HLB;
            command_Bytes[4] = x_HHB;
            command_Bytes[5] = y_LLB;
            command_Bytes[6] = y_LHB;
            command_Bytes[7] = y_HLB;
            command_Bytes[8] = y_HHB;
            command_Bytes[9] = z_LLB;
            command_Bytes[10] = z_LHB;
            command_Bytes[11] = z_HLB;
            command_Bytes[12] = z_HHB;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            command_Is_Ready = true;
            
        }

        private void picBox_Home_All_Axes_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();

            // Send Command Template
            command_Bytes[0] = 0x07;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            command_Is_Ready = true;

        }

        private void picBox_Center_All_Axes_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();

            // Send Command Template
            command_Bytes[0] = 0x08;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            command_Is_Ready = true;

        }

        private void picBox_Set_new_Center_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();

            // Send Command Template
            command_Bytes[0] = 0x09;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            command_Is_Ready = true;

        }


        private void comboBox_First_Axis_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_Second_Axis.Items.Clear();

            switch (comboBox_First_Axis.Text)
            {
                case "X":
                    comboBox_Second_Axis.Items.Add("Y");
                    comboBox_Second_Axis.Items.Add("Z");
                    comboBox_Second_Axis.Text = "Y";
                    break;


                case "Y":
                    comboBox_Second_Axis.Items.Add("X");
                    comboBox_Second_Axis.Items.Add("Z");
                    comboBox_Second_Axis.Text = "X";
                    break;


                case "Z":
                    comboBox_Second_Axis.Items.Add("X");
                    comboBox_Second_Axis.Items.Add("Y");
                    comboBox_Second_Axis.Text = "X";
                    break;

            }

            Update_Third_Axis();

        }

        private void comboBox_Second_Axis_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_Third_Axis();
        }

        private void Update_Third_Axis()
        {
            switch (comboBox_First_Axis.Text)
            {
                case "X":
                    if (comboBox_Second_Axis.Text == "Y")
                    {
                        comboBox_Third_Axis.Text = "Z";
                    }
                    else
                    {
                        comboBox_Third_Axis.Text = "Y";
                    }
                    break;


                case "Y":
                    if (comboBox_Second_Axis.Text == "Z")
                    {
                        comboBox_Third_Axis.Text = "X";
                    }
                    else
                    {
                        comboBox_Third_Axis.Text = "Z";
                    }
                    break;


                case "Z":
                    if (comboBox_Second_Axis.Text == "X")
                    {
                        comboBox_Third_Axis.Text = "Y";
                    }
                    else
                    {
                        comboBox_Third_Axis.Text = "X";
                    }
                    break;

            }

        }

        private void Update_State_Machine()
        {
            switch (auto_Positioning_System_State)
            {
                case Auto_Position_Enum.STEP_WAIT:
                    //if(is_Auto_Step_Running == true)
                    {
                        if (x_Controller_State == Controller_State_Enum.IDLE_STATE && y_Controller_State == Controller_State_Enum.IDLE_STATE && z_Controller_State == Controller_State_Enum.IDLE_STATE)
                        {
                            time_Counter = 0;
                            auto_Positioning_System_State = Auto_Position_Enum.PHOTOGRAPH;
                        }

                    }

                    break;

                case Auto_Position_Enum.PHOTOGRAPH:
                    if (is_Auto_Stepping_Canceled == true)
                    {

                        is_Auto_Stepping_Canceled = false;
                        is_Auto_Step_Running = false;
                        auto_Positioning_System_State = Auto_Position_Enum.CANCELED;
                    }


                    else if (is_Auto_Step_Running == true)
                    {
                        if(Take_Photo != null)
                        {
                            Take_Photo(this,null);
                        }
                        auto_Positioning_System_State = Auto_Position_Enum.WAIT_AFTER_TAKING_PICTURE;

                        
                    }

                    break;

                case Auto_Position_Enum.WAIT_AFTER_TAKING_PICTURE:
                    if (is_Auto_Stepping_Canceled == true)
                    {

                        is_Auto_Stepping_Canceled = false;
                        is_Auto_Step_Running = false;
                        auto_Positioning_System_State = Auto_Position_Enum.CANCELED;
                    }

                    else
                    {
                        time_Counter++;
                        if (time_Counter == 20)
                        {
                            time_Counter = 0;

                            auto_Positioning_System_State = Auto_Position_Enum.STEP;
                        }

                    }

                    break;


                case Auto_Position_Enum.STEP:
                    if (is_Auto_Stepping_Paused == true)
                    {
                        auto_Positioning_System_State = Auto_Position_Enum.PAUSED;
                    }


                    else
                    {
                        if (is_Auto_Stepping_Canceled == true)
                        {

                            is_Auto_Stepping_Canceled = false;
                            is_Auto_Step_Running = false;
                            auto_Positioning_System_State = Auto_Position_Enum.CANCELED;
                        }
                        else
                        {
                            Step();
                            photo_Counter++;
                            auto_Positioning_System_State = Auto_Position_Enum.WAIT;

                        }
                    }

                    break;

                case Auto_Position_Enum.WAIT:
                    if (is_Auto_Stepping_Canceled == true)
                    {

                        is_Auto_Stepping_Canceled = false;
                        is_Auto_Step_Running = false;
                        auto_Positioning_System_State = Auto_Position_Enum.CANCELED;
                    }
                    else
                    {
                        time_Counter++;
                        if (time_Counter == 15)
                        {
                            time_Counter = 0;

                            auto_Positioning_System_State = Auto_Position_Enum.STEP_WAIT;

                        }

                    }

                    break;

                case Auto_Position_Enum.PAUSED:
                    if (is_Auto_Stepping_Canceled == true)
                    {

                        is_Auto_Stepping_Canceled = false;
                        is_Auto_Step_Running = false;
                        auto_Positioning_System_State = Auto_Position_Enum.CANCELED;
                    }

                    else
                    {
                        if (is_Auto_Stepping_Paused == false)
                        {
                            auto_Positioning_System_State = Auto_Position_Enum.STEP;
                        }

                    }
                    break;


                case Auto_Position_Enum.CANCELED:
                    if(is_Auto_Step_Running == true)
                    {
                        auto_Positioning_System_State = Auto_Position_Enum.STEP_WAIT;

                    }
                    break;

            }
        }

        private void Move_Image_To_Project()
        {
            if (!is_Moving_image)
            {
                is_Moving_image = true;
                try
                {
                    string sourceFolder;

                    // Get the APPDATA folder path.
                    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    if (string.IsNullOrEmpty(appDataPath))
                    {
                        throw new Exception("APPDATA path could not be found.");
                    }

                    // Build the full path to the source folder.
                    sourceFolder = Path.Combine(appDataPath, images_Path);
                    if (!Directory.Exists(sourceFolder))
                    {
                        throw new DirectoryNotFoundException($"The source folder '{sourceFolder}' does not exist.");
                    }

                    // Define the valid image file extensions.
                    string[] validExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".tiff" };

                    // Get all files from the source folder that match the valid image extensions.
                    var imageFiles = Directory.GetFiles(sourceFolder)
                        .Where(file => validExtensions.Contains(Path.GetExtension(file).ToLower()))
                        .ToList();

                    if (!imageFiles.Any())
                    {
                        Console.WriteLine("No image !");
                        throw new Exception();
                    }

                    // Select the image file with the most recent modification time.
                    string latestImage = imageFiles
                        .OrderByDescending(file => File.GetLastWriteTime(file))
                        .First();

                    if (project_Path == null)
                    {
                        // If no project has been created , the images will move to the desktop.
                        project_Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }

                    // Build the destination folder path (e.g., project_Path\Images).
                    string imagesDestination = Path.Combine(project_Path, "Images");
                    if (!Directory.Exists(imagesDestination))
                    {
                        Directory.CreateDirectory(imagesDestination);
                    }

                    // Use a Regex to find files that follow the pattern "IMG_###" (e.g., IMG_001, IMG_002)
                    string pattern = @"IMG_(\d+)";
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    int maxNumber = 0;

                    // Scan the destination folder for existing images following the pattern.
                    foreach (var file in Directory.GetFiles(imagesDestination))
                    {
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                        Match match = regex.Match(fileNameWithoutExt);
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
                        {
                            if (number > maxNumber)
                            {
                                maxNumber = number;
                            }
                        }
                    }

                    // The new file number is one more than the highest found.
                    int newNumber = maxNumber + 1;
                    // Format the new name as "IMG_XXX" where XXX is padded to three digits.
                    string newFileName = $"IMG_{newNumber.ToString("D4")}{Path.GetExtension(latestImage)}";
                    string destinationPath = Path.Combine(imagesDestination, newFileName);

                    image_File_Name = newFileName;

                    // Load the image into memory without locking the file.
                    // This allows us to move the file afterward.
                    using (var ms = new MemoryStream(File.ReadAllBytes(latestImage)))
                    {
                        captured_Image = new Bitmap(ms);
                    }

                    // Move the image file to the destination folder with the new name.
                    File.Move(latestImage, destinationPath);

                    // Optionally, if you have an event or a method to notify that a new image was captured,
                    // you can call it here.
                    Fire_New_Image_Captured();

                    Console.WriteLine($"Image moved and renamed to '{destinationPath}'.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error While moving the image! , Error: {ex.Message}");
                }

                is_Moving_image = false;
            }

        }

        private void button_Info_TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (textBox_Info.Visible == true)
                {
                    textBox_Info.Visible = false;
                    textBox_Info.Size = new Size(10, 10);
                    textBox_Info.Location = new Point(166, 86);
                    textBox_Info.SendToBack();
                }
                else
                {
                    textBox_Info.Visible = true;
                    textBox_Info.Size = new Size(150, 90);
                    textBox_Info.Location = new Point(10, 86);
                    textBox_Info.BringToFront();
                }


                if (textBox_Rx_Data.Visible == true)
                {
                    textBox_Rx_Data.Visible = false;
                    textBox_Rx_Data.Size = new Size(10, 10);
                    textBox_Rx_Data.Location = new Point(183, 86);
                    textBox_Rx_Data.SendToBack();
                }
                else
                {
                    textBox_Rx_Data.Visible = true;
                    textBox_Rx_Data.Size = new Size(150, 40);
                    textBox_Rx_Data.Location = new Point(10, 140);
                    textBox_Rx_Data.BringToFront();
                }
            }
        }

        private void Find_Axis_Priority()
        {
            if (comboBox_First_Axis.Text == "X" && comboBox_Second_Axis.Text == "Y" && comboBox_Third_Axis.Text == "Z")
            {
                x_Auto_Step_Count = (int)numericUpDown_First_Step_Count.Value;
                y_Auto_Step_Count = (int)numericUpDown_Second_Step_Count.Value;
                z_Auto_Step_Count = (int)numericUpDown_Third_Step_Count.Value;

                x_Auto_Step_mm = (int)numericUpDown_First_Step.Value;
                y_Auto_Step_mm = (int)numericUpDown_Second_Step.Value;
                z_Auto_Step_mm = (int)numericUpDown_Third_Step.Value;

                x_Auto_Step_Counter++;
                if (x_Auto_Step_Counter >= x_Auto_Step_Count)
                {
                    x_Auto_Step_Counter = 0;

                    y_Auto_Step_Counter++;
                    if (y_Auto_Step_Counter >= y_Auto_Step_Count)
                    {
                        y_Auto_Step_Counter = 0;

                        z_Auto_Step_Counter++;
                        if (z_Auto_Step_Counter >= z_Auto_Step_Count)
                        {
                            z_Auto_Step_Counter = 0;
                        }
                    }
                }
            }

            if (comboBox_First_Axis.Text == "Y" && comboBox_Second_Axis.Text == "X" && comboBox_Third_Axis.Text == "Z")
            {
                y_Auto_Step_Count = (int)numericUpDown_First_Step_Count.Value;
                x_Auto_Step_Count = (int)numericUpDown_Second_Step_Count.Value;
                z_Auto_Step_Count = (int)numericUpDown_Third_Step_Count.Value;

                y_Auto_Step_mm = (int)numericUpDown_First_Step.Value;
                x_Auto_Step_mm = (int)numericUpDown_Second_Step.Value;
                z_Auto_Step_mm = (int)numericUpDown_Third_Step.Value;


                y_Auto_Step_Counter++;
                if (y_Auto_Step_Counter >= y_Auto_Step_Count)
                {
                    y_Auto_Step_Counter = 0;

                    x_Auto_Step_Counter++;
                    if (x_Auto_Step_Counter >= x_Auto_Step_Count)
                    {
                        x_Auto_Step_Counter = 0;

                        z_Auto_Step_Counter++;
                        if (z_Auto_Step_Counter >= z_Auto_Step_Count)
                        {
                            z_Auto_Step_Counter = 0;
                        }
                    }
                }
            }

            if (comboBox_First_Axis.Text == "X" && comboBox_Second_Axis.Text == "Z" && comboBox_Third_Axis.Text == "Y")
            {
                x_Auto_Step_Count = (int)numericUpDown_First_Step_Count.Value;
                z_Auto_Step_Count = (int)numericUpDown_Second_Step_Count.Value;
                y_Auto_Step_Count = (int)numericUpDown_Third_Step_Count.Value;

                x_Auto_Step_mm = (int)numericUpDown_First_Step.Value;
                z_Auto_Step_mm = (int)numericUpDown_Second_Step.Value;
                y_Auto_Step_mm = (int)numericUpDown_Third_Step.Value;


                x_Auto_Step_Counter++;
                if (x_Auto_Step_Counter >= x_Auto_Step_Count)
                {
                    x_Auto_Step_Counter = 0;

                    z_Auto_Step_Counter++;
                    if (z_Auto_Step_Counter >= z_Auto_Step_Count)
                    {
                        z_Auto_Step_Counter = 0;

                        y_Auto_Step_Counter++;
                        if (y_Auto_Step_Counter >= y_Auto_Step_Count)
                        {
                            y_Auto_Step_Counter = 0;
                        }
                    }
                }
            }


            if (comboBox_First_Axis.Text == "Y" && comboBox_Second_Axis.Text == "Z" && comboBox_Third_Axis.Text == "X")
            {
                y_Auto_Step_Count = (int)numericUpDown_First_Step_Count.Value;
                z_Auto_Step_Count = (int)numericUpDown_Second_Step_Count.Value;
                x_Auto_Step_Count = (int)numericUpDown_Third_Step_Count.Value;

                y_Auto_Step_mm = (int)numericUpDown_First_Step.Value;
                z_Auto_Step_mm = (int)numericUpDown_Second_Step.Value;
                x_Auto_Step_mm = (int)numericUpDown_Third_Step.Value;


                y_Auto_Step_Counter++;
                if (y_Auto_Step_Counter >= y_Auto_Step_Count)
                {
                    y_Auto_Step_Counter = 0;

                    z_Auto_Step_Counter++;
                    if (z_Auto_Step_Counter >= z_Auto_Step_Count)
                    {
                        z_Auto_Step_Counter = 0;

                        x_Auto_Step_Counter++;
                        if (x_Auto_Step_Counter >= x_Auto_Step_Count)
                        {
                            x_Auto_Step_Counter = 0;
                        }
                    }
                }
            }

            if (comboBox_First_Axis.Text == "Z" && comboBox_Second_Axis.Text == "Y" && comboBox_Third_Axis.Text == "X")
            {
                z_Auto_Step_Count = (int)numericUpDown_First_Step_Count.Value;
                y_Auto_Step_Count = (int)numericUpDown_Second_Step_Count.Value;
                x_Auto_Step_Count = (int)numericUpDown_Third_Step_Count.Value;

                x_Auto_Step_mm = (int)numericUpDown_First_Step.Value;
                y_Auto_Step_mm = (int)numericUpDown_Second_Step.Value;
                x_Auto_Step_mm = (int)numericUpDown_Third_Step.Value;


                z_Auto_Step_Counter++;
                if (z_Auto_Step_Counter >= z_Auto_Step_Count)
                {
                    z_Auto_Step_Counter = 0;

                    y_Auto_Step_Counter++;
                    if (y_Auto_Step_Counter >= y_Auto_Step_Count)
                    {
                        y_Auto_Step_Counter = 0;

                        x_Auto_Step_Counter++;
                        if (x_Auto_Step_Counter >= x_Auto_Step_Count)
                        {
                            x_Auto_Step_Counter = 0;
                        }
                    }
                }
            }

            if (comboBox_First_Axis.Text == "Z" && comboBox_Second_Axis.Text == "X" && comboBox_Third_Axis.Text == "Y")
            {

                z_Auto_Step_Count = (int)numericUpDown_First_Step_Count.Value;
                x_Auto_Step_Count = (int)numericUpDown_Second_Step_Count.Value;
                y_Auto_Step_Count = (int)numericUpDown_Third_Step_Count.Value;

                z_Auto_Step_mm = (int)numericUpDown_First_Step.Value;
                x_Auto_Step_mm = (int)numericUpDown_Second_Step.Value;
                y_Auto_Step_mm = (int)numericUpDown_Third_Step.Value;

                z_Auto_Step_Counter++;
                if (z_Auto_Step_Counter >= z_Auto_Step_Count)
                {
                    z_Auto_Step_Counter = 0;

                    x_Auto_Step_Counter++;
                    if (x_Auto_Step_Counter >= x_Auto_Step_Count)
                    {
                        x_Auto_Step_Counter = 0;

                        y_Auto_Step_Counter++;
                        if (y_Auto_Step_Counter >= y_Auto_Step_Count)
                        {
                            y_Auto_Step_Counter = 0;
                        }
                    }
                }
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage page = tabControl1.TabPages[e.Index];
            e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);

            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
            TextRenderer.DrawText(e.Graphics, page.Text, e.Font, paddedBounds, page.ForeColor);
        }

        private void pictureBox_Auto_Stepping_Click(object sender, EventArgs e)
        {
            is_Auto_Step_Running = !is_Auto_Step_Running;

            if (is_Auto_Step_Running)
            {
                if(is_Auto_Stepping_Paused)
                {
                    is_Auto_Stepping_Paused = false;
                    pictureBox_Auto_Stepping.Image = Properties.Resources.Stepping_Pause_Highlighted;
                    

                }
                else
                {
                    pictureBox_Auto_Stepping.Image = Properties.Resources.Stepping_Pause_Highlighted;
                    x_Start_Position = X.Current_Position;
                    y_Start_Position = Y.Current_Position;
                    z_Start_Position = Z.Current_Position;

                }
                
            }
            else
            {
                is_Auto_Stepping_Paused = true;
                pictureBox_Auto_Stepping.Image = Properties.Resources.Stepping_Run_Highlighted;
            }




            button_Cancel.Visible = true;
            photo_Count = ((int)numericUpDown_First_Step_Count.Value) * ((int)numericUpDown_Second_Step_Count.Value) * ((int)numericUpDown_Third_Step_Count.Value);
        }

        private void General_Mouse_Enter(object sender, EventArgs e)
        {
            PictureBox picbox = (PictureBox) sender;
            switch (picbox.Name)
            {
                case "pictureBox_Emergency_Stop":
                    pictureBox_Emergency_Stop.Image = Properties.Resources.Emergency_Stop_Highlighted;
                    break;
                case "pictureBox_Go_Home_All":
                    pictureBox_Go_Home_All.Image = Properties.Resources.Home_Highlighted;
                    break;                
                case "pictureBox_Center_All":
                    pictureBox_Center_All.Image = Properties.Resources.Center_Highlighted;
                    break;
                case "pictureBox_Set_New_Center":
                    pictureBox_Set_New_Center.Image = Properties.Resources.New_Center_Highlighted;
                    break;
                case "pictureBox_GoTo":
                    pictureBox_GoTo.Image = Properties.Resources.GoTo_Highlighted;
                    break;
                case "pictureBox_Help":
                    pictureBox_Help.Image = Properties.Resources.Help_Highlighted;
                    break;
                case "pictureBox_Info":
                    pictureBox_Info.Image = Properties.Resources.Info_Highlighted;
                    break;
                case "pictureBox_Auto_Stepping":
                    if (is_Auto_Step_Running)
                    {
                        pictureBox_Auto_Stepping.Image = Properties.Resources.Stepping_Pause_Highlighted;
                    }
                    else
                    {
                        pictureBox_Auto_Stepping.Image = Properties.Resources.Stepping_Run_Highlighted;
                    }
                    break;
            }
        }

        private void General_Mouse_Leave(object sender, EventArgs e)
        {
            PictureBox picbox = (PictureBox)sender;
            switch (picbox.Name)
            {
                case "pictureBox_Emergency_Stop":
                    pictureBox_Emergency_Stop.Image = Properties.Resources.Emergency_Stop;
                    break;
                case "pictureBox_Go_Home_All":
                    pictureBox_Go_Home_All.Image = Properties.Resources.Home;
                    break;
                case "pictureBox_Center_All":
                    pictureBox_Center_All.Image = Properties.Resources.Center;
                    break;
                case "pictureBox_Set_New_Center":
                    pictureBox_Set_New_Center.Image = Properties.Resources.New_Center;
                    break;
                case "pictureBox_GoTo":
                    pictureBox_GoTo.Image = Properties.Resources.GoTo;
                    break;
                case "pictureBox_Help":
                    pictureBox_Help.Image = Properties.Resources.Help;
                    break;
                case "pictureBox_Info":
                    pictureBox_Info.Image = Properties.Resources.Info;
                    break;
                case "pictureBox_Auto_Stepping":
                    if (is_Auto_Step_Running)
                    {
                        pictureBox_Auto_Stepping.Image = Properties.Resources.Stepping_Pause;
                    }
                    else
                    {
                        pictureBox_Auto_Stepping.Image = Properties.Resources.Stepping_Run;
                    }
                    break;
            }
        }

        private void pictureBox_Help_Click(object sender, EventArgs e)
        {
            if (Show_Help_Image != null)
            {
                Show_Help_Image(this, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //is_Auto_Stepping_Canceled = true;
            //button_Cancel.Visible = false;
        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            timer_Test.Start();
        }

        private void timer_Test_Tick(object sender, EventArgs e)
        {
            X.Current_Position += 0.1;
            Y.Current_Position += 0.1;
            Z.Current_Position += 0.1;
            Update_XYZ_Property();
        }
    }
}

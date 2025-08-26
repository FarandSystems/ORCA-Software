using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNC_Axis_Component
{
    public partial class Axis_Component: UserControl
    {
        
       
        int com_Counter = 0;
        public event EventHandler Command_Event;

        string axis_Unit;
        public string Axis_Unit
        {
            get { return axis_Unit; }
            set { axis_Unit = value; }
        }

        string axis_Name;
        public string Axis_Name 
        {
            get
            {
                return axis_Name;
            }
            set
            {
                axis_Name = value;
                label_Current_Position.Text = axis_Name + axis_Unit + ": " + current_Position;
                label_Axis.Text = axis_Name + axis_Unit;
                label1.Text = axis_Name + axis_Unit;
                //button_Step.Text = "Step " + axis_Name;
                groupBox1.Text = "Control " + axis_Name;
            }
        }

        byte command_Address = 0x01;
        public byte Command_Address
        {
            get
            {
                return command_Address;
            }
            set
            {
                command_Address = value;
            }
        }

        byte[] command_Bytes = new byte[32];
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

        double current_Position;
        public double Current_Position
        {
            get
            {
                return current_Position;
            }
            set
            {
                current_Position = value;
                label_Current_Position.Text = axis_Name + axis_Unit + ": " + current_Position.ToString("0.00");
            }
        }

        double mm_Per_Rotation = 5;
        public double Mm_Per_Rotation
        {
            get
            {
                return mm_Per_Rotation;
            }
            set
            {
                mm_Per_Rotation = value;
            }
        }

        double stepper_Motor_Resolution = 400;
        public double Stepper_Motor_Resolution
        {
            get
            {
                return stepper_Motor_Resolution;
            }
            set
            {
                stepper_Motor_Resolution = value;
            }
        }

        double axis_Course = 100;
        public double Axis_Course
        {
            get
            {
                return axis_Course;
            }
            set
            {
                axis_Course = value;
                label_Course.Text = "(" + (-axis_Course).ToString() + "..." + (axis_Course).ToString() + ")";


            }
        }


        string errors;
        public string Errors
        {
            get
            {
                return errors;
            }
            set
            {
                errors = value;

                if (value != "")
                {
                    pictureBox_Error.Visible = true;
                    toolTip1.SetToolTip(pictureBox_Error, value);
                }
                else
                {
                    pictureBox_Error.Visible = false;
                }
                //label_Errors.ForeColor = Color.Tomato;
                //label_Errors.Text = errors;
                
            }
        }

        bool command_Is_Ready = false;
        public bool Command_Is_Ready
        {
            get
            {
                return command_Is_Ready;
            }
            set
            {
                command_Is_Ready = value;
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

                //    groupBox_Go_To.Visible = true;
                //    groupBox_Step.Visible = true;
                //    this.Size = new Size(990, 66);
                //    groupBox1.Size = new Size(985, 66);
                //   // button_More_Less.Text = "less...";
                //}
                //else
                //{
                  

                //    groupBox_Go_To.Visible = false;
                //    groupBox_Step.Visible= false;
                //    this.Size = new Size(430, 66);
                //    groupBox1.Size = new Size(425, 66);
                //  //  button_More_Less.Text = "More...";
                //}


            }
        }


        int step = 0;
        public int Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }

        private void General_Mouse_Enter(object sender, EventArgs e)
        {
            PictureBox picbox = (PictureBox)sender;

            switch (picbox.Name)
            {
                case "pictureBox_Center":
                    picbox.Image = Properties.Resources.Center_Highlighted;
                    break;

                case "pictureBox_Home":
                    picbox.Image = Properties.Resources.Home_Highlighted;
                    break;

                case "pictureBox_Nodge_Forward":
                    picbox.Image = Properties.Resources.Nodge_Forward_Highlighted;
                    break;

                case "pictureBox_Nodge_Backward":
                    picbox.Image = Properties.Resources.Nodge_Backward_Highlighted;
                    break;

                case "pictureBox_Step_Forward":
                    picbox.Image = Properties.Resources.Manual_Step_Up_Highlighted;
                    break;

                case "pictureBox_Step_Backward":
                    picbox.Image = Properties.Resources.Manual_Step_Down_Highlighted;
                    break;

                case "pictureBox_GoTo":
                    picbox.Image = Properties.Resources.GoTo_Highlighted;
                    break;
                
                case "pictureBox_Error":
                    picbox.Image = Properties.Resources.Error_Highlighted;
                    break;
                
                default:
                    break;
            }
        }

        private void General_Mouse_Leave(object sender, EventArgs e)
        {
            PictureBox picbox = (PictureBox)sender;

            switch (picbox.Name)
            {
                case "pictureBox_Center":
                    picbox.Image = Properties.Resources.Center;
                    break;
                case "pictureBox_Home":
                    picbox.Image = Properties.Resources.Home;
                    break;


                case "pictureBox_Nodge_Forward":
                    picbox.Image = Properties.Resources.Nodge_Forward;
                    break;

                case "pictureBox_Nodge_Backward":
                    picbox.Image = Properties.Resources.Nodge_Backward;
                    break;

                case "pictureBox_Step_Forward":
                    picbox.Image = Properties.Resources.Manual_Step_Up;
                    break;

                case "pictureBox_Step_Backward":
                    picbox.Image = Properties.Resources.Manual_Step_Down;
                    break;

                case "pictureBox_GoTo":
                    picbox.Image = Properties.Resources.GoTo;
                    break;

                case "pictureBox_Error":
                    picbox.Image = Properties.Resources.Error;
                    break;

                default:
                    break;
            }
        }



        public Axis_Component()
        {
            InitializeComponent();
        }

        private void Axis_Component_Load(object sender, EventArgs e)
        {

        }

        private void picBox_Go_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();

            // Send Command Template
            int x_int = (int)(100 * numericUpDown_Go_To.Value);
            byte x_LLB = (byte)((x_int & 0x000000FF) >> 0);
            byte x_LHB = (byte)((x_int & 0x0000FF00) >> 8);
            byte x_HLB = (byte)((x_int & 0x00FF0000) >> 16);
            byte x_HHB = (byte)((x_int & 0xFF000000) >> 24);

            command_Bytes[0] = (byte)(command_Address + 3);
            command_Bytes[1] = x_LLB;
            command_Bytes[2] = x_LHB;
            command_Bytes[3] = x_HLB;
            command_Bytes[4] = x_HHB;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);
            command_Is_Ready = true;

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            //com_Counter++;
            //if (command_Is_Ready == true)
            //{
            //    if(Command_Event != null)
            //    {
            //        Command_Event(this, null);
            //    }
            //    command_Is_Ready = false;
            //}
            //else
            //{
            //    // Send No Operation Command
            //    command_Bytes[0] = 0x01;
            //    command_Bytes[29] = 0xAA;
            //    command_Bytes[30] = 0xAA;
            //    command_Bytes[31] = Calculate_CheckSum(command_Bytes);


            //}
            //vcp.Send_Data(command_Bytes);
        }


        private void picBox_Go_Home_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();

            // Send Command Template
            command_Bytes[0] = (byte)(command_Address + 1);
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            command_Is_Ready = true;

        }


        private void picBox_Go_To_Center_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();

            // Send Command Template
            command_Bytes[0] = (byte)(command_Address + 2);
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);


            command_Is_Ready = true;

        }


        private void button_Step_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();

            // Send Command Template
            int step_x_int = (int)(100 * numericUpDown_Step.Value);
            byte step_x_LLB = (byte)((step_x_int & 0x000000FF) >> 0);
            byte step_x_LHB = (byte)((step_x_int & 0x0000FF00) >> 8);
            byte step_x_HLB = (byte)((step_x_int & 0x00FF0000) >> 16);
            byte step_x_HHB = (byte)((step_x_int & 0xFF000000) >> 24);

            command_Bytes[0] = (byte)(command_Address + 4);
            command_Bytes[1] = step_x_LLB;
            command_Bytes[2] = step_x_LHB;
            command_Bytes[3] = step_x_HLB;
            command_Bytes[4] = step_x_HHB;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);
            command_Is_Ready = true;

        }

        private void picBox_Cleear_Errors_Click(object sender, EventArgs e)
        {
            Clear_All_Buffers();

            // Send Command Template
            command_Bytes[0] = (byte)(command_Address + 5);
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);

            command_Is_Ready = true;

        }

        private void picBox_Nodge_Click(object sender, EventArgs e)
        {
            double micro_Step = 0;
            PictureBox picbox = (PictureBox)sender;
            switch (picbox.Name)
            {
                case "button_One_Step_Up":
                    micro_Step = mm_Per_Rotation / stepper_Motor_Resolution;
                    break;

                case "button_One_Step_Down":
                    micro_Step = -mm_Per_Rotation / stepper_Motor_Resolution;
                    break;
            }

            Clear_All_Buffers();

            // Send Command Template
            Int32 micro_Step_int = (Int32)(100 * micro_Step);
            byte micro_Step_x_LLB = (byte)((micro_Step_int & 0x000000FF) >> 0);
            byte micro_Step_x_LHB = (byte)((micro_Step_int & 0x0000FF00) >> 8);
            byte micro_Step_x_HLB = (byte)((micro_Step_int & 0x00FF0000) >> 16);
            byte micro_Step_x_HHB = (byte)((micro_Step_int & 0xFF000000) >> 24);

            command_Bytes[0] = (byte)(command_Address + 4);
            command_Bytes[1] = micro_Step_x_LLB;
            command_Bytes[2] = micro_Step_x_LHB;
            command_Bytes[3] = micro_Step_x_HLB;
            command_Bytes[4] = micro_Step_x_HHB;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);
            command_Is_Ready = true;
        }


        private void Clear_All_Buffers()
        {
            for (int i = 0; i < command_Bytes.Length; i++)
            {
                command_Bytes[i] = 0x00;
            }
        }

        public void Step_Manually(double step)
        {
            Clear_All_Buffers();

            // Send Command Template
            Int32 Step_int = (Int32)(100 * step);
            byte Step_x_LLB = (byte)((Step_int & 0x000000FF) >> 0);
            byte Step_x_LHB = (byte)((Step_int & 0x0000FF00) >> 8);
            byte Step_x_HLB = (byte)((Step_int & 0x00FF0000) >> 16);
            byte Step_x_HHB = (byte)((Step_int & 0xFF000000) >> 24);

            command_Bytes[0] = (byte)(command_Address + 4);
            command_Bytes[1] = Step_x_LLB;
            command_Bytes[2] = Step_x_LHB;
            command_Bytes[3] = Step_x_HLB;
            command_Bytes[4] = Step_x_HHB;
            command_Bytes[29] = 0xAA;
            command_Bytes[30] = 0xAA;
            command_Bytes[31] = Calculate_CheckSum(command_Bytes);
            command_Is_Ready = true;

        }

        private void picBox_Manual_Step_Click(object sender, EventArgs e)
        {
            double Step = 0;
            PictureBox pic = (PictureBox)sender;
            switch (pic.Name)
            {
                case "pictureBox_Step_Forward":
                    Step = (double)numericUpDown_Step.Value;
                    break;

                case "pictureBox_Step_Backward":
                    Step = -(double)numericUpDown_Step.Value;
                    break;
            }

            Step_Manually(Step);
        }

        private void numericUpDown_Step_ValueChanged(object sender, EventArgs e)
        {
            step = (int)numericUpDown_Step.Value;
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
        // Set the font size for the current control without resizing the component
        control.Font = new Font(control.Font.FontFamily, newSize, control.Font.Style);

        // Recursively apply to all child controls
        foreach (Control childControl in control.Controls)
        {
            // If AutoSize is enabled, temporarily disable it to prevent resizing
            bool originalAutoSize = childControl.AutoSize;
            childControl.AutoSize = false;

            ChangeFontSizeOnly(childControl, newSize);

            // Restore original AutoSize setting
            childControl.AutoSize = originalAutoSize;
        }
    }

        private void button_Rx_TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left && (Control.ModifierKeys & Keys.Control) == Keys.Control)
            //{
            //    if (groupBox_Go_To.Visible == true)
            //    {
            //        groupBox_Go_To.Visible = false;
            //        this.Size = new Size(600, 66);
            //        button_More_Less.Text = "More...";
                    
            //    }
            //    else
            //    {
            //        groupBox_Go_To.Visible = true;
            //        this.Size = new Size(990, 66);
            //        button_More_Less.Text = "Less...";
            //    }
            //}

            //if (e.Button == MouseButtons.Left && (Control.ModifierKeys & Keys.Control) == Keys.Control)
            //{
            //    if (groupBox_Step.Visible == true)
            //    {
            //        groupBox_Step.Visible = false;
            //    }
            //    else
            //    {
            //        groupBox_Step.Visible = true;
            //    }
            //}


        }

        private void button_Rx_TextBox_Move(object sender, EventArgs e)
        {

        }

        private void groupBox_Step_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox_Go_To_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

namespace ORCA_Simulator_Application
{
    partial class Main_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox_Connection = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.pictureBox_Log = new System.Windows.Forms.PictureBox();
            this.pictureBox_Reconnect = new System.Windows.Forms.PictureBox();
            this.pictureBox_ORCA_Connection = new System.Windows.Forms.PictureBox();
            this.pictureBox_Controller_Connection = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ORCA_IP = new System.Windows.Forms.TextBox();
            this.textBox_Controller_IP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer_Connection_Demo = new System.Windows.Forms.Timer(this.components);
            this.panel_Visualizer = new System.Windows.Forms.Panel();
            this.panel_Command_Control = new System.Windows.Forms.Panel();
            this.command_Control = new Command_Control.Command_Control();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label_Roll_Position = new System.Windows.Forms.Label();
            this.label_Y_Position = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.toggleButton3 = new ORCA_Simulator_Application.ToggleButton();
            this.toggleButton2 = new ORCA_Simulator_Application.ToggleButton();
            this.toggleButton1 = new ORCA_Simulator_Application.ToggleButton();
            this.toggleButton_IMU_Status = new ORCA_Simulator_Application.ToggleButton();
            this.visualizer_Component = new Visualizer_Component.Visualizer_Component();
            this.groupBox_Connection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Log)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Reconnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ORCA_Connection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Controller_Connection)).BeginInit();
            this.panel_Visualizer.SuspendLayout();
            this.panel_Command_Control.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Connection
            // 
            this.groupBox_Connection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox_Connection.Controls.Add(this.toggleButton3);
            this.groupBox_Connection.Controls.Add(this.label16);
            this.groupBox_Connection.Controls.Add(this.toggleButton2);
            this.groupBox_Connection.Controls.Add(this.label15);
            this.groupBox_Connection.Controls.Add(this.toggleButton1);
            this.groupBox_Connection.Controls.Add(this.label14);
            this.groupBox_Connection.Controls.Add(this.toggleButton_IMU_Status);
            this.groupBox_Connection.Controls.Add(this.pictureBox_Log);
            this.groupBox_Connection.Controls.Add(this.pictureBox_Reconnect);
            this.groupBox_Connection.Controls.Add(this.pictureBox_ORCA_Connection);
            this.groupBox_Connection.Controls.Add(this.pictureBox_Controller_Connection);
            this.groupBox_Connection.Controls.Add(this.label3);
            this.groupBox_Connection.Controls.Add(this.textBox_ORCA_IP);
            this.groupBox_Connection.Controls.Add(this.textBox_Controller_IP);
            this.groupBox_Connection.Controls.Add(this.label2);
            this.groupBox_Connection.Controls.Add(this.label1);
            this.groupBox_Connection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.groupBox_Connection.Location = new System.Drawing.Point(12, 2);
            this.groupBox_Connection.Name = "groupBox_Connection";
            this.groupBox_Connection.Size = new System.Drawing.Size(166, 73);
            this.groupBox_Connection.TabIndex = 0;
            this.groupBox_Connection.TabStop = false;
            this.groupBox_Connection.Text = "Connection";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 363);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(37, 13);
            this.label16.TabIndex = 38;
            this.label16.Text = "GNSS";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 318);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 13);
            this.label15.TabIndex = 36;
            this.label15.Text = "Iridium";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 273);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "GSM";
            // 
            // pictureBox_Log
            // 
            this.pictureBox_Log.Image = global::ORCA_Simulator_Application.Properties.Resources.Log;
            this.pictureBox_Log.Location = new System.Drawing.Point(99, 19);
            this.pictureBox_Log.Name = "pictureBox_Log";
            this.pictureBox_Log.Size = new System.Drawing.Size(35, 35);
            this.pictureBox_Log.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Log.TabIndex = 32;
            this.pictureBox_Log.TabStop = false;
            // 
            // pictureBox_Reconnect
            // 
            this.pictureBox_Reconnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Reconnect.Image = global::ORCA_Simulator_Application.Properties.Resources.Reconnect;
            this.pictureBox_Reconnect.Location = new System.Drawing.Point(9, 19);
            this.pictureBox_Reconnect.Name = "pictureBox_Reconnect";
            this.pictureBox_Reconnect.Size = new System.Drawing.Size(35, 35);
            this.pictureBox_Reconnect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Reconnect.TabIndex = 31;
            this.pictureBox_Reconnect.TabStop = false;
            this.pictureBox_Reconnect.Click += new System.EventHandler(this.pictureBox_Reconnect_Click);
            // 
            // pictureBox_ORCA_Connection
            // 
            this.pictureBox_ORCA_Connection.Image = global::ORCA_Simulator_Application.Properties.Resources.Connection_Idle;
            this.pictureBox_ORCA_Connection.Location = new System.Drawing.Point(114, 136);
            this.pictureBox_ORCA_Connection.Name = "pictureBox_ORCA_Connection";
            this.pictureBox_ORCA_Connection.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_ORCA_Connection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_ORCA_Connection.TabIndex = 30;
            this.pictureBox_ORCA_Connection.TabStop = false;
            // 
            // pictureBox_Controller_Connection
            // 
            this.pictureBox_Controller_Connection.Image = global::ORCA_Simulator_Application.Properties.Resources.Connection_Idle;
            this.pictureBox_Controller_Connection.Location = new System.Drawing.Point(114, 74);
            this.pictureBox_Controller_Connection.Name = "pictureBox_Controller_Connection";
            this.pictureBox_Controller_Connection.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_Controller_Connection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Controller_Connection.TabIndex = 29;
            this.pictureBox_Controller_Connection.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "IMU";
            // 
            // textBox_ORCA_IP
            // 
            this.textBox_ORCA_IP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.textBox_ORCA_IP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.textBox_ORCA_IP.Location = new System.Drawing.Point(9, 167);
            this.textBox_ORCA_IP.Name = "textBox_ORCA_IP";
            this.textBox_ORCA_IP.Size = new System.Drawing.Size(100, 20);
            this.textBox_ORCA_IP.TabIndex = 3;
            this.textBox_ORCA_IP.Text = "192.168.0.4";
            // 
            // textBox_Controller_IP
            // 
            this.textBox_Controller_IP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.textBox_Controller_IP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.textBox_Controller_IP.Location = new System.Drawing.Point(9, 101);
            this.textBox_Controller_IP.Name = "textBox_Controller_IP";
            this.textBox_Controller_IP.Size = new System.Drawing.Size(100, 20);
            this.textBox_Controller_IP.TabIndex = 2;
            this.textBox_Controller_IP.Text = "192.168.0.3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ORCA IP Address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Controller IP Address";
            // 
            // timer_Connection_Demo
            // 
            this.timer_Connection_Demo.Interval = 125;
            this.timer_Connection_Demo.Tick += new System.EventHandler(this.timer_Connection_Demo_Tick);
            // 
            // panel_Visualizer
            // 
            this.panel_Visualizer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Visualizer.Controls.Add(this.visualizer_Component);
            this.panel_Visualizer.Location = new System.Drawing.Point(12, 81);
            this.panel_Visualizer.Name = "panel_Visualizer";
            this.panel_Visualizer.Size = new System.Drawing.Size(1012, 690);
            this.panel_Visualizer.TabIndex = 1;
            // 
            // panel_Command_Control
            // 
            this.panel_Command_Control.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Command_Control.Controls.Add(this.command_Control);
            this.panel_Command_Control.Location = new System.Drawing.Point(1030, 2);
            this.panel_Command_Control.Name = "panel_Command_Control";
            this.panel_Command_Control.Size = new System.Drawing.Size(330, 744);
            this.panel_Command_Control.TabIndex = 2;
            // 
            // command_Control
            // 
            this.command_Control.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.command_Control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.command_Control.Location = new System.Drawing.Point(0, 0);
            this.command_Control.Name = "command_Control";
            this.command_Control.Size = new System.Drawing.Size(330, 744);
            this.command_Control.TabIndex = 0;
            this.command_Control.Load += new System.EventHandler(this.command_Control_Load);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label_Roll_Position);
            this.groupBox1.Controls.Add(this.label_Y_Position);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.groupBox1.Location = new System.Drawing.Point(193, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 73);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controller Position";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(166, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 40;
            this.label7.Text = "10 deg.";
            // 
            // label_Roll_Position
            // 
            this.label_Roll_Position.AutoSize = true;
            this.label_Roll_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Roll_Position.Location = new System.Drawing.Point(241, 18);
            this.label_Roll_Position.Name = "label_Roll_Position";
            this.label_Roll_Position.Size = new System.Drawing.Size(55, 15);
            this.label_Roll_Position.TabIndex = 39;
            this.label_Roll_Position.Text = "10 deg.";
            // 
            // label_Y_Position
            // 
            this.label_Y_Position.AutoSize = true;
            this.label_Y_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Y_Position.Location = new System.Drawing.Point(86, 16);
            this.label_Y_Position.Name = "label_Y_Position";
            this.label_Y_Position.Size = new System.Drawing.Size(51, 15);
            this.label_Y_Position.TabIndex = 38;
            this.label_Y_Position.Text = "10 mm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(91, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 15);
            this.label6.TabIndex = 37;
            this.label6.Text = "Pitch:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(166, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 36;
            this.label5.Text = "Roll:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 15);
            this.label4.TabIndex = 35;
            this.label4.Text = "Heave:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.groupBox2.Location = new System.Drawing.Point(549, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(475, 73);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logger Position";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(219, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 15);
            this.label8.TabIndex = 40;
            this.label8.Text = "10 deg.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(312, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 15);
            this.label9.TabIndex = 39;
            this.label9.Text = "10 deg.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(86, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 15);
            this.label10.TabIndex = 38;
            this.label10.Text = "10 mm";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(144, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 15);
            this.label11.TabIndex = 37;
            this.label11.Text = "Pitch:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(237, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 15);
            this.label12.TabIndex = 36;
            this.label12.Text = "Roll:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(7, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 15);
            this.label13.TabIndex = 35;
            this.label13.Text = "Heave:";
            // 
            // toggleButton3
            // 
            this.toggleButton3.AutoSize = true;
            this.toggleButton3.Location = new System.Drawing.Point(89, 358);
            this.toggleButton3.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggleButton3.Name = "toggleButton3";
            this.toggleButton3.OffBackColor = System.Drawing.Color.Gray;
            this.toggleButton3.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.toggleButton3.OnBackColor = System.Drawing.Color.LimeGreen;
            this.toggleButton3.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.toggleButton3.Size = new System.Drawing.Size(45, 22);
            this.toggleButton3.TabIndex = 39;
            this.toggleButton3.UseVisualStyleBackColor = true;
            // 
            // toggleButton2
            // 
            this.toggleButton2.AutoSize = true;
            this.toggleButton2.Location = new System.Drawing.Point(89, 313);
            this.toggleButton2.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggleButton2.Name = "toggleButton2";
            this.toggleButton2.OffBackColor = System.Drawing.Color.Gray;
            this.toggleButton2.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.toggleButton2.OnBackColor = System.Drawing.Color.LimeGreen;
            this.toggleButton2.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.toggleButton2.Size = new System.Drawing.Size(45, 22);
            this.toggleButton2.TabIndex = 37;
            this.toggleButton2.UseVisualStyleBackColor = true;
            // 
            // toggleButton1
            // 
            this.toggleButton1.AutoSize = true;
            this.toggleButton1.Checked = true;
            this.toggleButton1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleButton1.Location = new System.Drawing.Point(89, 268);
            this.toggleButton1.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggleButton1.Name = "toggleButton1";
            this.toggleButton1.OffBackColor = System.Drawing.Color.Gray;
            this.toggleButton1.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.toggleButton1.OnBackColor = System.Drawing.Color.LimeGreen;
            this.toggleButton1.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.toggleButton1.Size = new System.Drawing.Size(45, 22);
            this.toggleButton1.TabIndex = 35;
            this.toggleButton1.UseVisualStyleBackColor = true;
            // 
            // toggleButton_IMU_Status
            // 
            this.toggleButton_IMU_Status.AutoSize = true;
            this.toggleButton_IMU_Status.Checked = true;
            this.toggleButton_IMU_Status.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleButton_IMU_Status.Location = new System.Drawing.Point(89, 224);
            this.toggleButton_IMU_Status.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggleButton_IMU_Status.Name = "toggleButton_IMU_Status";
            this.toggleButton_IMU_Status.OffBackColor = System.Drawing.Color.Gray;
            this.toggleButton_IMU_Status.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.toggleButton_IMU_Status.OnBackColor = System.Drawing.Color.LimeGreen;
            this.toggleButton_IMU_Status.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.toggleButton_IMU_Status.Size = new System.Drawing.Size(45, 22);
            this.toggleButton_IMU_Status.TabIndex = 33;
            this.toggleButton_IMU_Status.UseVisualStyleBackColor = true;
            // 
            // visualizer_Component
            // 
            this.visualizer_Component.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.visualizer_Component.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualizer_Component.Location = new System.Drawing.Point(0, 0);
            this.visualizer_Component.Name = "visualizer_Component";
            this.visualizer_Component.Size = new System.Drawing.Size(1012, 690);
            this.visualizer_Component.Start_3D_Test_Flag = false;
            this.visualizer_Component.TabIndex = 0;
            this.visualizer_Component.Load += new System.EventHandler(this.visualizer_Component_Load);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel_Command_Control);
            this.Controls.Add(this.panel_Visualizer);
            this.Controls.Add(this.groupBox_Connection);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1100, 600);
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ORCA Simulator Application";
            this.groupBox_Connection.ResumeLayout(false);
            this.groupBox_Connection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Log)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Reconnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ORCA_Connection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Controller_Connection)).EndInit();
            this.panel_Visualizer.ResumeLayout(false);
            this.panel_Command_Control.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Connection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Controller_IP;
        private System.Windows.Forms.TextBox textBox_ORCA_IP;
        private ToggleButton toggleButton_IMU;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox_Controller_Connection;
        private System.Windows.Forms.PictureBox pictureBox_ORCA_Connection;
        private System.Windows.Forms.PictureBox pictureBox_Reconnect;
        private System.Windows.Forms.PictureBox pictureBox_Log;
        private System.Windows.Forms.Timer timer_Connection_Demo;
        private System.Windows.Forms.Panel panel_Visualizer;
        private System.Windows.Forms.Panel panel_Command_Control;
        private Visualizer_Component.Visualizer_Component visualizer_Component;
        private ToggleButton toggleButton_IMU_Status;
        private Command_Control.Command_Control command_Control;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_Roll_Position;
        private System.Windows.Forms.Label label_Y_Position;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private ToggleButton toggleButton1;
        private System.Windows.Forms.Label label14;
        private ToggleButton toggleButton2;
        private System.Windows.Forms.Label label15;
        private ToggleButton toggleButton3;
        private System.Windows.Forms.Label label16;
    }
}


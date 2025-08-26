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
            this.pictureBox_Log = new System.Windows.Forms.PictureBox();
            this.pictureBox_Reconnect = new System.Windows.Forms.PictureBox();
            this.pictureBox_ORCA_Connection = new System.Windows.Forms.PictureBox();
            this.pictureBox_Controller_Connection = new System.Windows.Forms.PictureBox();
            this.panel_Position = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label_Roll_Position = new System.Windows.Forms.Label();
            this.label_Y_Position = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ORCA_IP = new System.Windows.Forms.TextBox();
            this.textBox_Controller_IP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer_Connection_Demo = new System.Windows.Forms.Timer(this.components);
            this.panel_Visualizer = new System.Windows.Forms.Panel();
            this.panel_Command_Control = new System.Windows.Forms.Panel();
            this.visualizer_Component = new Visualizer_Component.Visualizer_Component();
            this.toggleButton_IMU = new ORCA_Simulator_Application.ToggleButton();
            this.groupBox_Connection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Log)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Reconnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ORCA_Connection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Controller_Connection)).BeginInit();
            this.panel_Position.SuspendLayout();
            this.panel_Visualizer.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Connection
            // 
            this.groupBox_Connection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox_Connection.Controls.Add(this.pictureBox_Log);
            this.groupBox_Connection.Controls.Add(this.pictureBox_Reconnect);
            this.groupBox_Connection.Controls.Add(this.pictureBox_ORCA_Connection);
            this.groupBox_Connection.Controls.Add(this.pictureBox_Controller_Connection);
            this.groupBox_Connection.Controls.Add(this.panel_Position);
            this.groupBox_Connection.Controls.Add(this.label3);
            this.groupBox_Connection.Controls.Add(this.toggleButton_IMU);
            this.groupBox_Connection.Controls.Add(this.textBox_ORCA_IP);
            this.groupBox_Connection.Controls.Add(this.textBox_Controller_IP);
            this.groupBox_Connection.Controls.Add(this.label2);
            this.groupBox_Connection.Controls.Add(this.label1);
            this.groupBox_Connection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.groupBox_Connection.Location = new System.Drawing.Point(12, 12);
            this.groupBox_Connection.Name = "groupBox_Connection";
            this.groupBox_Connection.Size = new System.Drawing.Size(145, 748);
            this.groupBox_Connection.TabIndex = 0;
            this.groupBox_Connection.TabStop = false;
            this.groupBox_Connection.Text = "Connection";
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
            this.pictureBox_Controller_Connection.Location = new System.Drawing.Point(114, 70);
            this.pictureBox_Controller_Connection.Name = "pictureBox_Controller_Connection";
            this.pictureBox_Controller_Connection.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_Controller_Connection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Controller_Connection.TabIndex = 29;
            this.pictureBox_Controller_Connection.TabStop = false;
            // 
            // panel_Position
            // 
            this.panel_Position.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel_Position.Controls.Add(this.label7);
            this.panel_Position.Controls.Add(this.label_Roll_Position);
            this.panel_Position.Controls.Add(this.label_Y_Position);
            this.panel_Position.Controls.Add(this.label6);
            this.panel_Position.Controls.Add(this.label5);
            this.panel_Position.Controls.Add(this.label4);
            this.panel_Position.Location = new System.Drawing.Point(6, 467);
            this.panel_Position.Name = "panel_Position";
            this.panel_Position.Size = new System.Drawing.Size(133, 139);
            this.panel_Position.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(27, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 34;
            this.label7.Text = "10 deg.";
            // 
            // label_Roll_Position
            // 
            this.label_Roll_Position.AutoSize = true;
            this.label_Roll_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Roll_Position.Location = new System.Drawing.Point(27, 63);
            this.label_Roll_Position.Name = "label_Roll_Position";
            this.label_Roll_Position.Size = new System.Drawing.Size(55, 15);
            this.label_Roll_Position.TabIndex = 33;
            this.label_Roll_Position.Text = "10 deg.";
            // 
            // label_Y_Position
            // 
            this.label_Y_Position.AutoSize = true;
            this.label_Y_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Y_Position.Location = new System.Drawing.Point(29, 13);
            this.label_Y_Position.Name = "label_Y_Position";
            this.label_Y_Position.Size = new System.Drawing.Size(51, 15);
            this.label_Y_Position.TabIndex = 32;
            this.label_Y_Position.Text = "10 mm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 15);
            this.label6.TabIndex = 31;
            this.label6.Text = "P:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 15);
            this.label5.TabIndex = 30;
            this.label5.Text = "R:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 15);
            this.label4.TabIndex = 29;
            this.label4.Text = "Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 248);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "IMU Status";
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
            this.panel_Visualizer.Location = new System.Drawing.Point(786, 16);
            this.panel_Visualizer.Name = "panel_Visualizer";
            this.panel_Visualizer.Size = new System.Drawing.Size(577, 744);
            this.panel_Visualizer.TabIndex = 1;
            // 
            // panel_Command_Control
            // 
            this.panel_Command_Control.Location = new System.Drawing.Point(163, 16);
            this.panel_Command_Control.Name = "panel_Command_Control";
            this.panel_Command_Control.Size = new System.Drawing.Size(617, 744);
            this.panel_Command_Control.TabIndex = 2;
            // 
            // visualizer_Component
            // 
            this.visualizer_Component.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.visualizer_Component.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualizer_Component.Location = new System.Drawing.Point(0, 0);
            this.visualizer_Component.Name = "visualizer_Component";
            this.visualizer_Component.Size = new System.Drawing.Size(577, 744);
            this.visualizer_Component.Start_3D_Test_Flag = false;
            this.visualizer_Component.TabIndex = 0;
            // 
            // toggleButton_IMU
            // 
            this.toggleButton_IMU.AutoSize = true;
            this.toggleButton_IMU.Checked = true;
            this.toggleButton_IMU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleButton_IMU.Location = new System.Drawing.Point(94, 243);
            this.toggleButton_IMU.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggleButton_IMU.Name = "toggleButton_IMU";
            this.toggleButton_IMU.OffBackColor = System.Drawing.Color.Gray;
            this.toggleButton_IMU.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.toggleButton_IMU.OnBackColor = System.Drawing.Color.LimeGreen;
            this.toggleButton_IMU.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.toggleButton_IMU.Size = new System.Drawing.Size(45, 22);
            this.toggleButton_IMU.TabIndex = 1;
            this.toggleButton_IMU.UseVisualStyleBackColor = true;
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.panel_Command_Control);
            this.Controls.Add(this.panel_Visualizer);
            this.Controls.Add(this.groupBox_Connection);
            this.MaximizeBox = false;
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ORCA Simulator Application";
            this.groupBox_Connection.ResumeLayout(false);
            this.groupBox_Connection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Log)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Reconnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ORCA_Connection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Controller_Connection)).EndInit();
            this.panel_Position.ResumeLayout(false);
            this.panel_Position.PerformLayout();
            this.panel_Visualizer.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panel_Position;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_Y_Position;
        private System.Windows.Forms.Label label_Roll_Position;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox_Controller_Connection;
        private System.Windows.Forms.PictureBox pictureBox_ORCA_Connection;
        private System.Windows.Forms.PictureBox pictureBox_Reconnect;
        private System.Windows.Forms.PictureBox pictureBox_Log;
        private System.Windows.Forms.Timer timer_Connection_Demo;
        private System.Windows.Forms.Panel panel_Visualizer;
        private System.Windows.Forms.Panel panel_Command_Control;
        private Visualizer_Component.Visualizer_Component visualizer_Component;
    }
}


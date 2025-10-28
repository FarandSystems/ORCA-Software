namespace Auto_Detect_VCP_Control
{
    partial class Auto_Detect_VCP_Control
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_Start = new System.Windows.Forms.Button();
            this.textBox_Rx_Bytes = new System.Windows.Forms.TextBox();
            this.textBox_Tx_Bytes = new System.Windows.Forms.TextBox();
            this.label_Available_Ports = new System.Windows.Forms.Label();
            this.comboBox_Available_Ports = new System.Windows.Forms.ComboBox();
            this.checkBox_Tx_Bytes = new System.Windows.Forms.CheckBox();
            this.checkBox_Rx_Bytes = new System.Windows.Forms.CheckBox();
            this.label_Status = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serial_Port = new System.IO.Ports.SerialPort(this.components);
            this.timer_State_Machine = new System.Windows.Forms.Timer(this.components);
            this.label_Connection = new System.Windows.Forms.Label();
            this.label_State = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.button_Start.Location = new System.Drawing.Point(99, 108);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 23;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = false;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // textBox_Rx_Bytes
            // 
            this.textBox_Rx_Bytes.Location = new System.Drawing.Point(217, 3);
            this.textBox_Rx_Bytes.Multiline = true;
            this.textBox_Rx_Bytes.Name = "textBox_Rx_Bytes";
            this.textBox_Rx_Bytes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Rx_Bytes.Size = new System.Drawing.Size(260, 74);
            this.textBox_Rx_Bytes.TabIndex = 17;
            // 
            // textBox_Tx_Bytes
            // 
            this.textBox_Tx_Bytes.Location = new System.Drawing.Point(217, 78);
            this.textBox_Tx_Bytes.Multiline = true;
            this.textBox_Tx_Bytes.Name = "textBox_Tx_Bytes";
            this.textBox_Tx_Bytes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Tx_Bytes.Size = new System.Drawing.Size(260, 75);
            this.textBox_Tx_Bytes.TabIndex = 18;
            // 
            // label_Available_Ports
            // 
            this.label_Available_Ports.AutoSize = true;
            this.label_Available_Ports.Location = new System.Drawing.Point(16, 44);
            this.label_Available_Ports.Name = "label_Available_Ports";
            this.label_Available_Ports.Size = new System.Drawing.Size(77, 13);
            this.label_Available_Ports.TabIndex = 16;
            this.label_Available_Ports.Text = "Available Ports";
            // 
            // comboBox_Available_Ports
            // 
            this.comboBox_Available_Ports.FormattingEnabled = true;
            this.comboBox_Available_Ports.Location = new System.Drawing.Point(99, 41);
            this.comboBox_Available_Ports.Name = "comboBox_Available_Ports";
            this.comboBox_Available_Ports.Size = new System.Drawing.Size(63, 21);
            this.comboBox_Available_Ports.TabIndex = 15;
            // 
            // checkBox_Tx_Bytes
            // 
            this.checkBox_Tx_Bytes.AutoSize = true;
            this.checkBox_Tx_Bytes.Location = new System.Drawing.Point(147, 79);
            this.checkBox_Tx_Bytes.Name = "checkBox_Tx_Bytes";
            this.checkBox_Tx_Bytes.Size = new System.Drawing.Size(67, 17);
            this.checkBox_Tx_Bytes.TabIndex = 21;
            this.checkBox_Tx_Bytes.Text = "Tx Bytes";
            this.checkBox_Tx_Bytes.UseVisualStyleBackColor = true;
            // 
            // checkBox_Rx_Bytes
            // 
            this.checkBox_Rx_Bytes.AutoSize = true;
            this.checkBox_Rx_Bytes.Location = new System.Drawing.Point(147, 1);
            this.checkBox_Rx_Bytes.Name = "checkBox_Rx_Bytes";
            this.checkBox_Rx_Bytes.Size = new System.Drawing.Size(68, 17);
            this.checkBox_Rx_Bytes.TabIndex = 22;
            this.checkBox_Rx_Bytes.Text = "Rx Bytes";
            this.checkBox_Rx_Bytes.UseVisualStyleBackColor = true;
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(34, 1);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(79, 13);
            this.label_Status.TabIndex = 20;
            this.label_Status.Text = "Not Connected";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer_State_Machine
            // 
            this.timer_State_Machine.Tick += new System.EventHandler(this.timer_State_Machine_Tick);
            // 
            // label_Connection
            // 
            this.label_Connection.AutoSize = true;
            this.label_Connection.BackColor = System.Drawing.Color.Transparent;
            this.label_Connection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_Connection.Font = new System.Drawing.Font("Wingdings 2", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.label_Connection.ForeColor = System.Drawing.Color.DimGray;
            this.label_Connection.Location = new System.Drawing.Point(2, 3);
            this.label_Connection.Name = "label_Connection";
            this.label_Connection.Size = new System.Drawing.Size(12, 9);
            this.label_Connection.TabIndex = 24;
            this.label_Connection.Text = "Ä";
            this.label_Connection.Click += new System.EventHandler(this.label_Connection_Click);
            // 
            // label_State
            // 
            this.label_State.AutoSize = true;
            this.label_State.Location = new System.Drawing.Point(7, 22);
            this.label_State.Name = "label_State";
            this.label_State.Size = new System.Drawing.Size(0, 13);
            this.label_State.TabIndex = 25;
            // 
            // Auto_Detect_VCP_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_State);
            this.Controls.Add(this.textBox_Tx_Bytes);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.checkBox_Tx_Bytes);
            this.Controls.Add(this.label_Connection);
            this.Controls.Add(this.textBox_Rx_Bytes);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.comboBox_Available_Ports);
            this.Controls.Add(this.checkBox_Rx_Bytes);
            this.Controls.Add(this.label_Available_Ports);
            this.Name = "Auto_Detect_VCP_Control";
            this.Size = new System.Drawing.Size(479, 155);
            this.Load += new System.EventHandler(this.VCP_Control_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_Rx_Bytes;
        private System.Windows.Forms.TextBox textBox_Tx_Bytes;
        private System.Windows.Forms.Label label_Available_Ports;
        private System.Windows.Forms.ComboBox comboBox_Available_Ports;
        private System.Windows.Forms.CheckBox checkBox_Tx_Bytes;
        private System.Windows.Forms.CheckBox checkBox_Rx_Bytes;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
        private System.IO.Ports.SerialPort serial_Port;
        private System.Windows.Forms.Timer timer_State_Machine;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Label label_Connection;
        private System.Windows.Forms.Label label_State;
    }
}

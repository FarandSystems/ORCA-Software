namespace Simple_VCP_Control
{
    partial class Simple_VCP_Control
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel_COM_LED = new System.Windows.Forms.Panel();
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
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel_COM_LED);
            this.panel1.Controls.Add(this.textBox_Rx_Bytes);
            this.panel1.Controls.Add(this.textBox_Tx_Bytes);
            this.panel1.Controls.Add(this.label_Available_Ports);
            this.panel1.Controls.Add(this.comboBox_Available_Ports);
            this.panel1.Controls.Add(this.checkBox_Tx_Bytes);
            this.panel1.Controls.Add(this.checkBox_Rx_Bytes);
            this.panel1.Controls.Add(this.label_Status);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(480, 160);
            this.panel1.TabIndex = 0;
            // 
            // panel_COM_LED
            // 
            this.panel_COM_LED.BackColor = System.Drawing.Color.DimGray;
            this.panel_COM_LED.Location = new System.Drawing.Point(2, 2);
            this.panel_COM_LED.Name = "panel_COM_LED";
            this.panel_COM_LED.Size = new System.Drawing.Size(20, 20);
            this.panel_COM_LED.TabIndex = 19;
            this.panel_COM_LED.Click += new System.EventHandler(this.panel_COM_LED_Click);
            // 
            // textBox_Rx_Bytes
            // 
            this.textBox_Rx_Bytes.Location = new System.Drawing.Point(214, 2);
            this.textBox_Rx_Bytes.Multiline = true;
            this.textBox_Rx_Bytes.Name = "textBox_Rx_Bytes";
            this.textBox_Rx_Bytes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Rx_Bytes.Size = new System.Drawing.Size(260, 74);
            this.textBox_Rx_Bytes.TabIndex = 17;
            // 
            // textBox_Tx_Bytes
            // 
            this.textBox_Tx_Bytes.Location = new System.Drawing.Point(214, 80);
            this.textBox_Tx_Bytes.Multiline = true;
            this.textBox_Tx_Bytes.Name = "textBox_Tx_Bytes";
            this.textBox_Tx_Bytes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Tx_Bytes.Size = new System.Drawing.Size(260, 74);
            this.textBox_Tx_Bytes.TabIndex = 18;
            // 
            // label_Available_Ports
            // 
            this.label_Available_Ports.AutoSize = true;
            this.label_Available_Ports.Location = new System.Drawing.Point(5, 38);
            this.label_Available_Ports.Name = "label_Available_Ports";
            this.label_Available_Ports.Size = new System.Drawing.Size(77, 13);
            this.label_Available_Ports.TabIndex = 16;
            this.label_Available_Ports.Text = "Available Ports";
            // 
            // comboBox_Available_Ports
            // 
            this.comboBox_Available_Ports.FormattingEnabled = true;
            this.comboBox_Available_Ports.Location = new System.Drawing.Point(85, 35);
            this.comboBox_Available_Ports.Name = "comboBox_Available_Ports";
            this.comboBox_Available_Ports.Size = new System.Drawing.Size(63, 21);
            this.comboBox_Available_Ports.TabIndex = 15;
            // 
            // checkBox_Tx_Bytes
            // 
            this.checkBox_Tx_Bytes.AutoSize = true;
            this.checkBox_Tx_Bytes.Location = new System.Drawing.Point(148, 83);
            this.checkBox_Tx_Bytes.Name = "checkBox_Tx_Bytes";
            this.checkBox_Tx_Bytes.Size = new System.Drawing.Size(67, 17);
            this.checkBox_Tx_Bytes.TabIndex = 21;
            this.checkBox_Tx_Bytes.Text = "Tx Bytes";
            this.checkBox_Tx_Bytes.UseVisualStyleBackColor = true;
            // 
            // checkBox_Rx_Bytes
            // 
            this.checkBox_Rx_Bytes.AutoSize = true;
            this.checkBox_Rx_Bytes.Location = new System.Drawing.Point(148, 4);
            this.checkBox_Rx_Bytes.Name = "checkBox_Rx_Bytes";
            this.checkBox_Rx_Bytes.Size = new System.Drawing.Size(68, 17);
            this.checkBox_Rx_Bytes.TabIndex = 22;
            this.checkBox_Rx_Bytes.Text = "Rx Bytes";
            this.checkBox_Rx_Bytes.UseVisualStyleBackColor = true;
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(25, 6);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(79, 13);
            this.label_Status.TabIndex = 20;
            this.label_Status.Text = "Not Connected";
            // 
            // Simple_VCP_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "Simple_VCP_Control";
            this.Size = new System.Drawing.Size(480, 160);
            this.Load += new System.EventHandler(this.Simple_VCP_Control_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel_COM_LED;
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
    }
}

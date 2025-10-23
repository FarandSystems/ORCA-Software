namespace Simple_Client_LAN_Control
{
    partial class Simple_Client_LAN_Control
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
            this.textBox_Rx_Data = new System.Windows.Forms.TextBox();
            this.timer_LED = new System.Windows.Forms.Timer(this.components);
            this.timer_Connection_Retry = new System.Windows.Forms.Timer(this.components);
            this.label_Lan_Status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_Rx_Data
            // 
            this.textBox_Rx_Data.Location = new System.Drawing.Point(0, 38);
            this.textBox_Rx_Data.Multiline = true;
            this.textBox_Rx_Data.Name = "textBox_Rx_Data";
            this.textBox_Rx_Data.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Rx_Data.Size = new System.Drawing.Size(360, 142);
            this.textBox_Rx_Data.TabIndex = 9;
            this.textBox_Rx_Data.Visible = false;
            // 
            // timer_LED
            // 
            this.timer_LED.Enabled = true;
            this.timer_LED.Interval = 500;
            this.timer_LED.Tick += new System.EventHandler(this.timer_LED_Tick);
            // 
            // timer_Connection_Retry
            // 
            this.timer_Connection_Retry.Interval = 20000;
            this.timer_Connection_Retry.Tick += new System.EventHandler(this.timer_Connection_Retry_Tick);
            // 
            // label_Lan_Status
            // 
            this.label_Lan_Status.AutoSize = true;
            this.label_Lan_Status.Font = new System.Drawing.Font("Wingdings 2", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.label_Lan_Status.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label_Lan_Status.Location = new System.Drawing.Point(-2, 0);
            this.label_Lan_Status.Name = "label_Lan_Status";
            this.label_Lan_Status.Size = new System.Drawing.Size(12, 9);
            this.label_Lan_Status.TabIndex = 100;
            this.label_Lan_Status.Text = "Ä";
            // 
            // Simple_Client_LAN_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_Lan_Status);
            this.Controls.Add(this.textBox_Rx_Data);
            this.Name = "Simple_Client_LAN_Control";
            this.Size = new System.Drawing.Size(34, 23);
            this.Load += new System.EventHandler(this.Simple_Client_LAN_Control_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_Rx_Data;
        private System.Windows.Forms.Timer timer_LED;
        private System.Windows.Forms.Timer timer_Connection_Retry;
        private System.Windows.Forms.Label label_Lan_Status;
    }
}

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Simple_Client_LAN_Control
{
    partial class Simple_Client_LAN_Control
    {
        private IContainer components = null;
        private Label labelStatus;
        private TextBox textBoxLog;
        private System.Windows.Forms.Timer timerUiStatus;
        private System.Windows.Forms.Timer timerHeartbeat;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelStatus = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.timerUiStatus = new System.Windows.Forms.Timer(this.components);
            this.timerHeartbeat = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.labelStatus.ForeColor = System.Drawing.Color.Tomato;
            this.labelStatus.Location = new System.Drawing.Point(8, 8);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(98, 14);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "DISCONNECTED";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                            | System.Windows.Forms.AnchorStyles.Left)
                                                                            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.BackColor = System.Drawing.Color.Black;
            this.textBoxLog.ForeColor = System.Drawing.Color.Lime;
            this.textBoxLog.Location = new System.Drawing.Point(8, 30);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(260, 120);
            this.textBoxLog.TabIndex = 1;
            // 
            // timerUiStatus
            // 
            this.timerUiStatus.Interval = 200;
            this.timerUiStatus.Tick += new System.EventHandler(this.timerUiStatus_Tick);
            // 
            // timerHeartbeat
            // 
            this.timerHeartbeat.Interval = 5000; // HeartbeatIntervalMs
            this.timerHeartbeat.Tick += new System.EventHandler(this.timerHeartbeat_Tick);
            // 
            // Simple_Client_LAN_Control
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.labelStatus);
            this.Name = "Simple_Client_LAN_Control";
            this.Size = new System.Drawing.Size(280, 160);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

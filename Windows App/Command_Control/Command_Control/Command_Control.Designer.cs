namespace Command_Control
{
    partial class Command_Control
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
            this.metroTabControl1 = new ReaLTaiizor.Controls.MetroTabControl();
            this.tabPage_Manual_Control = new ReaLTaiizor.Child.Metro.MetroTabPage();
            this.axis_Roll = new CNC_Axis_Component.Axis_Component();
            this.tabPage_Scenario = new ReaLTaiizor.Child.Metro.MetroTabPage();
            this.pictureBox_Run_Scenario = new System.Windows.Forms.PictureBox();
            this.richTextBox_System_Messages = new LineNumberedRichTextBox();
            this.tabPage_Functions = new ReaLTaiizor.Child.Metro.MetroTabPage();
            this.axis_Heave = new CNC_Axis_Component.Axis_Component();
            this.axis_Component1 = new CNC_Axis_Component.Axis_Component();
            this.signalGeneratorControl1 = new Movement_Functions_Control.SignalGeneratorControl();
            this.metroTabControl1.SuspendLayout();
            this.tabPage_Manual_Control.SuspendLayout();
            this.tabPage_Scenario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Run_Scenario)).BeginInit();
            this.tabPage_Functions.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.AnimateEasingType = ReaLTaiizor.Enum.Metro.EasingType.CubeOut;
            this.metroTabControl1.AnimateTime = 200;
            this.metroTabControl1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.metroTabControl1.Controls.Add(this.tabPage_Scenario);
            this.metroTabControl1.Controls.Add(this.tabPage_Manual_Control);
            this.metroTabControl1.Controls.Add(this.tabPage_Functions);
            this.metroTabControl1.ControlsVisible = true;
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.IsDerivedStyle = true;
            this.metroTabControl1.ItemSize = new System.Drawing.Size(100, 38);
            this.metroTabControl1.Location = new System.Drawing.Point(0, 0);
            this.metroTabControl1.MCursor = System.Windows.Forms.Cursors.Hand;
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 1;
            this.metroTabControl1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroTabControl1.Size = new System.Drawing.Size(330, 744);
            this.metroTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.metroTabControl1.Speed = 100;
            this.metroTabControl1.Style = ReaLTaiizor.Enum.Metro.Style.Custom;
            this.metroTabControl1.StyleManager = null;
            this.metroTabControl1.TabIndex = 1;
            this.metroTabControl1.TabStyle = ReaLTaiizor.Enum.Metro.TabStyle.Style2;
            this.metroTabControl1.ThemeAuthor = "Taiizor";
            this.metroTabControl1.ThemeName = "MetroDark";
            this.metroTabControl1.UnselectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            // 
            // tabPage_Manual_Control
            // 
            this.tabPage_Manual_Control.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage_Manual_Control.Controls.Add(this.axis_Component1);
            this.tabPage_Manual_Control.Controls.Add(this.axis_Roll);
            this.tabPage_Manual_Control.Controls.Add(this.axis_Heave);
            this.tabPage_Manual_Control.Font = null;
            this.tabPage_Manual_Control.ImageIndex = 0;
            this.tabPage_Manual_Control.ImageKey = null;
            this.tabPage_Manual_Control.IsDerivedStyle = true;
            this.tabPage_Manual_Control.Location = new System.Drawing.Point(4, 42);
            this.tabPage_Manual_Control.Name = "tabPage_Manual_Control";
            this.tabPage_Manual_Control.Size = new System.Drawing.Size(322, 698);
            this.tabPage_Manual_Control.Style = ReaLTaiizor.Enum.Metro.Style.Custom;
            this.tabPage_Manual_Control.StyleManager = null;
            this.tabPage_Manual_Control.TabIndex = 1;
            this.tabPage_Manual_Control.Text = "Manual Control";
            this.tabPage_Manual_Control.ThemeAuthor = "Taiizor";
            this.tabPage_Manual_Control.ThemeName = "MetroLight";
            this.tabPage_Manual_Control.ToolTipText = null;
            // 
            // axis_Roll
            // 
            this.axis_Roll.Axis_Course = 100D;
            this.axis_Roll.Axis_Name = "Roll";
            this.axis_Roll.Axis_Unit = "(deg.)";
            this.axis_Roll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.axis_Roll.Command_Address = ((byte)(1));
            this.axis_Roll.Command_Bytes = new byte[] {
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0))};
            this.axis_Roll.Command_Is_Ready = false;
            this.axis_Roll.Current_Position = 0D;
            this.axis_Roll.Errors = null;
            this.axis_Roll.Location = new System.Drawing.Point(0, 236);
            this.axis_Roll.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.axis_Roll.Mm_Per_Rotation = 5D;
            this.axis_Roll.More_Settings = false;
            this.axis_Roll.Name = "axis_Roll";
            this.axis_Roll.Size = new System.Drawing.Size(321, 224);
            this.axis_Roll.Step = 0;
            this.axis_Roll.Stepper_Motor_Resolution = 400D;
            this.axis_Roll.TabIndex = 1;
            // 
            // tabPage_Scenario
            // 
            this.tabPage_Scenario.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage_Scenario.Controls.Add(this.pictureBox_Run_Scenario);
            this.tabPage_Scenario.Controls.Add(this.richTextBox_System_Messages);
            this.tabPage_Scenario.Font = null;
            this.tabPage_Scenario.ImageIndex = 0;
            this.tabPage_Scenario.ImageKey = null;
            this.tabPage_Scenario.IsDerivedStyle = true;
            this.tabPage_Scenario.Location = new System.Drawing.Point(4, 42);
            this.tabPage_Scenario.Name = "tabPage_Scenario";
            this.tabPage_Scenario.Size = new System.Drawing.Size(322, 698);
            this.tabPage_Scenario.Style = ReaLTaiizor.Enum.Metro.Style.Custom;
            this.tabPage_Scenario.StyleManager = null;
            this.tabPage_Scenario.TabIndex = 0;
            this.tabPage_Scenario.Text = "Scenario";
            this.tabPage_Scenario.ThemeAuthor = "Taiizor";
            this.tabPage_Scenario.ThemeName = "MetroLight";
            this.tabPage_Scenario.ToolTipText = null;
            // 
            // pictureBox_Run_Scenario
            // 
            this.pictureBox_Run_Scenario.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Run_Scenario.Image = global::Command_Control.Properties.Resources.Run;
            this.pictureBox_Run_Scenario.Location = new System.Drawing.Point(269, 645);
            this.pictureBox_Run_Scenario.Name = "pictureBox_Run_Scenario";
            this.pictureBox_Run_Scenario.Size = new System.Drawing.Size(50, 50);
            this.pictureBox_Run_Scenario.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Run_Scenario.TabIndex = 33;
            this.pictureBox_Run_Scenario.TabStop = false;
            // 
            // richTextBox_System_Messages
            // 
            this.richTextBox_System_Messages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.richTextBox_System_Messages.CurrentLineNumberBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.richTextBox_System_Messages.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox_System_Messages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.richTextBox_System_Messages.GutterBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.richTextBox_System_Messages.GutterForeColor = System.Drawing.Color.DimGray;
            this.richTextBox_System_Messages.GutterPadding = new System.Windows.Forms.Padding(0, 2, 6, 2);
            this.richTextBox_System_Messages.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_System_Messages.Name = "richTextBox_System_Messages";
            this.richTextBox_System_Messages.Size = new System.Drawing.Size(322, 470);
            this.richTextBox_System_Messages.TabIndex = 1;
            // 
            // tabPage_Functions
            // 
            this.tabPage_Functions.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage_Functions.Controls.Add(this.signalGeneratorControl1);
            this.tabPage_Functions.Font = null;
            this.tabPage_Functions.ImageIndex = 0;
            this.tabPage_Functions.ImageKey = null;
            this.tabPage_Functions.IsDerivedStyle = true;
            this.tabPage_Functions.Location = new System.Drawing.Point(4, 42);
            this.tabPage_Functions.Name = "tabPage_Functions";
            this.tabPage_Functions.Size = new System.Drawing.Size(322, 698);
            this.tabPage_Functions.Style = ReaLTaiizor.Enum.Metro.Style.Custom;
            this.tabPage_Functions.StyleManager = null;
            this.tabPage_Functions.TabIndex = 2;
            this.tabPage_Functions.Text = "Movement Functions";
            this.tabPage_Functions.ThemeAuthor = "Taiizor";
            this.tabPage_Functions.ThemeName = "MetroLight";
            this.tabPage_Functions.ToolTipText = null;
            // 
            // axis_Heave
            // 
            this.axis_Heave.Axis_Course = 100D;
            this.axis_Heave.Axis_Name = "Heave";
            this.axis_Heave.Axis_Unit = "(mm)";
            this.axis_Heave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.axis_Heave.Command_Address = ((byte)(1));
            this.axis_Heave.Command_Bytes = new byte[] {
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0))};
            this.axis_Heave.Command_Is_Ready = false;
            this.axis_Heave.Current_Position = 0D;
            this.axis_Heave.Errors = null;
            this.axis_Heave.Location = new System.Drawing.Point(0, 2);
            this.axis_Heave.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.axis_Heave.Mm_Per_Rotation = 5D;
            this.axis_Heave.More_Settings = false;
            this.axis_Heave.Name = "axis_Heave";
            this.axis_Heave.Size = new System.Drawing.Size(321, 224);
            this.axis_Heave.Step = 0;
            this.axis_Heave.Stepper_Motor_Resolution = 400D;
            this.axis_Heave.TabIndex = 0;
            this.axis_Heave.Load += new System.EventHandler(this.axis_Heave_Load_1);
            // 
            // axis_Component1
            // 
            this.axis_Component1.Axis_Course = 100D;
            this.axis_Component1.Axis_Name = "Pitch";
            this.axis_Component1.Axis_Unit = "(deg.)";
            this.axis_Component1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.axis_Component1.Command_Address = ((byte)(1));
            this.axis_Component1.Command_Bytes = new byte[] {
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0)),
        ((byte)(0))};
            this.axis_Component1.Command_Is_Ready = false;
            this.axis_Component1.Current_Position = 0D;
            this.axis_Component1.Errors = null;
            this.axis_Component1.Location = new System.Drawing.Point(0, 470);
            this.axis_Component1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.axis_Component1.Mm_Per_Rotation = 5D;
            this.axis_Component1.More_Settings = false;
            this.axis_Component1.Name = "axis_Component1";
            this.axis_Component1.Size = new System.Drawing.Size(321, 224);
            this.axis_Component1.Step = 0;
            this.axis_Component1.Stepper_Motor_Resolution = 400D;
            this.axis_Component1.TabIndex = 2;
            this.axis_Component1.Load += new System.EventHandler(this.axis_Component1_Load);
            // 
            // signalGeneratorControl1
            // 
            this.signalGeneratorControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.signalGeneratorControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.signalGeneratorControl1.Location = new System.Drawing.Point(3, 13);
            this.signalGeneratorControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.signalGeneratorControl1.Name = "signalGeneratorControl1";
            this.signalGeneratorControl1.Size = new System.Drawing.Size(328, 677);
            this.signalGeneratorControl1.TabIndex = 1;
            // 
            // Command_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.metroTabControl1);
            this.Name = "Command_Control";
            this.Size = new System.Drawing.Size(330, 744);
            this.metroTabControl1.ResumeLayout(false);
            this.tabPage_Manual_Control.ResumeLayout(false);
            this.tabPage_Scenario.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Run_Scenario)).EndInit();
            this.tabPage_Functions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MetroTabControl metroTabControl1;
        private ReaLTaiizor.Child.Metro.MetroTabPage tabPage_Manual_Control;
        private ReaLTaiizor.Child.Metro.MetroTabPage tabPage_Scenario;
        private ReaLTaiizor.Child.Metro.MetroTabPage tabPage_Functions;
        private LineNumberedRichTextBox richTextBox_System_Messages;
        private System.Windows.Forms.PictureBox pictureBox_Run_Scenario;
        private CNC_Axis_Component.Axis_Component axis_Roll;
        private CNC_Axis_Component.Axis_Component axis_Heave;
        private CNC_Axis_Component.Axis_Component axis_Component1;
        private Movement_Functions_Control.SignalGeneratorControl signalGeneratorControl1;
    }
}

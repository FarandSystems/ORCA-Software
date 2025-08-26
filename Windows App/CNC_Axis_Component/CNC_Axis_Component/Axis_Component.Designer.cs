namespace CNC_Axis_Component
{
    partial class Axis_Component
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Axis_Component));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox_Nodge_Forward = new System.Windows.Forms.PictureBox();
            this.pictureBox_Nodge_Backward = new System.Windows.Forms.PictureBox();
            this.groupBox_Step = new System.Windows.Forms.GroupBox();
            this.pictureBox_Step_Forward = new System.Windows.Forms.PictureBox();
            this.numericUpDown_Step = new System.Windows.Forms.NumericUpDown();
            this.pictureBox_Step_Backward = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_Go_To = new System.Windows.Forms.GroupBox();
            this.pictureBox_GoTo = new System.Windows.Forms.PictureBox();
            this.label_Axis = new System.Windows.Forms.Label();
            this.numericUpDown_Go_To = new System.Windows.Forms.NumericUpDown();
            this.label_Course = new System.Windows.Forms.Label();
            this.pictureBox_Error = new System.Windows.Forms.PictureBox();
            this.pictureBox_Center = new System.Windows.Forms.PictureBox();
            this.pictureBox_Home = new System.Windows.Forms.PictureBox();
            this.label_Current_Position = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Nodge_Forward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Nodge_Backward)).BeginInit();
            this.groupBox_Step.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step_Forward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Step)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step_Backward)).BeginInit();
            this.groupBox_Go_To.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_GoTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Go_To)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Error)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Center)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Home)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox1.Controls.Add(this.pictureBox_Nodge_Forward);
            this.groupBox1.Controls.Add(this.pictureBox_Nodge_Backward);
            this.groupBox1.Controls.Add(this.groupBox_Step);
            this.groupBox1.Controls.Add(this.groupBox_Go_To);
            this.groupBox1.Controls.Add(this.pictureBox_Error);
            this.groupBox1.Controls.Add(this.pictureBox_Center);
            this.groupBox1.Controls.Add(this.pictureBox_Home);
            this.groupBox1.Controls.Add(this.label_Current_Position);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 230);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control X";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // pictureBox_Nodge_Forward
            // 
            this.pictureBox_Nodge_Forward.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Nodge_Forward.Image = global::CNC_Axis_Component.Properties.Resources.Nodge_Forward;
            this.pictureBox_Nodge_Forward.Location = new System.Drawing.Point(210, 49);
            this.pictureBox_Nodge_Forward.Name = "pictureBox_Nodge_Forward";
            this.pictureBox_Nodge_Forward.Size = new System.Drawing.Size(30, 30);
            this.pictureBox_Nodge_Forward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Nodge_Forward.TabIndex = 67;
            this.pictureBox_Nodge_Forward.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Nodge_Forward, "Microstep forward!");
            this.pictureBox_Nodge_Forward.Click += new System.EventHandler(this.picBox_Nodge_Click);
            this.pictureBox_Nodge_Forward.MouseEnter += new System.EventHandler(this.General_Mouse_Enter);
            this.pictureBox_Nodge_Forward.MouseLeave += new System.EventHandler(this.General_Mouse_Leave);
            // 
            // pictureBox_Nodge_Backward
            // 
            this.pictureBox_Nodge_Backward.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Nodge_Backward.Image = global::CNC_Axis_Component.Properties.Resources.Nodge_Backward;
            this.pictureBox_Nodge_Backward.Location = new System.Drawing.Point(179, 49);
            this.pictureBox_Nodge_Backward.Name = "pictureBox_Nodge_Backward";
            this.pictureBox_Nodge_Backward.Size = new System.Drawing.Size(30, 30);
            this.pictureBox_Nodge_Backward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Nodge_Backward.TabIndex = 66;
            this.pictureBox_Nodge_Backward.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Nodge_Backward, "Microstep backward!");
            this.pictureBox_Nodge_Backward.Click += new System.EventHandler(this.picBox_Nodge_Click);
            this.pictureBox_Nodge_Backward.MouseEnter += new System.EventHandler(this.General_Mouse_Enter);
            this.pictureBox_Nodge_Backward.MouseLeave += new System.EventHandler(this.General_Mouse_Leave);
            // 
            // groupBox_Step
            // 
            this.groupBox_Step.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Step.Controls.Add(this.pictureBox_Step_Forward);
            this.groupBox_Step.Controls.Add(this.numericUpDown_Step);
            this.groupBox_Step.Controls.Add(this.pictureBox_Step_Backward);
            this.groupBox_Step.Controls.Add(this.label1);
            this.groupBox_Step.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.groupBox_Step.Location = new System.Drawing.Point(6, 177);
            this.groupBox_Step.Name = "groupBox_Step";
            this.groupBox_Step.Size = new System.Drawing.Size(234, 45);
            this.groupBox_Step.TabIndex = 65;
            this.groupBox_Step.TabStop = false;
            this.groupBox_Step.Text = "Step";
            this.groupBox_Step.Enter += new System.EventHandler(this.groupBox_Step_Enter);
            // 
            // pictureBox_Step_Forward
            // 
            this.pictureBox_Step_Forward.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Step_Forward.Image = global::CNC_Axis_Component.Properties.Resources.Manual_Step_Up;
            this.pictureBox_Step_Forward.Location = new System.Drawing.Point(200, 13);
            this.pictureBox_Step_Forward.Name = "pictureBox_Step_Forward";
            this.pictureBox_Step_Forward.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Step_Forward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Step_Forward.TabIndex = 69;
            this.pictureBox_Step_Forward.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Step_Forward, "Step forward!");
            this.pictureBox_Step_Forward.Click += new System.EventHandler(this.picBox_Manual_Step_Click);
            this.pictureBox_Step_Forward.MouseEnter += new System.EventHandler(this.General_Mouse_Enter);
            this.pictureBox_Step_Forward.MouseLeave += new System.EventHandler(this.General_Mouse_Leave);
            // 
            // numericUpDown_Step
            // 
            this.numericUpDown_Step.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.numericUpDown_Step.DecimalPlaces = 2;
            this.numericUpDown_Step.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.numericUpDown_Step.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_Step.Location = new System.Drawing.Point(83, 15);
            this.numericUpDown_Step.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDown_Step.Minimum = new decimal(new int[] {
            250,
            0,
            0,
            -2147483648});
            this.numericUpDown_Step.Name = "numericUpDown_Step";
            this.numericUpDown_Step.Size = new System.Drawing.Size(53, 20);
            this.numericUpDown_Step.TabIndex = 49;
            this.numericUpDown_Step.ValueChanged += new System.EventHandler(this.numericUpDown_Step_ValueChanged);
            // 
            // pictureBox_Step_Backward
            // 
            this.pictureBox_Step_Backward.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Step_Backward.Image = global::CNC_Axis_Component.Properties.Resources.Manual_Step_Down;
            this.pictureBox_Step_Backward.Location = new System.Drawing.Point(173, 13);
            this.pictureBox_Step_Backward.Name = "pictureBox_Step_Backward";
            this.pictureBox_Step_Backward.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Step_Backward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Step_Backward.TabIndex = 68;
            this.pictureBox_Step_Backward.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Step_Backward, "Step backward!");
            this.pictureBox_Step_Backward.Click += new System.EventHandler(this.picBox_Manual_Step_Click);
            this.pictureBox_Step_Backward.MouseEnter += new System.EventHandler(this.General_Mouse_Enter);
            this.pictureBox_Step_Backward.MouseLeave += new System.EventHandler(this.General_Mouse_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(2, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 55;
            this.label1.Text = "Step (mm)";
            // 
            // groupBox_Go_To
            // 
            this.groupBox_Go_To.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Go_To.Controls.Add(this.pictureBox_GoTo);
            this.groupBox_Go_To.Controls.Add(this.label_Axis);
            this.groupBox_Go_To.Controls.Add(this.numericUpDown_Go_To);
            this.groupBox_Go_To.Controls.Add(this.label_Course);
            this.groupBox_Go_To.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.groupBox_Go_To.Location = new System.Drawing.Point(6, 130);
            this.groupBox_Go_To.Name = "groupBox_Go_To";
            this.groupBox_Go_To.Size = new System.Drawing.Size(234, 45);
            this.groupBox_Go_To.TabIndex = 64;
            this.groupBox_Go_To.TabStop = false;
            this.groupBox_Go_To.Text = "Go To";
            this.groupBox_Go_To.Enter += new System.EventHandler(this.groupBox_Go_To_Enter);
            // 
            // pictureBox_GoTo
            // 
            this.pictureBox_GoTo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_GoTo.Image = global::CNC_Axis_Component.Properties.Resources.GoTo;
            this.pictureBox_GoTo.Location = new System.Drawing.Point(199, 11);
            this.pictureBox_GoTo.Name = "pictureBox_GoTo";
            this.pictureBox_GoTo.Size = new System.Drawing.Size(28, 28);
            this.pictureBox_GoTo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_GoTo.TabIndex = 68;
            this.pictureBox_GoTo.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_GoTo, "Go to position!");
            this.pictureBox_GoTo.Click += new System.EventHandler(this.picBox_Go_Click);
            this.pictureBox_GoTo.MouseEnter += new System.EventHandler(this.General_Mouse_Enter);
            this.pictureBox_GoTo.MouseLeave += new System.EventHandler(this.General_Mouse_Leave);
            // 
            // label_Axis
            // 
            this.label_Axis.AutoSize = true;
            this.label_Axis.Location = new System.Drawing.Point(2, 19);
            this.label_Axis.Name = "label_Axis";
            this.label_Axis.Size = new System.Drawing.Size(39, 13);
            this.label_Axis.TabIndex = 44;
            this.label_Axis.Text = "X (mm)";
            // 
            // numericUpDown_Go_To
            // 
            this.numericUpDown_Go_To.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.numericUpDown_Go_To.DecimalPlaces = 2;
            this.numericUpDown_Go_To.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_Go_To.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.numericUpDown_Go_To.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_Go_To.Location = new System.Drawing.Point(83, 17);
            this.numericUpDown_Go_To.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown_Go_To.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.numericUpDown_Go_To.Name = "numericUpDown_Go_To";
            this.numericUpDown_Go_To.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown_Go_To.TabIndex = 45;
            // 
            // label_Course
            // 
            this.label_Course.AutoSize = true;
            this.label_Course.Location = new System.Drawing.Point(133, 19);
            this.label_Course.Name = "label_Course";
            this.label_Course.Size = new System.Drawing.Size(61, 13);
            this.label_Course.TabIndex = 46;
            this.label_Course.Text = "(-100...100)";
            // 
            // pictureBox_Error
            // 
            this.pictureBox_Error.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Error.Image = global::CNC_Axis_Component.Properties.Resources.Error;
            this.pictureBox_Error.Location = new System.Drawing.Point(109, 49);
            this.pictureBox_Error.Name = "pictureBox_Error";
            this.pictureBox_Error.Size = new System.Drawing.Size(30, 30);
            this.pictureBox_Error.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Error.TabIndex = 63;
            this.pictureBox_Error.TabStop = false;
            this.pictureBox_Error.Visible = false;
            this.pictureBox_Error.Click += new System.EventHandler(this.picBox_Cleear_Errors_Click);
            this.pictureBox_Error.MouseEnter += new System.EventHandler(this.General_Mouse_Enter);
            this.pictureBox_Error.MouseLeave += new System.EventHandler(this.General_Mouse_Leave);
            // 
            // pictureBox_Center
            // 
            this.pictureBox_Center.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Center.Image = global::CNC_Axis_Component.Properties.Resources.Center;
            this.pictureBox_Center.Location = new System.Drawing.Point(38, 49);
            this.pictureBox_Center.Name = "pictureBox_Center";
            this.pictureBox_Center.Size = new System.Drawing.Size(30, 30);
            this.pictureBox_Center.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Center.TabIndex = 62;
            this.pictureBox_Center.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Center, "Go to center position!");
            this.pictureBox_Center.Click += new System.EventHandler(this.picBox_Go_To_Center_Click);
            this.pictureBox_Center.MouseEnter += new System.EventHandler(this.General_Mouse_Enter);
            this.pictureBox_Center.MouseLeave += new System.EventHandler(this.General_Mouse_Leave);
            // 
            // pictureBox_Home
            // 
            this.pictureBox_Home.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Home.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Home.Image")));
            this.pictureBox_Home.Location = new System.Drawing.Point(6, 49);
            this.pictureBox_Home.Name = "pictureBox_Home";
            this.pictureBox_Home.Size = new System.Drawing.Size(30, 30);
            this.pictureBox_Home.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Home.TabIndex = 61;
            this.pictureBox_Home.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Home, "Go to home position!");
            this.pictureBox_Home.Click += new System.EventHandler(this.picBox_Go_Home_Click);
            this.pictureBox_Home.MouseEnter += new System.EventHandler(this.General_Mouse_Enter);
            this.pictureBox_Home.MouseLeave += new System.EventHandler(this.General_Mouse_Leave);
            // 
            // label_Current_Position
            // 
            this.label_Current_Position.AutoSize = true;
            this.label_Current_Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Current_Position.Location = new System.Drawing.Point(6, 17);
            this.label_Current_Position.Name = "label_Current_Position";
            this.label_Current_Position.Size = new System.Drawing.Size(91, 24);
            this.label_Current_Position.TabIndex = 51;
            this.label_Current_Position.Text = "X (mm) :";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 125;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.toolTip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // Axis_Component
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.groupBox1);
            this.Name = "Axis_Component";
            this.Size = new System.Drawing.Size(246, 230);
            this.Load += new System.EventHandler(this.Axis_Component_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Nodge_Forward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Nodge_Backward)).EndInit();
            this.groupBox_Step.ResumeLayout(false);
            this.groupBox_Step.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step_Forward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Step)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step_Backward)).EndInit();
            this.groupBox_Go_To.ResumeLayout(false);
            this.groupBox_Go_To.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_GoTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Go_To)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Error)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Center)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Home)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_Current_Position;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox_Center;
        private System.Windows.Forms.PictureBox pictureBox_Home;
        private System.Windows.Forms.PictureBox pictureBox_Error;
        private System.Windows.Forms.GroupBox groupBox_Step;
        private System.Windows.Forms.NumericUpDown numericUpDown_Step;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox_Go_To;
        private System.Windows.Forms.Label label_Axis;
        private System.Windows.Forms.NumericUpDown numericUpDown_Go_To;
        private System.Windows.Forms.Label label_Course;
        private System.Windows.Forms.PictureBox pictureBox_Nodge_Backward;
        private System.Windows.Forms.PictureBox pictureBox_Nodge_Forward;
        private System.Windows.Forms.PictureBox pictureBox_GoTo;
        private System.Windows.Forms.PictureBox pictureBox_Step_Backward;
        private System.Windows.Forms.PictureBox pictureBox_Step_Forward;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

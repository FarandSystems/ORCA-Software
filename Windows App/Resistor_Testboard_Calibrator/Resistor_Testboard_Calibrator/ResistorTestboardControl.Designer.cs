namespace Resistor_Testboard_Calibrator
{
    partial class Resistor_Board_Calibrator_2018
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxCalibration = new System.Windows.Forms.GroupBox();
            this.label_Operating_Range = new System.Windows.Forms.Label();
            this.radioButton_Auto = new System.Windows.Forms.RadioButton();
            this.radioButton_HighRange = new System.Windows.Forms.RadioButton();
            this.radioButton_LowRange = new System.Windows.Forms.RadioButton();
            this.label_Calibrated_Value_Display = new System.Windows.Forms.Label();
            this.label_Device_Serial_No = new System.Windows.Forms.Label();
            this.buttonOpenCalFile = new System.Windows.Forms.Button();
            this.buttonSaveCalFile = new System.Windows.Forms.Button();
            this.labelMeasuredValueHex = new System.Windows.Forms.Label();
            this.textBoxMeasuredLevels = new System.Windows.Forms.TextBox();
            this.label_Measured_Levels = new System.Windows.Forms.Label();
            this.buttonGenerate_Calibration_File = new System.Windows.Forms.Button();
            this.textBox_Calibration_File = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Device_Serial_No = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.temperature_Calibrator_High = new Resistor_Calibrator_971124.Resistor_Calibrator2018();
            this.temperature_Calibrator_Low = new Resistor_Calibrator_971124.Resistor_Calibrator2018();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButton_UltraLowRange = new System.Windows.Forms.RadioButton();
            this.groupBoxCalibration.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Rated";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(73, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Real";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(117, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Measured Levels";
            // 
            // groupBoxCalibration
            // 
            this.groupBoxCalibration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.groupBoxCalibration.Controls.Add(this.radioButton_UltraLowRange);
            this.groupBoxCalibration.Controls.Add(this.label_Operating_Range);
            this.groupBoxCalibration.Controls.Add(this.radioButton_Auto);
            this.groupBoxCalibration.Controls.Add(this.radioButton_HighRange);
            this.groupBoxCalibration.Controls.Add(this.radioButton_LowRange);
            this.groupBoxCalibration.ForeColor = System.Drawing.Color.White;
            this.groupBoxCalibration.Location = new System.Drawing.Point(18, 26);
            this.groupBoxCalibration.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxCalibration.Name = "groupBoxCalibration";
            this.groupBoxCalibration.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxCalibration.Size = new System.Drawing.Size(554, 38);
            this.groupBoxCalibration.TabIndex = 39;
            this.groupBoxCalibration.TabStop = false;
            this.groupBoxCalibration.Text = "Operatnig Range";
            // 
            // label_Operating_Range
            // 
            this.label_Operating_Range.AutoSize = true;
            this.label_Operating_Range.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Operating_Range.ForeColor = System.Drawing.Color.Gold;
            this.label_Operating_Range.Location = new System.Drawing.Point(489, 12);
            this.label_Operating_Range.Name = "label_Operating_Range";
            this.label_Operating_Range.Size = new System.Drawing.Size(47, 20);
            this.label_Operating_Range.TabIndex = 36;
            this.label_Operating_Range.Text = "Auto";
            // 
            // radioButton_Auto
            // 
            this.radioButton_Auto.AutoSize = true;
            this.radioButton_Auto.Checked = true;
            this.radioButton_Auto.Location = new System.Drawing.Point(15, 15);
            this.radioButton_Auto.Name = "radioButton_Auto";
            this.radioButton_Auto.Size = new System.Drawing.Size(82, 17);
            this.radioButton_Auto.TabIndex = 35;
            this.radioButton_Auto.TabStop = true;
            this.radioButton_Auto.Text = "Auto Range";
            this.radioButton_Auto.UseVisualStyleBackColor = true;
            this.radioButton_Auto.Click += new System.EventHandler(this.radioButton_Auto_Click);
            // 
            // radioButton_HighRange
            // 
            this.radioButton_HighRange.AutoSize = true;
            this.radioButton_HighRange.Location = new System.Drawing.Point(297, 15);
            this.radioButton_HighRange.Name = "radioButton_HighRange";
            this.radioButton_HighRange.Size = new System.Drawing.Size(82, 17);
            this.radioButton_HighRange.TabIndex = 34;
            this.radioButton_HighRange.Text = "High Range";
            this.radioButton_HighRange.UseVisualStyleBackColor = true;
            this.radioButton_HighRange.Click += new System.EventHandler(this.radioButton_Auto_Click);
            // 
            // radioButton_LowRange
            // 
            this.radioButton_LowRange.AutoSize = true;
            this.radioButton_LowRange.Location = new System.Drawing.Point(211, 15);
            this.radioButton_LowRange.Name = "radioButton_LowRange";
            this.radioButton_LowRange.Size = new System.Drawing.Size(80, 17);
            this.radioButton_LowRange.TabIndex = 34;
            this.radioButton_LowRange.Text = "Low Range";
            this.radioButton_LowRange.UseVisualStyleBackColor = true;
            this.radioButton_LowRange.Click += new System.EventHandler(this.radioButton_Auto_Click);
            // 
            // label_Calibrated_Value_Display
            // 
            this.label_Calibrated_Value_Display.AutoSize = true;
            this.label_Calibrated_Value_Display.BackColor = System.Drawing.Color.Black;
            this.label_Calibrated_Value_Display.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Calibrated_Value_Display.ForeColor = System.Drawing.Color.White;
            this.label_Calibrated_Value_Display.Location = new System.Drawing.Point(601, 38);
            this.label_Calibrated_Value_Display.Name = "label_Calibrated_Value_Display";
            this.label_Calibrated_Value_Display.Size = new System.Drawing.Size(103, 22);
            this.label_Calibrated_Value_Display.TabIndex = 45;
            this.label_Calibrated_Value_Display.Text = "999999.99";
            // 
            // label_Device_Serial_No
            // 
            this.label_Device_Serial_No.AutoSize = true;
            this.label_Device_Serial_No.ForeColor = System.Drawing.Color.White;
            this.label_Device_Serial_No.Location = new System.Drawing.Point(22, 511);
            this.label_Device_Serial_No.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Device_Serial_No.Name = "label_Device_Serial_No";
            this.label_Device_Serial_No.Size = new System.Drawing.Size(93, 13);
            this.label_Device_Serial_No.TabIndex = 44;
            this.label_Device_Serial_No.Text = "Device Serial No.:";
            // 
            // buttonOpenCalFile
            // 
            this.buttonOpenCalFile.BackgroundImage = global::Resistor_Testboard_Calibrator.Properties.Resources.Open;
            this.buttonOpenCalFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonOpenCalFile.FlatAppearance.BorderSize = 0;
            this.buttonOpenCalFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenCalFile.Location = new System.Drawing.Point(144, 527);
            this.buttonOpenCalFile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOpenCalFile.Name = "buttonOpenCalFile";
            this.buttonOpenCalFile.Size = new System.Drawing.Size(20, 20);
            this.buttonOpenCalFile.TabIndex = 42;
            this.buttonOpenCalFile.UseVisualStyleBackColor = true;
            this.buttonOpenCalFile.Click += new System.EventHandler(this.buttonOpenCalFile_Click);
            // 
            // buttonSaveCalFile
            // 
            this.buttonSaveCalFile.BackgroundImage = global::Resistor_Testboard_Calibrator.Properties.Resources.Save;
            this.buttonSaveCalFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSaveCalFile.FlatAppearance.BorderSize = 0;
            this.buttonSaveCalFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveCalFile.Location = new System.Drawing.Point(168, 527);
            this.buttonSaveCalFile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveCalFile.Name = "buttonSaveCalFile";
            this.buttonSaveCalFile.Size = new System.Drawing.Size(20, 20);
            this.buttonSaveCalFile.TabIndex = 43;
            this.buttonSaveCalFile.UseVisualStyleBackColor = true;
            this.buttonSaveCalFile.Click += new System.EventHandler(this.buttonSaveCalFile_Click);
            // 
            // labelMeasuredValueHex
            // 
            this.labelMeasuredValueHex.AutoSize = true;
            this.labelMeasuredValueHex.ForeColor = System.Drawing.Color.White;
            this.labelMeasuredValueHex.Location = new System.Drawing.Point(676, 9);
            this.labelMeasuredValueHex.Name = "labelMeasuredValueHex";
            this.labelMeasuredValueHex.Size = new System.Drawing.Size(38, 13);
            this.labelMeasuredValueHex.TabIndex = 41;
            this.labelMeasuredValueHex.Text = "... Hex";
            // 
            // textBoxMeasuredLevels
            // 
            this.textBoxMeasuredLevels.Location = new System.Drawing.Point(605, 5);
            this.textBoxMeasuredLevels.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMeasuredLevels.Name = "textBoxMeasuredLevels";
            this.textBoxMeasuredLevels.Size = new System.Drawing.Size(66, 20);
            this.textBoxMeasuredLevels.TabIndex = 40;
            this.textBoxMeasuredLevels.Text = "1234000";
            // 
            // label_Measured_Levels
            // 
            this.label_Measured_Levels.AutoSize = true;
            this.label_Measured_Levels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Measured_Levels.Location = new System.Drawing.Point(490, 9);
            this.label_Measured_Levels.Name = "label_Measured_Levels";
            this.label_Measured_Levels.Size = new System.Drawing.Size(110, 13);
            this.label_Measured_Levels.TabIndex = 38;
            this.label_Measured_Levels.Text = "Measuered Levels";
            // 
            // buttonGenerate_Calibration_File
            // 
            this.buttonGenerate_Calibration_File.BackColor = System.Drawing.Color.Gray;
            this.buttonGenerate_Calibration_File.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGenerate_Calibration_File.Location = new System.Drawing.Point(26, 553);
            this.buttonGenerate_Calibration_File.Name = "buttonGenerate_Calibration_File";
            this.buttonGenerate_Calibration_File.Size = new System.Drawing.Size(164, 22);
            this.buttonGenerate_Calibration_File.TabIndex = 46;
            this.buttonGenerate_Calibration_File.Text = "Generate Calibration File";
            this.buttonGenerate_Calibration_File.UseVisualStyleBackColor = false;
            this.buttonGenerate_Calibration_File.Click += new System.EventHandler(this.buttonGenerate_Calibration_File_Click);
            // 
            // textBox_Calibration_File
            // 
            this.textBox_Calibration_File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_Calibration_File.Location = new System.Drawing.Point(196, 527);
            this.textBox_Calibration_File.Multiline = true;
            this.textBox_Calibration_File.Name = "textBox_Calibration_File";
            this.textBox_Calibration_File.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Calibration_File.Size = new System.Drawing.Size(506, 105);
            this.textBox_Calibration_File.TabIndex = 47;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(660, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Points";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.YellowGreen;
            this.label5.Location = new System.Drawing.Point(15, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(226, 16);
            this.label5.TabIndex = 48;
            this.label5.Text = "Resistor Test Board Calibration";
            // 
            // textBox_Device_Serial_No
            // 
            this.textBox_Device_Serial_No.Location = new System.Drawing.Point(26, 527);
            this.textBox_Device_Serial_No.Name = "textBox_Device_Serial_No";
            this.textBox_Device_Serial_No.Size = new System.Drawing.Size(89, 20);
            this.textBox_Device_Serial_No.TabIndex = 49;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // temperature_Calibrator_High
            // 
            this.temperature_Calibrator_High.Average_Lavel = 0D;
            this.temperature_Calibrator_High.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.temperature_Calibrator_High.Location = new System.Drawing.Point(12, 471);
            this.temperature_Calibrator_High.Measured_Level = 0D;
            this.temperature_Calibrator_High.Name = "temperature_Calibrator_High";
            this.temperature_Calibrator_High.Operating_Range = "High";
            this.temperature_Calibrator_High.Physical_Value = 30D;
            this.temperature_Calibrator_High.Point_Count = 0;
            this.temperature_Calibrator_High.Rated_Value = "Temp";
            this.temperature_Calibrator_High.Size = new System.Drawing.Size(690, 26);
            this.temperature_Calibrator_High.TabIndex = 50;
            // 
            // temperature_Calibrator_Low
            // 
            this.temperature_Calibrator_Low.Average_Lavel = 0D;
            this.temperature_Calibrator_Low.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.temperature_Calibrator_Low.Location = new System.Drawing.Point(12, 450);
            this.temperature_Calibrator_Low.Measured_Level = 0D;
            this.temperature_Calibrator_Low.Name = "temperature_Calibrator_Low";
            this.temperature_Calibrator_Low.Operating_Range = "Low";
            this.temperature_Calibrator_Low.Physical_Value = 15D;
            this.temperature_Calibrator_Low.Point_Count = 0;
            this.temperature_Calibrator_Low.Rated_Value = "Temp";
            this.temperature_Calibrator_Low.Size = new System.Drawing.Size(690, 26);
            this.temperature_Calibrator_Low.TabIndex = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Coral;
            this.label6.Location = new System.Drawing.Point(15, 432);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(176, 16);
            this.label6.TabIndex = 51;
            this.label6.Text = "Temperature Calibration";
            // 
            // radioButton_UltraLowRange
            // 
            this.radioButton_UltraLowRange.AutoSize = true;
            this.radioButton_UltraLowRange.Location = new System.Drawing.Point(103, 15);
            this.radioButton_UltraLowRange.Name = "radioButton_UltraLowRange";
            this.radioButton_UltraLowRange.Size = new System.Drawing.Size(105, 17);
            this.radioButton_UltraLowRange.TabIndex = 37;
            this.radioButton_UltraLowRange.Text = "Ultra Low Range";
            this.radioButton_UltraLowRange.UseVisualStyleBackColor = true;
            this.radioButton_UltraLowRange.Click += new System.EventHandler(this.radioButton_Auto_Click);
            // 
            // Resistor_Board_Calibrator_2018
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.label6);
            this.Controls.Add(this.temperature_Calibrator_Low);
            this.Controls.Add(this.temperature_Calibrator_High);
            this.Controls.Add(this.textBox_Device_Serial_No);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_Calibration_File);
            this.Controls.Add(this.buttonGenerate_Calibration_File);
            this.Controls.Add(this.groupBoxCalibration);
            this.Controls.Add(this.label_Calibrated_Value_Display);
            this.Controls.Add(this.label_Device_Serial_No);
            this.Controls.Add(this.buttonOpenCalFile);
            this.Controls.Add(this.buttonSaveCalFile);
            this.Controls.Add(this.labelMeasuredValueHex);
            this.Controls.Add(this.textBoxMeasuredLevels);
            this.Controls.Add(this.label_Measured_Levels);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Resistor_Board_Calibrator_2018";
            this.Size = new System.Drawing.Size(722, 638);
            this.groupBoxCalibration.ResumeLayout(false);
            this.groupBoxCalibration.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxCalibration;
        private System.Windows.Forms.RadioButton radioButton_Auto;
        private System.Windows.Forms.RadioButton radioButton_HighRange;
        private System.Windows.Forms.RadioButton radioButton_LowRange;
        private System.Windows.Forms.Label label_Calibrated_Value_Display;
        private System.Windows.Forms.Label label_Device_Serial_No;
        private System.Windows.Forms.Button buttonOpenCalFile;
        private System.Windows.Forms.Button buttonSaveCalFile;
        private System.Windows.Forms.Label labelMeasuredValueHex;
        private System.Windows.Forms.TextBox textBoxMeasuredLevels;
        private System.Windows.Forms.Label label_Measured_Levels;
        private System.Windows.Forms.Button buttonGenerate_Calibration_File;
        private System.Windows.Forms.TextBox textBox_Calibration_File;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_Operating_Range;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Resistor_Calibrator_971124.Resistor_Calibrator2018 temperature_Calibrator_High;
        private Resistor_Calibrator_971124.Resistor_Calibrator2018 temperature_Calibrator_Low;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox textBox_Device_Serial_No;
        private System.Windows.Forms.RadioButton radioButton_UltraLowRange;
    }
}

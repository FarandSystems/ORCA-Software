using System.Windows.Forms.DataVisualization.Charting;
namespace Movement_Functions_Control
{
    partial class SignalGeneratorControl
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.GroupBox groupParams;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericAmplitude;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericFrequency;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericPhase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericOffset;

        private System.Windows.Forms.GroupBox groupSampling;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericSampleRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericDuration;

        private System.Windows.Forms.GroupBox groupType;
        private System.Windows.Forms.RadioButton radioSine;
        private System.Windows.Forms.RadioButton radioCosine;

        private System.Windows.Forms.ComboBox comboPreset;
        private System.Windows.Forms.Label labelPreset;

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnReset;

        /// <summary> Clean up any resources being used. </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.groupParams = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericAmplitude = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericFrequency = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericPhase = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericOffset = new System.Windows.Forms.NumericUpDown();
            this.groupSampling = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericSampleRate = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericDuration = new System.Windows.Forms.NumericUpDown();
            this.groupType = new System.Windows.Forms.GroupBox();
            this.radioSine = new System.Windows.Forms.RadioButton();
            this.radioCosine = new System.Windows.Forms.RadioButton();
            this.comboPreset = new System.Windows.Forms.ComboBox();
            this.labelPreset = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericAmplitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPhase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericOffset)).BeginInit();
            this.groupSampling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSampleRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).BeginInit();
            this.groupType.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupParams
            // 
            this.groupParams.Controls.Add(this.label1);
            this.groupParams.Controls.Add(this.numericAmplitude);
            this.groupParams.Controls.Add(this.label2);
            this.groupParams.Controls.Add(this.numericFrequency);
            this.groupParams.Controls.Add(this.label3);
            this.groupParams.Controls.Add(this.numericPhase);
            this.groupParams.Controls.Add(this.label4);
            this.groupParams.Controls.Add(this.numericOffset);
            this.groupParams.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.groupParams.Location = new System.Drawing.Point(6, 3);
            this.groupParams.Name = "groupParams";
            this.groupParams.Size = new System.Drawing.Size(226, 166);
            this.groupParams.TabIndex = 0;
            this.groupParams.TabStop = false;
            this.groupParams.Text = "Signal Parameters";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Amplitude";
            // 
            // numericAmplitude
            // 
            this.numericAmplitude.DecimalPlaces = 4;
            this.numericAmplitude.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericAmplitude.Location = new System.Drawing.Point(130, 23);
            this.numericAmplitude.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericAmplitude.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numericAmplitude.Name = "numericAmplitude";
            this.numericAmplitude.Size = new System.Drawing.Size(90, 20);
            this.numericAmplitude.TabIndex = 1;
            this.numericAmplitude.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Frequency (Hz)";
            // 
            // numericFrequency
            // 
            this.numericFrequency.DecimalPlaces = 3;
            this.numericFrequency.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericFrequency.Location = new System.Drawing.Point(130, 96);
            this.numericFrequency.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericFrequency.Name = "numericFrequency";
            this.numericFrequency.Size = new System.Drawing.Size(90, 20);
            this.numericFrequency.TabIndex = 3;
            this.numericFrequency.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Phase (deg)";
            // 
            // numericPhase
            // 
            this.numericPhase.DecimalPlaces = 2;
            this.numericPhase.Location = new System.Drawing.Point(130, 135);
            this.numericPhase.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericPhase.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numericPhase.Name = "numericPhase";
            this.numericPhase.Size = new System.Drawing.Size(90, 20);
            this.numericPhase.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "DC Offset";
            // 
            // numericOffset
            // 
            this.numericOffset.DecimalPlaces = 4;
            this.numericOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericOffset.Location = new System.Drawing.Point(130, 59);
            this.numericOffset.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericOffset.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numericOffset.Name = "numericOffset";
            this.numericOffset.Size = new System.Drawing.Size(90, 20);
            this.numericOffset.TabIndex = 7;
            // 
            // groupSampling
            // 
            this.groupSampling.Controls.Add(this.label5);
            this.groupSampling.Controls.Add(this.numericSampleRate);
            this.groupSampling.Controls.Add(this.label6);
            this.groupSampling.Controls.Add(this.numericDuration);
            this.groupSampling.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.groupSampling.Location = new System.Drawing.Point(6, 174);
            this.groupSampling.Name = "groupSampling";
            this.groupSampling.Size = new System.Drawing.Size(226, 121);
            this.groupSampling.TabIndex = 1;
            this.groupSampling.TabStop = false;
            this.groupSampling.Text = "Sampling";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Sample Rate (Hz)";
            // 
            // numericSampleRate
            // 
            this.numericSampleRate.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericSampleRate.Location = new System.Drawing.Point(130, 26);
            this.numericSampleRate.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericSampleRate.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericSampleRate.Name = "numericSampleRate";
            this.numericSampleRate.Size = new System.Drawing.Size(90, 20);
            this.numericSampleRate.TabIndex = 1;
            this.numericSampleRate.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Duration (s)";
            // 
            // numericDuration
            // 
            this.numericDuration.DecimalPlaces = 4;
            this.numericDuration.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericDuration.Location = new System.Drawing.Point(130, 67);
            this.numericDuration.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numericDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericDuration.Name = "numericDuration";
            this.numericDuration.Size = new System.Drawing.Size(90, 20);
            this.numericDuration.TabIndex = 3;
            this.numericDuration.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // groupType
            // 
            this.groupType.Controls.Add(this.radioSine);
            this.groupType.Controls.Add(this.radioCosine);
            this.groupType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.groupType.Location = new System.Drawing.Point(6, 367);
            this.groupType.Name = "groupType";
            this.groupType.Size = new System.Drawing.Size(226, 60);
            this.groupType.TabIndex = 2;
            this.groupType.TabStop = false;
            this.groupType.Text = "Waveform";
            // 
            // radioSine
            // 
            this.radioSine.AutoSize = true;
            this.radioSine.Checked = true;
            this.radioSine.Location = new System.Drawing.Point(15, 25);
            this.radioSine.Name = "radioSine";
            this.radioSine.Size = new System.Drawing.Size(46, 17);
            this.radioSine.TabIndex = 0;
            this.radioSine.TabStop = true;
            this.radioSine.Text = "Sine";
            // 
            // radioCosine
            // 
            this.radioCosine.AutoSize = true;
            this.radioCosine.Location = new System.Drawing.Point(160, 25);
            this.radioCosine.Name = "radioCosine";
            this.radioCosine.Size = new System.Drawing.Size(57, 17);
            this.radioCosine.TabIndex = 1;
            this.radioCosine.Text = "Cosine";
            // 
            // comboPreset
            // 
            this.comboPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPreset.Location = new System.Drawing.Point(61, 497);
            this.comboPreset.Name = "comboPreset";
            this.comboPreset.Size = new System.Drawing.Size(165, 21);
            this.comboPreset.TabIndex = 4;
            // 
            // labelPreset
            // 
            this.labelPreset.AutoSize = true;
            this.labelPreset.Location = new System.Drawing.Point(13, 501);
            this.labelPreset.Name = "labelPreset";
            this.labelPreset.Size = new System.Drawing.Size(37, 13);
            this.labelPreset.TabIndex = 3;
            this.labelPreset.Text = "Preset";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(13, 301);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(65, 27);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Generate";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(166, 301);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(60, 27);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            // 
            // SignalGeneratorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.groupParams);
            this.Controls.Add(this.groupSampling);
            this.Controls.Add(this.groupType);
            this.Controls.Add(this.labelPreset);
            this.Controls.Add(this.comboPreset);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnReset);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.Name = "SignalGeneratorControl";
            this.Size = new System.Drawing.Size(246, 550);
            this.groupParams.ResumeLayout(false);
            this.groupParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericAmplitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPhase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericOffset)).EndInit();
            this.groupSampling.ResumeLayout(false);
            this.groupSampling.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSampleRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).EndInit();
            this.groupType.ResumeLayout(false);
            this.groupType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}

namespace Visualizer_Test_App
{
    partial class Form1
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
            this.visualizer_Component1 = new Visualizer_Component.Visualizer_Component();
            this.SuspendLayout();
            // 
            // visualizer_Component1
            // 
            this.visualizer_Component1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.visualizer_Component1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualizer_Component1.Location = new System.Drawing.Point(0, 0);
            this.visualizer_Component1.Name = "visualizer_Component1";
            this.visualizer_Component1.Size = new System.Drawing.Size(806, 535);
            this.visualizer_Component1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 535);
            this.Controls.Add(this.visualizer_Component1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Visualizer_Component.Visualizer_Component visualizer_Component1;
    }
}


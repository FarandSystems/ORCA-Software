namespace Visualizer_Component
{
    partial class Visualizer_Component
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
            this.tabPage_3DView = new ReaLTaiizor.Child.Metro.MetroTabPage();
            this.tabPage_Graphs = new ReaLTaiizor.Child.Metro.MetroTabPage();
            this.tabPage_System_Messages = new ReaLTaiizor.Child.Metro.MetroTabPage();
            this.metroTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.AnimateEasingType = ReaLTaiizor.Enum.Metro.EasingType.CubeOut;
            this.metroTabControl1.AnimateTime = 200;
            this.metroTabControl1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.metroTabControl1.Controls.Add(this.tabPage_3DView);
            this.metroTabControl1.Controls.Add(this.tabPage_Graphs);
            this.metroTabControl1.Controls.Add(this.tabPage_System_Messages);
            this.metroTabControl1.ControlsVisible = true;
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.IsDerivedStyle = true;
            this.metroTabControl1.ItemSize = new System.Drawing.Size(100, 38);
            this.metroTabControl1.Location = new System.Drawing.Point(0, 0);
            this.metroTabControl1.MCursor = System.Windows.Forms.Cursors.Hand;
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroTabControl1.Size = new System.Drawing.Size(866, 1145);
            this.metroTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.metroTabControl1.Speed = 100;
            this.metroTabControl1.Style = ReaLTaiizor.Enum.Metro.Style.Custom;
            this.metroTabControl1.StyleManager = null;
            this.metroTabControl1.TabIndex = 0;
            this.metroTabControl1.TabStyle = ReaLTaiizor.Enum.Metro.TabStyle.Style2;
            this.metroTabControl1.ThemeAuthor = "Taiizor";
            this.metroTabControl1.ThemeName = "MetroDark";
            this.metroTabControl1.UnselectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            // 
            // tabPage_3DView
            // 
            this.tabPage_3DView.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage_3DView.Font = null;
            this.tabPage_3DView.ImageIndex = 0;
            this.tabPage_3DView.ImageKey = null;
            this.tabPage_3DView.IsDerivedStyle = true;
            this.tabPage_3DView.Location = new System.Drawing.Point(4, 42);
            this.tabPage_3DView.Name = "tabPage_3DView";
            this.tabPage_3DView.Size = new System.Drawing.Size(858, 1099);
            this.tabPage_3DView.Style = ReaLTaiizor.Enum.Metro.Style.Custom;
            this.tabPage_3DView.StyleManager = null;
            this.tabPage_3DView.TabIndex = 0;
            this.tabPage_3DView.Text = "3D View";
            this.tabPage_3DView.ThemeAuthor = "Taiizor";
            this.tabPage_3DView.ThemeName = "MetroLight";
            this.tabPage_3DView.ToolTipText = null;
            // 
            // tabPage_Graphs
            // 
            this.tabPage_Graphs.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage_Graphs.Font = null;
            this.tabPage_Graphs.ImageIndex = 0;
            this.tabPage_Graphs.ImageKey = null;
            this.tabPage_Graphs.IsDerivedStyle = true;
            this.tabPage_Graphs.Location = new System.Drawing.Point(4, 42);
            this.tabPage_Graphs.Name = "tabPage_Graphs";
            this.tabPage_Graphs.Size = new System.Drawing.Size(858, 1099);
            this.tabPage_Graphs.Style = ReaLTaiizor.Enum.Metro.Style.Custom;
            this.tabPage_Graphs.StyleManager = null;
            this.tabPage_Graphs.TabIndex = 1;
            this.tabPage_Graphs.Text = "Graphs";
            this.tabPage_Graphs.ThemeAuthor = "Taiizor";
            this.tabPage_Graphs.ThemeName = "MetroLight";
            this.tabPage_Graphs.ToolTipText = null;
            // 
            // tabPage_System_Messages
            // 
            this.tabPage_System_Messages.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tabPage_System_Messages.Font = null;
            this.tabPage_System_Messages.ImageIndex = 0;
            this.tabPage_System_Messages.ImageKey = null;
            this.tabPage_System_Messages.IsDerivedStyle = true;
            this.tabPage_System_Messages.Location = new System.Drawing.Point(4, 42);
            this.tabPage_System_Messages.Name = "tabPage_System_Messages";
            this.tabPage_System_Messages.Size = new System.Drawing.Size(858, 1099);
            this.tabPage_System_Messages.Style = ReaLTaiizor.Enum.Metro.Style.Custom;
            this.tabPage_System_Messages.StyleManager = null;
            this.tabPage_System_Messages.TabIndex = 2;
            this.tabPage_System_Messages.Text = "System Messages";
            this.tabPage_System_Messages.ThemeAuthor = "Taiizor";
            this.tabPage_System_Messages.ThemeName = "MetroLight";
            this.tabPage_System_Messages.ToolTipText = null;
            // 
            // Visualizer_Component
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.metroTabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Visualizer_Component";
            this.Size = new System.Drawing.Size(866, 1145);
            this.metroTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MetroTabControl metroTabControl1;
        private ReaLTaiizor.Child.Metro.MetroTabPage tabPage_3DView;
        private ReaLTaiizor.Child.Metro.MetroTabPage tabPage_Graphs;
        private ReaLTaiizor.Child.Metro.MetroTabPage tabPage_System_Messages;
    }
}

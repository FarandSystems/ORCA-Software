using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Viewport3DControl;
using System.Numerics;

namespace Visualizer_Component
{
    public partial class Visualizer_Component: UserControl
    {
        private Viewport3D viewport;

        private bool start_3D_Test_Flag = false;
        public bool Start_3D_Test_Flag
        {
            get { return start_3D_Test_Flag; }
            set 
            {
                if (value)
                {
                    start_3D_Test_Flag = value;
                    Start_3D_Test();
                }
            }
        }
        public Visualizer_Component()
        {
            InitializeComponent();

            viewport = new Viewport3D { Dock = DockStyle.Fill };
            tabPage_3DView.Controls.Add(viewport);


            // Example:
            richTextBox_System_Messages.Editor.Text = "Simulator ON\nWaveLogger ON\nIMU is OK";
            richTextBox_System_Messages.Editor.Select(richTextBox_System_Messages.Editor.TextLength, 0);
        }

        private void Visualizer_Component_Load(object sender, EventArgs e)
        {
            //metroTabControl1.TabPages[0] = tabPage_3DView;
            //metroTabControl1.TabPages[1] = tabPage_Graphs;
            //metroTabControl1.TabPages[2] = tabPage_System_Messages;
            //metroTabControl1.SelectedTab = tabPage_3DView;
        }

        private void Start_3D_Test()
        {

            // Animate triangle to prove real-time updates
            var t = new Timer { Interval = 16 };
            float time = 0f;
            t.Tick += (s, e) =>
            {
                time += 0.016f;

                // oscillating roll, pitch, and heave
                float roll = (float)Math.Sin(time) * 30f;   // ±30° roll
                float pitch = (float)Math.Cos(time) * 20f;   // ±20° pitch
                float yaw = 0f;                            // keep zero for now
                float heave = (float)Math.Sin(time) * 0.5f;  // ±0.5 units up/down

                viewport.SetPoseRPYHeave(roll, pitch, yaw, heave);
            };
            t.Start();

            viewport.FrameTo(Vector3.Zero, 4.0f);
        }
    }
}

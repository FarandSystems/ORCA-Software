using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORCA_Simulator_Application
{
    public partial class Main_Form : Form
    {
        private bool blinkOn;
        public Main_Form()
        {
            InitializeComponent();
        }

        private void pictureBox_Reconnect_Click(object sender, EventArgs e)
        {
            timer_Connection_Demo.Start();
        }
        private void timer_Connection_Demo_Tick(object sender, EventArgs e)
        {
            blinkOn = !blinkOn;

            pictureBox_Controller_Connection.Image =
                blinkOn ? Properties.Resources.Connection_Ok : Properties.Resources.Connection_Idle;

            pictureBox_ORCA_Connection.Image =
                blinkOn ? Properties.Resources.Connection_Ok : Properties.Resources.Connection_Idle;
        }
    }
}

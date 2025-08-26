using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Command_Control
{
    public partial class Command_Control: UserControl
    {
        public Command_Control()
        {
            InitializeComponent();

            // Example:
            richTextBox_System_Messages.Editor.Text = "enable,True\nWaveLogger,True\nmove,0,0,0\ndelay,5\nmove,10,10,10\nenable,False\nWaveLogger,False";
            richTextBox_System_Messages.Editor.Select(richTextBox_System_Messages.Editor.TextLength, 0);
            richTextBox_System_Messages.BeginEditing();
        }
    }
}

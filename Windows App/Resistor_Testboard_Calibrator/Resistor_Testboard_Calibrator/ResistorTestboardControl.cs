using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Resistor_Calibrator_971124;
using System.IO;

namespace Resistor_Testboard_Calibrator
{

    
    public partial class Resistor_Board_Calibrator_2018: UserControl
    {
        string operating_Range = "Auto";
        double measured_Level = 0;
        double calibrated_Value = 0;
        double temperature_Level = 0;

        double CON_LOW_L1_G;
        double CON_LOW_L2_G;
        double CON_LOW_L3_G;
        double CON_LOW_L4_G;

        //// Concrete_LowRange_Line_Offset
        double CON_LOW_L1_O;
        double CON_LOW_L2_O;
        double CON_LOW_L3_O;
        double CON_LOW_L4_O;

        //// Concrete_HighRange_Line_Gain
        double CON_HIGH_L1_G;
        double CON_HIGH_L2_G;
        double CON_HIGH_L3_G;
        double CON_HIGH_L4_G;

        //// Concrete_HighRange_Line_Offset
        double CON_HIGH_L1_O;
        double CON_HIGH_L2_O;
        double CON_HIGH_L3_O;
        double CON_HIGH_L4_O;

        double TEMP_G;
        double TEMP_O;

        #region (* Properties *)
        public string Operating_Range
        {
            get
            {
                return operating_Range;
            }
            set
            {
                operating_Range = value;               
            }
        }

        public double Measured_Level
        {
            get
            {
                return measured_Level;
            }
            set
            {
                measured_Level = value;
                textBoxMeasuredLevels.Text = measured_Level.ToString();
                Update_Measured_Level();
            }
        }

        public double Temperature_Level
        {
            get
            {
                return temperature_Level;
            }
            set
            {
                temperature_Level = value;
                temperature_Calibrator_Low.Measured_Level = temperature_Level;
                temperature_Calibrator_High.Measured_Level = temperature_Level;
            }
        }

        public double Calibrated_Value
        {
            get
            {
                return calibrated_Value;
            }
            set
            {
                calibrated_Value = value;
                label_Calibrated_Value_Display.Text = calibrated_Value.ToString("000.00");
            }
        }

        #endregion 

        #region (* Events *)
        public event EventHandler Operating_Range_Changed;
        #endregion

        Resistor_Calibrator2018[] resistor_Calibrator = new Resistor_Calibrator2018[16];
        public Resistor_Board_Calibrator_2018()
        {
            InitializeComponent();
            Generate_Resistor_Calibrators();
        }

        private void Update_Measured_Level()
        {
            for (int i = 0; i < resistor_Calibrator.Length; i++)
            {
                resistor_Calibrator[i].Measured_Level = measured_Level;
            }            
        }

        private void Generate_Resistor_Calibrators()
        {
            int left = 13;
            int verticalSpacing = 21;

            for (int i = 0; i < resistor_Calibrator.Length; i++)
            {
                resistor_Calibrator[i] = new Resistor_Calibrator2018();
                resistor_Calibrator[i].Left = left;
                resistor_Calibrator[i].Top = 83 + i*verticalSpacing;
                this.Controls.Add(resistor_Calibrator[i]);
                switch (i)
                { 
                    case 0:
                        resistor_Calibrator[i].Rated_Value = "1 Ohm";
                        resistor_Calibrator[i].Physical_Value = 0.966;
                        resistor_Calibrator[i].Operating_Range = "Low";
                        break;
                    case 1:
                        resistor_Calibrator[i].Rated_Value = "2 Ohm";
                        resistor_Calibrator[i].Physical_Value = 2.2;
                        resistor_Calibrator[i].Operating_Range = "Low";
                        break;
                    case 2:
                        resistor_Calibrator[i].Rated_Value = "5 Ohm";
                        resistor_Calibrator[i].Physical_Value = 5.09;
                        resistor_Calibrator[i].Operating_Range = "Low";
                        break;
                    case 3:
                        resistor_Calibrator[i].Rated_Value = "10 Ohm";
                        resistor_Calibrator[i].Physical_Value = 10.0;
                        resistor_Calibrator[i].Operating_Range = "Low";
                        break;
                    case 4:
                        resistor_Calibrator[i].Rated_Value = "20 Ohm";
                        resistor_Calibrator[i].Physical_Value = 20.8;
                        resistor_Calibrator[i].Operating_Range = "Low";
                        break;
                    case 5:
                        resistor_Calibrator[i].Rated_Value = "50 Ohm";
                        resistor_Calibrator[i].Physical_Value = 50.4;
                        resistor_Calibrator[i].Operating_Range = "Low";
                        break;
                    case 6:
                        resistor_Calibrator[i].Rated_Value = "100 Ohm";
                        resistor_Calibrator[i].Physical_Value = 100.00;
                        resistor_Calibrator[i].Operating_Range = "Low";
                        break;
                    case 7:
                        resistor_Calibrator[i].Rated_Value = "200 Ohm";
                        resistor_Calibrator[i].Physical_Value = 203;
                        resistor_Calibrator[i].Operating_Range = "Low";
                        break;
                    case 8:
                        resistor_Calibrator[i].Rated_Value = "200 Ohm";
                        resistor_Calibrator[i].Physical_Value = 203;
                        resistor_Calibrator[i].Operating_Range = "High";
                        break;
                    case 9:
                        resistor_Calibrator[i].Rated_Value = "500 Ohm";
                        resistor_Calibrator[i].Physical_Value = 500;
                        resistor_Calibrator[i].Operating_Range = "High";
                        break;
                    case 10:
                        resistor_Calibrator[i].Rated_Value = "1 kOhm";
                        resistor_Calibrator[i].Physical_Value = 998.9;
                        resistor_Calibrator[i].Operating_Range = "High";
                        break;
                    case 11:
                        resistor_Calibrator[i].Rated_Value = "2 kOhm";
                        resistor_Calibrator[i].Physical_Value = 1995;
                        resistor_Calibrator[i].Operating_Range = "High";
                        break;
                    case 12:
                        resistor_Calibrator[i].Rated_Value = "5 kOhm";
                        resistor_Calibrator[i].Physical_Value = 5138;
                        resistor_Calibrator[i].Operating_Range = "High";
                        break;
                    case 13:
                        resistor_Calibrator[i].Rated_Value = "10 kOhm";
                        resistor_Calibrator[i].Physical_Value = 9999;
                        resistor_Calibrator[i].Operating_Range = "High";
                        break;
                    case 14:
                        resistor_Calibrator[i].Rated_Value = "20 kOhm";
                        resistor_Calibrator[i].Physical_Value = 20070;
                        resistor_Calibrator[i].Operating_Range = "High";
                        break;
                    case 15:
                        resistor_Calibrator[i].Rated_Value = "50 kOhm";
                        resistor_Calibrator[i].Physical_Value = 52165;
                        resistor_Calibrator[i].Operating_Range = "High";
                        break;

                }
            }            
        }

        private void radioButton_Auto_Click(object sender, EventArgs e)
        {
            if (radioButton_Auto.Checked == true)
            {
                operating_Range = "Auto";
                label_Operating_Range.ForeColor = Color.Gold;
            }

            if (radioButton_UltraLowRange.Checked == true)
            {
                operating_Range = "Ultra Low";
                label_Operating_Range.ForeColor = Color.Navy;
            }

            if (radioButton_LowRange.Checked == true)
            {
                operating_Range = "Low";
                label_Operating_Range.ForeColor = Color.LightBlue;
            }

            if (radioButton_HighRange.Checked == true)
            {
                operating_Range = "High";
                label_Operating_Range.ForeColor = Color.Tomato;
            }

          

            label_Operating_Range.Text = operating_Range;
            
            if (Operating_Range_Changed != null)
            {
                Operating_Range_Changed(null, null);
            }
        }

        private void buttonGenerate_Calibration_File_Click(object sender, EventArgs e)
        {
            bool points_Not_Complete = false;
            for(int i=0; i<resistor_Calibrator.Length; i++)
            {
                if(resistor_Calibrator[i].Point_Count == 0)
                {
                    points_Not_Complete = true;
                }
                
            }

            if(points_Not_Complete == true)
            {
                MessageBox.Show("None of the test resistors can have 0 calibrting points, Please add points!");
            }
            else
            {

                
                // Low Range
                // Values are amgnified by 1E4 to get easier numbers for calculation
                CON_LOW_L1_G = 1E4 * (resistor_Calibrator[1].Physical_Value - resistor_Calibrator[0].Physical_Value) / (resistor_Calibrator[1].Average_Lavel - resistor_Calibrator[0].Average_Lavel);
                CON_LOW_L1_O = 1E4 * resistor_Calibrator[1].Physical_Value - CON_LOW_L1_G * resistor_Calibrator[1].Average_Lavel;

                CON_LOW_L2_G = 1E4 * (resistor_Calibrator[3].Physical_Value - resistor_Calibrator[2].Physical_Value) / (resistor_Calibrator[3].Average_Lavel - resistor_Calibrator[2].Average_Lavel);
                CON_LOW_L2_O = 1E4 * resistor_Calibrator[3].Physical_Value - CON_LOW_L2_G * resistor_Calibrator[3].Average_Lavel;

                CON_LOW_L3_G = 1E4 * (resistor_Calibrator[5].Physical_Value - resistor_Calibrator[4].Physical_Value) / (resistor_Calibrator[5].Average_Lavel - resistor_Calibrator[4].Average_Lavel);
                CON_LOW_L3_O = 1E4 * resistor_Calibrator[5].Physical_Value - CON_LOW_L3_G * resistor_Calibrator[5].Average_Lavel;

                CON_LOW_L4_G = 1E4 * (resistor_Calibrator[7].Physical_Value - resistor_Calibrator[6].Physical_Value) / (resistor_Calibrator[7].Average_Lavel - resistor_Calibrator[6].Average_Lavel);
                CON_LOW_L4_O = 1E4 * resistor_Calibrator[7].Physical_Value - CON_LOW_L4_G * resistor_Calibrator[7].Average_Lavel;

                // High Range
                // Values are amgnified by 1E2 to get easier numbers for calculation
                CON_HIGH_L1_G = 1E2 * (resistor_Calibrator[9].Physical_Value - resistor_Calibrator[8].Physical_Value) / (resistor_Calibrator[9].Average_Lavel - resistor_Calibrator[8].Average_Lavel);
                CON_HIGH_L1_O = 1E2 * resistor_Calibrator[9].Physical_Value - CON_HIGH_L1_G * resistor_Calibrator[9].Average_Lavel;

                CON_HIGH_L2_G = 1E2 * (resistor_Calibrator[11].Physical_Value - resistor_Calibrator[10].Physical_Value) / (resistor_Calibrator[11].Average_Lavel - resistor_Calibrator[10].Average_Lavel);
                CON_HIGH_L2_O = 1E2 * resistor_Calibrator[11].Physical_Value - CON_HIGH_L2_G * resistor_Calibrator[11].Average_Lavel;

                CON_HIGH_L3_G = 1E2 * (resistor_Calibrator[13].Physical_Value - resistor_Calibrator[12].Physical_Value) / (resistor_Calibrator[13].Average_Lavel - resistor_Calibrator[12].Average_Lavel);
                CON_HIGH_L3_O = 1E2 * resistor_Calibrator[13].Physical_Value - CON_HIGH_L3_G * resistor_Calibrator[13].Average_Lavel;

                CON_HIGH_L4_G = 1E2 * (resistor_Calibrator[15].Physical_Value - resistor_Calibrator[14].Physical_Value) / (resistor_Calibrator[15].Average_Lavel - resistor_Calibrator[14].Average_Lavel);
                CON_HIGH_L4_O = 1E2 * resistor_Calibrator[15].Physical_Value - CON_HIGH_L4_G * resistor_Calibrator[15].Average_Lavel;
            }


            if (temperature_Calibrator_Low.Point_Count == 0 || temperature_Calibrator_High.Point_Count == 0)
            {
                MessageBox.Show("Temperature calibration ponits are missing. Default values for temperature  calibration will be generated!");
                TEMP_G =  (3.3 / 4095)/0.01; // 10mV/C, 3.3V reference voltage, 12 bit ADC
                TEMP_O = 0;                 
            }
            else
            {
                TEMP_G = (temperature_Calibrator_High.Physical_Value - temperature_Calibrator_Low.Physical_Value) / (temperature_Calibrator_High.Average_Lavel - temperature_Calibrator_Low.Average_Lavel);
                TEMP_O = temperature_Calibrator_High.Physical_Value - TEMP_G * temperature_Calibrator_High.Average_Lavel;
            }

            DateTime dt = DateTime.Now;
            // Generate the formatted report
            string s = "Calibration Report for FW411 \r\n";
            s += "Serial No.: " + textBox_Device_Serial_No.Text + "\r\n";
            s += "Date/Time: " + dt.ToString() + "\r\n\r\n";

            // Temperature Calibration Data Report
            s+= "//Temperature Calibration Coefficients:\r\n" +
                "#define TEMP_G" + "\t\t" + TEMP_G.ToString("000.000") + "\r\n" +
                "#define TEMP_O" + "\t\t" + TEMP_O.ToString("000.000") + "\r\n";

           
            s += "\r\nTemperature Calibration Data Points:\r\n";
            s += "Physical Value (C)\t" + "Measured Levels\r\n";
            s += temperature_Calibrator_Low.Physical_Value.ToString("000.00") + "\t" + temperature_Calibrator_Low.textBox_Measured_Levels.Text + "\r\n";
            s += temperature_Calibrator_High.Physical_Value.ToString("000.00") + "\t" + temperature_Calibrator_High.textBox_Measured_Levels.Text + "\r\n";

            // Resistance Calibration Data Report
            s += "\r\n\r\n//Resistance Calibration Coefficients:\r\n" +
            "\r\n// Concrete_LowRange_Line_Gain X10000\r\n" +
            "#define CON_LOW_L1_G" + "\t\t" + CON_LOW_L1_G.ToString("000.000") + "\r\n" +
            "#define CON_LOW_L2_G" + "\t\t" + CON_LOW_L2_G.ToString("000.000") + "\r\n" +
            "#define CON_LOW_L3_G" + "\t\t" + CON_LOW_L3_G.ToString("000.000") + "\r\n" +
            "#define CON_LOW_L4_G" + "\t\t" + CON_LOW_L4_G.ToString("000.000") + "\r\n" +

            "\r\n// Concrete_LowRange_Line_Offset X10000" + "\r\n" +
            "#define CON_LOW_L1_O" + "\t\t" + CON_LOW_L1_O.ToString("000.000") + "\r\n" +
            "#define CON_LOW_L2_O" + "\t\t" + CON_LOW_L2_O.ToString("000.000") + "\r\n" +
            "#define CON_LOW_L3_O" + "\t\t" + CON_LOW_L3_O.ToString("000.000") + "\r\n" +
            "#define CON_LOW_L4_O" + "\t\t" + CON_LOW_L4_O.ToString("000.000") + "\r\n" +

           "\r\n// Concrete_HighRange_Line_Gain X100\r\n" +
            "#define CON_HIGH_L1_G" + "\t\t" + CON_HIGH_L1_G.ToString("000.000") + "\r\n" +
            "#define CON_HIGH_L2_G" + "\t\t" + CON_HIGH_L2_G.ToString("000.000") + "\r\n" +
            "#define CON_HIGH_L3_G" + "\t\t" + CON_HIGH_L3_G.ToString("000.000") + "\r\n" +
            "#define CON_HIGH_L4_G" + "\t\t" + CON_HIGH_L4_G.ToString("000.000") + "\r\n" +

            "\r\n// Concrete_HighRange_Line_Offset X100" + "\r\n" +
            "#define CON_HIGH_L1_O" + "\t\t" + CON_HIGH_L1_O.ToString("000.000") + "\r\n" +
            "#define CON_HIGH_L2_O" + "\t\t" + CON_HIGH_L2_O.ToString("000.000") + "\r\n" +
            "#define CON_HIGH_L3_O" + "\t\t" + CON_HIGH_L3_O.ToString("000.000") + "\r\n" +
            "#define CON_HIGH_L4_O" + "\t\t" + CON_HIGH_L4_O.ToString("000.000") + "\r\n";
           
            // Resistance Calibration Data Report
            s += "\r\n\r\nResistance Calibration Data Points:\r\n";
            s += "Physical Value (Ohm)\t" + "Measured Levels\r\n";

            for(int i=0; i<resistor_Calibrator.Length; i++)
            {
                s += resistor_Calibrator[i].Physical_Value.ToString("000.00") + "\t" +  resistor_Calibrator[i].textBox_Measured_Levels.Text + "\r\n";
            }

            textBox_Calibration_File.Text = s;

        }

        private void buttonSaveCalFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "FW411_SN" + textBox_Device_Serial_No.Text + "_Calibration_File.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);                
                sw.Write(textBox_Calibration_File.Text);
                sw.Close();
            }
        }

        private void buttonOpenCalFile_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                textBox_Calibration_File.Text = sr.ReadToEnd();
                sr.Close();

                // Read Device Serial Number
                string s = textBox_Calibration_File.Lines[1];
                string[] sSplit = s.Split();
                textBox_Device_Serial_No.Text = sSplit[2];

                // Read Temperature Calibration Data
                // Lower Temperature Calibration Data
                s = textBox_Calibration_File.Lines[10];
                sSplit = s.Split();
                temperature_Calibrator_Low.Physical_Value = Convert.ToDouble(sSplit[0]);
                s = "";
                for (int i = 1; i < sSplit.Length - 1; i++)
                {
                    s += sSplit[i] + " ";
                }
                temperature_Calibrator_Low.textBox_Measured_Levels.Text = s;

                // Higher Temperature Calibration Data
                s = textBox_Calibration_File.Lines[11];
                sSplit = s.Split();
                temperature_Calibrator_High.Physical_Value = Convert.ToDouble(sSplit[0]);
                s = "";
                for (int i = 1; i < sSplit.Length - 1; i++)
                {
                    s += sSplit[i] + " ";
                }
                temperature_Calibrator_High.textBox_Measured_Levels.Text = s;



                // Read Calibration Data
                for (int k = 0; k < resistor_Calibrator.Length; k++)
                {
                    s = textBox_Calibration_File.Lines[43 + k];
                    sSplit = s.Split();
                    resistor_Calibrator[k].Physical_Value = Convert.ToDouble(sSplit[0]);
                    s = "";
                    for (int i = 1; i < sSplit.Length - 1; i++)
                    {
                        s += sSplit[i] + " ";
                    }
                    resistor_Calibrator[k].textBox_Measured_Levels.Text = s;
                }

            }
        }

    }
}

using System;
using System.Windows.Forms;
using System.Drawing.Design;
using Farand_Chart_Lib_Ver3;
using System.Drawing;

namespace Artemis_Esp32_Test_App
{
    public partial class Form1 : Form
    {
        Simple_Client_LAN_Control.Simple_Client_LAN_Control lan =
            new Simple_Client_LAN_Control.Simple_Client_LAN_Control();

        double time_Sec = 0;

        short Temperature = 0;
        short Pressure = 0;
        short Humidity = 0;

        float Acc_X = 0;
        float Acc_Y = 0;
        float Acc_Z = 0;
        float GyroX = 0;
        float GyroY = 0;
        float GyroZ = 0;
        float MagX =  0;
        float MagY =  0;
        float MagZ = 0;


        public Form1()
        {
            InitializeComponent();

            // tell the control to expect 64-byte packets
            lan.RX_Byte_Count = 64;

            lan.IPAddress = "192.168.4.1";
            lan.Port = 80;

            // fire when each 64-byte report arrives
            lan.Recieved_Data += LanControl_Recieved_Data;

            // start connecting on load
            this.Load += async (_, __) => await lan.ConnectAsync();
        }

        private void LanControl_Recieved_Data(object sender, EventArgs e)
        {
            var buf = lan.RX_Data;
            if (buf.Length < 64) return;
            Show_Rx_Data(buf);
            Graph_Data();

        }

        private void Show_Rx_Data(byte[] buf)
        {
            // bytes 4–5 = Temperature (°C, integer)
            Temperature = (short)((buf[4] << 8) + buf[5]);

            // bytes 6–7 = Pressure (hPa, integer)
            Pressure = (short)((buf[6] << 8) + buf[7]);

            // byte 8 is reserved

            // bytes 9–10 = Humidity (%RH, integer)
            Humidity = (short)((buf[9] << 8) + buf[10]);


            // Extract 32-bit values
            int Acc_X_i32 = GetInt32FromBuffer(buf, 11);

            // Acc_Y is spread across buf[15], [17], [18], [19] (skip [16])
            int Acc_Y_i32 = (buf[15] << 24) + (buf[17] << 16) + (buf[18] << 8) + buf[19];

            int Acc_Z_i32 = GetInt32FromBuffer(buf, 20);  // 20–23

            int GyroX_i32 = GetInt32FromBuffer(buf, 25);

            // GyroY is spread across buf[29], [30], [31], [33] (skip [32])
            int GyroY_i32 = (buf[29] << 24) + (buf[30] << 16) + (buf[31] << 8) + buf[33];

            int GyroZ_i32 = GetInt32FromBuffer(buf, 34);

            // Magnetometer
            int Mag_X_i32 = (buf[38] << 24) + (buf[39] << 16) + (buf[41] << 8) + buf[42];
            int Mag_Y_i32 = GetInt32FromBuffer(buf, 43);
            int Mag_Z_i32 = (buf[47] << 24) + (buf[49] << 16) + (buf[50] << 8) + buf[51];

            // Convert back to float (in original units)
            Acc_X = Acc_X_i32 / 1000.0f;
            Acc_Y = Acc_Y_i32 / 1000.0f;
            Acc_Z = Acc_Z_i32 / 1000.0f;

            GyroX = GyroX_i32 / 1000.0f;
            GyroY = GyroY_i32 / 1000.0f;
            GyroZ = GyroZ_i32 / 1000.0f;

            MagX = Mag_X_i32 / 1000.0f;
            MagY = Mag_Y_i32 / 1000.0f;
            MagZ = Mag_Z_i32 / 1000.0f;


            // now update the UI
            this.Invoke((MethodInvoker)(() =>
            {
                // PHT
                lblTemp.Text = $"Temp: {Temperature} °C";
                lblPres.Text = $"Pressure: {Pressure} hPa";
                lblHum.Text = $"Humidity: {Humidity} %RH";

                // IMU
                label_AccX.Text = $"Acc_X: {Acc_X}";
                label_AccY.Text = $"Acc_Y: {Acc_Y}";
                label_AccZ.Text = $"Acc_Z: {Acc_Z}";

                label_GyroX.Text = $"GyroX: {GyroX}";
                label_GyroY.Text = $"GyroY: {GyroY}";
                label_GyroZ.Text = $"GyroZ: {GyroZ}";

                //Magnetometer
                label_MagX.Text = $"X: {MagX}";
                label_MagY.Text = $"Y: {MagY}";
                label_MagZ.Text = $"Z: {MagZ}";
            }));
        }
        // Helper to extract 4 bytes (big-endian) and convert to int32
        int GetInt32FromBuffer(byte[] buf, int startIdx)
        {
            return (buf[startIdx] << 24) + (buf[startIdx + 1] << 16) + (buf[startIdx + 2] << 8) + buf[startIdx + 3];
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize_Chart1();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Graph_Data();
            time_Sec += timer1.Interval * 0.001;
        }

        void Graph_Data()
        {
            // PHT
            Farand_Chart.Farand_Graph graph_Temperature = farand_Chart1.Get_Farand_Graph_Object(0);
            Farand_Chart.Farand_Graph graph_Humidity = farand_Chart1.Get_Farand_Graph_Object(1);
            Farand_Chart.Farand_Graph graph_Pressure = farand_Chart1.Get_Farand_Graph_Object(2);

            // IMU
            Farand_Chart.Farand_Graph graph_Acc_X = farand_Chart1.Get_Farand_Graph_Object(3);
            Farand_Chart.Farand_Graph graph_Acc_Y = farand_Chart1.Get_Farand_Graph_Object(4);
            Farand_Chart.Farand_Graph graph_Acc_Z = farand_Chart1.Get_Farand_Graph_Object(5);

            Farand_Chart.Farand_Graph graph_Gyro_X = farand_Chart1.Get_Farand_Graph_Object(6);
            Farand_Chart.Farand_Graph graph_Gyro_Y = farand_Chart1.Get_Farand_Graph_Object(7);
            Farand_Chart.Farand_Graph graph_Gyro_Z = farand_Chart1.Get_Farand_Graph_Object(8);

            // Mag
            Farand_Chart.Farand_Graph graph_Mag_X = farand_Chart1.Get_Farand_Graph_Object(9);
            Farand_Chart.Farand_Graph graph_Mag_Y = farand_Chart1.Get_Farand_Graph_Object(10);
            Farand_Chart.Farand_Graph graph_Mag_Z = farand_Chart1.Get_Farand_Graph_Object(11);


            if (time_Sec > farand_Chart1.XAxis.Maximum)
            {
                if (radioButton_PHT.Checked == true)
                {
                    graph_Temperature.Points.Clear();
                    graph_Humidity.Points.Clear();
                    graph_Pressure.Points.Clear();


                }
                if (radioButton_IMU.Checked == true)
                {
                    graph_Acc_X.Points.Clear();
                    graph_Acc_Y.Points.Clear();
                    graph_Acc_Z.Points.Clear();
                }
                if (radioButton_Mag.Checked)
                {
                    graph_Mag_X.Points.Clear();
                    graph_Mag_Y.Points.Clear();
                    graph_Mag_Z.Points.Clear();
                }

                //graph_Z_Motor_Measured_deg.Points.Clear();
                //graph_Theta_Measured_Angle_deg.Points.Clear();
                //graph_Z_Motor_Measured_mm.Points.Clear();
                //graph_Z_Motor_Desired_mm.Points.Clear();

                farand_Chart1.XAxis.Initial_Minimum += 60;
                farand_Chart1.XAxis.Initial_Maximum += 60;
            }

            if (radioButton_PHT.Checked == true)
            {
                graph_Temperature.Add_Point(time_Sec, (double) Temperature);
                graph_Pressure.Add_Point(time_Sec, (double)Pressure);
                graph_Humidity.Add_Point(time_Sec, (double)Humidity);

                graph_Acc_X.Points.Clear();
                graph_Acc_Y.Points.Clear();
                graph_Acc_Z.Points.Clear();

                graph_Gyro_X.Points.Clear();
                graph_Gyro_Y.Points.Clear();
                graph_Gyro_Z.Points.Clear();

                graph_Mag_X.Points.Clear();
                graph_Mag_Y.Points.Clear();
                graph_Mag_Z.Points.Clear();

            }
            if (radioButton_IMU.Checked == true)
            {
                graph_Acc_X.Add_Point(time_Sec, (double)Acc_X);
                graph_Acc_Y.Add_Point(time_Sec, (double)Acc_Y);
                graph_Acc_Z.Add_Point(time_Sec, (double)Acc_Z);

                graph_Gyro_X.Add_Point(time_Sec, (double) GyroX * 100.0f);
                graph_Gyro_Y.Add_Point(time_Sec, (double) GyroY * 100.0f);
                graph_Gyro_Z.Add_Point(time_Sec, (double) GyroZ * 100.0f);

                graph_Temperature.Points.Clear();
                graph_Pressure.Points.Clear();
                graph_Humidity.Points.Clear();

                graph_Mag_X.Points.Clear();
                graph_Mag_Y.Points.Clear();
                graph_Mag_Z.Points.Clear();

            }
            if (radioButton_Mag.Checked)
            {
                graph_Mag_X.Add_Point(time_Sec, (double)MagX);
                graph_Mag_Y.Add_Point(time_Sec, (double)MagY);
                graph_Mag_Z.Add_Point(time_Sec, (double)MagZ);

                graph_Temperature.Points.Clear();
                graph_Pressure.Points.Clear();
                graph_Humidity.Points.Clear();

                graph_Acc_X.Points.Clear();
                graph_Acc_Y.Points.Clear();
                graph_Acc_Z.Points.Clear();

                graph_Gyro_X.Points.Clear();
                graph_Gyro_Y.Points.Clear();
                graph_Gyro_Z.Points.Clear();
            }

            //graph_Z_Motor_Measured_deg.Add_Point(time_Sec, (double)Z_Motor_Measured_Angle_deg);
            //graph_Z_Motor_Measured_mm.Add_Point(time_Sec, (double)Z_Motor_Measured_mm);
            //graph_Z_Motor_Desired_mm.Add_Point(time_Sec, (double)Z_Motor_Desired_mm);

            //graph_Theta_Measured_Angle_deg.Add_Point(time_Sec, (double)Theta_Motor_Measured_Angle_deg);
            //graph_Theta_Desired_Angle_deg.Add_Point(time_Sec, (double)Theta_Motor_Desired_Angle_deg);

        }

        private void Farand_Chart1_View_Changed(object sender, EventArgs e)
        {

        }

        private void Initialize_Chart1()
        {
            farand_Chart1.View_Changed += Farand_Chart1_View_Changed;

            farand_Chart1.XAxis.Initial_Minimum = 0;
            farand_Chart1.XAxis.Initial_Maximum = 60;
            farand_Chart1.XAxis.MajorGrid.Interval = 10;
            farand_Chart1.XAxis.MajorGrid.Labels.DecimalPlaces = 0;
            farand_Chart1.XAxis.Title.FrameSize = new SizeF(100F, 20F);
            farand_Chart1.XAxis.Title.TopMargin = -20F;
            farand_Chart1.XAxis.Title.LabelStyle.Font = new Font("Arial", 10.0F);

            farand_Chart1.YAxis.Initial_Minimum = -1000;
            farand_Chart1.YAxis.Initial_Maximum = 1000;
            farand_Chart1.YAxis.MajorGrid.Interval = 90;
            farand_Chart1.YAxis.MajorGrid.Labels.DecimalPlaces = 0;
            farand_Chart1.YAxis.Title.FrameSize = new SizeF(100F, 20F);
            farand_Chart1.YAxis.Title.RightMargin = -100F;
            farand_Chart1.YAxis.Title.LabelStyle.Font = new Font("Arial", 10.0F);

            farand_Chart1.Title.LabelStyle.Font = new Font("Arial", 10.0F);
            farand_Chart1.Title.FrameSize = new SizeF(200F, 20F);
            farand_Chart1.Title.Text = "Sensors";

            farand_Chart1.Coordinates.LabelStyle.Font = new Font("Arial", 10.0F);
            farand_Chart1.Coordinates.FrameSize = new SizeF(120F, 20F);

            farand_Chart1.Legends.LeftMargin = 10.0F;
            farand_Chart1.GraphArea.RightMargin = 200;

            farand_Chart1.Clear_All_Farand_Graphs();

            // PHT Sensor
            Farand_Chart.Farand_Graph myGraph0 = new Farand_Chart.Farand_Graph();
            myGraph0.Name = "Temperature (°C)";
            myGraph0.PointStyle.Visible = false;
            myGraph0.LineStyle.Color = Color.SkyBlue;
            myGraph0.PointStyle.Size = 5.0F;
            myGraph0.PointStyle.FillColor = Color.SkyBlue;
            myGraph0.PointStyle.LineColor = Color.SkyBlue;
            myGraph0.PointStyle.LineWidth = 1.0F;
            myGraph0.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph0);

            Farand_Chart.Farand_Graph myGraph1 = new Farand_Chart.Farand_Graph();
            myGraph1.Name = "Humidity (%RH)";
            myGraph1.PointStyle.Visible = false;
            myGraph1.LineStyle.Color = Color.Coral;
            myGraph1.PointStyle.Size = 5.0F;
            myGraph1.PointStyle.FillColor = Color.Coral;
            myGraph1.PointStyle.LineColor = Color.Coral;
            myGraph1.PointStyle.LineWidth = 1.0F;
            myGraph1.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph1);

            Farand_Chart.Farand_Graph myGraph2 = new Farand_Chart.Farand_Graph();
            myGraph2.Name = "Pressure (hPa)";
            myGraph2.PointStyle.Visible = false;
            myGraph2.LineStyle.Color = Color.Gold;
            myGraph2.PointStyle.Size = 5.0F;
            myGraph2.PointStyle.FillColor = Color.Gold;
            myGraph2.PointStyle.LineColor = Color.Gold;
            myGraph2.PointStyle.LineWidth = 1.0F;
            myGraph2.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph2);

            // IMU Sensor
            Farand_Chart.Farand_Graph myGraph3 = new Farand_Chart.Farand_Graph();
            myGraph3.Name = "Acceleration X (m/s^2)";
            myGraph3.PointStyle.Visible = false;
            myGraph3.LineStyle.Color = Color.SkyBlue;
            myGraph3.PointStyle.Size = 5.0F;
            myGraph3.PointStyle.FillColor = Color.SkyBlue;
            myGraph3.PointStyle.LineColor = Color.SkyBlue;
            myGraph3.PointStyle.LineWidth = 1.0F;
            myGraph3.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph3);

            Farand_Chart.Farand_Graph myGraph4 = new Farand_Chart.Farand_Graph();
            myGraph4.Name = "Acceleration Y (m/s^2)";
            myGraph4.PointStyle.Visible = false;
            myGraph4.LineStyle.Color = Color.Coral;
            myGraph4.PointStyle.Size = 5.0F;
            myGraph4.PointStyle.FillColor = Color.Coral;
            myGraph4.PointStyle.LineColor = Color.Coral;
            myGraph4.PointStyle.LineWidth = 1.0F;
            myGraph4.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph4);

            Farand_Chart.Farand_Graph myGraph5 = new Farand_Chart.Farand_Graph();
            myGraph5.Name = "Acceleration Z (m/s^2)";
            myGraph5.PointStyle.Visible = false;
            myGraph5.LineStyle.Color = Color.Gold;
            myGraph5.PointStyle.Size = 5.0F;
            myGraph5.PointStyle.FillColor = Color.Gold;
            myGraph5.PointStyle.LineColor = Color.Gold;
            myGraph5.PointStyle.LineWidth = 1.0F;
            myGraph5.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph5);

            Farand_Chart.Farand_Graph myGraph6 = new Farand_Chart.Farand_Graph();
            myGraph6.Name = "Gyroscope X (rad/s^2)";
            myGraph6.PointStyle.Visible = false;
            myGraph6.LineStyle.Color = Color.LimeGreen;
            myGraph6.PointStyle.Size = 5.0F;
            myGraph6.PointStyle.FillColor = Color.LimeGreen;
            myGraph6.PointStyle.LineColor = Color.LimeGreen;
            myGraph6.PointStyle.LineWidth = 1.0F;
            myGraph6.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph6);

            Farand_Chart.Farand_Graph myGraph7 = new Farand_Chart.Farand_Graph();
            myGraph7.Name = "Gyroscope Y (rad/s^2)";
            myGraph7.PointStyle.Visible = false;
            myGraph7.LineStyle.Color = Color.LightGray;
            myGraph7.PointStyle.Size = 5.0F;
            myGraph7.PointStyle.FillColor = Color.LightGray;
            myGraph7.PointStyle.LineColor = Color.LightGray;
            myGraph7.PointStyle.LineWidth = 1.0F;
            myGraph7.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph7);

            Farand_Chart.Farand_Graph myGraph8 = new Farand_Chart.Farand_Graph();
            myGraph8.Name = "Gyroscope Z (rad/s^2)";
            myGraph8.PointStyle.Visible = false;
            myGraph8.LineStyle.Color = Color.LightPink;
            myGraph8.PointStyle.Size = 5.0F;
            myGraph8.PointStyle.FillColor = Color.LightPink;
            myGraph8.PointStyle.LineColor = Color.LightPink;
            myGraph8.PointStyle.LineWidth = 1.0F;
            myGraph8.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph8);

            // Magnetometer Sensor
            Farand_Chart.Farand_Graph myGraph9 = new Farand_Chart.Farand_Graph();
            myGraph9.Name = "Magnetic X (uT)";
            myGraph9.PointStyle.Visible = false;
            myGraph9.LineStyle.Color = Color.SkyBlue;
            myGraph9.PointStyle.Size = 5.0F;
            myGraph9.PointStyle.FillColor = Color.SkyBlue;
            myGraph9.PointStyle.LineColor = Color.SkyBlue;
            myGraph9.PointStyle.LineWidth = 1.0F;
            myGraph9.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph9);

            Farand_Chart.Farand_Graph myGraph10 = new Farand_Chart.Farand_Graph();
            myGraph10.Name = "Magnetic Y (uT)";
            myGraph10.PointStyle.Visible = false;
            myGraph10.LineStyle.Color = Color.Coral;
            myGraph10.PointStyle.Size = 5.0F;
            myGraph10.PointStyle.FillColor = Color.Coral;
            myGraph10.PointStyle.LineColor = Color.Coral;
            myGraph10.PointStyle.LineWidth = 1.0F;
            myGraph10.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph10);

            Farand_Chart.Farand_Graph myGraph11 = new Farand_Chart.Farand_Graph();
            myGraph11.Name = "Magnetic Z (uT)";
            myGraph11.PointStyle.Visible = false;
            myGraph11.LineStyle.Color = Color.Gold;
            myGraph11.PointStyle.Size = 5.0F;
            myGraph11.PointStyle.FillColor = Color.Gold;
            myGraph11.PointStyle.LineColor = Color.Gold;
            myGraph11.PointStyle.LineWidth = 1.0F;
            myGraph11.LineStyle.Width = 1.5F;
            farand_Chart1.Add_Farand_Graph(myGraph11);

            farand_Chart1.Refresh_All();
        }
    }
}

using System;
using System.Windows.Forms;
using System.Drawing.Design;
using Farand_Chart_Lib_Ver3;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;

namespace Artemis_Esp32_Test_App
{
    public partial class Form1 : Form
    {
        private Simple_Client_LAN_Control.Simple_Client_LAN_Control esp;
        byte[] command_bytes = new byte[8];
        bool is_Command_Ready = false;

        System.Threading.Timer timer_Connection;

        const byte PACKETS_HEADER = 0xFA;

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
        float MagX = 0; 
        float MagY = 0; 
        float MagZ = 0;

        private struct SensorSnapshot
        {
            public short Temperature;
            public short Pressure;
            public short Humidity;

            public float Acc_X;
            public float Acc_Y;
            public float Acc_Z;

            public float GyroX;
            public float GyroY;
            public float GyroZ;

            public float MagX;
            public float MagY;
            public float MagZ;
        }

        private struct ImuSample
        {
            public byte Header;
            public UInt16 AccX;
            public UInt16 AccY;
            public UInt16 AccZ;
            public UInt16 GyroX;
            public UInt16 GyroY;
            public UInt16 GyroZ;
            public byte Checksum;
        }

        private ImuSample[] _incomingBatch = new ImuSample[5];
        private volatile bool _newBatchReady = false;

        // shared latest data from ESP
        private SensorSnapshot _latestSample;
        // flag to tell UI "new data arrived"
        private volatile bool _newSampleReady = false;

        private readonly object _cmdLock = new object();
        public Form1()
        {
            InitializeComponent();

            timer_Connection = new System.Threading.Timer(Timer_Connection_Tick, null, 0, 200);

            esp = new Simple_Client_LAN_Control.Simple_Client_LAN_Control
            {
                IPAddress = "192.168.1.101",
                Port = 9000,
                RX_Byte_Count = 16 * 5 // 16 bytes on 25msec , 80 bytes each 125 msec from artemis
            };
            esp.onConnected += Esp_onConnected;
            esp.onDisconnect += Esp_onDisconnect;
            esp.Recieved_Data += Esp_Recieved_Data;

            Controls.Add(esp);

            // tell the control to expect 64-byte packets

    }

        private void Timer_Connection_Tick(object state)
        {
            if (!is_Command_Ready) //Heartbeat
            {
                Clear_All_Buffers();
                command_bytes[1] = 0xFA;

            }
            else
            {
                is_Command_Ready = false;

            }

            command_bytes[7] = Calculate_Checksum(command_bytes);

            Console.WriteLine($"Sending {command_bytes[1]}");
            esp.Send_Data(command_bytes);
        }

        private void Esp_Recieved_Data(object sender, EventArgs e)
        {
            byte[] raw = esp.RX_Data; // length = 80

            // safety check
            if (raw == null || raw.Length < 80)
                return;

            // decode 5 samples
            for (int i = 0; i < 5; i++)
            {
                int baseIndex = i * 16;

                if (!VerifyChecksumAndHeader(raw, baseIndex))
                {
                    // bad frame, skip or mark it
                    Console.WriteLine($"Bad Checksum or Header on sample {i}!!!");
                    continue;
                }

                ImuSample s = new ImuSample();

                s.Header = raw[baseIndex + 0];

                s.AccX = GetInt16_BE(raw, baseIndex + 1);
                s.AccY = GetInt16_BE(raw, baseIndex + 3);
                s.AccZ = GetInt16_BE(raw, baseIndex + 5);

                s.GyroX = GetInt16_BE(raw, baseIndex + 7);
                s.GyroY = GetInt16_BE(raw, baseIndex + 9);
                s.GyroZ = GetInt16_BE(raw, baseIndex + 11);

                // bytes 13,14 reserved -> we can read them if you need them later
                // short reserved = GetInt16_BE(raw, baseIndex + 13);

                s.Checksum = raw[baseIndex + 15];

                // OPTIONAL: verify checksum here before trusting
                // if (!VerifyChecksum(raw, baseIndex)) { /* mark invalid, etc */ }

                _incomingBatch[i] = s;
            }

            // tell the UI timer there is fresh data
            _newBatchReady = true;
        }

        private UInt16 GetInt16_BE(byte[] buf, int start)
        {
            // big-endian -> high byte first
            int hi = buf[start];
            int lo = buf[start + 1];
            // combine: hi << 8 | lo, then cast to short
            int combined = (hi << 8) + lo;
            return (UInt16)combined;
        }

        private void Esp_onDisconnect(object sender, EventArgs e)
        {
            timer_Connection.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void Esp_onConnected(object sender, EventArgs e)
        {
            timer_Connection.Change(0, 200);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize_Chart1();
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


            if (time_Sec > farand_Chart1.XAxis.Minimum + 10)
            {
                // Ensure that the first point is visible on the graph
                if (radioButton_PHT.Checked == true)
                {
                    // Shift points by removing the first (oldest) point and keeping the new ones
                    if (graph_Temperature.Points.Count > 10) graph_Temperature.Points.RemoveAt(0);
                    if (graph_Humidity.Points.Count > 10) graph_Humidity.Points.RemoveAt(0);
                    if (graph_Pressure.Points.Count > 10) graph_Pressure.Points.RemoveAt(0);
                }
                if (radioButton_IMU.Checked == true)
                {
                    if (graph_Acc_X.Points.Count > 10) graph_Acc_X.Points.RemoveAt(0);
                    if (graph_Acc_Y.Points.Count > 10) graph_Acc_Y.Points.RemoveAt(0);
                    if (graph_Acc_Z.Points.Count > 10) graph_Acc_Z.Points.RemoveAt(0);
                }
                if (radioButton_Mag.Checked)
                {
                    if (graph_Mag_X.Points.Count > 10) graph_Mag_X.Points.RemoveAt(0);
                    if (graph_Mag_Y.Points.Count > 10) graph_Mag_Y.Points.RemoveAt(0);
                    if (graph_Mag_Z.Points.Count > 10) graph_Mag_Z.Points.RemoveAt(0);
                }

                // Increment the X-Axis range
                farand_Chart1.XAxis.Initial_Minimum += 1;
                farand_Chart1.XAxis.Initial_Maximum += 1;
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
                graph_Acc_X.Add_Point(time_Sec, (double)Acc_X * 10.0f);
                graph_Acc_Y.Add_Point(time_Sec, (double)Acc_Y * 10.0f);
                graph_Acc_Z.Add_Point(time_Sec, (double)Acc_Z * 10.0f);

                graph_Gyro_X.Add_Point(time_Sec, (double) GyroX);
                graph_Gyro_Y.Add_Point(time_Sec, (double) GyroY);
                graph_Gyro_Z.Add_Point(time_Sec, (double) GyroZ);

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

        }

        private void Farand_Chart1_View_Changed(object sender, EventArgs e)
        {

        }

        private void Initialize_Chart1()
        {
            farand_Chart1.View_Changed += Farand_Chart1_View_Changed;

            farand_Chart1.XAxis.Initial_Minimum = 0;
            farand_Chart1.XAxis.Initial_Maximum = 10;
            farand_Chart1.XAxis.MajorGrid.Interval = 1;
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

        private void checkBox_Start_Qwiic_CheckedChanged(object sender, EventArgs e)
        {
            Power_Switch_Sensors(checkBox_Start_Qwiic.Checked);
        }

        private void Power_Switch_Sensors(bool is_On)
        {
            if (is_On)
            {
                command_bytes[1] = 0x01;

                checkBox_Reporting.Enabled = true;
            }
            else
            {
                command_bytes[1] = 0x02;

                checkBox_Reporting.Enabled = false;
            }

            is_Command_Ready = true;

        }

        private byte Calculate_Checksum(byte[] packet)
        {
            byte checksum = 0;
            for (int i = 0; i < packet.Length - 1; i++)
            {
                checksum += packet[i];
            }
            return checksum;
        }
        private bool VerifyChecksumAndHeader(byte[] buf, int baseIndex)
        {
            byte sum = 0;
            // sum bytes 0..14
            for (int i = 0; i < buf.Length - 1; i++)
            {
                sum += buf[baseIndex + i];
            }
            byte cs = buf[baseIndex + buf.Length - 1];
            return (sum == cs) && (buf[baseIndex] == PACKETS_HEADER);
        }

        private void Clear_All_Buffers()
        {
            for (int i = 0; i < command_bytes.Length; i++)
            {
                command_bytes[i] = 0x00;
            }
        }

        private void checkBox_Reporting_CheckedChanged(object sender, EventArgs e)
        {
            Switch_Reporting(checkBox_Reporting.Checked);
        }


        private void Switch_Reporting(bool is_Reporting)
        {
            if (is_Reporting)
            {
                command_bytes[1] = 0x04;

                checkBox_Reporting.Enabled = true;
            }
            else
            {
                command_bytes[1] = 0x05;
            }

            is_Command_Ready = true;

        }

        private void timerUiUpdate_Tick(object sender, EventArgs e)
        {
            if (!_newBatchReady)
                return;

            _newBatchReady = false;

            // Copy locally so we don't get race conditions mid-loop
            ImuSample[] localBatch = new ImuSample[5];
            for (int i = 0; i < 5; i++)
                localBatch[i] = _incomingBatch[i];

            // the last sample in the batch is the newest (~now)
            ImuSample latest = localBatch[4];

            // Convert raw sensor units to display units
            // e.g. if Acc is in mg or m/s² * 100, apply your scaling here.
            // I'll assume raw 'short' is already in some meaningful LSB-per-unit.
            UpdateLabelsFromSample(latest);

            // Now add ALL samples to the chart timeline
            PushBatchToChart(localBatch);
        }

        private void UpdateLabelsFromSample(ImuSample s)
        {
            // Example scaling:
            // Let's say accelerometer is in milli-g or milli-(m/s^2).
            // If you know it's 1000x scaled, divide by 1000.0f before display.

            float accX = s.AccX / 100.0f;
            float accY = s.AccY / 100.0f;
            float accZ = s.AccZ / 100.0f;

            float gyroX = s.GyroX / 100.0f;
            float gyroY = s.GyroY / 100.0f;
            float gyroZ = s.GyroZ / 100.0f;

            lblTemp.Text = $"AccX: {accX:0.00}";
            lblPres.Text = $"AccY: {accY:0.00}";
            lblHum.Text = $"AccZ: {accZ:0.00}";

            label_AccX.Text = $"GyroX: {gyroX:0.00}";
            label_AccY.Text = $"GyroY: {gyroY:0.00}";
            label_AccZ.Text = $"GyroZ: {gyroZ:0.00}";

            // If you want separate labels for accel vs gyro, map them however you like.
            // You can rename the labels for clarity. I'm reusing your labels here.
        }

        private void PushBatchToChart(ImuSample[] batch)
        {
            // You already do shifting and Add_Point in Graph_Data().
            // We’ll feed it in a loop.

            for (int i = 0; i < batch.Length; i++)
            {
                ImuSample s = batch[i];

                // scale raw sensor units here too:
                float accX = s.AccX / 100.0f;
                float accY = s.AccY / 100.0f;
                float accZ = s.AccZ / 100.0f;

                float gyroX = s.GyroX / 100.0f;
                float gyroY = s.GyroY / 100.0f;
                float gyroZ = s.GyroZ / 100.0f;

                // update your globals so Graph_Data can reuse them
                Acc_X = accX;
                Acc_Y = accY;
                Acc_Z = accZ;

                GyroX = gyroX;
                GyroY = gyroY;
                GyroZ = gyroZ;

                // we advance our local timeline for each sub-sample
                // each sub-sample is 25 ms = 0.025 seconds
                time_Sec += 0.025;

                // now call your existing Graph_Data() to push these into Farand_Chart
                Graph_Data();
            }
        }
    }
}

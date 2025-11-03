using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Farand_Chart_Lib_Ver3;

namespace Artemis_Esp32_Test_App
{
    public partial class Form1 : Form
    {
        private Simple_Client_LAN_Control.Simple_Client_LAN_Control esp;

        byte[] command_bytes = new byte[8];
        bool is_Command_Ready = false;

        // this runs on a ThreadPool thread, used to send commands/heartbeat
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

        // one 16-byte IMU frame from ESP
        private struct ImuSample
        {
            public byte Header;
            public short AccX;
            public short AccY;
            public short AccZ;
            public short GyroX;
            public short GyroY;
            public short GyroZ;
            public byte Checksum; // keep last byte; [13],[14] are reserved and ignored
        }

        // we receive 5 samples per batch (80 bytes total)
        private ImuSample[] _incomingBatch = new ImuSample[5];
        private bool _newBatchReady = false; // set true when new batch decoded

        private readonly object _cmdLock = new object();

        public Form1()
        {
            InitializeComponent();

            // create the background TX timer (but don't start it yet)
            // we'll start it when connected
            timer_Connection = new System.Threading.Timer(
                Timer_Connection_Tick,
                null,
                Timeout.Infinite,
                Timeout.Infinite
            );

            // init TCP client control
            esp = new Simple_Client_LAN_Control.Simple_Client_LAN_Control
            {
                IPAddress = "192.168.0.117",
                Port = 5001,
                RX_Byte_Count = 16 * 5 // 16 bytes/sample * 5 samples = 80 bytes per batch
            };

            // hook events from the LAN control
            esp.Connected += Esp_onConnected;
            esp.Disconnected += Esp_onDisconnect;
            esp.DataReceived += Esp_DataReceived;

            // add it to the form so it can live/get disposed with the form
            Controls.Add(esp);

            // start async connect/reconnect loop in the control
            esp.Start();
        }

        // =========================================
        // CONNECTION HANDLERS
        // =========================================

        private void Esp_onConnected(object sender, EventArgs e)
        {
            // start sending commands/heartbeat every 200 ms
            timer_Connection.Change(0, 200);
        }

        private void Esp_onDisconnect(object sender, EventArgs e)
        {
            // stop sending commands if link is down
            timer_Connection.Change(Timeout.Infinite, Timeout.Infinite);
        }

        // =========================================
        // RECEIVE HANDLER
        // called on UI thread (BeginInvoke inside control)
        // =========================================

        private void Esp_DataReceived(object sender, Simple_Client_LAN_Control.DataReceivedEventArgs e)
        {
            byte[] raw = e.Data; // should be length 80 (5 * 16)
            if (raw == null || raw.Length < 80)
                return;

            for (int i = 0; i < 5; i++)
            {
                int baseIndex = i * 16;

                if (!VerifyChecksumAndHeader(raw, baseIndex))
                {
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

                // bytes [13],[14] reserved if you want later
                s.Checksum = raw[baseIndex + 15];

                _incomingBatch[i] = s;
            }

            _newBatchReady = true;
        }

        // =========================================
        // BACKGROUND SEND / HEARTBEAT
        // runs on ThreadPool thread
        // =========================================

        private void Timer_Connection_Tick(object state)
        {
            lock (_cmdLock)
            {
                if (!is_Command_Ready) // heartbeat / keep-alive mode
                {
                    Clear_All_Buffers();
                    command_bytes[1] = 0xFA;
                }
                else
                {
                    // we had a pending command set by UI (Power_Switch_Sensors etc.)
                    is_Command_Ready = false;
                }

                command_bytes[7] = Calculate_Checksum(command_bytes);

                // send bytes to ESP
                esp.Send(command_bytes);
            }
        }

        // =========================================
        // FORM EVENTS / UI TIMERS
        // =========================================

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize_Chart1();
        }

        // This runs on WinForms timer (UI thread).
        // It pulls the latest decoded batch and updates labels + charts.
        private void timerUiUpdate_Tick(object sender, EventArgs e)
        {
            if (!_newBatchReady)
                return;

            _newBatchReady = false;

            // copy batch locally
            ImuSample[] localBatch = new ImuSample[5];
            for (int i = 0; i < 5; i++)
                localBatch[i] = _incomingBatch[i];

            // newest sample is the last one in the batch
            ImuSample latest = localBatch[4];

            UpdateLabelsFromSample(latest);
            PushBatchToChart(localBatch);
        }

        // =========================================
        // LABEL / UI UPDATE HELPERS
        // =========================================

        private void UpdateLabelsFromSample(ImuSample s)
        {
            // apply scaling here (example: divide by 100.0f)
            float accX = s.AccX / 100.0f;
            float accY = s.AccY / 100.0f;
            float accZ = s.AccZ / 100.0f;

            float gyroX = s.GyroX / 100.0f;
            float gyroY = s.GyroY / 100.0f;
            float gyroZ = s.GyroZ / 100.0f;

            // update UI labels
            lblTemp.Text = $"AccX: {accX:0.00}";
            lblPres.Text = $"AccY: {accY:0.00}";
            lblHum.Text = $"AccZ: {accZ:0.00}";

            label_AccX.Text = $"GyroX: {gyroX:0.00}";
            label_AccY.Text = $"GyroY: {gyroY:0.00}";
            label_AccZ.Text = $"GyroZ: {gyroZ:0.00}";
        }

        private void PushBatchToChart(ImuSample[] batch)
        {
            for (int i = 0; i < batch.Length; i++)
            {
                ImuSample s = batch[i];

                float accX = s.AccX / 100.0f;
                float accY = s.AccY / 100.0f;
                float accZ = s.AccZ / 100.0f;

                float gyroX = s.GyroX / 100.0f;
                float gyroY = s.GyroY / 100.0f;
                float gyroZ = s.GyroZ / 100.0f;

                // assign globals so Graph_Data() can consume them
                Acc_X = accX;
                Acc_Y = accY;
                Acc_Z = accZ;

                GyroX = gyroX;
                GyroY = gyroY;
                GyroZ = gyroZ;

                // if you later include mag and PHT, assign those too

                // each sub-sample is 25 ms apart
                time_Sec += 0.025;

                Graph_Data();
            }
        }

        // =========================================
        // GRAPHING / CHART MANAGEMENT
        // =========================================

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

            // scroll window after ~10 seconds
            if (time_Sec > farand_Chart1.XAxis.Minimum + 10)
            {
                if (radioButton_PHT.Checked)
                {
                    if (graph_Temperature.Points.Count > 10) graph_Temperature.Points.RemoveAt(0);
                    if (graph_Humidity.Points.Count > 10) graph_Humidity.Points.RemoveAt(0);
                    if (graph_Pressure.Points.Count > 10) graph_Pressure.Points.RemoveAt(0);
                }
                if (radioButton_IMU.Checked)
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

                farand_Chart1.XAxis.Initial_Minimum += 1;
                farand_Chart1.XAxis.Initial_Maximum += 1;
            }

            if (radioButton_PHT.Checked)
            {
                graph_Temperature.Add_Point(time_Sec, (double)Temperature);
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
            else if (radioButton_IMU.Checked)
            {
                graph_Acc_X.Add_Point(time_Sec, (double)Acc_X * 10.0f);
                graph_Acc_Y.Add_Point(time_Sec, (double)Acc_Y * 10.0f);
                graph_Acc_Z.Add_Point(time_Sec, (double)Acc_Z * 10.0f);

                graph_Gyro_X.Add_Point(time_Sec, (double)GyroX);
                graph_Gyro_Y.Add_Point(time_Sec, (double)GyroY);
                graph_Gyro_Z.Add_Point(time_Sec, (double)GyroZ);

                graph_Temperature.Points.Clear();
                graph_Pressure.Points.Clear();
                graph_Humidity.Points.Clear();

                graph_Mag_X.Points.Clear();
                graph_Mag_Y.Points.Clear();
                graph_Mag_Z.Points.Clear();
            }
            else if (radioButton_Mag.Checked)
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

            // PHT
            {
                var g0 = new Farand_Chart.Farand_Graph();
                g0.Name = "Temperature (°C)";
                g0.PointStyle.Visible = false;
                g0.LineStyle.Color = Color.SkyBlue;
                g0.PointStyle.Size = 5.0F;
                g0.PointStyle.FillColor = Color.SkyBlue;
                g0.PointStyle.LineColor = Color.SkyBlue;
                g0.PointStyle.LineWidth = 1.0F;
                g0.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g0);

                var g1 = new Farand_Chart.Farand_Graph();
                g1.Name = "Humidity (%RH)";
                g1.PointStyle.Visible = false;
                g1.LineStyle.Color = Color.Coral;
                g1.PointStyle.Size = 5.0F;
                g1.PointStyle.FillColor = Color.Coral;
                g1.PointStyle.LineColor = Color.Coral;
                g1.PointStyle.LineWidth = 1.0F;
                g1.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g1);

                var g2 = new Farand_Chart.Farand_Graph();
                g2.Name = "Pressure (hPa)";
                g2.PointStyle.Visible = false;
                g2.LineStyle.Color = Color.Gold;
                g2.PointStyle.Size = 5.0F;
                g2.PointStyle.FillColor = Color.Gold;
                g2.PointStyle.LineColor = Color.Gold;
                g2.PointStyle.LineWidth = 1.0F;
                g2.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g2);
            }

            // IMU
            {
                var g3 = new Farand_Chart.Farand_Graph();
                g3.Name = "Acceleration X (m/s^2)";
                g3.PointStyle.Visible = false;
                g3.LineStyle.Color = Color.SkyBlue;
                g3.PointStyle.Size = 5.0F;
                g3.PointStyle.FillColor = Color.SkyBlue;
                g3.PointStyle.LineColor = Color.SkyBlue;
                g3.PointStyle.LineWidth = 1.0F;
                g3.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g3);

                var g4 = new Farand_Chart.Farand_Graph();
                g4.Name = "Acceleration Y (m/s^2)";
                g4.PointStyle.Visible = false;
                g4.LineStyle.Color = Color.Coral;
                g4.PointStyle.Size = 5.0F;
                g4.PointStyle.FillColor = Color.Coral;
                g4.PointStyle.LineColor = Color.Coral;
                g4.PointStyle.LineWidth = 1.0F;
                g4.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g4);

                var g5 = new Farand_Chart.Farand_Graph();
                g5.Name = "Acceleration Z (m/s^2)";
                g5.PointStyle.Visible = false;
                g5.LineStyle.Color = Color.Gold;
                g5.PointStyle.Size = 5.0F;
                g5.PointStyle.FillColor = Color.Gold;
                g5.PointStyle.LineColor = Color.Gold;
                g5.PointStyle.LineWidth = 1.0F;
                g5.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g5);

                var g6 = new Farand_Chart.Farand_Graph();
                g6.Name = "Gyroscope X (rad/s)";
                g6.PointStyle.Visible = false;
                g6.LineStyle.Color = Color.LimeGreen;
                g6.PointStyle.Size = 5.0F;
                g6.PointStyle.FillColor = Color.LimeGreen;
                g6.PointStyle.LineColor = Color.LimeGreen;
                g6.PointStyle.LineWidth = 1.0F;
                g6.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g6);

                var g7 = new Farand_Chart.Farand_Graph();
                g7.Name = "Gyroscope Y (rad/s)";
                g7.PointStyle.Visible = false;
                g7.LineStyle.Color = Color.LightGray;
                g7.PointStyle.Size = 5.0F;
                g7.PointStyle.FillColor = Color.LightGray;
                g7.PointStyle.LineColor = Color.LightGray;
                g7.PointStyle.LineWidth = 1.0F;
                g7.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g7);

                var g8 = new Farand_Chart.Farand_Graph();
                g8.Name = "Gyroscope Z (rad/s)";
                g8.PointStyle.Visible = false;
                g8.LineStyle.Color = Color.LightPink;
                g8.PointStyle.Size = 5.0F;
                g8.PointStyle.FillColor = Color.LightPink;
                g8.PointStyle.LineColor = Color.LightPink;
                g8.PointStyle.LineWidth = 1.0F;
                g8.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g8);
            }

            // Magnetometer
            {
                var g9 = new Farand_Chart.Farand_Graph();
                g9.Name = "Magnetic X (uT)";
                g9.PointStyle.Visible = false;
                g9.LineStyle.Color = Color.SkyBlue;
                g9.PointStyle.Size = 5.0F;
                g9.PointStyle.FillColor = Color.SkyBlue;
                g9.PointStyle.LineColor = Color.SkyBlue;
                g9.PointStyle.LineWidth = 1.0F;
                g9.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g9);

                var g10 = new Farand_Chart.Farand_Graph();
                g10.Name = "Magnetic Y (uT)";
                g10.PointStyle.Visible = false;
                g10.LineStyle.Color = Color.Coral;
                g10.PointStyle.Size = 5.0F;
                g10.PointStyle.FillColor = Color.Coral;
                g10.PointStyle.LineColor = Color.Coral;
                g10.PointStyle.LineWidth = 1.0F;
                g10.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g10);

                var g11 = new Farand_Chart.Farand_Graph();
                g11.Name = "Magnetic Z (uT)";
                g11.PointStyle.Visible = false;
                g11.LineStyle.Color = Color.Gold;
                g11.PointStyle.Size = 5.0F;
                g11.PointStyle.FillColor = Color.Gold;
                g11.PointStyle.LineColor = Color.Gold;
                g11.PointStyle.LineWidth = 1.0F;
                g11.LineStyle.Width = 1.5F;
                farand_Chart1.Add_Farand_Graph(g11);
            }

            farand_Chart1.Refresh_All();
        }

        private void Farand_Chart1_View_Changed(object sender, EventArgs e)
        {
            // optional: handle zoom/pan feedback here if you want
        }

        // =========================================
        // COMMAND GENERATION / UI CONTROLS
        // =========================================

        private void checkBox_Start_Qwiic_CheckedChanged(object sender, EventArgs e)
        {
            Power_Switch_Sensors(checkBox_Start_Qwiic.Checked);
        }

        private void Power_Switch_Sensors(bool is_On)
        {
            lock (_cmdLock)
            {
                if (is_On)
                {
                    command_bytes[1] = 0x01; // your "power on sensors" opcode
                    checkBox_Reporting.Enabled = true;
                }
                else
                {
                    command_bytes[1] = 0x02; // "power off sensors"
                    checkBox_Reporting.Enabled = false;
                }

                is_Command_Ready = true;
            }
        }

        private void checkBox_Reporting_CheckedChanged(object sender, EventArgs e)
        {
            Switch_Reporting(checkBox_Reporting.Checked);
        }

        private void Switch_Reporting(bool is_Reporting)
        {
            lock (_cmdLock)
            {
                if (is_Reporting)
                {
                    command_bytes[1] = 0x04; // "start reporting"
                    checkBox_Reporting.Enabled = true;
                }
                else
                {
                    command_bytes[1] = 0x05; // "stop reporting"
                }

                is_Command_Ready = true;
            }
        }

        // =========================================
        // HELPERS / UTILS
        // =========================================

        private byte Calculate_Checksum(byte[] packet)
        {
            byte checksum = 0;
            // sum all except last slot (checksum byte)
            for (int i = 0; i < packet.Length - 1; i++)
            {
                checksum += packet[i];
            }
            return checksum;
        }

        // Verify 16-byte sub-frame at baseIndex:
        // [0] header, [1..14] payload, [15] checksum
        private bool VerifyChecksumAndHeader(byte[] buf, int baseIndex)
        {
            if (buf[baseIndex] != PACKETS_HEADER)
                return false;

            byte sum = 0;
            // sum bytes 0..14 of this sub-frame
            for (int i = 0; i < 15; i++)
            {
                sum += buf[baseIndex + i];
            }

            byte cs = buf[baseIndex + 15];
            return (sum == cs);
        }

        // read a big-endian unsigned 16-bit from buf[start], buf[start+1]
        private short GetInt16_BE(byte[] buf, int start)
        {
            int hi = buf[start];
            int lo = buf[start + 1];
            return unchecked((short)((hi << 8) | lo)); // sign-preserving
        }

        private void Clear_All_Buffers()
        {
            for (int i = 0; i < command_bytes.Length; i++)
            {
                command_bytes[i] = 0x00;
            }
        }
    }
}

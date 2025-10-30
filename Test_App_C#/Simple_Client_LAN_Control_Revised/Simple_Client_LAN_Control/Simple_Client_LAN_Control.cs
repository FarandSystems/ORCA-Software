using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Client_LAN_Control
{
    public partial class Simple_Client_LAN_Control : UserControl, IDisposable
    {
        // ===========================
        // PUBLIC PROPERTIES / EVENTS
        // ===========================

        [Browsable(true)]
        [Category("LAN Client")]
        [Description("Target ESP32 IP address (static IP).")]
        public string IPAddress { get; set; } = "192.168.1.101";

        [Browsable(true)]
        [Category("LAN Client")]
        [Description("Target TCP port on ESP32.")]
        public int Port { get; set; } = 9000;

        [Browsable(true)]
        [Category("LAN Client")]
        [Description("How many bytes we expect per received frame.")]
        public int RX_Byte_Count
        {
            get => _rxByteCount;
            set
            {
                if (value <= 0) return;
                _rxByteCount = value;
                _rxBuffer = new byte[_rxByteCount];
                _tempRxBuf = new byte[_rxByteCount];
            }
        }

        [Browsable(false)]
        [Description("Latest received frame (copy).")]
        public byte[] RX_Data
        {
            get
            {
                var copy = new byte[_rxByteCount];
                Buffer.BlockCopy(_rxBuffer, 0, copy, 0, _rxByteCount);
                return copy;
            }
        }

        [Browsable(false)]
        [Description("True if TCP socket is currently connected.")]
        public bool IsConnected => _isConnected;

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler DataReceived;

        // ===========================
        // PUBLIC METHODS
        // ===========================

        // Call once (e.g. in Form.Load)
        public void Start()
        {
            if (_mainCts != null && !_mainCts.IsCancellationRequested)
                return; // already running

            _mainCts = new CancellationTokenSource();
            _ = RunConnectionLoopAsync(_mainCts.Token);

            timerUiStatus?.Start();
        }

        // Call on shutdown (e.g. FormClosing)
        public void Stop()
        {
            try { _mainCts?.Cancel(); } catch { }

            timerUiStatus?.Stop();
            timerHeartbeat?.Stop();

            CleanupSocket();
        }

        // Queue bytes to send to ESP (thread-safe)
        public void Send(byte[] data)
        {
            if (data == null || data.Length == 0) return;
            _txQueue.Enqueue(data);
        }

        // ===========================
        // INTERNAL STATE
        // ===========================

        private TcpClient _tcpClient;
        private NetworkStream _stream;

        private CancellationTokenSource _mainCts;     // runs forever, handles reconnect logic
        private CancellationTokenSource _sessionCts;  // per-connection session (rx/tx loops)

        private volatile bool _isConnected = false;

        private int _rxByteCount = 64;
        private byte[] _rxBuffer = new byte[64];   // shared "latest frame"
        private byte[] _tempRxBuf = new byte[64];  // temp read buffer

        private readonly ConcurrentQueue<byte[]> _txQueue = new ConcurrentQueue<byte[]>();

        // Heartbeat config
        private const int HeartbeatIntervalMs = 5000;    // send ping every 5s
        private readonly byte[] _heartbeatFrame = { 0xAA, 0x55, 0xAA, 0x55 }; // customize if you want

        // status blink
        private bool _blink = false;
        private volatile bool _packetFlag = false; // set true when new data arrives

        // timers & ui elements are declared in Designer.cs
        // private System.Windows.Forms.Timer timerUiStatus;
        // private System.Windows.Forms.Timer timerHeartbeat;
        // private Label labelStatus;
        // private TextBox textBoxLog;
        // private System.ComponentModel.IContainer components;

        // ===========================
        // CONSTRUCTOR
        // ===========================

        public Simple_Client_LAN_Control()
        {
            InitializeComponent();

            // sanity init color
            SafeSetStatus("DISCONNECTED", Color.Tomato);
        }

        // We KEEP Dispose() here (designer will NOT also generate one now)
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();

                components?.Dispose();
                timerUiStatus?.Dispose();
                timerHeartbeat?.Dispose();
            }
            base.Dispose(disposing);
        }

        // ===========================
        // CORE LOOP: CONNECT / RECONNECT
        // ===========================

        private async Task RunConnectionLoopAsync(CancellationToken token)
        {
            int backoffMs = 500;       // start retry delay
            int backoffMaxMs = 5000;   // cap retry delay

            while (!token.IsCancellationRequested)
            {
                if (!_isConnected)
                {
                    bool ok = await TryConnectOnceAsync(token);
                    if (!ok)
                    {
                        await Task.Delay(backoffMs, token).ContinueWith(_ => { });
                        backoffMs = Math.Min(backoffMs * 2, backoffMaxMs);
                        continue;
                    }
                    backoffMs = 500; // reset after success
                }

                await Task.Delay(200, token).ContinueWith(_ => { });
            }
        }

        private async Task<bool> TryConnectOnceAsync(CancellationToken outerToken)
        {
            Log("Connecting...");

            var client = new TcpClient();
            client.NoDelay = true; // low latency

            try
            {
                await client.ConnectAsync(IPAddress, Port);
            }
            catch (Exception ex)
            {
                Log("Connect failed: " + ex.Message);
                return false;
            }

            _tcpClient = client;
            _stream = _tcpClient.GetStream();
            _isConnected = true;
            _packetFlag = false;

            SafeSetStatus("CONNECTED", Color.LimeGreen);
            SafeFireConnected();
            Log("Connected.");

            _sessionCts?.Cancel();
            _sessionCts = new CancellationTokenSource();

            timerHeartbeat?.Start();

            _ = ReceiveLoopAsync(_sessionCts.Token);
            _ = SendLoopAsync(_sessionCts.Token);

            return true;
        }

        // ===========================
        // RECEIVE LOOP
        // ===========================

        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && _isConnected)
                {
                    int totalRead = 0;

                    // read exactly RX_Byte_Count bytes
                    while (totalRead < _rxByteCount && _isConnected && !token.IsCancellationRequested)
                    {
                        int n;
                        try
                        {
                            n = await _stream.ReadAsync(
                                _tempRxBuf,
                                totalRead,
                                _rxByteCount - totalRead,
                                token
                            );
                        }
                        catch (Exception ex)
                        {
                            Log("RX error: " + ex.Message);
                            n = 0;
                        }

                        if (n <= 0)
                        {
                            // peer closed or error
                            Log("RX loop: remote closed");
                            HandleDisconnect();
                            return;
                        }

                        totalRead += n;
                    }

                    if (!_isConnected || token.IsCancellationRequested)
                        break;

                    // full frame received
                    Buffer.BlockCopy(_tempRxBuf, 0, _rxBuffer, 0, _rxByteCount);

                    _packetFlag = true;
                    SafeFireDataReceived();
                }
            }
            finally
            {
                HandleDisconnect();
            }
        }

        // ===========================
        // SEND LOOP
        // ===========================

        private async Task SendLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && _isConnected)
                {
                    if (_txQueue.TryDequeue(out var frame))
                    {
                        try
                        {
                            if (_stream != null)
                            {
                                await _stream.WriteAsync(frame, 0, frame.Length, token);
                                await _stream.FlushAsync(token);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log("TX error: " + ex.Message);
                            HandleDisconnect();
                            return;
                        }
                    }
                    else
                    {
                        await Task.Delay(5, token).ContinueWith(_ => { });
                    }
                }
            }
            finally
            {
                HandleDisconnect();
            }
        }

        // ===========================
        // HEARTBEAT
        // ===========================

        private void timerHeartbeat_Tick(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                // alive ping to ESP
                Send(_heartbeatFrame);
            }
        }

        // ===========================
        // DISCONNECT HANDLING
        // ===========================

        private void HandleDisconnect()
        {
            if (!_isConnected) return;

            _isConnected = false;
            timerHeartbeat?.Stop();

            try { _sessionCts?.Cancel(); } catch { }

            CleanupSocket();

            SafeSetStatus("DISCONNECTED", Color.Tomato);
            SafeFireDisconnected();

            Log("Disconnected. Will retry automatically.");
        }

        private void CleanupSocket()
        {
            try { _stream?.Close(); } catch { }
            try { _tcpClient?.Close(); } catch { }

            _stream = null;
            _tcpClient = null;
        }

        // ===========================
        // UI STATUS TICK (BLINK)
        // ===========================

        private void timerUiStatus_Tick(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                if (_packetFlag)
                {
                    _packetFlag = false;
                    _blink = !_blink;
                    var c = _blink ? Color.LimeGreen : Color.Black;
                    SafeSetStatus("RX", c);
                }
                else
                {
                    SafeSetStatus("CONNECTED", Color.LimeGreen);
                }
            }
            else
            {
                SafeSetStatus("DISCONNECTED", Color.Tomato);
            }
        }

        // ===========================
        // THREAD-SAFE UI HELPERS
        // ===========================

        private void SafeSetStatus(string text, Color color)
        {
            if (labelStatus == null) return;

            if (labelStatus.InvokeRequired)
            {
                try
                {
                    labelStatus.Invoke((MethodInvoker)(() =>
                    {
                        labelStatus.Text = text;
                        labelStatus.ForeColor = color;
                    }));
                }
                catch { }
            }
            else
            {
                labelStatus.Text = text;
                labelStatus.ForeColor = color;
            }
        }

        private void Log(string msg)
        {
            if (textBoxLog == null) return;

            string line = $"{DateTime.Now:HH:mm:ss} {msg}\r\n";

            if (textBoxLog.InvokeRequired)
            {
                try
                {
                    textBoxLog.Invoke((MethodInvoker)(() =>
                    {
                        textBoxLog.AppendText(line);
                        textBoxLog.SelectionStart = textBoxLog.TextLength;
                        textBoxLog.ScrollToCaret();
                    }));
                }
                catch { }
            }
            else
            {
                textBoxLog.AppendText(line);
                textBoxLog.SelectionStart = textBoxLog.TextLength;
                textBoxLog.ScrollToCaret();
            }
        }

        // ===========================
        // SAFE EVENT FIRERS
        // ===========================

        private void SafeFireConnected()
        {
            try
            {
                if (Connected != null)
                {
                    if (InvokeRequired)
                        Invoke((MethodInvoker)(() => Connected?.Invoke(this, EventArgs.Empty)));
                    else
                        Connected?.Invoke(this, EventArgs.Empty);
                }
            }
            catch { }
        }

        private void SafeFireDisconnected()
        {
            try
            {
                if (Disconnected != null)
                {
                    if (InvokeRequired)
                        Invoke((MethodInvoker)(() => Disconnected?.Invoke(this, EventArgs.Empty)));
                    else
                        Disconnected?.Invoke(this, EventArgs.Empty);
                }
            }
            catch { }
        }

        private void SafeFireDataReceived()
        {
            try
            {
                if (DataReceived != null)
                {
                    if (InvokeRequired)
                        Invoke((MethodInvoker)(() => DataReceived?.Invoke(this, EventArgs.Empty)));
                    else
                        DataReceived?.Invoke(this, EventArgs.Empty);
                }
            }
            catch { }
        }
    }
}
#include "main.h"

// Set your Static IP address
IPAddress local_IP(192, 168, 0, 117);
// Set your Gateway IP address
IPAddress gateway(192, 168, 0, 1);

IPAddress subnet(255, 255, 255, 0);

static WiFiServer server(TCP_PORT);
static WiFiClient client;

volatile bool wifi_connected = false;
volatile bool tcp_client_connected = false;
volatile bool tcp_rx_ready         = false;

uint8_t tcp_rx_buf[TCP_RX_BUF_SIZE];
size_t  tcp_rx_len = 0;

void wifi_config();
void wifi_init();
void wifi_task();

// === Event handler ===
void WiFiEvent(WiFiEvent_t event) 
{
  switch (event) {
    case ARDUINO_EVENT_WIFI_STA_CONNECTED:
      Serial.println("[WiFi] Connected to AP.");
      break;

    case ARDUINO_EVENT_WIFI_STA_GOT_IP:
      Serial.printf("[WiFi] IP address: %s\n", WiFi.localIP().toString().c_str());
      wifi_connected = true;
      break;

    case ARDUINO_EVENT_WIFI_STA_DISCONNECTED:
      Serial.println("[WiFi] Disconnected! Attempting reconnect...");
      wifi_connected = false;
      WiFi.reconnect();
      break;

    default:
      break;
  }
}

// === Initialization ===
void wifi_init(void) 
{
    WiFi.mode(WIFI_STA);
    WiFi.begin(WIFI_SSID, WIFI_PASSWORD);

    Serial.println();
    Serial.println("[WiFi] Connecting as station...");

    // Small blocking connect loop at startup is OK.
    // After boot we stay non-blocking.
    uint32_t t0 = millis();
    while (WiFi.status() != WL_CONNECTED)
    {
        if (millis() - t0 > 10000) // 10s timeout safety
        {
            Serial.println("[WiFi] Failed to connect, continuing anyway...");
            break;
        }
        delay(200);
        Serial.print(".");
    }
    Serial.println();

    if (WiFi.status() == WL_CONNECTED)
    {
        wifi_connected = true;
        Serial.print("[WiFi] Connected. IP = ");
        Serial.println(WiFi.localIP());
    }
    else
    {
        wifi_connected = false;
    }

    // Start TCP server so the PC can open a socket to ESP32_IP:TCP_PORT
    server.begin();
    server.setNoDelay(true);
    Serial.print("[WiFi] TCP server listening on port ");
    Serial.println(TCP_PORT);

  Serial.println(WiFi.localIP());
}

// === Optional periodic check (state-machine style) ===
void wifi_process(void) 
{
  static uint32_t lastPrint = 0;

  if (millis() - lastPrint > 5000) {
    lastPrint = millis();
    if (wifi_connected)
      Serial.println("[WiFi] Connected and running.");
    else
      Serial.println("[WiFi] Still connecting...");
  }
}
void wifi_config(void)
{
  if (!WiFi.config(local_IP, gateway, subnet)) 
  {
    Serial.println("STA Failed to configure");
  }
}

static void try_accept_client()
{
    if (!tcp_client_connected)
    {
        WiFiClient newClient = server.available();
        if (newClient)
        {
            client = newClient;
            client.setNoDelay(true);
            tcp_client_connected = true;
            Serial.println("[WiFi] Client connected");
        }
    }
    else
    {
        if (!client.connected())
        {
            client.stop();
            tcp_client_connected = false;
            Serial.println("[WiFi] Client disconnected");
        }
    }
}

static void read_from_client_nonblocking()
{
    if (!tcp_client_connected) return;

    // Only capture one "message" per loop() to stay lightweight
    if (client.available() > 0)
    {
        tcp_rx_len = 0;
        while (client.available() && tcp_rx_len < TCP_RX_BUF_SIZE)
        {
            tcp_rx_buf[tcp_rx_len++] = client.read();
        }

        if (tcp_rx_len > 0)
        {
            tcp_rx_ready = true; // <-- main.cpp can act on this
        }
    }
}

void wifi_task()
{
    // Update connection state
    if (WiFi.status() == WL_CONNECTED)
    {
        if (!wifi_connected)
        {
            wifi_connected = true;
            Serial.print("[WiFi] Reconnected. IP = ");
            Serial.println(WiFi.localIP());
        }
    }
    else
    {
        if (wifi_connected)
        {
            wifi_connected = false;
            Serial.println("[WiFi] Lost WiFi");
        }
    }

    // Handle incoming / dropped TCP client
    try_accept_client();

    // Non-blocking RX read from TCP client
    read_from_client_nonblocking();
}

bool tcp_send_bytes(uint8_t *data, size_t len)
{
    if (!tcp_client_connected) return false;
    if (!client.connected())   return false;
    
    GPIO.out_w1ts = (1 << PIN2);  // HIGH

    size_t sent = client.write(data, len);
     
    GPIO.out_w1tc = (1 << PIN2);  // LOW
    return (sent == len);
}

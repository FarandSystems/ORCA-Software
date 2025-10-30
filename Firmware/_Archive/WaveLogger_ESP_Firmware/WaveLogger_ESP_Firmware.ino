#include <Arduino.h>
#include "UartPort.h"
#include "TcpServer.h"

// ---------------- Configuration ----------------
static constexpr int  UART_NUM = 0;
static constexpr int  UART_RX  = 3; // ESP32 GPIO for Artemis TX
static constexpr int  UART_TX  = 1; // ESP32 GPIO for Artemis RX
static constexpr uint32_t UART_BAUD = 115200;

// Choose one of these network modes in setup():
static constexpr char WIFI_SSID[] = "ORCA";
static constexpr char WIFI_PASS[] = "Orca8868";
static constexpr char HOSTNAME [] = "WaveLogger";  // -> WaveLogger.local
static constexpr uint16_t TCP_PORT = 80;
// ------------------------------------------------

static UartPort  gUart;
static TcpServer gTcp;

// UART -> PC (64-byte packets)
static void onUartPacket(const uint8_t* data, size_t len, void*) {
  gTcp.sendToClient(data, len);
}

// PC -> UART (8-byte commands)
static void onTcpCommand(const uint8_t* data, size_t len, void*) {
  gUart.send(data, len);
}

void setup() {
  Serial.begin(115200);
  delay(50);
  Serial.println("\nESP32 UART<->TCP Bridge (non-blocking, mDNS)");

  // Start UART1 to Artemis
  gUart.begin(UART_NUM, UART_BAUD, UART_RX, UART_TX);
  gUart.onPacket(onUartPacket, nullptr);

  // ---- Pick one network mode ----
  // 1) Join existing Wi-Fi (recommended): reach via http://WaveLogger.local/
  gTcp.beginSTA(WIFI_SSID, WIFI_PASS, HOSTNAME, TCP_PORT);

  // 2) Or run your own AP (comment the STA line above and uncomment below)
  // gTcp.beginAP("ESP32_AP", "12345678", TCP_PORT);

  gTcp.onCommand(onTcpCommand, nullptr);
}

void loop() {
  // All work is done in FreeRTOS tasks
  vTaskDelay(1000);
}

#include <WiFi.h>
#include <HardwareSerial.h>

// Set up UART1 for communication with Artemis
HardwareSerial mySerial(0);

// Wi-Fi credentials for the Access Point
const char* ssid     = "ESP32_AP";
const char* password = "12345678";

WiFiServer server(80);

WiFiClient client;  // keep this global so we can check .connected()

uint8_t rx_Buffer[8];
uint8_t tx_Buffer[64];

void setup() {
  // Serial.begin(115200);
  mySerial.begin(115200, SERIAL_8N1, 3, 1);
  mySerial.setTimeout(200);

  WiFi.softAP(ssid, password);
  delay(500);
  // Serial.print("AP IP: "); Serial.println(WiFi.softAPIP());

  server.begin();
}

void loop() {
  // Accept a new client if none
  if (!client || !client.connected()) {
    client = server.available();
    if (client && client.connected()) {
      // Serial.println("Client connected");
    }
  }

  // If we have a connected client, shuttle data both ways
  if (client && client.connected()) {
    // 1) Check if Artemis sent us a 64-byte packet
    if (mySerial.available() >= 64) {
      int got = mySerial.readBytes(tx_Buffer, 64);
      if (got == 64) {
        // Serial.println("UART→TCP (64 bytes)");
        client.write(tx_Buffer, 64);
      }
    }

    // 2) Check if the PC sent us an 8-byte command
    if (client.available() >= 8) {
      int got = client.read(rx_Buffer, 8);
      if (got == 8) {
        // Serial.println("TCP→UART (8 bytes)");
        mySerial.write(rx_Buffer, 8);
      }
    }
  }

  delay(1); // let WiFi background run
}

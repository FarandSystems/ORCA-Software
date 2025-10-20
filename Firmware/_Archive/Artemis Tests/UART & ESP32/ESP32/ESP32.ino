#include <WiFi.h>
#include <HardwareSerial.h>
#include <ESPmDNS.h>

// ---- UART1 on GPIO16 (RX) / GPIO17 (TX) ----
HardwareSerial mySerial(0);
static const int UART_RX = 3;
static const int UART_TX = 1;

// Station WiFi
const char* sta_ssid     = "ORCA";
const char* sta_password = "Orca8868";

WiFiServer server(3333);
WiFiClient client;

uint8_t rx_Buffer[8];
uint8_t tx_Buffer[64];

const int rxPin = 16;  // debug pulse (not UART function)
const int txPin = 17;  // debug pulse (not UART function)
const int ledPin = 2;  // onboard LED

unsigned long last_rx_time = 0;
const unsigned long RX_TIMEOUT = 2000;  // ms, relaxed

const char* hostname = "WaveLogger";

void quickPulse(int pin, int times=1, int us=2000) {
  for (int i=0; i<times; ++i) {
    digitalWrite(pin, HIGH); delayMicroseconds(us);
    digitalWrite(pin, LOW);  delayMicroseconds(us);
  }
}

void setup() {
  // UART1 at 115200, 8N1 on 16/17
  mySerial.begin(115200, SERIAL_8N1, UART_RX, UART_TX);

  pinMode(rxPin, OUTPUT);
  pinMode(txPin, OUTPUT);
  pinMode(ledPin, OUTPUT);
  digitalWrite(rxPin, LOW);
  digitalWrite(txPin, LOW);
  digitalWrite(ledPin, LOW);

  WiFi.mode(WIFI_STA);
  WiFi.begin(sta_ssid, sta_password);
  unsigned long t0 = millis();
  while (WiFi.status() != WL_CONNECTED) {
    delay(200);
    digitalWrite(ledPin, !digitalRead(ledPin));
    if (millis() - t0 > 15000) break;  // 15 s safety timeout
  }
  digitalWrite(ledPin, HIGH);

  if (MDNS.begin(hostname)) {
    // service name "raw" over tcp on 3333 (choose any service name you like)
    MDNS.addService("raw", "tcp", 3333);
  }

  server.begin();
  server.setNoDelay(true);
}

void acceptClientIfNeeded() {
  if (client.connected()) return;
  WiFiClient c = server.available();
  if (c) {
    if (client) client.stop();          // drop old one if any
    client = c;
    client.setNoDelay(true);
    last_rx_time = millis();
    quickPulse(rxPin, 3);
  }
}

inline void bumpActivity() {
  last_rx_time = millis();
  // Optional visual: toggle LED
  digitalWrite(ledPin, !digitalRead(ledPin));
}

void loop() {
  acceptClientIfNeeded();

  if (client.connected()) {
    // ---- UART -> TCP (64B frames) ----
    if (mySerial.available() >= 64) {
      int got = mySerial.readBytes(tx_Buffer, 64);
      if (got == 64) {
        digitalWrite(rxPin, HIGH);
        (void)client.write(tx_Buffer, 64);   // no client.flush()
        digitalWrite(rxPin, LOW);
        bumpActivity();                      // count as traffic
      }
    }

    // ---- TCP -> UART (8B commands) ----
    if (client.available() >= 8) {
      int got = client.read(rx_Buffer, 8);
      if (got == 8) {
        digitalWrite(txPin, HIGH);
        (void)mySerial.write(rx_Buffer, 8);  // no mySerial.flush()
        digitalWrite(txPin, LOW);
        bumpActivity();                      // count as traffic
      } else if (got > 0) {
        // read remainder later (optional: accumulate exactly 8)
      }
    }

    // ---- Idle/timeout ----
    if (millis() - last_rx_time > RX_TIMEOUT) {
      client.stop();
      quickPulse(txPin, 5);
      digitalWrite(ledPin, HIGH); // steady until next connect
    }
  }

  delay(1); // yield to WiFi stack
}

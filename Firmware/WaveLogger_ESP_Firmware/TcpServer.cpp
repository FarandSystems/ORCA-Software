#include "TcpServer.h"
#include <ESPmDNS.h>
#include <string.h>

static void safeCopy(char* dst, size_t dstSize, const char* src) {
  if (!dst || !dstSize) return;
  if (!src) { dst[0] = '\0'; return; }
  snprintf(dst, dstSize, "%s", src); // ensures null-termination
}

void TcpServer::beginAP(const char* ssid, const char* pass, uint16_t port) {
  port_ = port;
  server_ = WiFiServer(port_);
  WiFi.mode(WIFI_AP);
  WiFi.softAP(ssid, pass);
  delay(200);
  server_.begin();
  qTx_ = xQueueCreate(kQDepth, sizeof(Packet));
  // Pin TCP task to core 0 (WiFi core)
  xTaskCreatePinnedToCore(taskTrampoline, "tcp_task", 6144, this, 2, nullptr, 0);
  Serial.print("AP IP: "); Serial.println(WiFi.softAPIP());
}

void TcpServer::beginSTA(const char* ssid, const char* pass, const char* hostname, uint16_t port) {
  port_ = port;
  server_ = WiFiServer(port_);
  WiFi.mode(WIFI_STA);
  WiFi.persistent(false);            // don't write WiFi creds to NVS
  WiFi.setHostname(hostname);        // DHCP hostname
  WiFi.begin(ssid, pass);
  safeCopy(host_, sizeof(host_), hostname);
  staMode_ = true;

  qTx_ = xQueueCreate(kQDepth, sizeof(Packet));
  xTaskCreatePinnedToCore(taskTrampoline, "tcp_task", 6144, this, 2, nullptr, 0);

  Serial.printf("Connecting to %s as %s.local ...\n", ssid, host_);
}

void TcpServer::onCommand(RxCallback cb, void* ctx) {
  cb_ = cb; cbCtx_ = ctx;
}

void TcpServer::sendToClient(const uint8_t* data, size_t len) {
  if (!qTx_ || !data || len == 0) return;
  Packet p{};
  p.len = (len > kPktSize) ? kPktSize : len;
  memcpy(p.data, data, p.len);
  // non-blocking: drop if queue is full
  xQueueSend(qTx_, &p, 0);
}

void TcpServer::taskTrampoline(void* arg) {
  static_cast<TcpServer*>(arg)->task();
}

void TcpServer::task() {
  TickType_t last = xTaskGetTickCount();

  for (;;) {
    // Bring up server + mDNS once STA gets an IP
    if (staMode_ && !serverStarted_ && WiFi.status() == WL_CONNECTED) {
      server_.begin();
      serverStarted_ = true;

      Serial.print("STA IP: "); Serial.println(WiFi.localIP());

      if (MDNS.begin(host_)) {
        MDNS.addService("uartbridge", "tcp", port_);  // custom service
        MDNS.addService("http", "tcp", port_);        // optional/common
        Serial.printf("mDNS started: %s.local\n", host_);
      } else {
        Serial.println("mDNS start FAILED");
      }
    }

    // Detect disconnect and allow recovery
    if (staMode_ && serverStarted_ && WiFi.status() != WL_CONNECTED) {
      serverStarted_ = false;
      // MDNS will be re-initialized on next connection
    }

    // Accept/refresh client
    if (!client_ || !client_.connected()) {
      WiFiClient newc = server_.available();
      if (newc && newc.connected()) {
        if (client_) client_.stop();
        client_ = newc;
        Serial.println("TCP client connected");
        rxIdx_ = 0;
      }
    }

    // Flush packets to client and read 8B commands
    if (client_ && client_.connected()) {
      // TX to client
      Packet out{};
      while (xQueueReceive(qTx_, &out, 0) == pdTRUE) {
        client_.write(out.data, out.len);
      }

      // RX from client -> assemble 8B commands
      while (client_.available()) {
        int b = client_.read();
        if (b < 0) break;
        rxBuf_[rxIdx_++] = static_cast<uint8_t>(b);
        if (rxIdx_ == kCmdSize) {
          Serial.println("Rx: %i", rxBuf_[1]);
          if (cb_) cb_(rxBuf_, kCmdSize, cbCtx_);
          rxIdx_ = 0;
        }
      }
    }

    vTaskDelayUntil(&last, 1); // ~1ms periodic yield
  }
}

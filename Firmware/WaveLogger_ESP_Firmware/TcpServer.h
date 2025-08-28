#pragma once
#include <Arduino.h>
#include <WiFi.h>
#include "Packets.h"

// FreeRTOS
#include "freertos/FreeRTOS.h"
#include "freertos/queue.h"
#include "freertos/task.h"

class TcpServer {
public:
  using RxCallback = void(*)(const uint8_t* data, size_t len, void* ctx);

  void beginAP (const char* ssid, const char* pass, uint16_t port = 80);
  void beginSTA(const char* ssid, const char* pass, const char* hostname, uint16_t port = 80);

  void onCommand(RxCallback cb, void* ctx);
  void sendToClient(const uint8_t* data, size_t len); // thread-safe, non-blocking

private:
  static void taskTrampoline(void* arg);
  void task();

  WiFiServer server_{80};
  WiFiClient client_;

  // RX assembly for fixed 8B commands from PC
  uint8_t rxBuf_[kCmdSize];
  size_t  rxIdx_ = 0;

  QueueHandle_t qTx_ = nullptr; // queue of Packet to send to PC
  RxCallback cb_ = nullptr;
  void* cbCtx_ = nullptr;

  // STA/mDNS state
  bool     staMode_ = false;
  bool     serverStarted_ = false;
  uint16_t port_ = 80;
  char     host_[33] = {0};
};

#pragma once
#include <Arduino.h>
#include <HardwareSerial.h>
#include "Packets.h"

// FreeRTOS
#include "freertos/FreeRTOS.h"
#include "freertos/queue.h"
#include "freertos/task.h"

class UartPort {
public:
  using RxCallback = void(*)(const uint8_t* data, size_t len, void* ctx);

  void begin(int uartNum, uint32_t baud, int rxPin, int txPin);
  void onPacket(RxCallback cb, void* ctx);
  void send(const uint8_t* data, size_t len); // thread-safe, non-blocking

private:
  static void taskTrampoline(void* arg);
  void task();

  HardwareSerial* serial_ = nullptr;
  RxCallback cb_ = nullptr;
  void* cbCtx_ = nullptr;

  QueueHandle_t qTx_ = nullptr; // queue of Packet to transmit

  // RX assembly for fixed 64B packets
  uint8_t rxBuf_[kPktSize];
  size_t  rxIdx_ = 0;
};
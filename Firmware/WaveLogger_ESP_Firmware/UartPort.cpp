#include "UartPort.h"

void UartPort::begin(int uartNum, uint32_t baud, int rxPin, int txPin) {
  serial_ = new HardwareSerial(uartNum);
  serial_->begin(baud, SERIAL_8N1, rxPin, txPin);
  serial_->setRxBufferSize(2048);
  serial_->setTxBufferSize(2048);

  qTx_ = xQueueCreate(kQDepth, sizeof(Packet));
  // Pin UART task to core 1 (keep WiFi on core 0 happy)
  xTaskCreatePinnedToCore(taskTrampoline, "uart_task", 4096, this, 2, nullptr, 1);
}

void UartPort::onPacket(RxCallback cb, void* ctx) {
  cb_ = cb; cbCtx_ = ctx;
}

void UartPort::send(const uint8_t* data, size_t len) {
  if (!qTx_ || !data || len == 0) return;
  Packet p{};
  p.len = (len > kPktSize) ? kPktSize : len;
  memcpy(p.data, data, p.len);
  // non-blocking: drop if queue is full

  xQueueSend(qTx_, &p, 0);
}

void UartPort::taskTrampoline(void* arg) {
  static_cast<UartPort*>(arg)->task();
}

void UartPort::task() {
  TickType_t last = xTaskGetTickCount();
  for (;;) {
    // 1) Drain TX queue to UART
    if (serial_) {
      Packet out{};
      while (xQueueReceive(qTx_, &out, 0) == pdTRUE) {
        serial_->write(out.data, out.len);
      }
    }

    // 2) Assemble RX into fixed 64-byte packets
    while (serial_ && serial_->available()) {
      int b = serial_->read();
      if (b < 0) break;
      rxBuf_[rxIdx_++] = static_cast<uint8_t>(b);
      if (rxIdx_ == kPktSize) {
        if (cb_) cb_(rxBuf_, kPktSize, cbCtx_);
        rxIdx_ = 0;
      }
    }

    // ~1ms periodic yield
    vTaskDelayUntil(&last, 1);
  }
}
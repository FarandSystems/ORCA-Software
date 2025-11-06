#include "main.h"

uint8_t rx_buffer[RX_Buffer_Size];
uint8_t rx_index = 0;
uint8_t tx_buffer[TX_Buffer_Size];

uint8_t encoded_length = 0;
uint8_t uart_timeout_counter = 0;
bool rx_frame_ready = false;

bool uart_reset_request = false;

bool rx_reading_request = false;

enum class RXState : uint8_t { FIND_SYNC, READ_PAYLOAD, READ_CSUM };

void uart_begin(uint32_t baud);
void Reset_UART(void);
uint8_t Calculate_Checksum(uint8_t *buffer, uint8_t length);

void check_rx_ready(void);

void uart_begin(uint32_t baud)
{
  uart_pins_init();
  Serial1.begin(baud, SERIAL_8N1);
  while (!Serial1)
  {
    // wait if needed for USB<->UART bridges; usually quick
  }
  // Serial1.attach(mbed::callback(&Serial1, &UART::rxISR), mbed::SerialBase::RxIrq);
  
  Serial.print("UART Started! at speed:  ");Serial.println(baud);
}

void check_rx_ready()
{
  if (rx_frame_ready) return; // don't overwrite an unread frame

  static RXState  st         = RXState::FIND_SYNC;
  static uint8_t  frame[RX_Buffer_Size];
  static uint8_t  idx        = 0;          // next write index
  static uint32_t last_rx_ms = 0;

  // Only gap-reset if we're mid-frame (not already hunting sync)
  if (st != RXState::FIND_SYNC) {
    uint32_t now = millis();
    if (now - last_rx_ms > RX_GAP_RESET_MS) {
      st  = RXState::FIND_SYNC;
      idx = 0;
      // Serial.println("[UART] gap reset -> FIND_SYNC");
    }
  }

  uint16_t todo = Serial1.available();
  // Serial.println("[UART] bytes available: " + String(todo));

  if (todo > MAX_BYTES_PER_CALL) todo = MAX_BYTES_PER_CALL;

  while (todo--)
  {
    int rb = Serial1.read();
    if (rb < 0) break;
    uint8_t b = (uint8_t)rb;
    last_rx_ms = millis();

    switch (st)
    {
      case RXState::FIND_SYNC:
        if (isSync(b)) {
          frame[0] = b;
          idx = 1;
          st  = RXState::READ_PAYLOAD;
          // Serial.print("[UART] SYNC="); Serial.println(frame[0], HEX);
        }
        break;

      case RXState::READ_PAYLOAD:
        frame[idx++] = b;
        if (idx == RX_Buffer_Size - 1) {
          st = RXState::READ_CSUM;
          // Serial.println("[UART] → READ_CSUM");
        }
        break;

      case RXState::READ_CSUM:
      {
        frame[RX_Buffer_Size - 1] = b;

        // FIX 1: use full length; function already sums [0..len-2]
        uint8_t cs = Calculate_Checksum(frame, RX_Buffer_Size);
        bool ok = (cs == frame[RX_Buffer_Size - 1]);

        // Serial.print("[UART] CS exp="); Serial.print(cs, HEX);
        // Serial.print(" got=");          Serial.print(frame[RX_Buffer_Size - 1], HEX);
        // Serial.print(" -> ");           Serial.println(ok ? "OK" : "FAIL");

        if (ok) 
        {
          memcpy(rx_buffer, frame, RX_Buffer_Size);
          rx_frame_ready = true;
          uart_timeout_counter = 0;

          // Serial.print("[UART] FRAME OK: ");
          // for (uint8_t i = 0; i < RX_Buffer_Size; i++) {
          //   if (frame[i] < 16) Serial.print('0');
          //   Serial.print(frame[i], HEX);
          //   if (i + 1 < RX_Buffer_Size) Serial.print(' ');
        // }
          // Serial.println();
        } 
        else 
        {
          // Serial.println("[UART] FRAME BAD");

          // FIX 2: DO NOT reuse the checksum byte as sync.
          // Instead, quickly purge bytes until next sync is at the head.
          // (Non-blocking: only consume what’s already buffered.)
          while (Serial1.available() > 0) 
          {
            int peekb = Serial1.peek();
            if (peekb < 0) break;
            if (isSync((uint8_t)peekb)) break;   // stop when the next byte is a sync
            (void)Serial1.read();                // discard one byte and keep looking
          }
        }

        // Reset to hunt next frame
        st  = RXState::FIND_SYNC;
        idx = 0;
        // Serial.println("[UART] → FIND_SYNC");
      } break;
    }
  }
}


uint8_t Calculate_Checksum(uint8_t *buffer, uint8_t length)
{
  uint8_t sum = 0;
  for(uint8_t i = 0; i < length - 1; i++)
  {
    sum += buffer[i];
  }
  
  return sum;
}

void Reset_UART(void)
{
  Alarm(SHORT_BEEP_X2, 1, 40, 1);
  Serial.println("Reseting UART!");
  // DeInit
  Serial1.flush();                   // wait for TX to finish
  while (Serial1.available()) (void)Serial1.read(); // drain RX
  // Init
  Serial1.end();
  delay(5);
  uart_begin(UART_BAUDRATE);
  // Optionally clear any stale RX
  while (Serial1.available()) (void)Serial1.read();
}

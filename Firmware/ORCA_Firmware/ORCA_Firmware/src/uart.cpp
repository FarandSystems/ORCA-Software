#include "main.h"

uint8_t rx_buffer[RX_Buffer_Size];
uint8_t rx_index = 0;
uint8_t tx_buffer[TX_Buffer_Size];

uint8_t encoded_length = 0;
uint8_t uart_timeout_counter = 0;

bool uart_reset_request = false;

void uart_begin(uint32_t baud);
void Reset_UART(void);
uint8_t Calculate_Checksum(uint8_t *buffer, uint8_t length);

void uart_begin(uint32_t baud)
{
  uart_pins_init();
  Serial1.begin(baud);
  while (!Serial1)
  {
    // wait if needed for USB<->UART bridges; usually quick
  }
  
  Serial.print("UART Started! at speed:  ");Serial.println(baud);
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

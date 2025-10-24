#include "main.h"

uint8_t rx_buffer[RX_Buffer_Size];
uint8_t rx_index = 0;
uint8_t tx_buffer[TX_Buffer_Size];


void uart_begin(uint32_t baud);
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
  for(uint8_t i; i < length - 1; i++)
  {
    sum += buffer[i];
  }
  return sum;
}

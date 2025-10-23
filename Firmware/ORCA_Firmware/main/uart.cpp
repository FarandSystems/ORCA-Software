#include "main.h"


void uart_begin(uint32_t baud);


void uart_begin(uint32_t baud)
{
  uart_pins_init();
  Serial1.begin(baud);
  while (!Serial1)
  {
    // wait if needed for USB<->UART bridges; usually quick
  }
  
  Serial.print("UART Started! at speed:  ");Serial.println(baud)
}

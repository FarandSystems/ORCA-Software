#include "main.h"

bool rx_frame_ready = false;

bool report_to_pc_ready = false;

bool is_power_requested = false;

void check_rx_ready(void);
void Service_Input_Command(uint8_t* RxBuffer);
void Report_To_PC(void);
uint8_t Encode_0x1A(uint8_t *input, uint8_t length, uint8_t *output);

void check_rx_ready()
{
  while (Serial1.available()) 
  {
    uint8_t b = Serial1.read();
    rx_buffer[rx_index++] = b;

    // When 8 bytes are collected → mark frame ready
    if (rx_index >= RX_Buffer_Size) 
    {
      if (rx_buffer[RX_Buffer_Size - 1] == Calculate_Checksum(rx_buffer, RX_Buffer_Size))
      {
        rx_frame_ready = true;
      }
      rx_index = 0;
    }
  }
}

void Service_Input_Command(uint8_t* RxBuffer)
{
  switch (RxBuffer[1])
  {
    case 0x00:
      // Nothing
      break;
    case 0x01:
      is_power_requested = true;
      is_qwiic_on = true;
      am_hal_gpio_output_toggle(pin_LED);
      // am_hal_gpio_output_toggle(pin_Buzzer);
      break;
    case 0x02:
      is_power_requested = true;
      is_qwiic_on = false;
      am_hal_gpio_output_toggle(pin_LED);
      // am_hal_gpio_output_toggle(pin_Buzzer);
      break;
    case 0x04:
      am_hal_gpio_output_toggle(pin_LED);
      // am_hal_gpio_output_toggle(pin_Buzzer);
      break;
    case 0x05:
      am_hal_gpio_output_toggle(pin_LED);
      // am_hal_gpio_output_toggle(pin_Buzzer);
      break;
  }
}

void Report_To_PC()
{
  // Ramp
  for (uint8_t i = 0; i < TX_Buffer_Size - 1; i++)
  {
    tx_buffer[i] = i;  // 0x00, 0x01, 0x02, ...
  }
  
  uint8_t cs = Calculate_Checksum(tx_buffer, TX_Buffer_Size);
  tx_buffer[TX_Buffer_Size - 1] = cs;
  
  report_to_pc_ready = true;
}

uint8_t Encode_0x1A(uint8_t *input, uint8_t length, uint8_t *output)
{
    uint8_t j = 0;
    for (uint8_t i = 0; i < length; i++)
    {
        if (input[i] == 0x1A)
        {
            output[j++] = 0x1A;
            output[j++] = 0x00;
        }
        else if (input[i] == 0x1B)
        {
            output[j++] = 0x1A;
            output[j++] = 0x01;
        }
        else if (input[i] == 0xC0)
        {
            output[j++] = 0x1A;
            output[j++] = 0x02;
        }
        else
        {
            output[j++] = input[i];
        }
    }
    return j; // returns encoded length
}

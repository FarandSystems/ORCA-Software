#include "main.h"

void setup() 
{
  am_hal_cachectrl_config(&am_hal_cachectrl_defaults);
  am_hal_cachectrl_enable();
  am_hal_clkgen_control(AM_HAL_CLKGEN_CONTROL_SYSCLK_MAX, 0);
  am_hal_sysctrl_fpu_enable();
  am_hal_sysctrl_fpu_stacking_enable(true);
  Serial.begin(115200);
  setup_GPIO();

  setup_i2c();

  uart_begin(115200);

  // Initialize and configure the single 800 Hz timer + counters
  setupTimers();
}

void loop() 
{
  if(is_800hz_Timer_Int_Ready)
  {
    is_800hz_Timer_Int_Ready = false;
    read_IMU();
  }

  while (Serial1.available()) 
  {
    uint8_t b = Serial1.read();
    rx_buffer[rx_index++] = b;

    // When 8 bytes are collected → mark frame ready
    if (rx_index >= RX_Buffer_Size) 
    {
      if (rx_buffer[RX_Buffer_Size - 1] == Calculate_Checksum(rx_buffer, RX_Buffer_Size))
      {
        am_hal_gpio_output_toggle(pin_LED);
        
        frame_ready = true;
      }
      rx_index = 0;
    }
  }
}

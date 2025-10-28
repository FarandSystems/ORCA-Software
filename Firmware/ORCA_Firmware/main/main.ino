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
  disable_iridium();
  setup_i2c();

  uart_begin(9600);

  // Initialize and configure the single 800 Hz timer + counters
  setupTimers();
}

void loop() 
{
  // Check Timer 800Hz Int for reading IMU
  if(is_800hz_Timer_Int_Ready)
  {
    is_800hz_Timer_Int_Ready = false;
    read_IMU();
  }

  // Check if Power Commands received
  if(is_power_requested)
  {
    is_power_requested = false;
    Switch_Qwiic(is_qwiic_on);
  }
  
  // Check for any receiving RX Data
  check_rx_ready();

  // Check if we should send data to PC
  if(report_to_pc_ready)
  {
    report_to_pc_ready = false;
    Serial1.write(tx_buffer, TX_Buffer_Size);
  }
}

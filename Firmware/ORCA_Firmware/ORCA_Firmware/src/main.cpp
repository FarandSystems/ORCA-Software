#include "main.h"

System_State system_state;

uint8_t fixType = 0;
uint8_t numSV   = 0;

void setup() 
{
  am_hal_cachectrl_config(&am_hal_cachectrl_defaults);
  am_hal_cachectrl_enable();
  am_hal_clkgen_control(AM_HAL_CLKGEN_CONTROL_SYSCLK_MAX, 0);
  am_hal_sysctrl_fpu_enable();
  am_hal_sysctrl_fpu_stacking_enable(true);


  Serial.begin(115200);
  setup_GPIO();
  Alarm_Init(pin_Buzzer, true);
  setup_i2c();
  
  // Initialize and configure the single 800 Hz timer + counters
  setupTimers();
  Alarm(SHORT_BEEP_X2, 1, 40, 1);
  uart_begin(UART_BAUDRATE);

  

  system_state = STARTUP_STATE;
}

void loop() 
{
  // Handle UART Commands like Send/Receive and RESET
  Handle_UART_Commands();

  switch (system_state)
  {

    case STARTUP_STATE:

      Alarm(SHORT_BEEP_X1, 1, 40, 1);
      system_state = GNSS_ACQUIRE_STATE;
      break;
    
    case GNSS_ACQUIRE_STATE:

      // Send this command via PC
      if (is_skip_gnss_requested)
      {
        is_skip_gnss_requested = false;
        system_state = NORMAL_STATE;
      }

      // Only logic difference: we ignore GNSS data until it has a fix
      fixType = gnss.getFixType();
      numSV   = gnss.getSIV();


      if ((fixType >= 3) && (numSV >= 4))
      {
        Read_Gnss();
        
        system_state = NORMAL_STATE;
      }

      break;

    case NORMAL_STATE:
      Alarm(MEDIUM_BEEP_X1, 1, 40, 1);
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

      if(is_1hz_Timer_Int_Ready)
      {
        is_1hz_Timer_Int_Ready = false;
        
        //Read_Gnss();
      }

      break;

    case ERROR_STATE:
      break;
    
    default:
      break;
  }


 
}

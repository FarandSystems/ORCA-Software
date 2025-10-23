// tim.cpp (or add to main - updated ISR with counters + functions)
#include "main.h"

uint32_t counter_100hz = 0;
uint32_t counter_8hz = 0;
uint32_t counter_1hz = 0;

uint32_t counter_800 = 0;

bool is_800hz_Timer_Int_Ready = false;

extern "C" void Timer_ISR_800Hz(void) 
{
  am_hal_gpio_output_toggle(pin_800); // Toggle pin for 800Hz

  is_800hz_Timer_Int_Ready = true;

  // NEW: Increment counters every 800 Hz tick
  counter_100hz++;
  counter_8hz++;
  counter_1hz++;

  // Check for 100 Hz: Every 8 ticks (800 / 100 = 8)
  if (counter_100hz >= 8) 
  {
    counter_100hz = 0;  // Reset to 0
    on_100hz_tick();    // Call the 100 Hz function
  }

  // Check for 8 Hz: Every 100 ticks (800 / 8 = 100)
  if (counter_8hz >= 100) 
  {
    counter_8hz = 0;    // Reset to 0
    on_8hz_tick();      // Call the 8 Hz function
  }

  // Check for 1 Hz: Every 800 ticks (800 / 1 = 800)
  if (counter_1hz >= 800) 
  {
    counter_1hz = 0;    // Reset to 0
    on_1hz_tick();      // Call the 1 Hz function
  }

  // Clear the interrupt bit
  am_hal_ctimer_int_clear(AM_HAL_CTIMER_INT_TIMERA0C0);
  
}

// Configure ONLY the 800 Hz timer (high-freq clock)
void configureTimer(uint32_t timer, uint32_t frequency) 
{
  uint32_t interval;
  uint32_t clockCfg;
  uint32_t intBit = AM_HAL_CTIMER_INT_TIMERA0C0;  // Fixed for timer 0

  // High freq: Use 12 MHz clock
  clockCfg = AM_HAL_CTIMER_HFRC_12MHZ;
  uint32_t timer_freq = 12000000;
  interval = timer_freq / frequency;  // ~15000 for 800 Hz
  if (interval > 65535) interval = 65535;

  // Configure single segment (A) in repeat mode with interrupt
  am_hal_ctimer_config_single(timer, AM_HAL_CTIMER_TIMERA,
                              clockCfg |
                              AM_HAL_CTIMER_FN_REPEAT |
                              AM_HAL_CTIMER_INT_ENABLE);

  // Set period for periodic interrupts
  am_hal_ctimer_period_set(timer, AM_HAL_CTIMER_TIMERA, interval, 0);

  // Clear timer and interrupt
  am_hal_ctimer_clear(timer, AM_HAL_CTIMER_TIMERA);
  am_hal_ctimer_int_clear(intBit);

  // Enable specific interrupt bit and register ISR
  am_hal_ctimer_int_enable(intBit);
  am_hal_ctimer_int_register(AM_HAL_CTIMER_INT_TIMERA0C0, Timer_ISR_800Hz);
}
// Updated wrapper function (only configures 800 Hz timer)
void setupTimers() 
{
  // Enable global CTIMER peripheral (HFRC auto-starts)
  am_hal_ctimer_globen(1);

  // Configure ONLY the 800 Hz timer (CTIMER0)
  configureTimer(0, 796);  // Uses HFRC_12MHZ, interval ~15000 ticks

  // Start the timer
  am_hal_ctimer_start(0, AM_HAL_CTIMER_TIMERA);

  NVIC_SetPriority(CTIMER_IRQn, 5);  // lower prio than IOM
  // Enable the shared CTIMER NVIC IRQ
  NVIC_EnableIRQ(CTIMER_IRQn);
  Serial.println("800 Hz CTIMER enabled with software counters - Check pins with scope!");
}

// NEW: Implement these functions with your logic (e.g., toggle pins, set flags, etc.)
void on_100hz_tick(void) 
{
//  am_hal_gpio_output_toggle(pin_100);  // Example: Toggle pin for 100Hz
  // Add more 100 Hz logic here (e.g., Serial.println("100Hz!"); but keep short)
}

void on_8hz_tick(void) 
{
  // digitalWrite(pin_8, !digitalRead(pin_8));  // Example: Toggle pin for 8Hz
  // Add more 8 Hz logic here
}

void on_1hz_tick(void) 
{
//  am_hal_gpio_output_toggle(pin_1);  // Example: Toggle pin for 1Hz
  // Add more 1 Hz logic here
}

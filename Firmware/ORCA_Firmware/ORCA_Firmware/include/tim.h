// tim.h (updated - remove old ISR decls/flags, add new)
#ifndef TIM_H
#define TIM_H
#include <Arduino.h>
#include <am_hal_ctimer.h>     // CTIMER HAL
#include <am_hal_interrupt.h>  // Interrupt HAL (NVIC)
#include <am_hal_sysctrl.h>    // For clocks (HFRC auto-enabled)

extern bool is_800hz_Timer_Int_Ready;

extern uint32_t counter_800;
extern uint32_t counter_100hz;
extern uint32_t counter_5hz;
extern uint32_t counter_1hz;

// Timer ISR declaration (only 800 Hz)
extern "C" 
{
  void Timer_ISR_800Hz(void);
}

// Function to configure the 800 Hz timer
void configureTimer(uint32_t timer, uint32_t frequency);

// NEW: Function prototypes for lower-freq actions (implement in tim.cpp or main)
void on_100hz_tick(void);
void on_5hz_tick(void);
void on_1hz_tick(void);

// Wrapper for full setup
extern void setupTimers(void);

#endif // TIM_H

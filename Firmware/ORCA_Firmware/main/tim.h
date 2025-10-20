// tim.h (updated - remove old ISR decls/flags, add new)
#ifndef TIM_H
#define TIM_H
#include <Arduino.h>

// Pin definitions (extern - defined in main)
extern uint8_t pin_800;
extern uint8_t pin_100;
extern uint8_t pin_8;
extern uint8_t pin_1;

// Declare interrupt flag (only for 800 Hz)
extern volatile bool interrupt800Hz;

// NEW: Software counters (extern - defined in main)
extern volatile uint32_t counter_100hz;
extern volatile uint32_t counter_8hz;
extern volatile uint32_t counter_1hz;

// Timer ISR declaration (only 800 Hz)
extern "C" {
  void Timer_ISR_800Hz(void);
}

// Function to configure the 800 Hz timer
void configureTimer(uint32_t timer, uint32_t frequency);

// NEW: Function prototypes for lower-freq actions (implement in tim.cpp or main)
void on_100hz_tick(void);
void on_8hz_tick(void);
void on_1hz_tick(void);

// Wrapper for full setup
void setupTimers(void);

#endif // TIM_H

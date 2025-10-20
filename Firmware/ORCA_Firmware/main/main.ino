#include <Arduino.h>
#include <am_hal_sysctrl.h>    // For clocks (HFRC auto-enabled)
#include <am_hal_ctimer.h>     // CTIMER HAL
#include <am_hal_interrupt.h>  // Interrupt HAL (NVIC)
#include "tim.h"               // Your timer header

// Pin definitions
uint8_t pin_800 = 42;
uint8_t pin_100 = 43;
uint8_t pin_8 = 37;
uint8_t pin_1 = 4;

// Interrupt flag for 800 Hz only (others handled via counters)
volatile bool interrupt800Hz = false;

// NEW: Software counters for lower frequencies (volatile for ISR access)
volatile uint32_t counter_100hz = 0;
volatile uint32_t counter_8hz = 0;
volatile uint32_t counter_1hz = 0;

// Function prototypes for lower-freq actions (called from ISR when counter % N == 0)
void on_100hz_tick(void);
void on_8hz_tick(void);
void on_1hz_tick(void);

void setup() {
  Serial.begin(115200);
  while (!Serial) {}  // Wait for Serial

  // Set the pins as outputs
  pinMode(pin_800, OUTPUT);
  pinMode(pin_100, OUTPUT);
  pinMode(pin_8, OUTPUT);
  pinMode(pin_1, OUTPUT);

  // Initialize and configure the single 800 Hz timer + counters
  setupTimers();
}

void loop() {
  // Check the 800 Hz flag (if needed for other handling)
  if (interrupt800Hz) {
    interrupt800Hz = false;
    // Handle 800Hz interrupt (already toggled in ISR)
    // e.g., Serial.println("800Hz tick!");
  }

  // No need for other flags—lower freq actions are called directly in ISR
  // Add non-time-critical loop code here if needed
}

// Updated wrapper function (only configures 800 Hz timer)
void setupTimers() {
  // Enable global CTIMER peripheral (HFRC auto-starts)
  am_hal_ctimer_globen(1);

  // Configure ONLY the 800 Hz timer (CTIMER0)
  configureTimer(0, 800);  // Uses HFRC_12MHZ, interval ~15000 ticks

  // Start the timer
  am_hal_ctimer_start(0, AM_HAL_CTIMER_TIMERA);

  // Enable the shared CTIMER NVIC IRQ
  NVIC_EnableIRQ(CTIMER_IRQn);
  Serial.println("800 Hz CTIMER enabled with software counters - Check pins with scope!");
}

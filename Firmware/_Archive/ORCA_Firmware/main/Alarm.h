#pragma once
#include <Arduino.h>
#include <stdint.h>

// ======= Pattern definitions (same as your STM32) =======
#define MEDIUM_BEEP_X1         0x000000FFUL
#define MEDIUM_BEEP_X2         0x00FF00FFUL
#define LONG_BEEP              0x0000FFFFUL
#define VERY_LONG_BEEP         0xFFFFFFFFUL
#define EMERGENCY_BEEP         0x0F0F0F0FUL
#define PULSE_TRAIN_BEEP       0x33333333UL
#define SHORT_BEEP_X1          0x00000003UL
#define SHORT_BEEP_X2          0x00000033UL
#define SHORT_BEEP_X3          0x00000333UL
#define SHORT_BEEP_X4          0x00003333UL
#define DELAYED_SHORT_BEEP_X1  0xC0000000UL
#define DELAYED_SHORT_BEEP_X2  0xCC000000UL
#define VERYSHORT_BEEP_X1      0x00000001UL

#define BEEP_ON   0x01
#define BEEP_OFF  0x00

#ifndef ALARM_MESSAGE_QUEUE_SIZE
#  define ALARM_MESSAGE_QUEUE_SIZE 32
#endif

// Enqueued alarm message
typedef struct {
  uint32_t pattern;     // 32-bit bitfield, LSB is the current time slice
  uint8_t  repeats;     // how many times to replay the pattern
  uint8_t  timePeriod;  // number of slices to advance before counting a repeat (usually 32)
  uint8_t  beepOnOff;   // BEEP_ON/BEEP_OFF (if OFF, pattern runs but output muted)
} AlarmMessage;

// ======= API =======
// Call once to set up the buzzer pin. If your buzzer is active-low, pass activeHigh=false.
void Alarm_Init(uint8_t buzzerPin, bool activeHigh = true);

// Enqueue an alarm (compat name kept): Alarm(PATTERN, COUNT, TIMEPERIOD, BEEP_ON/OFF)
bool Alarm(uint32_t pattern, uint8_t repeats, uint8_t timePeriod, uint8_t beepOnOff);

// Call at ~32 Hz (31–32 ms period). This advances the pattern and drives the pin.
void Alarm_Update_32Hz(void);

// Utility
void Alarm_ClearQueue(void);
bool Alarm_IsActive(void);

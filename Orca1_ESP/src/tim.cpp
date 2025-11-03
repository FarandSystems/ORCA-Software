#include "main.h"

void init_timers();


hw_timer_t *timer1 = nullptr; //Timer 1
hw_timer_t *timer2 = nullptr; // Timer 2

// === ISR for Timer 1 ===
void IRAM_ATTR onTimer1() //
{
  // Fastest possible pin toggle: direct register access
  GPIO.out_w1ts = (1 << PIN1);  // HIGH
  GPIO.out_w1tc = (1 << PIN1);  // LOW

  uart_tx_ready = true; //Start to send data
  

  // static bool state1 = false;
  // if (state1)
  //   GPIO.out_w1ts = (1 << PIN1);  // HIGH
  // else
  //   GPIO.out_w1tc = (1 << PIN1);  // LOW
  // state1 = !state1;
  // That creates a short HIGH pulse (~100 ns wide)
}

// === ISR for Timer 2 ===
void IRAM_ATTR onTimer2()
{
  // static bool state2 = false;
  // if (state2)
  //   GPIO.out_w1ts = (1 << PIN2);  // HIGH
  // else
  //   GPIO.out_w1tc = (1 << PIN2);  // LOW
  // state2 = !state2;
}

void init_timers()
{
// === Timer 1 setup: 8 Hz === 125ms
  timer1 = timerBegin(0, 80, true);         // 1 µs tick
  timerAttachInterrupt(timer1, &onTimer1, true);
  timerAlarmWrite(timer1, 125000, true);      // 1000 µs = 1 kHz
  timerAlarmEnable(timer1);

  // === Timer 2 setup: 2 kHz ===
  timer2 = timerBegin(1, 80, true);
  timerAttachInterrupt(timer2, &onTimer2, true);
  timerAlarmWrite(timer2, 500, true);       // 500 µs = 2 kHz
  timerAlarmEnable(timer2);
}

  
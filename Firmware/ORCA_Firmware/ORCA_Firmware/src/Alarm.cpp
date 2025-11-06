#include "main.h"

// ---------- Ring buffer ----------
static AlarmMessage s_queue[ALARM_MESSAGE_QUEUE_SIZE];
static volatile uint8_t s_head = 0, s_tail = 0;
static inline bool q_empty() { return s_head == s_tail; }
static inline bool q_full () { return (uint8_t)(s_head + 1) == s_tail; }
static inline void q_push(const AlarmMessage& m){ s_queue[s_head]=m; s_head=(uint8_t)(s_head+1); }
static inline AlarmMessage& q_front(){ return s_queue[s_tail]; }
static inline void q_pop(){ s_tail=(uint8_t)(s_tail+1); }

// ---------- Playback state ----------
static bool      s_active       = false;
static uint32_t  s_pattern_orig = 0;   // <— keep original
static uint32_t  s_pattern_shift= 0;   // shifts each tick
static uint8_t   s_bits_done    = 0;   // slices consumed in this repeat
static uint8_t   s_repeats_left = 0;
static uint8_t   s_time_period  = 32;
static uint8_t   s_beep_flag    = BEEP_ON;

static uint8_t   s_buzzer_pin   = 255;
static bool      s_active_high  = true;

static void buzzerWrite(bool on){
  if (s_buzzer_pin == 255) return;
  uint8_t level = (on ^ !s_active_high) ? am_hal_gpio_output_set(s_buzzer_pin) : am_hal_gpio_output_clear(s_buzzer_pin); // invert if active-low
}

static void start_next(){
  if (q_empty()){
    s_active=false; buzzerWrite(false); return;
  }
  AlarmMessage m = q_front(); q_pop();
  if (m.timePeriod==0 || m.timePeriod>32) m.timePeriod=32;

  s_pattern_orig  = m.pattern;
  s_pattern_shift = m.pattern;
  s_repeats_left  = (m.repeats? m.repeats : 1);
  s_time_period   = m.timePeriod;
  s_beep_flag     = (m.beepOnOff? BEEP_ON : BEEP_OFF);
  s_bits_done     = 0;
  s_active        = true;
}

void Alarm_Init(uint8_t pin, bool activeHigh){
  s_buzzer_pin = pin; s_active_high = activeHigh;
  if (s_buzzer_pin != 255){ pinMode(s_buzzer_pin, OUTPUT); buzzerWrite(false); }
  s_head = s_tail = 0; s_active=false;
}

bool Alarm(uint32_t pattern, uint8_t repeats, uint8_t timePeriod, uint8_t beepOnOff){
  AlarmMessage m{};
  m.pattern    = pattern;
  m.repeats    = (repeats? repeats:1);
  m.timePeriod = (timePeriod==0 || timePeriod>32)? 32 : timePeriod;
  m.beepOnOff  = (beepOnOff? BEEP_ON : BEEP_OFF);

  if (q_full()) q_pop(); // drop oldest to make room
  q_push(m);
  if (!s_active) start_next();
  return true;
}

void Alarm_Update_32Hz(void){
  if (!s_active){ start_next(); return; }

  // Drive current slice
  bool bit_on = (s_pattern_shift & 1u) && (s_beep_flag == BEEP_ON);
  buzzerWrite(bit_on);

  // Advance
  s_pattern_shift >>= 1;
  s_bits_done++;

  if (s_bits_done >= s_time_period){
    s_bits_done = 0;
    if (s_repeats_left > 0) s_repeats_left--;
    if (s_repeats_left > 0){
      // reload for next repeat
      s_pattern_shift = s_pattern_orig;
    } else {
      // finished this message
      s_active = false;
      buzzerWrite(false);
      start_next();
    }
  }
}

void Alarm_ClearQueue(void){
  s_head = s_tail = 0; s_active=false; buzzerWrite(false);
}

bool Alarm_IsActive(void){ return s_active || !q_empty(); }

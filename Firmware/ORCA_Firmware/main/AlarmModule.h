#ifndef ALARM_MODULE_H
#define ALARM_MODULE_H

#include <Arduino.h>
#include <mbed.h>
#include "I2CModule.h"

using namespace rtos;
using namespace std::chrono;

#define ALARM_MESSAGE_QUEUE_SIZE 32
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

typedef struct {
    uint32_t pattern;
    uint8_t repeats;
    uint8_t timePeriod;
    uint8_t beepOnOff;
} AlarmMessage;

class AlarmModule : public Tickable
{
public:
    AlarmModule(uint32_t trigger_every_n_ticks, osPriority priority = osPriorityAboveNormal);
    bool init();
    void start();
    virtual void onTick() override;
    bool enqueue(uint32_t pattern, uint8_t repeats, uint8_t time_period, uint8_t beep_on_off);
    void clearQueue();
    bool isActive();

private:
    void alarmThread();
    void buzzerWrite(bool on);
    void startNext();

    static constexpr uint8_t BUZZER_PIN = 42;
    static constexpr bool ACTIVE_HIGH = true;
    uint32_t m_trigger_every_n_ticks;
    uint32_t m_tick_count;
    Thread m_thread;
    Semaphore m_trigger_sem;
    AlarmMessage m_queue[ALARM_MESSAGE_QUEUE_SIZE];
    volatile uint8_t m_head;
    volatile uint8_t m_tail;
    bool m_active;
    uint32_t m_pattern_orig;
    uint32_t m_pattern_shift;
    uint8_t m_bits_done;
    uint8_t m_repeats_left;
    uint8_t m_time_period;
    uint8_t m_beep_flag;
};

#endif // ALARM_MODULE_H
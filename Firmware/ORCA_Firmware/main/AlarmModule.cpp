#include "AlarmModule.h"

AlarmModule::AlarmModule(uint32_t trigger_every_n_ticks, osPriority priority)
    : m_trigger_every_n_ticks(trigger_every_n_ticks),
      m_tick_count(0),
      m_thread(priority, 1024),
      m_trigger_sem(0),
      m_head(0),
      m_tail(0),
      m_active(false),
      m_pattern_orig(0),
      m_pattern_shift(0),
      m_bits_done(0),
      m_repeats_left(0),
      m_time_period(32),
      m_beep_flag(BEEP_ON)
{
}

bool AlarmModule::init()
{
    pinMode(BUZZER_PIN, OUTPUT);
    buzzerWrite(false);
    return true;
}

void AlarmModule::start()
{
    m_thread.start(mbed::callback(this, &AlarmModule::alarmThread));
}

void AlarmModule::onTick()
{
    m_tick_count++;
    if (m_tick_count >= m_trigger_every_n_ticks)
    {
        m_tick_count = 0;
        m_trigger_sem.release();
    }
}

void AlarmModule::alarmThread()
{
    while (true)
    {
        m_trigger_sem.acquire();
        if (!m_active)
        {
            startNext();
            continue;
        }

        bool bit_on = (m_pattern_shift & 1u) && (m_beep_flag == BEEP_ON);
        buzzerWrite(bit_on);

        m_pattern_shift >>= 1;
        m_bits_done++;

        if (m_bits_done >= m_time_period)
        {
            m_bits_done = 0;
            if (m_repeats_left > 0)
            {
                m_repeats_left--;
            }
            if (m_repeats_left > 0)
            {
                m_pattern_shift = m_pattern_orig;
            }
            else
            {
                m_active = false;
                buzzerWrite(false);
                startNext();
            }
        }
    }
}

void AlarmModule::buzzerWrite(bool on)
{
    uint8_t level = (on ^ !ACTIVE_HIGH) ? HIGH : LOW;
    digitalWrite(BUZZER_PIN, level);
}

void AlarmModule::startNext()
{
    if (m_head == m_tail)
    {
        m_active = false;
        buzzerWrite(false);
        return;
    }
    AlarmMessage m = m_queue[m_tail];
    m_tail = (uint8_t)(m_tail + 1);
    if (m.timePeriod == 0 || m.timePeriod > 32)
    {
        m.timePeriod = 32;
    }

    m_pattern_orig = m.pattern;
    m_pattern_shift = m.pattern;
    m_repeats_left = (m.repeats ? m.repeats : 1);
    m_time_period = m.timePeriod;
    m_beep_flag = (m.beepOnOff ? BEEP_ON : BEEP_OFF);
    m_bits_done = 0;
    m_active = true;
}

bool AlarmModule::enqueue(uint32_t pattern, uint8_t repeats, uint8_t time_period, uint8_t beep_on_off)
{
    AlarmMessage m{};
    m.pattern = pattern;
    m.repeats = (repeats ? repeats : 1);
    m.timePeriod = (time_period == 0 || time_period > 32) ? 32 : time_period;
    m.beepOnOff = (beep_on_off ? BEEP_ON : BEEP_OFF);

    if ((uint8_t)(m_head + 1) == m_tail)
    {
        m_tail = (uint8_t)(m_tail + 1);
    }
    m_queue[m_head] = m;
    m_head = (uint8_t)(m_head + 1);
    if (!m_active)
    {
        startNext();
    }
    return true;
}

void AlarmModule::clearQueue()
{
    m_head = m_tail = 0;
    m_active = false;
    buzzerWrite(false);
}

bool AlarmModule::isActive()
{
    return m_active || (m_head != m_tail);
}
#include "SensorsModule.h"

SensorModule::SensorModule(I2CModule& i2c_module, uint32_t trigger_every_n_ticks, osPriority priority)
    : m_i2c_module(i2c_module),
      m_trigger_every_n_ticks(trigger_every_n_ticks),
      m_tick_count(0),
      m_thread(priority, 1536),
      m_trigger_sem(0),
      m_ms8607_state(MS8607State::IDLE),
      m_ms8607_wait_ticks(0),
      m_ms8607_temp(0.0f)
{
}

bool SensorModule::init()
{
    if (!m_i2c_module.writeByte(LIS3MDL_ADDRESS, 0x20, 0x70)) return false;
    if (!m_i2c_module.writeByte(LIS3MDL_ADDRESS, 0x21, 0x00)) return false;
    if (!m_i2c_module.writeByte(LIS3MDL_ADDRESS, 0x22, 0x00)) return false;
    if (!m_i2c_module.writeByte(LIS3MDL_ADDRESS, 0x23, 0x0C)) return false;

    if (!m_i2c_module.writeByte(MS8607_ADDRESS, 0x1E, 0x00)) return false;
    return true;
}

void SensorModule::start()
{
    m_thread.start(mbed::callback(this, &SensorModule::sensorThread));
}

void SensorModule::onTick()
{
    m_tick_count++;
    if (m_tick_count >= m_trigger_every_n_ticks)
    {
        m_tick_count = 0;
        m_trigger_sem.release();
    }
}

void SensorModule::sensorThread()
{
    while (true)
    {
        m_trigger_sem.acquire();
        readLIS3MDL();
        readMS8607();
    }
}

void SensorModule::readLIS3MDL()
{
    m_i2c_module.readBytes(LIS3MDL_ADDRESS, 0x28, m_lis3mdl_buffer, 6, [this]() {
        int16_t x = (int16_t)(m_lis3mdl_buffer[0] | (m_lis3mdl_buffer[1] << 8));
        int16_t y = (int16_t)(m_lis3mdl_buffer[2] | (m_lis3mdl_buffer[3] << 8));
        int16_t z = (int16_t)(m_lis3mdl_buffer[4] | (m_lis3mdl_buffer[5] << 8));
        MagSample m = {x * 0.000171f, y * 0.000171f, z * 0.000171f};
        m_mag_mutex.lock();
        m_latest_mag = m;
        m_mag_mutex.unlock();
    });
}

void SensorModule::readMS8607()
{
    advanceMS8607State();
}

void SensorModule::advanceMS8607State()
{
    switch (m_ms8607_state)
    {
        case MS8607State::IDLE:
            if (m_i2c_module.writeByte(MS8607_ADDRESS, 0xF3, 0x00))
            {
                m_ms8607_state = MS8607State::WAIT_TEMP;
                m_ms8607_wait_ticks = 100; // 10 ms at 100 µs ticks
            }
            break;

        case MS8607State::WAIT_TEMP:
            if (--m_ms8607_wait_ticks == 0)
            {
                m_i2c_module.readBytes(MS8607_ADDRESS, 0x00, m_ms8607_temp_buffer, 3, [this]() {
                    uint32_t raw_temp = (m_ms8607_temp_buffer[0] << 16) | (m_ms8607_temp_buffer[1] << 8) | m_ms8607_temp_buffer[2];
                    m_ms8607_temp = -45.0f + 175.0f * (raw_temp / 16777216.0f);
                    m_ms8607_state = MS8607State::TRIGGER_PRESS;
                });
                m_ms8607_state = MS8607State::READ_TEMP;
            }
            break;

        case MS8607State::READ_TEMP:
            // Wait for callback to set TRIGGER_PRESS
            break;

        case MS8607State::TRIGGER_PRESS:
            if (m_i2c_module.writeByte(MS8607_ADDRESS, 0xF5, 0x00))
            {
                m_ms8607_state = MS8607State::WAIT_PRESS;
                m_ms8607_wait_ticks = 100; // 10 ms at 100 µs ticks
            }
            break;

        case MS8607State::WAIT_PRESS:
            if (--m_ms8607_wait_ticks == 0)
            {
                m_i2c_module.readBytes(MS8607_ADDRESS, 0x00, m_ms8607_press_buffer, 3, [this]() {
                    uint32_t raw_press = (m_ms8607_press_buffer[0] << 16) | (m_ms8607_press_buffer[1] << 8) | m_ms8607_press_buffer[2];
                    float press = 300.0f + 700.0f * (raw_press / 16777216.0f);
                    PHTSample p = {m_ms8607_temp, press, 50.0f};
                    m_pht_mutex.lock();
                    m_latest_pht = p;
                    m_pht_mutex.unlock();
                    m_ms8607_state = MS8607State::IDLE;
                });
                m_ms8607_state = MS8607State::READ_PRESS;
            }
            break;

        case MS8607State::READ_PRESS:
            // Wait for callback to set IDLE
            break;

        default:
            m_ms8607_state = MS8607State::IDLE;
            break;
    }
}

PHTSample SensorModule::getLatestPHT()
{
    m_pht_mutex.lock();
    PHTSample p = m_latest_pht;
    m_pht_mutex.unlock();
    return p;
}

MagSample SensorModule::getLatestMag()
{
    m_mag_mutex.lock();
    MagSample m = m_latest_mag;
    m_mag_mutex.unlock();
    return m;
}
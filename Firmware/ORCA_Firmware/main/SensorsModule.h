#ifndef SENSORS_MODULE_H
#define SENSORS_MODULE_H

#include <Arduino.h>
#include <mbed.h>
#include "I2CModule.h"

using namespace rtos;

#define LIS3MDL_ADDRESS 0x1E
#define MS8607_ADDRESS 0x76

struct PHTSample
{
    float temperature;
    float pressure;
    float humidity;
};

struct MagSample
{
    float x, y, z;
};

class SensorModule : public Tickable
{
public:
    SensorModule(I2CModule& i2c_module, uint32_t trigger_every_n_ticks, osPriority priority = osPriorityAboveNormal);
    bool init();
    void start();
    virtual void onTick() override;
    PHTSample getLatestPHT();
    MagSample getLatestMag();

private:
    enum class MS8607State {
        IDLE,
        TRIGGER_TEMP,
        WAIT_TEMP,
        READ_TEMP,
        TRIGGER_PRESS,
        WAIT_PRESS,
        READ_PRESS
    };

    void sensorThread();
    void readLIS3MDL();
    void readMS8607();
    void advanceMS8607State();

    I2CModule& m_i2c_module;
    uint32_t m_trigger_every_n_ticks;
    uint32_t m_tick_count;
    Thread m_thread;
    Semaphore m_trigger_sem;
    Mutex m_pht_mutex;
    Mutex m_mag_mutex;
    PHTSample m_latest_pht;
    MagSample m_latest_mag;
    __attribute__((aligned(4))) uint8_t m_lis3mdl_buffer[6];
    __attribute__((aligned(4))) uint8_t m_ms8607_temp_buffer[3];
    __attribute__((aligned(4))) uint8_t m_ms8607_press_buffer[3];
    MS8607State m_ms8607_state;
    uint32_t m_ms8607_wait_ticks;
    float m_ms8607_temp;
};

#endif // SENSOR_MODULE_H
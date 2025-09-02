#ifndef IMU_READER_H
#define IMU_READER_H

#include <Arduino.h>
#include <mbed.h>
#include "I2CModule.h"

using namespace rtos;

#define REG_WHO_AM_I 0x0F
#define IMU_START_REG 0x22
#define IMU_READ_LEN 12

struct ImuSample
{
    int16_t gx, gy, gz;
    int16_t ax, ay, az;
};

class ImuReader
{
public:
    ImuReader(I2CModule& i2c_module, uint8_t addr, int rdy_pin = -1);
    bool probeAndConfigure();
    ImuSample getLatestSample();
    void start();
    void processData();

private:
    I2CModule& m_i2c_module;
    uint8_t m_addr;
    int m_rdy_pin;
    Mutex m_sample_mutex;
    ImuSample m_latest_sample;
    __attribute__((aligned(4))) uint8_t m_rx_buffer[IMU_READ_LEN];
};

#endif // IMU_READER_H
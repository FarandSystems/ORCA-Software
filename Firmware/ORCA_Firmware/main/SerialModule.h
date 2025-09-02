#include "api/HardwareSerial.h"
#ifndef SERIAL_MODULE_H
#define SERIAL_MODULE_H

#include <Arduino.h>
#include <mbed.h>
#include "ImuReader.h"
#include "SensorsModule.h"
#include "QwiicPowerModule.h"

using namespace rtos;
using namespace std::chrono;

class SerialModule : public Tickable
{
public:
    SerialModule(HardwareSerial& serial, uint32_t baud_rate, uint32_t trigger_every_n_ticks, QwiicPowerModule& power_module, HardwareSerial& debug_Serial);
    ~SerialModule();

    void init(ImuReader& imu_reader, SensorModule& sensor_module);
    void start();
    virtual void onTick() override;

private:
    void threadLoop();
    void serviceInputCommand(uint8_t* rx_buffer);
    void reportData();

    HardwareSerial& m_serial;
    HardwareSerial& m_debug_Serial;
    ImuReader* m_imu_reader;
    SensorModule* m_sensor_module;
    QwiicPowerModule& m_power_module;
    uint32_t m_baud_rate;
    uint32_t m_trigger_every_n_ticks;
    uint32_t m_tick_count;
    Thread m_thread;
    Semaphore m_trigger_sem;
    uint8_t m_rx_buffer[8];
    uint8_t m_tx_buffer[64];
    uint8_t m_tx_counter;
    float m_time_ticks;
    bool m_report_enabled;
};

#endif // SERIAL_MODULE_H
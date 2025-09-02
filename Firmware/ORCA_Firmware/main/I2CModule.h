#ifndef I2C_MODULE_H
#define I2C_MODULE_H

#include <Arduino.h>
#include <mbed.h>
#include <functional>

using namespace rtos;
using namespace std::chrono;

extern "C"
{
    #include "am_mcu_apollo.h"
    #include "am_hal_iom.h"
    #include "am_bsp.h"
}

class Tickable
{
public:
    virtual void onTick() = 0;
    virtual ~Tickable() {}
};

class I2CModule : public Tickable
{
public:
    I2CModule(uint32_t iom_index, uint32_t clock_freq, uint32_t trigger_every_n_ticks, osPriority priority = osPriorityHigh);
    ~I2CModule();

    bool init();
    void start();
    bool readBytes(uint8_t addr, uint8_t reg, uint8_t* buf, uint32_t len, std::function<void()> complete_callback);
    bool writeByte(uint8_t addr, uint8_t reg, uint8_t val);
    virtual void onTick() override;
    void handleISR(uint32_t status);
    void handleComplete();
    void* m_iom_handle;

private:
    void i2cThread();
    void initiateRead();
    

    uint32_t m_iom_index;
    uint32_t m_clock_freq;
    uint32_t m_trigger_every_n_ticks;
    uint32_t m_tick_count;
    Thread m_thread;
    Semaphore m_trigger_sem;
    std::function<void()> m_complete_callback;
    volatile bool m_busy;
    uint8_t m_current_addr;
    uint8_t m_current_reg;
    uint8_t* m_rx_buffer;
    uint32_t m_rx_len;
};

#endif // I2C_MODULE_H
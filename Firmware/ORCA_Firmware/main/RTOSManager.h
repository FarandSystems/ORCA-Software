#ifndef RTOS_MANAGER_H
#define RTOS_MANAGER_H

#include <Arduino.h>
#include <mbed.h>
#include <vector>
#include "I2CModule.h"

using namespace rtos;
using namespace std::chrono;

class RTOSManager
{
public:
    static RTOSManager& getInstance();
    void registerTickable(Tickable* tickable);
    void startTicker(microseconds period = 100us);
    void handleIomInterrupt(uint32_t iom_index);

private:
    RTOSManager();
    ~RTOSManager();

    void tickerISR();

    static constexpr int PIN_1MS_REF = 37;
    mbed::Ticker m_ticker;
    std::vector<Tickable*> m_tickables;
};

#endif // RTOS_MANAGER_H
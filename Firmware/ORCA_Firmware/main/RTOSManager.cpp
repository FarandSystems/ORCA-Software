#include "RTOSManager.h"

RTOSManager::RTOSManager()
{
    // pinMode(PIN_1MS_REF, OUTPUT);
    // digitalWrite(PIN_1MS_REF, LOW);
}

RTOSManager::~RTOSManager()
{
}

RTOSManager& RTOSManager::getInstance()
{
    static RTOSManager instance;
    return instance;
}

void RTOSManager::registerTickable(Tickable* tickable)
{
    m_tickables.push_back(tickable);
}

void RTOSManager::startTicker(microseconds period)
{
    m_ticker.attach(mbed::callback(this, &RTOSManager::tickerISR), period);
}

void RTOSManager::tickerISR()
{
    // static uint8_t c_1ms = 0;
    // if (++c_1ms >= 10)
    // {
    //     c_1ms = 0;
    //     am_hal_gpio_output_toggle(43);
    // }

    for (auto tickable : m_tickables)
    {
        tickable->onTick();
    }
}

void RTOSManager::handleIomInterrupt(uint32_t iom_index)
{
}
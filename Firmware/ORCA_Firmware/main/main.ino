#include "I2CModule.h"
#include "RTOSManager.h"
#include "ImuReader.h"
#include "SerialModule.h"
#include "QwiicPowerModule.h"
#include "SensorsModule.h"
#include "AlarmModule.h"

I2CModule g_i2c_module(1, AM_HAL_IOM_1MHZ, 120);
ImuReader imu_reader(g_i2c_module, 0);
SensorModule sensor_module(g_i2c_module, 166);
QwiicPowerModule power_module(g_i2c_module, imu_reader, sensor_module);
SerialModule serial_module(Serial1, 115200, 166, power_module, Serial);
AlarmModule g_alarm_module(312);

void setup()
{
    Serial.begin(115200);
    // am_hal_gpio_pinconfig(43, g_AM_HAL_GPIO_OUTPUT);
    // pinMode(43, OUTPUT);  digitalWrite(43, LOW);

    if (!g_i2c_module.init())
    {
        Serial.println("i2c init failed!");
        while (1) {}
    }

    if (!g_alarm_module.init())
    {
        Serial.println("alarm init failed!");
        while (1) {}
    }
    if (!power_module.powerOn())
    {
        Serial.println("qwiic powering failed!");
        while (1) {}
    }

    serial_module.init(imu_reader, sensor_module);

    RTOSManager& rtos = RTOSManager::getInstance();
    rtos.registerTickable(&g_i2c_module);
    rtos.registerTickable(&serial_module);
    rtos.registerTickable(&sensor_module);
    rtos.registerTickable(&g_alarm_module);
    rtos.startTicker();

    g_i2c_module.start();
    imu_reader.start();
    sensor_module.start();
    serial_module.start();
    g_alarm_module.start();
}

void loop()
{
}
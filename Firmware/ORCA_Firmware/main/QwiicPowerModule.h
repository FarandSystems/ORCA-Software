#ifndef QWIIC_POWER_MODULE_H
#define QWIIC_POWER_MODULE_H

#include <Arduino.h>
#include "I2CModule.h"
#include "ImuReader.h"
#include "SensorsModule.h"

#define PCA9536_ADDRESS 0x41
#define PCA9536_REGISTER_OUTPUT_PORT 0x01
#define PCA9536_REGISTER_CONFIGURATION 0x03

class QwiicPowerModule
{
public:
    QwiicPowerModule(I2CModule& i2c_module, ImuReader& imu_reader, SensorModule& sensor_module);
    bool powerOn();
    void powerOff();
    bool reinit();

private:
    bool setPinMode(uint8_t pin, bool output);
    bool writePin(uint8_t pin, bool value);

    I2CModule& m_i2c_module;
    ImuReader& m_imu_reader;
    SensorModule& m_sensor_module;
};

#endif // QWIIC_POWER_MODULE_H
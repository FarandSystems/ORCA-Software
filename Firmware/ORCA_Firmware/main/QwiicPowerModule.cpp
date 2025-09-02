#include "QwiicPowerModule.h"

QwiicPowerModule::QwiicPowerModule(I2CModule& i2c_module, ImuReader& imu_reader, SensorModule& sensor_module)
    : m_i2c_module(i2c_module),
      m_imu_reader(imu_reader),
      m_sensor_module(sensor_module)
{
    
}

bool QwiicPowerModule::setPinMode(uint8_t pin, bool output)
{
    uint8_t cfg = 0;
    if (!m_i2c_module.readBytes(PCA9536_ADDRESS, PCA9536_REGISTER_CONFIGURATION, &cfg, 1, nullptr))
    {
        return false;
    }
    cfg &= ~(1 << pin);
    if (!output)
    {
        cfg |= (1 << pin);
    }
    return m_i2c_module.writeByte(PCA9536_ADDRESS, PCA9536_REGISTER_CONFIGURATION, cfg);
}

bool QwiicPowerModule::writePin(uint8_t pin, bool value)
{
    uint8_t output = 0;
    if (!m_i2c_module.readBytes(PCA9536_ADDRESS, PCA9536_REGISTER_OUTPUT_PORT, &output, 1, nullptr))
    {
        return false;
    }
    output &= ~(1 << pin);
    if (value)
    {
        output |= (1 << pin);
    }
    return m_i2c_module.writeByte(PCA9536_ADDRESS, PCA9536_REGISTER_OUTPUT_PORT, output);
}

bool QwiicPowerModule::powerOn()
{
    // Enable Qwiic Power by pulling GPIO0 high
    pinMode(0, OUTPUT); // Make GPIO0 an output
    digitalWrite(0, HIGH); // Set the output high

    // Wait for power to stabilize
    delay(10);

    // Now enable I2C by pulling GPIO3 high
    pinMode(3, OUTPUT); // Make GPIO3 an output
    digitalWrite(3, HIGH); // Set the output high
    return (digitalRead(0) == HIGH && digitalRead(0) == HIGH);
}

void QwiicPowerModule::powerOff()
{
    setPinMode(3, true);
    writePin(3, false);
    setPinMode(0, true);
    writePin(0, false);
}

bool QwiicPowerModule::reinit()
{
    return m_imu_reader.probeAndConfigure() && m_sensor_module.init();
}
#include "ImuReader.h"

ImuReader::ImuReader(I2CModule& i2c_module, uint8_t addr, int rdy_pin)
    : m_i2c_module(i2c_module),
      m_addr(addr),
      m_rdy_pin(rdy_pin)
{
    if (m_rdy_pin != -1)
    {
        pinMode(m_rdy_pin, OUTPUT);
        digitalWrite(m_rdy_pin, LOW);
    }
}

bool ImuReader::probeAndConfigure()
{
    uint8_t test_addr = m_addr;
    uint8_t v = 0;
    if (test_addr == 0)
    {
        if (m_i2c_module.readBytes(0x6A, REG_WHO_AM_I, &v, 1, nullptr) && v == 0x6B)
        {
            test_addr = 0x6A;
        }
        else if (m_i2c_module.readBytes(0x6B, REG_WHO_AM_I, &v, 1, nullptr) && v == 0x6B)
        {
            test_addr = 0x6B;
        }
        else
        {
            return false;
        }
        m_addr = test_addr;
    }
    else
    {
        if (!m_i2c_module.readBytes(test_addr, REG_WHO_AM_I, &v, 1, nullptr) || v != 0x6B)
        {
            return false;
        }
    }

    if (!m_i2c_module.writeByte(m_addr, 0x12, 0x44)) return false;
    if (!m_i2c_module.writeByte(m_addr, 0x10, 0x70)) return false;
    if (!m_i2c_module.writeByte(m_addr, 0x11, 0x72)) return false;

    return true;
}

void ImuReader::start()
{
    m_i2c_module.readBytes(m_addr, IMU_START_REG, m_rx_buffer, IMU_READ_LEN, [this]() { processData(); });
}

void ImuReader::processData()
{
    static auto le16 = [](const uint8_t* p) -> int16_t { return (int16_t)(p[0] | (p[1] << 8)); };

    ImuSample s;
    s.gx = le16(&m_rx_buffer[0]);
    s.gy = le16(&m_rx_buffer[2]);
    s.gz = le16(&m_rx_buffer[4]);
    s.ax = le16(&m_rx_buffer[6]);
    s.ay = le16(&m_rx_buffer[8]);
    s.az = le16(&m_rx_buffer[10]);

    m_sample_mutex.lock();
    m_latest_sample = s;
    m_sample_mutex.unlock();

    if (m_rdy_pin != -1)
    {
        digitalWrite(m_rdy_pin, !digitalRead(m_rdy_pin));
    }

    m_i2c_module.readBytes(m_addr, IMU_START_REG, m_rx_buffer, IMU_READ_LEN, [this]() { processData(); });
}

ImuSample ImuReader::getLatestSample()
{
    m_sample_mutex.lock();
    ImuSample sample = m_latest_sample;
    m_sample_mutex.unlock();
    return sample;
}
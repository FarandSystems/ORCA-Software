#include "SerialModule.h"
#include "AlarmModule.h"

extern AlarmModule g_alarm_module;

SerialModule::SerialModule(HardwareSerial& serial, uint32_t baud_rate, uint32_t trigger_every_n_ticks, QwiicPowerModule& power_module, HardwareSerial& debug_Serial)
    : m_serial(serial),
      m_debug_Serial(debug_Serial),
      m_imu_reader(nullptr),
      m_sensor_module(nullptr),
      m_power_module(power_module),
      m_baud_rate(baud_rate),
      m_trigger_every_n_ticks(trigger_every_n_ticks),
      m_tick_count(0),
      m_thread(osPriorityAboveNormal, 1024),
      m_trigger_sem(0),
      m_tx_counter(0),
      m_time_ticks(0.0f),
      m_report_enabled(false)
{
    for (int i = 0; i < 8; i++)
    {
        m_rx_buffer[i] = 0;
    }
    for (int i = 0; i < 64; i++)
    {
        m_tx_buffer[i] = 0;
    }
}

SerialModule::~SerialModule()
{
}

void SerialModule::init(ImuReader& imu_reader, SensorModule& sensor_module)
{
    m_imu_reader = &imu_reader;
    m_sensor_module = &sensor_module;
    m_serial.begin(m_baud_rate);
}

void SerialModule::start()
{
    m_thread.start(mbed::callback(this, &SerialModule::threadLoop));
}

void SerialModule::onTick()
{
    m_tick_count++;
    if (m_tick_count >= m_trigger_every_n_ticks)
    {
        m_tick_count = 0;
        m_trigger_sem.release();
    }
}

void SerialModule::threadLoop()
{
    while (true)
    {
        m_trigger_sem.acquire();
        if (m_serial.available() >= 8)
        {
            m_serial.readBytes(m_rx_buffer, 8);
            serviceInputCommand(m_rx_buffer);
        }
        if (m_report_enabled)
        {
            reportData();
        }
    }
}

void SerialModule::serviceInputCommand(uint8_t* rx_buffer)
{
    switch (rx_buffer[1])
    {
        case 0x01:
        {
            g_alarm_module.enqueue(SHORT_BEEP_X1, 1, 32, BEEP_ON);
            m_debug_Serial.println("CMD: QWIIC ON");
            if (m_power_module.powerOn())
            {
                m_debug_Serial.println("Sensors OK");
            }
            else
            {
                m_debug_Serial.println("Sensors FAIL");
            }
            break;
        }
        case 0x02:
        {
            g_alarm_module.enqueue(SHORT_BEEP_X1, 1, 32, BEEP_ON);
            m_debug_Serial.println("CMD: QWIIC OFF");
            m_power_module.powerOff();
            break;
        }
        case 0x03:
        {
            g_alarm_module.enqueue(SHORT_BEEP_X1, 1, 32, BEEP_ON);
            m_debug_Serial.println("CMD: REINIT");
            if (m_power_module.reinit())
            {
                m_debug_Serial.println("Reinit OK");
            }
            else
            {
                m_debug_Serial.println("Reinit FAIL");
            }
            break;
        }
        case 0x04:
        {
            g_alarm_module.enqueue(SHORT_BEEP_X1, 1, 32, BEEP_ON);
            m_debug_Serial.println("CMD: REPORT START");
            m_report_enabled = true;
            break;
        }
        case 0x05:
        {
            g_alarm_module.enqueue(SHORT_BEEP_X1, 1, 32, BEEP_ON);
            m_debug_Serial.println("CMD: REPORT STOP");
            m_report_enabled = false;
            break;
        }
        case 0x06:
        {
            g_alarm_module.enqueue(SHORT_BEEP_X1, 1, 32, BEEP_ON);
            m_debug_Serial.print("CMD: IMU ODR ");
            m_debug_Serial.print(rx_buffer[2]);
            m_debug_Serial.print(", ");
            m_debug_Serial.println(rx_buffer[3]);
            break;
        }
        default:
            break;
    }
}

void SerialModule::reportData()
{
    m_tx_counter++;
    m_time_ticks += 0.016666f;

    m_tx_buffer[0] = 0;
    m_tx_buffer[1] = m_tx_counter;
    m_tx_buffer[2] = 0x55;
    m_tx_buffer[3] = 0x55;

    PHTSample pht = m_sensor_module->getLatestPHT();
    float temp = pht.temperature;
    float press = pht.pressure;
    float hum = pht.humidity;

    uint16_t temp_u16 = (uint16_t)(temp * 100.0f);
    m_tx_buffer[4] = (temp_u16 >> 8) & 0xFF;
    m_tx_buffer[5] = temp_u16 & 0xFF;

    uint16_t press_u16 = (uint16_t)press;
    m_tx_buffer[6] = (press_u16 >> 8) & 0xFF;
    m_tx_buffer[7] = press_u16 & 0xFF;

    uint16_t hum_u16 = (uint16_t)(hum * 100.0f);
    m_tx_buffer[8] = (hum_u16 >> 8) & 0xFF;
    m_tx_buffer[9] = hum_u16 & 0xFF;

    ImuSample s = m_imu_reader->getLatestSample();
    float acc_x = s.ax * 0.000061f;
    float acc_y = s.ay * 0.000061f;
    float acc_z = s.az * 0.000061f;
    int32_t acc_x_i32 = (int32_t)(acc_x * 1000.0f);
    int32_t acc_y_i32 = (int32_t)(acc_y * 1000.0f);
    int32_t acc_z_i32 = (int32_t)(acc_z * 1000.0f);
    m_tx_buffer[10] = (acc_x_i32 >> 24) & 0xFF;
    m_tx_buffer[11] = (acc_x_i32 >> 16) & 0xFF;
    m_tx_buffer[12] = (acc_x_i32 >> 8) & 0xFF;
    m_tx_buffer[13] = acc_x_i32 & 0xFF;
    m_tx_buffer[14] = (acc_y_i32 >> 24) & 0xFF;
    m_tx_buffer[15] = (acc_y_i32 >> 16) & 0xFF;
    m_tx_buffer[16] = (acc_y_i32 >> 8) & 0xFF;
    m_tx_buffer[17] = acc_y_i32 & 0xFF;
    m_tx_buffer[18] = (acc_z_i32 >> 24) & 0xFF;
    m_tx_buffer[19] = (acc_z_i32 >> 16) & 0xFF;
    m_tx_buffer[20] = (acc_z_i32 >> 8) & 0xFF;
    m_tx_buffer[21] = acc_z_i32 & 0xFF;

    float gyro_x = s.gx * 0.004375f;
    float gyro_y = s.gy * 0.004375f;
    float gyro_z = s.gz * 0.004375f;
    int32_t gyro_x_i32 = (int32_t)(gyro_x * 1000.0f);
    int32_t gyro_y_i32 = (int32_t)(gyro_y * 1000.0f);
    int32_t gyro_z_i32 = (int32_t)(gyro_z * 1000.0f);
    m_tx_buffer[22] = (gyro_x_i32 >> 24) & 0xFF;
    m_tx_buffer[23] = (gyro_x_i32 >> 16) & 0xFF;
    m_tx_buffer[24] = (gyro_x_i32 >> 8) & 0xFF;
    m_tx_buffer[25] = gyro_x_i32 & 0xFF;
    m_tx_buffer[26] = (gyro_y_i32 >> 24) & 0xFF;
    m_tx_buffer[27] = (gyro_y_i32 >> 16) & 0xFF;
    m_tx_buffer[28] = (gyro_y_i32 >> 8) & 0xFF;
    m_tx_buffer[29] = gyro_y_i32 & 0xFF;
    m_tx_buffer[30] = (gyro_z_i32 >> 24) & 0xFF;
    m_tx_buffer[31] = (gyro_z_i32 >> 16) & 0xFF;
    m_tx_buffer[32] = (gyro_z_i32 >> 8) & 0xFF;
    m_tx_buffer[33] = gyro_z_i32 & 0xFF;

    MagSample mag = m_sensor_module->getLatestMag();
    float mag_x = mag.x;
    float mag_y = mag.y;
    float mag_z = mag.z;
    int32_t mag_x_i32 = (int32_t)(mag_x * 1000.0f);
    int32_t mag_y_i32 = (int32_t)(mag_y * 1000.0f);
    int32_t mag_z_i32 = (int32_t)(mag_z * 1000.0f);
    m_tx_buffer[34] = (mag_x_i32 >> 24) & 0xFF;
    m_tx_buffer[35] = (mag_x_i32 >> 16) & 0xFF;
    m_tx_buffer[36] = (mag_x_i32 >> 8) & 0xFF;
    m_tx_buffer[37] = mag_x_i32 & 0xFF;
    m_tx_buffer[38] = (mag_y_i32 >> 24) & 0xFF;
    m_tx_buffer[39] = (mag_y_i32 >> 16) & 0xFF;
    m_tx_buffer[40] = (mag_y_i32 >> 8) & 0xFF;
    m_tx_buffer[41] = mag_y_i32 & 0xFF;
    m_tx_buffer[42] = (mag_z_i32 >> 24) & 0xFF;
    m_tx_buffer[43] = (mag_z_i32 >> 16) & 0xFF;
    m_tx_buffer[44] = (mag_z_i32 >> 8) & 0xFF;
    m_tx_buffer[45] = mag_z_i32 & 0xFF;

    for (int i = 46; i < 63; i++)
    {
        m_tx_buffer[i] = 0;
    }

    uint8_t cs = 0;
    for (int i = 0; i < 63; i++)
    {
        cs += m_tx_buffer[i];
    }
    m_tx_buffer[63] = cs;

    m_serial.write(m_tx_buffer, 64);
}
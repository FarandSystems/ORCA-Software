#include "RTOSManager.h"
#include "SerialDebugger.h"
#include "Alarm.h"
#include <Arduino.h>
using namespace rtos;
using namespace std::chrono;

static constexpr float ACC_G_PER_LSB   = 0.000061f;
static constexpr float GYR_DPS_PER_LSB = 0.004375f;

static constexpr int PIN_IMU_RDY = 43;

RTOSManager::RTOSManager(SensorsManager& sensors, UartManager& uart)
  : m_sensors(sensors),
    m_uart(uart),
    m_semIMU(0),
    m_sem100(0),
    m_semAlarm(0),
    m_semUART(0),
    m_tIMU(osPriorityHigh,          2048, nullptr, "IMU"),
    m_tLog(osPriorityAboveNormal,   1536, nullptr, "LOG"),
    m_tAlarm(osPriorityAboveNormal, 1024, nullptr, "ALARM"),
    m_tUART(osPriorityAboveNormal,  2304, nullptr, "UART"),
    m_cIMU(0),
    m_c100(0),
    m_cUART125(0),
    m_accAlarmUs(0)
{
}
void RTOSManager::start()
{
  serial_debugger.println("t[ms], ax[g], ay[g], az[g], gx[dps], gy[dps], gz[dps]");

  m_tIMU.start(mbed::callback(this, &RTOSManager::imuThread_));
  m_tLog.start(mbed::callback(this, &RTOSManager::logThread_));
  m_tAlarm.start(mbed::callback(this, &RTOSManager::alarmThread_));
  m_tUART.start(mbed::callback(this, &RTOSManager::uartThread_));

  m_masterTick.attach(mbed::callback(this, &RTOSManager::masterISR_), 200us);
  LOGI("RTOS started (200us ticker: IMU~833Hz, log 100Hz, alarm 32Hz, UART 8Hz)");
}

void RTOSManager::masterISRThunk_(void* ctx)
{
  static_cast<RTOSManager*>(ctx)->masterISR_();
}

void RTOSManager::masterISR_()
{
  // IMU ~833 Hz
  if (++m_cIMU >= 6)
  {
    m_cIMU = 0;
    m_semIMU.release();
  }

  // Logger 100 Hz
  if (++m_c100 >= 50)
  {
    m_c100 = 0;
    m_sem100.release();
  }

  // UART 8 Hz (every 125 ms) -> 125ms / 200us = 625 ticks
  if (++m_cUART125 >= 625)
  {
    m_cUART125 = 0;
    m_semUART.release();
  }

  // Alarm 32 Hz (every 31,250 µs)
  m_accAlarmUs += 200;
  if (m_accAlarmUs >= 31250)
  {
    m_accAlarmUs -= 31250;
    m_semAlarm.release();
  }
}

void RTOSManager::imuThreadThunk_(void* arg)
{
  static_cast<RTOSManager*>(arg)->imuThread_();
}

void RTOSManager::imuThread_()
{
  while (true)
  {
    m_semIMU.acquire();

    ImuSample s;
    if (m_sensors.readIMU(s))
    {
      m_sensors.setLatest(s);

      
    }
    else
    {
      LOGT("IMU read failed");
    }
  }
}

void RTOSManager::logThreadThunk_(void* arg)
{
  static_cast<RTOSManager*>(arg)->logThread_();
}

void RTOSManager::logThread_()
{
  static uint32_t t0 = millis();

  while (true)
  {
    m_sem100.acquire();

    ImuSample s = m_sensors.getLatest();

    float ax = s.ax * ACC_G_PER_LSB;
    float ay = s.ay * ACC_G_PER_LSB;
    float az = s.az * ACC_G_PER_LSB;
    float gx = s.gx * GYR_DPS_PER_LSB;
    float gy = s.gy * GYR_DPS_PER_LSB;
    float gz = s.gz * GYR_DPS_PER_LSB;

    uint32_t tms = millis() - t0;

    // serial_debugger.printf("%lu,%.4f,%.4f,%.4f,%.2f,%.2f,%.2f\r\n",
    //                        (unsigned long)tms, ax, ay, az, gx, gy, gz);
  }
}

void RTOSManager::alarmThreadThunk_(void* arg)
{
  static_cast<RTOSManager*>(arg)->alarmThread_();
}

void RTOSManager::alarmThread_()
{
  while (true)
  {
    m_semAlarm.acquire();
    Alarm_Update_32Hz();
  }
}

// ---- UART 10 ms poll thread ----
void RTOSManager::uartThreadThunk_(void* arg)
{
  static_cast<RTOSManager*>(arg)->uartThread_();
}

void RTOSManager::uartThread_()
{
  while (true)
  {
    m_semUART.acquire();
    m_uart.poll(125);   // one pass every 125 ms
    am_hal_gpio_output_toggle(PIN_IMU_RDY);
  }
}

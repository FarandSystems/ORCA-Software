#include "RTOSManager.h"
#include "SerialDebugger.h"
#include <Arduino.h>
using namespace rtos;
using namespace std::chrono;

static constexpr float ACC_G_PER_LSB   = 0.000061f;
static constexpr float GYR_DPS_PER_LSB = 0.004375f;

RTOSManager::RTOSManager(SensorsManager& sensors)
  : m_sensors(sensors),
    m_semIMU(0),
    m_sem100(0),
    m_tIMU(osPriorityHigh, 1536),
    m_tLog(osPriorityAboveNormal, 1024),
    m_cIMU(0),
    m_c100(0)
{
}

void RTOSManager::start()
{
  serial_debugger.println("t[ms], ax[g], ay[g], az[g], gx[dps], gy[dps], gz[dps]");

  m_tIMU.start(mbed::callback(this, &RTOSManager::imuThread_));
  m_tLog.start(mbed::callback(this, &RTOSManager::logThread_));

  m_masterTick.attach(mbed::callback(this, &RTOSManager::masterISR_), 200us);
  LOGI("RTOS started (200us ticker, IMU~833Hz, log 100Hz)");
}

void RTOSManager::masterISRThunk_(void* ctx)
{
  static_cast<RTOSManager*>(ctx)->masterISR_();
}

void RTOSManager::masterISR_()
{
  if (++m_cIMU >= 6)
  {
    m_cIMU = 0;
    m_semIMU.release();
  }

  if (++m_c100 >= 50)
  {
    m_c100 = 0;
    m_sem100.release();
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
      // noisy if sensor absent; keep at TRACE
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

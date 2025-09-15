#pragma once
#include <mbed.h>
#include "SensorsManager.h"

class RTOSManager
{
public:
  explicit RTOSManager(SensorsManager& sensors);

  void start();

private:
  static void masterISRThunk_(void* ctx);
  void masterISR_();

  static void imuThreadThunk_(void* arg);
  void imuThread_();

  static void logThreadThunk_(void* arg);
  void logThread_();

  // ---- Alarm thread (32 Hz) ----
  static void alarmThreadThunk_(void* arg);
  void alarmThread_();

private:
  SensorsManager&  m_sensors;

  mbed::Ticker     m_masterTick;
  rtos::Semaphore  m_semIMU;
  rtos::Semaphore  m_sem100;
  rtos::Semaphore  m_semAlarm;     // ← new (32 Hz)
  rtos::Thread     m_tIMU;
  rtos::Thread     m_tLog;
  rtos::Thread     m_tAlarm;       // ← new (32 Hz)

  volatile uint8_t m_cIMU;
  volatile uint8_t m_c100;

  // Accumulator for precise 32 Hz based on 200 µs ticks
  volatile uint32_t m_accAlarmUs;
};

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

private:
  SensorsManager&  m_sensors;

  mbed::Ticker     m_masterTick;
  rtos::Semaphore  m_semIMU;
  rtos::Semaphore  m_sem100;
  rtos::Thread     m_tIMU;
  rtos::Thread     m_tLog;

  volatile uint8_t m_cIMU;
  volatile uint8_t m_c100;
};

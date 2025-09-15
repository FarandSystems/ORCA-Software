#pragma once
#include <mbed.h>
#include "SensorsManager.h"
#include "UartManager.h"

class RTOSManager
{
public:
  RTOSManager(SensorsManager& sensors, UartManager& uart);

  void start();

private:
  static void masterISRThunk_(void* ctx);
  void masterISR_();

  static void imuThreadThunk_(void* arg);
  void imuThread_();

  static void logThreadThunk_(void* arg);
  void logThread_();

  static void alarmThreadThunk_(void* arg);
  void alarmThread_();

  static void uartThreadThunk_(void* arg);
  void uartThread_();

private:
  SensorsManager&  m_sensors;
  UartManager&     m_uart;

  mbed::Ticker     m_masterTick;
  rtos::Semaphore  m_semIMU;
  rtos::Semaphore  m_sem100;
  rtos::Semaphore  m_semAlarm;
  rtos::Semaphore  m_semUART;

  // ↑ Stack sizes will be set in the .cpp constructor init list
  rtos::Thread     m_tIMU;
  rtos::Thread     m_tLog;
  rtos::Thread     m_tAlarm;
  rtos::Thread     m_tUART;

  volatile uint8_t  m_cIMU;
  volatile uint8_t  m_c100;
  volatile uint16_t m_cUART125;
  volatile uint32_t m_accAlarmUs;
};
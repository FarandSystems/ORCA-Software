#include <Arduino.h>
#include "BoardControl.h"
#include "PowerManager.h"
#include "SensorsManager.h"
#include "RTOSManager.h"
#include "SerialDebugger.h"
#include "Alarm.h"
#include "UartManager.h"

BoardControl    g_board;
PowerManager    g_power;
SensorsManager  g_sensors;
UartManager     g_uart(g_board, g_power, g_sensors);
RTOSManager     g_rtos(g_sensors, g_uart);

void setup()
{
  
  serial_debugger.begin(115200);
  serial_debugger.setLevel(DBG_DEBUG);
  LOGI("Booting…");

  // Buzzer (pin 37, active-high as you said)
  Alarm_Init(37, true);

  // Power rails up first
  g_power.init(g_board);
  g_power.enableSensorsRail(true);
  delay(20);

  if (!g_board.initI2C_IOM1_D8D9_1MHz())
  {
    LOGE("I2C init FAILED");
    while (1) {}
  }
  LOGI("I2C init OK");

  if (!g_sensors.init(g_board))
  {
    LOGE("Sensors init FAILED");
    while (1) {}
  }
  LOGI("Sensors init OK");

  // UART1 to PC/host
  g_uart.begin(19200);

  // Start all threads (IMU, logger, alarm, UART)
  g_rtos.start();

  Alarm(SHORT_BEEP_X2, 1, 32, BEEP_ON);
}

void loop()
{
  // nothing — RTOS owns the work
}

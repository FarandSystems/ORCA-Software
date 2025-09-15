#include <Arduino.h>
#include "BoardControl.h"
#include "PowerManager.h"
#include "SensorsManager.h"
#include "RTOSManager.h"
#include "SerialDebugger.h"
#include "Alarm.h"   // ← add this

BoardControl    g_board;
PowerManager    g_power;
SensorsManager  g_sensors;
RTOSManager     g_rtos(g_sensors);

void setup()
{
  serial_debugger.begin(115200);
  serial_debugger.setLevel(DBG_DEBUG);

  LOGI("Booting…");

  g_power.init(g_board);
  g_power.enableSensorsRail(true);
  delay(20);

  // ---- Buzzer init: pin 42, active-high ----
  Alarm_Init(42, true);
  // Optional: short boot chirp (pattern, repeats, slices=32, on)
  Alarm(SHORT_BEEP_X2, 1, 32, BEEP_ON);

  if (!g_board.initI2C_IOM1_D8D9_1MHz())
  {
    LOGE("I2C init FAILED");
    while (1) { }
  }
  LOGI("I2C init OK");

  if (!g_sensors.init(g_board))
  {
    LOGE("Sensors init FAILED");
    while (1) { }
  }
  LOGI("Sensors init OK");

  // ---- Start RTOS (includes the new 32 Hz alarm thread) ----
  g_rtos.start();
}

void loop()
{
  // all work in threads/ISRs
}

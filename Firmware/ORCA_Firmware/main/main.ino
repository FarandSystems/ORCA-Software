#include <Arduino.h>
#include "BoardControl.h"
#include "PowerManager.h"
#include "SensorsManager.h"
#include "RTOSManager.h"

BoardControl    g_board;
PowerManager    g_power;
SensorsManager  g_sensors;
RTOSManager     g_rtos(g_sensors);

void setup()
{
  Serial.begin(115200);
  while (!Serial) { }

  g_power.init(g_board);
  g_power.enableSensorsRail(true);
  delay(20);

  if (!g_board.initI2C_IOM1_D8D9_1MHz())
  {
    Serial.println("[BOOT] I2C init FAILED");
    while (1) { }
  }
  Serial.println("[BOOT] I2C init OK");

  if (!g_sensors.init(g_board))
  {
    Serial.println("[BOOT] Sensors init FAILED");
    while (1) { }
  }
  Serial.println("[BOOT] Sensors init OK");

  g_rtos.start();
}

void loop()
{
  // all work is in threads/ISRs
}

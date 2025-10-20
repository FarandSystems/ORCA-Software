
#include "PowerManager.h"
#include "BoardControl.h"
#include "SerialDebugger.h"

bool PowerManager::init(BoardControl& board)
{
  (void)board;
  LOGI("PowerManager init");
  return true;
}

bool PowerManager::enableSensorsRail(bool on)
{
  LOGI("Sensors rail -> %s", on ? "ON" : "OFF");
  // TODO: add GPIO toggle if you wire an EN pin
  return true;
}

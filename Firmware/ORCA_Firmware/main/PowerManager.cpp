#include "PowerManager.h"
#include "BoardControl.h"

bool PowerManager::init(BoardControl& board)
{
  (void)board;
  return true;
}

bool PowerManager::enableSensorsRail(bool on)
{
  (void)on;
  return true;
}

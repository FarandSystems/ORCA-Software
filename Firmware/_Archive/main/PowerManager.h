#pragma once
class BoardControl;

class PowerManager
{
public:
  bool init(BoardControl& board);
  bool enableSensorsRail(bool on);
};

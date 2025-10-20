#pragma once
#include <stdint.h>
#include <mbed.h>

class BoardControl;

struct ImuSample
{
  int16_t gx, gy, gz;
  int16_t ax, ay, az;
};

class IMU_ISM330
{
public:
  IMU_ISM330();

  bool init(BoardControl& board);
  bool readBurst(ImuSample& out);

private:
  static int16_t le16_(const uint8_t* p);

private:
  BoardControl* m_board;
  uint8_t       m_addr;
  bool          m_inited;
};

class SensorsManager
{
public:
  SensorsManager();

  bool init(BoardControl& board);

  bool readIMU(ImuSample& out);

  void setLatest(const ImuSample& s);
  ImuSample getLatest();

private:
  IMU_ISM330 m_imu;
  ImuSample  m_latest;
  rtos::Mutex m_mtx;
};

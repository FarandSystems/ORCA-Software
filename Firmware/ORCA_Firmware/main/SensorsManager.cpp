#include "SensorsManager.h"
#include "BoardControl.h"
#include <Arduino.h>

static constexpr uint8_t REG_WHO_AM_I  = 0x0F;
static constexpr uint8_t REG_CTRL3_C   = 0x12;
static constexpr uint8_t REG_CTRL1_XL  = 0x10;
static constexpr uint8_t REG_CTRL2_G   = 0x11;
static constexpr uint8_t REG_OUT_START = 0x22;
static constexpr uint8_t WHO_EXPECT    = 0x6B;

IMU_ISM330::IMU_ISM330()
{
  m_board  = nullptr;
  m_addr   = 0;
  m_inited = false;
}

bool IMU_ISM330::init(BoardControl& board)
{
  m_board = &board;

  uint8_t v = 0;

  if (m_board->readBytes(0x6A, REG_WHO_AM_I, &v, 1) && v == WHO_EXPECT)
  {
    m_addr = 0x6A;
  }
  else if (m_board->readBytes(0x6B, REG_WHO_AM_I, &v, 1) && v == WHO_EXPECT)
  {
    m_addr = 0x6B;
  }
  else
  {
    Serial.println("[IMU] WHO_AM_I failed");
    return false;
  }

  m_board->writeU8(m_addr, REG_CTRL3_C,  0x44); // BDU=1, IF_INC=1
  m_board->writeU8(m_addr, REG_CTRL1_XL, 0x70); // 833 Hz, ±2 g
  m_board->writeU8(m_addr, REG_CTRL2_G,  0x72); // 833 Hz, 125 dps

  m_inited = true;
  Serial.print("[IMU] ISM330DHCX @0x");
  Serial.println(m_addr, HEX);
  return true;
}

int16_t IMU_ISM330::le16_(const uint8_t* p)
{
  return static_cast<int16_t>(static_cast<uint16_t>(p[0]) | (static_cast<uint16_t>(p[1]) << 8));
}

bool IMU_ISM330::readBurst(ImuSample& out)
{
  if (!m_inited || !m_board)
  {
    return false;
  }

  uint8_t buf[12];

  if (!m_board->readBytes(m_addr, REG_OUT_START, buf, sizeof(buf)))
  {
    return false;
  }

  out.gx = le16_(&buf[0]);
  out.gy = le16_(&buf[2]);
  out.gz = le16_(&buf[4]);
  out.ax = le16_(&buf[6]);
  out.ay = le16_(&buf[8]);
  out.az = le16_(&buf[10]);
  return true;
}

SensorsManager::SensorsManager()
{
  m_latest = {};
}

bool SensorsManager::init(BoardControl& board)
{
  return m_imu.init(board);
}

bool SensorsManager::readIMU(ImuSample& out)
{
  return m_imu.readBurst(out);
}

void SensorsManager::setLatest(const ImuSample& s)
{
  m_mtx.lock();
  m_latest = s;
  m_mtx.unlock();
}

ImuSample SensorsManager::getLatest()
{
  m_mtx.lock();
  ImuSample s = m_latest;
  m_mtx.unlock();
  return s;
}

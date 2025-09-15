#pragma once
#include <stdint.h>

extern "C"
{
  #include "am_mcu_apollo.h"
  #include "am_hal_gpio.h"
  #include "am_hal_iom.h"
  #include "am_bsp.h"
}

class BoardControl
{
public:
  BoardControl();

  // I2C on IOM1 using D8 (SCL) / D9 (SDA) @ 1 MHz
  bool initI2C_IOM1_D8D9_1MHz();

  // Low-level I2C helpers (blocking)
  bool readBytes(uint8_t addr, uint8_t reg, uint8_t* buf, uint32_t len);
  bool writeU8(uint8_t addr, uint8_t reg, uint8_t val);

  void scanI2C();

  void* iomHandle() const;
  uint32_t iomIndex() const;

private:
  void*    m_iomHandle;
  uint32_t m_iomIndex;
};

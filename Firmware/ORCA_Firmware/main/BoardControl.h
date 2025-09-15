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

  bool initI2C_IOM1_D8D9_1MHz();

  bool readBytes(uint8_t addr, uint8_t reg, uint8_t* buf, uint32_t len);
  bool writeU8(uint8_t addr, uint8_t reg, uint8_t val);

  void scanI2C();

  void* iomHandle() const;
  uint32_t iomIndex() const;

private:
  void*    m_iomHandle;
  uint32_t m_iomIndex;
};

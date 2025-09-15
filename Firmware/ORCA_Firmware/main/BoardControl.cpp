#include "BoardControl.h"
#include <stdlib.h>
#include <string.h>

static inline bool isAligned4_(const void* p)
{
  return ((reinterpret_cast<uintptr_t>(p) & 0x3u) == 0);
}

BoardControl::BoardControl()
{
  m_iomHandle = nullptr;
  m_iomIndex  = 0xFFFFFFFFu;
}

bool BoardControl::initI2C_IOM1_D8D9_1MHz()
{
  am_hal_gpio_pinconfig(AM_BSP_GPIO_IOM1_SCL, g_AM_BSP_GPIO_IOM1_SCL);
  am_hal_gpio_pinconfig(AM_BSP_GPIO_IOM1_SDA, g_AM_BSP_GPIO_IOM1_SDA);

  m_iomIndex = 1;

  if (am_hal_iom_initialize(m_iomIndex, &m_iomHandle) != AM_HAL_STATUS_SUCCESS)
  {
    return false;
  }
  if (am_hal_iom_power_ctrl(m_iomHandle, AM_HAL_SYSCTRL_WAKE, false) != AM_HAL_STATUS_SUCCESS)
  {
    return false;
  }

  am_hal_iom_config_t cfg;
  memset(&cfg, 0, sizeof(cfg));
  cfg.eInterfaceMode = AM_HAL_IOM_I2C_MODE;
  cfg.ui32ClockFreq  = AM_HAL_IOM_1MHZ;

  if (am_hal_iom_configure(m_iomHandle, &cfg) != AM_HAL_STATUS_SUCCESS)
  {
    return false;
  }
  if (am_hal_iom_enable(m_iomHandle) != AM_HAL_STATUS_SUCCESS)
  {
    return false;
  }

  return true;
}

bool BoardControl::readBytes(uint8_t addr, uint8_t reg, uint8_t* buf, uint32_t len)
{
  if (!m_iomHandle || len == 0)
  {
    return false;
  }

  am_hal_iom_transfer_t x;
  memset(&x, 0, sizeof(x));
  x.uPeerInfo.ui32I2CDevAddr = addr;
  x.ui32InstrLen = 1;
  x.ui32Instr    = reg;
  x.eDirection   = AM_HAL_IOM_RX;
  x.ui32NumBytes = len;

  uint32_t* bounce = nullptr;

  if (!isAligned4_(buf) || ((len & 0x3u) != 0))
  {
    size_t words = (len + 3u) / 4u;
    bounce = static_cast<uint32_t*>(malloc(words * sizeof(uint32_t)));
    if (!bounce)
    {
      return false;
    }
    x.pui32RxBuffer = bounce;
  }
  else
  {
    x.pui32RxBuffer = reinterpret_cast<uint32_t*>(buf);
  }

  uint32_t st = am_hal_iom_blocking_transfer(m_iomHandle, &x);
  bool ok = (st == AM_HAL_STATUS_SUCCESS);

  if (bounce)
  {
    if (ok)
    {
      memcpy(buf, bounce, len);
    }
    free(bounce);
  }

  return ok;
}

bool BoardControl::writeU8(uint8_t addr, uint8_t reg, uint8_t val)
{
  if (!m_iomHandle)
  {
    return false;
  }

  uint32_t word = val;

  am_hal_iom_transfer_t x;
  memset(&x, 0, sizeof(x));
  x.uPeerInfo.ui32I2CDevAddr = addr;
  x.ui32InstrLen = 1;
  x.ui32Instr    = reg;
  x.eDirection   = AM_HAL_IOM_TX;
  x.ui32NumBytes = 1;
  x.pui32TxBuffer = &word;

  return am_hal_iom_blocking_transfer(m_iomHandle, &x) == AM_HAL_STATUS_SUCCESS;
}

void BoardControl::scanI2C()
{
  if (!m_iomHandle)
  {
    return;
  }

  Serial.println("[I2C] scan:");
  for (uint8_t a = 0x08; a <= 0x77; ++a)
  {
    am_hal_iom_transfer_t t;
    memset(&t, 0, sizeof(t));
    t.uPeerInfo.ui32I2CDevAddr = a;
    t.eDirection   = AM_HAL_IOM_TX;
    t.ui32NumBytes = 0;

    if (am_hal_iom_blocking_transfer(m_iomHandle, &t) == AM_HAL_STATUS_SUCCESS)
    {
      Serial.print("  - 0x");
      Serial.println(a, HEX);
    }
  }
}

void* BoardControl::iomHandle() const
{
  return m_iomHandle;
}

uint32_t BoardControl::iomIndex() const
{
  return m_iomIndex;
}

#include "main.h"

// ---------- IOM state ----------
static void*    s_iom       = nullptr;
static uint32_t s_iom_mod   = IMU_IOM_MODULE;

// NB transaction queue (needed for ALL non-blocking ops)
#ifndef I2C_NBQ_WORDS
#  define I2C_NBQ_WORDS 256        // 256 words = 1KB is plenty
#endif
static uint32_t s_nbq[I2C_NBQ_WORDS] __attribute__((aligned(16)));

// Core busy gate (single outstanding I²C-core op)
static volatile bool s_core_busy = false;

void i2c_core_cb(void* pCtxt, uint32_t status);

// ---------- ISR (shared for all NB ops, including DMA layer) ----------
static inline void iom_isr_common(void)
{
  uint32_t status = 0;
  am_hal_iom_interrupt_status_get(s_iom, true, &status);
  if (!status) return;
  am_hal_iom_interrupt_clear(s_iom, status);
  am_hal_iom_interrupt_service(s_iom, status);
}

#if (IMU_IOM_MODULE == 0)
extern "C" void am_iomaster0_isr(void) { iom_isr_common(); }
extern "C" void am_iomstr0_isr(void)   { iom_isr_common(); }
#elif (IMU_IOM_MODULE == 1)
extern "C" void am_iomaster1_isr(void) { iom_isr_common(); }
extern "C" void am_iomstr1_isr(void)   { iom_isr_common(); }
#elif (IMU_IOM_MODULE == 2)
extern "C" void am_iomaster2_isr(void) { iom_isr_common(); }
extern "C" void am_iomstr2_isr(void)   { iom_isr_common(); }
#elif (IMU_IOM_MODULE == 3)
extern "C" void am_iomaster3_isr(void) { iom_isr_common(); }
extern "C" void am_iomstr3_isr(void)   { iom_isr_common(); }
#elif (IMU_IOM_MODULE == 4)
extern "C" void am_iomaster4_isr(void) { iom_isr_common(); }
extern "C" void am_iomstr4_isr(void)   { iom_isr_common(); }
#elif (IMU_IOM_MODULE == 5)
extern "C" void am_iomaster5_isr(void) { iom_isr_common(); }
extern "C" void am_iomstr5_isr(void)   { iom_isr_common(); }
#endif

// ---------- Init ----------
void imu_i2c_init(void)
{
  am_hal_pwrctrl_periph_enable((am_hal_pwrctrl_periph_e)(AM_HAL_PWRCTRL_PERIPH_IOM0 + s_iom_mod));
  am_hal_iom_initialize(s_iom_mod, &s_iom);
  am_hal_iom_power_ctrl(s_iom, AM_HAL_SYSCTRL_WAKE, false);

  am_hal_iom_config_t cfg = {};
  cfg.eInterfaceMode     = AM_HAL_IOM_I2C_MODE;
  cfg.ui32ClockFreq      = AM_HAL_IOM_1MHZ;   // bump to AM_HAL_IOM_1MHZ later if needed
  cfg.pNBTxnBuf          = s_nbq;
  cfg.ui32NBTxnBufLength = sizeof(s_nbq);
  am_hal_iom_configure(s_iom, &cfg);

  i2c_pins_init();
  am_hal_iom_enable(s_iom);

  // Enable NB interrupts
  am_hal_iom_interrupt_clear(s_iom, 0xFFFFFFFF);
  am_hal_iom_interrupt_enable(s_iom,
      AM_HAL_IOM_INT_CMDCMP |
      AM_HAL_IOM_INT_NAK    |
      AM_HAL_IOM_INT_ARB    |
      AM_HAL_IOM_INT_ERR);

  IRQn_Type irq = (IRQn_Type)(IOMSTR0_IRQn + s_iom_mod);
  NVIC_SetPriority(irq, 10);
  NVIC_EnableIRQ(irq);
}

void*    imu_i2c_get_handle(void)  { return s_iom; }
uint32_t imu_i2c_get_module(void)  { return s_iom_mod; }
bool     imu_i2c_core_busy(void)   { return s_core_busy; }

// ---------- Core async op context & callback ----------
typedef struct {
  imu_i2c_cb_t user_cb;
  void*        user_arg;
  uint8_t*     out_u8;       // for RX-1B
  uint32_t     w;            // aligned bounce (also used as TX 1B)
  bool         is_rx_u8;
  // for auto-retry
  uint8_t      retries_left;
  // capture original transfer params so we can re-submit
  uint8_t      reg;
  uint32_t     len;
  bool         is_write;     // TX when true, RX when false
  uint8_t*     buf;          // RX-N buffer (len>1)
} i2c_op_t;

static i2c_op_t s_op; // single outstanding op

bool i2c_submit_current(void)
{
  am_hal_iom_transfer_t t = {0};
  t.uPeerInfo.ui32I2CDevAddr = ISM330_I2C_ADDR;
  t.ui32InstrLen   = 1;
  t.ui32Instr      = s_op.reg;
  t.eDirection     = s_op.is_write ? AM_HAL_IOM_TX : AM_HAL_IOM_RX;
  t.ui32NumBytes   = s_op.len;
  if (s_op.is_write) 
  {
    t.pui32TxBuffer = &s_op.w;
  } 
  else 
  {
    t.pui32RxBuffer = (s_op.len == 1) ? &s_op.w : (uint32_t*)s_op.buf;
  }
  return (am_hal_iom_nonblocking_transfer(s_iom, &t, i2c_core_cb, &s_op) == AM_HAL_STATUS_SUCCESS);
}

void i2c_core_cb(void* pCtxt, uint32_t status) // HAL_I2C_ErrorCallback
{
  i2c_op_t* op = (i2c_op_t*)pCtxt;
  bool ok = (status == AM_HAL_STATUS_SUCCESS);

  // Auto-retry on failure (bounded)
  if (!ok && op->retries_left > 0)
  {
    op->retries_left--;
    (void)i2c_submit_current();  // keep s_core_busy asserted
    return;
  }

  // Finalize result
  if (ok && op->is_rx_u8 && op->out_u8)
  {
    *(op->out_u8) = (uint8_t)(op->w & 0xFF);
  }

  s_core_busy = false;
  if (op->user_cb) { op->user_cb(ok, op->user_arg); }
}

bool imu_reg_write_u8_async(uint8_t reg, uint8_t val, imu_i2c_cb_t cb, void* user)
{
  if (s_core_busy) return false;
  s_core_busy = true;

  s_op.user_cb  = cb;
  s_op.user_arg = user;
  s_op.out_u8   = NULL;
  s_op.w        = val;
  s_op.is_rx_u8 = false;

  // retry metadata + transfer description
  s_op.retries_left = I2C_CORE_MAX_RETRIES;
  s_op.reg      = reg;
  s_op.len      = 1;
  s_op.is_write = true;
  s_op.buf      = NULL;

  if (!i2c_submit_current()) { s_core_busy = false; return false; }
  return true;
}

bool imu_reg_read_u8_async(uint8_t reg, uint8_t* out, imu_i2c_cb_t cb, void* user)
{
  if (s_core_busy) return false;
  s_core_busy = true;

  s_op.user_cb  = cb;
  s_op.user_arg = user;
  s_op.out_u8   = out;
  s_op.w        = 0;
  s_op.is_rx_u8 = true;

  // retry metadata + transfer description
  s_op.retries_left = I2C_CORE_MAX_RETRIES;
  s_op.reg      = reg;
  s_op.len      = 1;
  s_op.is_write = false;
  s_op.buf      = NULL;

  if (!i2c_submit_current()) { s_core_busy = false; return false; }
  return true;
}

bool imu_reg_read_n_async(uint8_t start_reg, void* buf, uint32_t len,
                          imu_i2c_cb_t cb, void* user)
{
  if (s_core_busy) return false;
  s_core_busy = true;

  s_op.user_cb  = cb;
  s_op.user_arg = user;
  s_op.out_u8   = NULL;
  s_op.is_rx_u8 = false;

  // retry metadata + transfer description
  s_op.retries_left = I2C_CORE_MAX_RETRIES;
  s_op.reg      = start_reg;
  s_op.len      = len;
  s_op.is_write = false;
  s_op.buf      = (uint8_t*)buf;   // ensure 'buf' is 4-byte aligned

  if (!i2c_submit_current()) { s_core_busy = false; return false; }
  return true;
}

// ---------- Non-blocking IMU init sequencer ----------
static struct {
  uint8_t      who;
  uint8_t      step;     // 0..4
  imu_i2c_cb_t done_cb;
  void*        user;
  bool         ok;
} s_init = {0};

static void _imu_init_next(bool ok, void* /*user*/)
{
  s_init.ok = s_init.ok && ok;

  switch (++s_init.step) {
    case 1: // WHOAMI
      imu_reg_read_u8_async(ISM330_REG_WHOAMI, &s_init.who, _imu_init_next, NULL);
      break;

    case 2: // CTRL3_C: IF_INC | BDU
      imu_reg_write_u8_async(ISM330_REG_CTRL3_C, (1<<2) | (1<<6), _imu_init_next, NULL);
      break;

    case 3: // CTRL1_XL
      imu_reg_write_u8_async(ISM330_REG_CTRL1_XL, 0xDC, _imu_init_next, NULL);
      break;

    case 4: // CTRL2_G
      imu_reg_write_u8_async(ISM330_REG_CTRL2_G,  0xDC, _imu_init_next, NULL);
      break;

    default: // done
      if (s_init.done_cb) s_init.done_cb(s_init.ok, s_init.user);
      break;
  }
}

bool imu_init_async(imu_i2c_cb_t done_cb, void* user)
{
  if (s_core_busy) return false; // keep simple: start when idle
  s_init.step = 0;
  s_init.ok   = true;
  s_init.done_cb = done_cb;
  s_init.user    = user;
  _imu_init_next(true, NULL);    // kick step 1
  return true;
}

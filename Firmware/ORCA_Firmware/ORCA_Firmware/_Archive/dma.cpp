#include "main.h"

volatile bool g_imu_kick = false;
// Borrow IOM from i2c core
static void* s_iom = nullptr;

// DMA busy flag (separate from I²C core’s single-op gate)
static volatile bool s_dma_busy = false;

// RX double buffer (word-aligned)
static uint32_t s_rx0[(ISM330_BURST_LEN + 3) / 4] __attribute__((aligned(4)));
static uint32_t s_rx1[(ISM330_BURST_LEN + 3) / 4] __attribute__((aligned(4)));
static volatile uint32_t* s_rx_inflight = s_rx0;

// Ring buffer
static imu_sample_t s_ring[IMU_RING_SIZE] = {};
static volatile uint32_t s_head = 0, s_tail = 0;

static inline bool ring_push_(const imu_sample_t& s)
{
  uint32_t n = (s_head + 1) % IMU_RING_SIZE;
  if (n == s_tail) return false;
  s_ring[s_head] = s;
  s_head = n;
  return true;
}

bool imu_pop_sample(imu_sample_t* out)
{
  if (s_tail == s_head) return false;
  *out = s_ring[s_tail];
  s_tail = (s_tail + 1) % IMU_RING_SIZE;
  return true;
}

bool imu_dma_busy(void) { return s_dma_busy; }

void imu_dma_init(void)
{
  s_iom = imu_i2c_get_handle();
  // Nothing else required — the NB queue & ISR are already enabled by imu_i2c_init().
}

// Static callback context (valid until cb runs)
typedef struct { uint32_t tick; volatile uint32_t* rxw; } rx_ctxt_t;
static rx_ctxt_t s_ctxt;

static void iom_read_cb(void* pCtxt, uint32_t /*status*/)
{
  const rx_ctxt_t* c = (const rx_ctxt_t*)pCtxt;
  const uint8_t* b   = (const uint8_t*)c->rxw;

  imu_sample_t s;
  s.tick = c->tick;

  // GxL,GxH,GyL,GyH,GzL,GzH, AxL,AxH,AyL,AyH,AzL,AzH
  s.gx = (int16_t)((uint16_t)b[0]  | ((uint16_t)b[1]  << 8));
  s.gy = (int16_t)((uint16_t)b[2]  | ((uint16_t)b[3]  << 8));
  s.gz = (int16_t)((uint16_t)b[4]  | ((uint16_t)b[5]  << 8));
  s.ax = (int16_t)((uint16_t)b[6]  | ((uint16_t)b[7]  << 8));
  s.ay = (int16_t)((uint16_t)b[8]  | ((uint16_t)b[9]  << 8));
  s.az = (int16_t)((uint16_t)b[10] | ((uint16_t)b[11] << 8));

  (void)ring_push_(s);
  s_dma_busy = false;
}

bool imu_dma_kick(uint32_t tick)
{
  if (s_dma_busy) return false;
  s_dma_busy = true;

  // Toggle RX buffer
  s_rx_inflight = (s_rx_inflight == s_rx0) ? s_rx1 : s_rx0;

  // Prepare context
  s_ctxt.tick = tick;
  s_ctxt.rxw  = s_rx_inflight;

  am_hal_iom_transfer_t x = {0};
  x.uPeerInfo.ui32I2CDevAddr = ISM330_I2C_ADDR;
  x.ui32InstrLen   = 1;
  x.ui32Instr      = ISM330_REG_BURST_START;
  x.eDirection     = AM_HAL_IOM_RX;
  x.ui32NumBytes   = ISM330_BURST_LEN;
  x.pui32RxBuffer  = (uint32_t*)s_rx_inflight;

  uint32_t rc = am_hal_iom_nonblocking_transfer(s_iom, &x, iom_read_cb, &s_ctxt);
  if (rc != AM_HAL_STATUS_SUCCESS) {
    s_dma_busy = false;
    return false;
  }
  return true;
}

#pragma once
#include <stdint.h>
#include <stdbool.h>
#include "am_mcu_apollo.h"
#include "am_bsp.h"
#include "am_hal_iom.h"
#include "am_hal_interrupt.h"

#ifdef __cplusplus
extern "C" {
#endif

// ===== Board & device config =====
#ifndef IMU_IOM_MODULE
#  define IMU_IOM_MODULE  1        // IOM instance (0..5)
#endif

#ifndef ISM330_I2C_ADDR
#  define ISM330_I2C_ADDR 0x6A     // SA0=0 -> 0x6A, SA0=1 -> 0x6B
#endif

#define ISM330_REG_WHOAMI       0x0F
#define ISM330_REG_CTRL1_XL     0x10
#define ISM330_REG_CTRL2_G      0x11
#define ISM330_REG_CTRL3_C      0x12
#define ISM330_REG_BURST_START  0x22    // OUTX_L_G
#define ISM330_BURST_LEN        12      // Gx,Gy,Gz,Ax,Ay,Az

// Auto-retry count for non-blocking I²C core ops (write_u8/read_u8/read_n)
#ifndef I2C_CORE_MAX_RETRIES
#define I2C_CORE_MAX_RETRIES 2
#endif

typedef struct 
{
  uint32_t tick;
  int16_t  gx, gy, gz;
  int16_t  ax, ay, az;
} imu_sample_t;

// User callback type (true on success)
typedef void (*imu_i2c_cb_t)(bool ok, void* user);

// ===== I²C core (all non-blocking) =====
// Initializes IOM, configures NB queue, enables IRQs. Returns immediately.
void     imu_i2c_init(void);

// Async write 1 byte: buffer is internal, safe to pass a literal.
// Only 1 I²C core op at a time (returns false if busy).
bool     imu_reg_write_u8_async(uint8_t reg, uint8_t val,
                                imu_i2c_cb_t cb, void* user);

// Async read 1 byte: 'out' must remain valid until cb fires.
bool     imu_reg_read_u8_async(uint8_t reg, uint8_t* out,
                               imu_i2c_cb_t cb, void* user);

// Async read N bytes: 'buf' must remain valid until cb fires.
bool     imu_reg_read_n_async(uint8_t start_reg, void* buf, uint32_t len,
                              imu_i2c_cb_t cb, void* user);

// Convenience: non-blocking, chained IMU config (WHOAMI -> CTRL3_C -> CTRL1_XL -> CTRL2_G).
// Calls 'done_cb' when finished (ok indicates full success).
bool     imu_init_async(imu_i2c_cb_t done_cb, void* user);

// Low-level handles (used by DMA layer)
void*    imu_i2c_get_handle(void);
uint32_t imu_i2c_get_module(void);

// Is the I²C core currently busy with an async op?
extern bool     imu_i2c_core_busy(void);
extern void i2c_core_cb(void* pCtxt, uint32_t status);
extern void imu_i2c_poll(void);
extern bool i2c_submit_current(void);

#ifdef __cplusplus
}
#endif

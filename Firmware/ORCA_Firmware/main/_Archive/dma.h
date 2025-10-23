#pragma once
#include <stdint.h>
#include <stdbool.h>
#include "am_hal_iom.h"

#ifdef __cplusplus
extern "C" {
#endif

#ifndef IMU_RING_SIZE
#  define IMU_RING_SIZE 64
#endif

#ifndef IMU_DMA_MAX_RETRIES
#define IMU_DMA_MAX_RETRIES 2
#endif

// Start DMA (NB) side — nothing blocking, just prepares internal buffers.
// Call after imu_i2c_init().
void imu_dma_init(void);

// Schedule a 12-byte burst read (OUTX_L_G..AzH).
// Returns false if the previous burst is still in flight.
bool imu_dma_kick(uint32_t tick);
extern volatile bool g_imu_kick;

// Pull the next sample from the ring (false if empty).
bool imu_pop_sample(imu_sample_t* out);

// Is a burst currently in flight?
bool imu_dma_busy(void);

#ifdef __cplusplus
}
#endif

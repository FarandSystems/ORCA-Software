#ifndef I2C_H
#define I2C_H

#include <stdbool.h>
#include <stdint.h>

// Init I2C bus, Qwiic power switch (if present), and try sensors once
void i2c_init(void);

// Periodic non-blocking sensor read (call via Ticker)
void i2c_read(void);

// Print IMU settings (debug)
void IMU_print_settings(void);

// Power control for Qwiic switch (safe if not present)
bool i2c_power_on(void);
void i2c_power_off(void);

// (Re)initialize sensors without changing power
bool i2c_reinit(void);

// Convenience: true if sensors are initialized & should be read
bool sensors_ready(void);

// Change IMU output data rates using compact codes
// acc_code: 0..9 -> 12.5,26,52,104,208,416,833,1.66k,3.33k,6.66k Hz
// gyr_code: same mapping
void imu_set_rates(uint8_t acc_code, uint8_t gyr_code);

#endif // I2C_H

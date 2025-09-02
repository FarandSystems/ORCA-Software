#ifndef MAIN_H
#define MAIN_H

#include <stdint.h>

// Pins (from your sketch)
#define IRIDIUM_PWR_PIN     22
#define IRIDIUM_SLEEP_PIN   17
#define LED_PIN             19
#define BUZZER_PIN          42
#define TOGGLE_PIN          43


// PHT (integer - you can choose scale: here native library floats -> cast to int)
extern volatile int32_t g_Temperature;
extern volatile int32_t g_Pressure;
extern volatile int32_t g_Humidity;

// IMU outputs (float SI units)
extern float Acc_X, Acc_Y, Acc_Z;     // m/s^2
extern float GyroX, GyroY, GyroZ;     // rad/s (Adafruit returns rad/s)

// Magnetometer outputs (microTesla)
extern float Mag_X, Mag_Y, Mag_Z;

// UART Rx/Tx buffers
extern uint8_t rx_Buffer[8];
extern uint8_t tx_Buffer[64];

// Reporting enable flag
extern volatile bool g_report_enabled;

#endif // ORCA_Firmware_H

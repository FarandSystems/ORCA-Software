#ifndef MAIN_H
#define MAIN_H

#include <stdint.h>

// Real-time PHT values (integer, no decimals)
extern volatile int32_t g_Temperature;
extern volatile int32_t g_Pressure;
extern volatile int32_t g_Humidity;

// // IMU outputs (integers or scaled as you like; here we take one decimal)
extern float Acc_X;   // N‐axis acceleration ×10
extern float Acc_Y;   // E‐axis acceleration ×10
extern float Acc_Z;   // D‐axis acceleration ×10
extern float GyroX;     // degrees ×10
extern float GyroY;   // degrees ×10
extern float GyroZ;    // degrees ×10

// Magnetometer outputs
extern float Mag_X;
extern float Mag_Y;
extern float Mag_Z;

// UART Rx/Tx buffers (16 or larger as you need)
extern uint8_t rx_Buffer[8];
extern uint8_t tx_Buffer[64];

#endif // MAIN_H
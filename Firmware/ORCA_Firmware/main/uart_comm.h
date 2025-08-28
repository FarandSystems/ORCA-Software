#pragma once
#include <stdint.h>

// Call once after Serial1.begin()
void UART_init();

// Non-blocking: feed bytes from UART1 and dispatch full 8-byte commands
void UART_poll();

// Build & send the 64-byte telemetry frame (call periodically)
void Report_Measured_Data();

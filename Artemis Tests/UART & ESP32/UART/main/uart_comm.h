#pragma once

// Call once after Serial1.begin()
void UART_init();

// Called periodically (e.g. via Ticker) to poll RX and prepare TX
void UART_poll();

// Called every report tick (e.g. 1000 ms)
void Report_Measured_Data();
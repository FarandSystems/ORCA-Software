#ifndef UART_H
#define UART_H

#include <Arduino.h>

#define UART_RX_PIN   16
#define UART_TX_PIN   17
#define UART_BAUDRATE 9600
#define UART_RX_PACKET_SIZE 80
#define UART_TX_PACKET_SIZE 8


// === Shared state flags ===
extern volatile bool uart_rx_ready;
extern volatile bool uart_tx_ready;

// === Buffers ===
extern uint8_t uart_rx_buffer[UART_RX_PACKET_SIZE];
extern uint8_t uart_rx_buffer_temp[UART_RX_PACKET_SIZE];

extern uint8_t uart_tx_buffer[UART_TX_PACKET_SIZE];
extern uint8_t uart_tx_buffer_temp[UART_TX_PACKET_SIZE];

// === API Functions ===
extern void uart_init(void);
extern void uart_send_packet(void);
extern void uart_process_rx(void);

#endif // UART_H

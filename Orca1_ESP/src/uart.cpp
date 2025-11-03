#include "main.h"
#include "uart.h"
#include <HardwareSerial.h>


HardwareSerial SerialPort(2);// Use UART2 (RX=16, TX=17)

// === Globals ===
volatile bool uart_rx_ready = false;
volatile bool uart_tx_ready = false;

uint8_t uart_rx_buffer[UART_RX_PACKET_SIZE];
uint8_t uart_rx_buffer_temp[UART_RX_PACKET_SIZE];

uint8_t uart_tx_buffer[UART_TX_PACKET_SIZE];
uint8_t uart_tx_buffer_temp[UART_TX_PACKET_SIZE];

//=== ISR for RX ===
void IRAM_ATTR uart_rx_isr() 
{
  while (SerialPort.available() >= UART_RX_PACKET_SIZE) 
  {
    for (int i = 0; i < UART_RX_PACKET_SIZE; i++) 
    {
      uart_rx_buffer_temp[i] = SerialPort.read();
    }
    uart_rx_ready = true;

  }
}

// === Initialization ===
void uart_init(void) 
{
//   SerialPort.begin(UART_BAUDRATE, SERIAL_8N1, UART_RX_PIN, UART_TX_PIN);
  SerialPort.onReceive(uart_rx_isr);
    SerialPort.begin(9600, SERIAL_8N1, 16, 17); 

  // Optional: fill TX buffer with example data
  // for (int i = 0; i < UART_TX_PACKET_SIZE; i++) 
  // {
  //   uart_tx_buffer[i] = i + 1;
  // }
}

// === Send an 8-byte packet ===
void uart_send_packet(void) 
{
  
  for (int i = 0; i < UART_TX_PACKET_SIZE; i++) 
  {
   uart_tx_buffer[i] = tcp_rx_buf[i] ;
  }
  SerialPort.write(uart_tx_buffer,UART_TX_PACKET_SIZE);
  
  uart_tx_ready = false;

}


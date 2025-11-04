#include "main.h"
#include "uart.h"
#include <HardwareSerial.h>


HardwareSerial SerialPort(2);// Use UART2 (RX=16, TX=17)

// === Globals ===
volatile bool uart_rx_ready = false;
volatile bool uart_tx_ready = false;

uint32_t uart_timeout_counter = 0;   // increments each tick
volatile bool uart_reset_request   = false; // set in tick, handled in loop

bool VerifyCheckSumandHeader(uint8_t* frame);
void Clear_All_Buffers();
void send_uart_heartbeat_packet();


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
    
   
  }
  if (VerifyCheckSumandHeader(uart_rx_buffer_temp))
  {
    uart_rx_ready = true;
    uart_timeout_counter = 0;
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
  if(tcp_rx_ready)
  {
    
    for (int i = 0; i < UART_TX_PACKET_SIZE; i++) 
    {
      uart_tx_buffer[i] = tcp_rx_buf[i] ;
    }
  }
  else
  {
    send_uart_heartbeat_packet();
  }
  
  tcp_rx_ready = false;
  
  SerialPort.write(uart_tx_buffer,UART_TX_PACKET_SIZE);

}
void send_uart_heartbeat_packet()
{
    Clear_All_Buffers();
    uart_tx_buffer[0] = 0x55;  // Header

    uart_tx_buffer[UART_TX_PACKET_SIZE - 1] = 0x55;  // Checksum
}

void Clear_All_Buffers()
{
   for (int i = 0; i < UART_TX_PACKET_SIZE; i++) 
   {
     uart_tx_buffer[i] = 0 ;
   }
}



bool VerifyCheckSumandHeader(uint8_t* frame)
{
   for (int p = 0; p < 5; ++p)
  {
    int base = p * 16;

    // 1) header
    if (frame[base + 0] != 0xFA)
      return false;

    // 2) checksum (includes header by default)
    uint8_t sum = 0;
    for (int b = 0; b < 15; ++b)
      sum += frame[base + b];

    if (sum != frame[base + 15])
      return false;
  }
  return true;
}

void uart_force_reset()
{
  // Stop RX state flags if you use them
  // uart_rx_ready = false; uart_rx_error = false;  // if present

  SerialPort.end();
  delay(5);  // brief pause allows driver to settle
  
  uart_init();
  // Flush any garbage
  while (SerialPort.available())
   { 
    SerialPort.read();
   }

  // Reset the watchdog counter
  uart_timeout_counter = 0;
}

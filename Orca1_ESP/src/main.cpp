#include "main.h"


uint8_t demo_tcp_tx [80];

const uint32_t TX_PERIOD_MS = 125;
uint32_t last_tx_ms = 0;

void setup()
{
  // put your setup code here, to run once:
 init_gpio();
 init_timers();
 wifi_init();
 wifi_config();
 uart_init();
 Serial.begin(115200);

 last_tx_ms = millis();



// for (uint8_t i = 0; i < 5; i++)
// {
//     uint8_t cs = 0xFA;
//     uint8_t base_Index = i * 16;

//     demo_tcp_tx[base_Index + 0] = 0xFA; // header

//     // j = 0..13 → write to bytes 1..14
//     for (uint8_t j = 0; j < 14; j++)
//     {
//         uint8_t value = i * (j + 1);   // same math as before
//         demo_tcp_tx[base_Index + 1 + j] = value;
//         cs += value;
//     }

//     demo_tcp_tx[base_Index + 15] = cs; // checksum
// }
// for (uint8_t i = 0; i < 80; i++)
// {
//   demo_tcp_tx[i] = i;
// }


 
}

void loop() 
{
  // put your main code here, to run repeatedly:
  // === UART State Machine ===

  if (uart_tx_ready) 
  {

    uart_send_packet();  // Send prepared packet
  }

  if(uart_rx_ready)
  {
    for(int i = 0; i < UART_RX_PACKET_SIZE ;i++)
    {
      uart_rx_buffer[i] = uart_rx_buffer_temp[i];
      uart_rx_buffer_temp[i] = 0;
    }

    tcp_send_bytes (uart_rx_buffer , sizeof(uart_rx_buffer));
    uart_rx_ready = false;
  }

  wifi_task();

    if (tcp_rx_ready)
    {
        tcp_rx_ready = false;

        Serial.print("[TCP] Got ");
        Serial.print(tcp_rx_len);
        Serial.println(" bytes from socket:");

        for (size_t i = 0; i < tcp_rx_len; i++)
        {
            Serial.printf(" %02X", tcp_rx_buf[i]);
        }
        Serial.println();

        // Echo to client
        // tcp_send_bytes((uint8_t*)tcp_rx_buf, tcp_rx_len);
    }
      
      

  //  uart_process_rx();   // Process received data

 for (uint8_t i = 0; i < 80; i++) 
 {
  Serial.printf("Buf[%u] = %u\n", i, uart_rx_buffer[i]);
 }
//  wifi_process();

  
}
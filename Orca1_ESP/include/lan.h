#ifndef WIFI_MODULE_H
#define WIFI_MODULE_H

#include <Arduino.h>
#include <WiFi.h>
#include <WiFiClient.h>  // TCP client

// === Wi-Fi Configuration ===
#define WIFI_SSID     "ORCA"
#define WIFI_PASSWORD "Orca8868"
#define TCP_PORT        5001

// === Connection flags ===
extern volatile bool wifi_connected;
extern volatile bool tcp_client_connected;
extern volatile bool tcp_rx_ready;

// RX buffer for TCP data
const uint16_t TCP_RX_BUF_SIZE = 8; 
extern uint8_t tcp_rx_buf[TCP_RX_BUF_SIZE];
extern size_t  tcp_rx_len;

// === API ===
extern void wifi_init(void);
extern void wifi_process(void);    // optional state-machine updater
extern void wifi_config(void);

extern void wifi_task();                 // service WiFi/TCP events (call often but fast)
bool tcp_send_bytes(uint8_t *data, size_t len); // send to PC/client

#endif // WIFI_MODULE_H

#include <Arduino.h>
#include <Ticker.h>

#include "main.h"
#include "uart_comm.h"
#include "i2c.h"



// define globals from main.h
volatile int32_t g_Temperature = 0;
volatile int32_t g_Pressure    = 0;
volatile int32_t g_Humidity    = 0;

float Acc_X  = 0;
float Acc_Y  = 0;
float Acc_Z  = 0;
float GyroX    = 0;
float GyroY  = 0;
float GyroZ   = 0;

float Mag_X = 0;
float Mag_Y = 0;
float Mag_Z = 0;

uint8_t rx_Buffer[8];
uint8_t tx_Buffer[64];

#define IRIDIUM_PWR_PIN  22
#define IRIDIUM_SLEEP_PIN 17

// Ticker objects: callback, interval_ms, repeats=0 (forever), mode=MILLIS
Ticker i2cTicker((void (*)())i2c_read, 1, 0, MILLIS); // 1khz
Ticker uartTicker((void (*)())UART_poll, 10, 0, MILLIS); // 100 hz
Ticker reportTicker((void (*)())Report_Measured_Data, 50, 0, MILLIS);


void setup() {
  // power‐control pins
  pinMode(IRIDIUM_PWR_PIN, OUTPUT);
  digitalWrite(IRIDIUM_PWR_PIN, LOW);
  pinMode(IRIDIUM_SLEEP_PIN, OUTPUT);
  digitalWrite(IRIDIUM_SLEEP_PIN, LOW);

  Serial.begin(115200);
  while (!Serial);
  Serial1.begin(115200);
  while (!Serial1);

  // init subsystems
  i2c_init();
  UART_init();

  // start all tickers
  uartTicker.start();
  //phtTicker.start();
  i2cTicker.start();
  reportTicker.start();

  Serial.println("Artemis w/ I2C & UART tickers");
}

void loop() {
  // must call update() each pass to service callbacks
  uartTicker. update();
  i2cTicker.  update();
  reportTicker.update();
}

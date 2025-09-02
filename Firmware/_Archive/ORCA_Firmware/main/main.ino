#include <Arduino.h>
#include <Ticker.h>
#include "main.h"
#include "i2c.h"
#include "uart_comm.h"
#include "Alarm.h"

// -------- Globals (defined) --------
volatile int32_t g_Temperature = 0;
volatile int32_t g_Pressure    = 0;
volatile int32_t g_Humidity    = 0;

float Acc_X = 0, Acc_Y = 0, Acc_Z = 0;
float GyroX = 0, GyroY = 0, GyroZ = 0;
float Mag_X = 0, Mag_Y = 0, Mag_Z = 0;

uint8_t rx_Buffer[8];
uint8_t tx_Buffer[64];

volatile bool g_report_enabled = true;

static void ReportTick()
{
  if (g_report_enabled) {
    Report_Measured_Data();   // keeps your 64-byte format exactly the same
  }
}

// Tickers: keep periods short but reasonable for libs
// IMU @ ~500 Hz, Mag ~155 Hz, Baro slower; we read all at 2 ms (500 Hz) to keep it simple
Ticker i2cTicker   ((void (*)())i2c_read,            10, 0, MILLIS);
Ticker uartTicker  ((void (*)())UART_poll,          10, 0, MILLIS); // 100 Hz poll for 8B cmds
Ticker reportTicker((void (*)())ReportTick, 125, 0, MILLIS); // 20 Hz telemetry


Ticker alarmTicker([]{
  Alarm_Update_32Hz();           // ~32 Hz driver; choose 31 or 32 ms
}, 31, 0, MILLIS);

void setup() {
  // Basic rails
  pinMode(IRIDIUM_PWR_PIN, OUTPUT);
  digitalWrite(IRIDIUM_PWR_PIN, LOW);
  pinMode(IRIDIUM_SLEEP_PIN, OUTPUT);
  digitalWrite(IRIDIUM_SLEEP_PIN, LOW);

  Serial.begin(115200);
  while (!Serial) { }  // if USB

  // Your buzzer pin here (example: D19), active-high
  Alarm_Init(BUZZER_PIN, /*activeHigh=*/true);

  

  // UART1 <-> ESP32
  Serial1.begin(115200);
  

  // Subsystems
  i2c_init();
  UART_init();

  // Start timers
  i2cTicker.start();
  //uartTicker.start();
  //reportTicker.start();
  //alarmTicker.start();
  
  Serial.println("AGT: I2C + UART + Qwiic (non-blocking)");
}

void loop() {
  // Service tickers regularly
  //uartTicker.update();
  i2cTicker.update();
  
  //reportTicker.update();
  //alarmTicker.update();
}

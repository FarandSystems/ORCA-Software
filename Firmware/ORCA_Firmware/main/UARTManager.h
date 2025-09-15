#pragma once
#include <Arduino.h>
#include <mbed.h>
#include <stdint.h>
#include <math.h>

#include "BoardControl.h"
#include "PowerManager.h"
#include "SensorsManager.h"
#include "SerialDebugger.h"
#include "Alarm.h"

// Optional: define LED pin here if you want the blink lines enabled.
// Commented out to avoid board-specific pin assumptions.
// #define UART_LED_PIN 13

class UartManager
{
public:
  UartManager(BoardControl& board, PowerManager& power, SensorsManager& sensors);

  // Start Serial1 at given baud
  void begin(uint32_t baud);

   // New: poll with explicit period (ms). We’ll call poll(125) from RTOS.
  void poll(uint32_t periodMs);

  // Force a telemetry frame right now (optional helper)
  void sendReportNow();

  // Query keepalive state
  uint32_t lastHeartbeatMs() const;
  bool linkAlive(uint32_t timeoutMs) const;

  // Expose report enable (mirrors 0x04/0x05 commands)
  void setReportEnabled(bool on);
  bool reportEnabled() const;

private:
  // === Command handling ===
  void serviceInputCommand_();                 // read & dispatch 8B command if available
  void handleCommand_(uint8_t* rx);            // your switch-case, plus 0xFF keepalive

  // === Operations mapped to your original API names ===
  bool i2c_power_on_();                        // power + (re)init sensors
  void i2c_power_off_();                       // power off sensors rail
  bool i2c_reinit_();                          // reinit sensors only
  void imu_set_rates_(uint8_t acc_code, uint8_t gyr_code); // (stub/log for now)

  // === Telemetry ===
  void maybeSendReport_();                     // 125 ms cadence (8 Hz)
  void buildAndSendReport_();                  // fills tx_Buffer[64] and Serial1.write()

  void buildAndSendHeartbeat_();

private:
  BoardControl&   m_board;
  PowerManager&   m_power;
  SensorsManager& m_sensors;

  // RX/TX buffers per your layout
  uint8_t m_rx[8];
  uint8_t m_tx[64];

  // state
  volatile bool   m_reportEnabled;
  uint8_t         m_cntTX;
  float           m_timeTicks;

  // Accumulators in ms
  uint32_t        m_sinceReportMs;
  uint32_t        m_hbAccumMs;

  volatile uint32_t m_lastHeartbeatMs;

  // cached floats for packaging (units see buildAndSendReport_)
  float m_accX, m_accY, m_accZ;
  float m_gyroX, m_gyroY, m_gyroZ;
  float m_magX, m_magY, m_magZ;

  static constexpr uint32_t REPORT_MS    = 125;  // 8 Hz
};

#include "UartManager.h"

static constexpr float G_PER_LSB       = 0.000061f;     // ISM330 acc
static constexpr float DPS_PER_LSB     = 0.004375f;     // ISM330 gyro
static constexpr float G_TO_MS2        = 9.80665f;

UartManager::UartManager(BoardControl& board, PowerManager& power, SensorsManager& sensors)
  : m_board(board),
    m_power(power),
    m_sensors(sensors),
    m_reportEnabled(false),
    m_cntTX(0),
    m_sinceReportMs(0),
    m_hbAccumMs(0),
    m_lastHeartbeatMs(0),
    m_accX(0), m_accY(0), m_accZ(0),
    m_gyroX(0), m_gyroY(0), m_gyroZ(0),
    m_magX(0), m_magY(0), m_magZ(0)
{
}

void UartManager::begin(uint32_t baud)
{
  Serial1.begin(baud);
  while (!Serial1)
  {
    // wait if needed for USB<->UART bridges; usually quick
  }

#ifdef UART_LED_PIN
  pinMode(UART_LED_PIN, OUTPUT);
  digitalWrite(UART_LED_PIN, LOW);
#endif

  LOGI("[UART] Serial1 started @%lu", (unsigned long)baud);
}

void UartManager::poll(uint32_t periodMs)
{
  // RX: handle any pending 8B command(s)
  serviceInputCommand_();

  // TX: pace telemetry at 8 Hz
  m_sinceReportMs += periodMs;
  if (m_sinceReportMs >= REPORT_MS)
  {
    m_sinceReportMs = 0;
    maybeSendReport_();
  }
}

void UartManager::sendReportNow()
{
  maybeSendReport_();
}

uint32_t UartManager::lastHeartbeatMs() const
{
  return m_lastHeartbeatMs;
}

bool UartManager::linkAlive(uint32_t timeoutMs) const
{
  return (millis() - m_lastHeartbeatMs) <= timeoutMs;
}

void UartManager::setReportEnabled(bool on)
{
  m_reportEnabled = on;
  LOGI("[UART] report %s", on ? "ENABLED" : "DISABLED");
}

bool UartManager::reportEnabled() const
{
  return m_reportEnabled;
}

// === private ===

void UartManager::serviceInputCommand_()
{
  // Expect fixed 8-byte commands
  while (Serial1.available() >= 8)
  {
    size_t got = Serial1.readBytes(m_rx, sizeof(m_rx));
    if (got != sizeof(m_rx))
    {
      LOGW("[UART] short read: %lu", (unsigned long)got);
      return;
    }
    handleCommand_(m_rx);
  }
}

void UartManager::handleCommand_(uint8_t* Rx)
{
  // Rx[0] = don't care/sync, Rx[1] = opcode
  uint8_t op = Rx[1];

  switch (op)
  {
    case 0x01: // Qwiic ON -> power + reinit
    {
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      LOGI("CMD: QWIIC ON");
      if (i2c_power_on_())
      {
        LOGI("Sensors OK");
      }
      else
      {
        LOGE("Sensors FAIL");
      }
    } break;

    case 0x02: // Qwiic OFF
    {
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      LOGI("CMD: QWIIC OFF");
      i2c_power_off_();
    } break;

    case 0x03: // Reinit sensors only
    {
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      LOGI("CMD: REINIT");
      if (i2c_reinit_())
      {
        LOGI("Reinit OK");
      }
      else
      {
        LOGE("Reinit FAIL");
      }
    } break;

    case 0x04: // Start reporting
    {
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      LOGI("CMD: REPORT START");
      setReportEnabled(true);
    } break;

    case 0x05: // Stop reporting
    {
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      LOGI("CMD: REPORT STOP");
      setReportEnabled(false);
    } break;

    case 0x06: // Set IMU ODR: Rx[2]=acc_code, Rx[3]=gyr_code
    {
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      LOGI("CMD: IMU ODR %u,%u", Rx[2], Rx[3]);
      imu_set_rates_(Rx[2], Rx[3]);
    } break;

    default:
    {
      // Expand with your legacy slots if needed
      LOGW("CMD: unknown 0x%02X", op);
    } break;
  }
}

bool UartManager::i2c_power_on_()
{
  // Power rail on, small delay, (re)init sensors
  if (!m_power.enableSensorsRail(true))
  {
    LOGE("enableSensorsRail(true) failed");
    return false;
  }
  delay(20);

  // If IOM was never started, start it now (safe to call if already up)
  if (!m_board.iomHandle())
  {
    if (!m_board.initI2C_IOM1_D8D9_1MHz())
    {
      LOGE("I2C init failed on power-on");
      return false;
    }
  }

  return m_sensors.init(m_board);
}

void UartManager::i2c_power_off_()
{
  m_power.enableSensorsRail(false);
}

bool UartManager::i2c_reinit_()
{
  return m_sensors.init(m_board);
}

void UartManager::imu_set_rates_(uint8_t acc_code, uint8_t gyr_code)
{
  // Placeholder: wire to SensorsManager method when available.
  // For now just log the request.
  LOGI("imu_set_rates(acc=%u, gyr=%u) [stub]", acc_code, gyr_code);
}

void UartManager::maybeSendReport_()
{
  if (!m_reportEnabled)
  {
    buildAndSendHeartbeat_();
  }
  else 
  {
    buildAndSendReport_();
  }
  
}

void UartManager::buildAndSendReport_()
{
  // Pull freshest IMU sample and map to floats
  ImuSample s = m_sensors.getLatest();

  // Convert raw -> engineering-ish units:
  // acc: g -> m/s^2 ; gyro: dps (stay in deg/s)
  float ax_ms2 = s.ax * G_PER_LSB * G_TO_MS2;
  float ay_ms2 = s.ay * G_PER_LSB * G_TO_MS2;
  float az_ms2 = s.az * G_PER_LSB * G_TO_MS2;

  float gx_dps = s.gx * DPS_PER_LSB;
  float gy_dps = s.gy * DPS_PER_LSB;
  float gz_dps = s.gz * DPS_PER_LSB;

  // If you wanted the demo sin/cos wave instead, uncomment:
  // static float time_ticks_local = 0.0f;
  // time_ticks_local += 0.125f;
  // ax_ms2 = 10.0f * cosf(2.0f * (float)M_PI / 2.0f * time_ticks_local);
  // ay_ms2 = 10.0f * sinf(2.0f * (float)M_PI / 2.0f * time_ticks_local);

  m_accX = ax_ms2;
  m_accY = ay_ms2;
  m_accZ = az_ms2;

  m_gyroX = gx_dps;
  m_gyroY = gy_dps;
  m_gyroZ = gz_dps;

  // Mag/PHT not wired yet — send zeros
  m_magX = 0.0f; m_magY = 0.0f; m_magZ = 0.0f;

  // ==== Build 64B packet (exact byte layout you provided) ====
  m_cntTX++;
  m_timeTicks += 0.125f;

  // Header
  m_tx[0] = 0x00;          // sync / reserved
  m_tx[1] = m_cntTX;
  m_tx[2] = 0x55;
  m_tx[3] = 0x55;

  // Temperature (short), Pressure (short), Humidity (short)
  int16_t temperature = 0; // fill when you have PHT
  int16_t pressure    = 0;
  int16_t humidity    = 0;

  m_tx[4] = (uint8_t)((temperature >> 8) & 0xFF);
  m_tx[5] = (uint8_t)((temperature >> 0) & 0xFF);

  m_tx[6] = (uint8_t)((pressure >> 8) & 0xFF);
  m_tx[7] = (uint8_t)((pressure >> 0) & 0xFF);

  m_tx[8] = (uint8_t)((humidity >> 8) & 0xFF);
  m_tx[9] = (uint8_t)((humidity >> 0) & 0xFF);

  // Helpers to write big-endian int32
  auto put_i32 = [&](int idx, int32_t v)
  {
    m_tx[idx + 0] = (uint8_t)((v >> 24) & 0xFF);
    m_tx[idx + 1] = (uint8_t)((v >> 16) & 0xFF);
    m_tx[idx + 2] = (uint8_t)((v >>  8) & 0xFF);
    m_tx[idx + 3] = (uint8_t)((v >>  0) & 0xFF);
  };

  // Acc in mm/s^2 * 1000 per your C# decoder (float*1000 then pack)
  int32_t accX_i32 = (int32_t)(m_accX * 1000.0f);
  int32_t accY_i32 = (int32_t)(m_accY * 1000.0f);
  int32_t accZ_i32 = (int32_t)(m_accZ * 1000.0f);

  put_i32(10, accX_i32);
  put_i32(14, accY_i32);
  put_i32(18, accZ_i32);

  // Gyro (deg/s) * 1000
  int32_t gyroX_i32 = (int32_t)(m_gyroX * 1000.0f);
  int32_t gyroY_i32 = (int32_t)(m_gyroY * 1000.0f);
  int32_t gyroZ_i32 = (int32_t)(m_gyroZ * 1000.0f);

  put_i32(22, gyroX_i32);
  put_i32(26, gyroY_i32);
  put_i32(30, gyroZ_i32);

  // Mag (uT) * 1000 (currently zeros)
  int32_t magX_i32 = (int32_t)(m_magX * 1000.0f);
  int32_t magY_i32 = (int32_t)(m_magY * 1000.0f);
  int32_t magZ_i32 = (int32_t)(m_magZ * 1000.0f);

  put_i32(34, magX_i32);
  put_i32(38, magY_i32);
  put_i32(42, magZ_i32);

  // Pad 46..62
  for (int i = 46; i < 63; ++i)
  {
    m_tx[i] = 0x00;
  }

  // Simple checksum over first 63 bytes
  uint8_t cs = 0;
  for (int i = 0; i < 63; ++i)
  {
    cs += m_tx[i];
  }
  m_tx[63] = cs;

  // Transmit
  Serial1.write(m_tx, sizeof(m_tx));

#ifdef UART_LED_PIN
  digitalWrite(UART_LED_PIN, HIGH);
  delay(2);
  digitalWrite(UART_LED_PIN, LOW);
#endif
}

void UartManager::buildAndSendHeartbeat_()
{
    // Build heartbeat packet
      // ==== Build 64B packet (exact byte layout you provided) ====
    m_cntTX++;
    m_timeTicks += 0.125f;

    // Header
    m_tx[0] = 0x00;          // sync / reserved
    m_tx[1] = m_cntTX;
    m_tx[2] = 0x55;
    m_tx[3] = 0x55;

    for (int i = 4; i < 63; ++i)
    {
        m_tx[i] = 0x00;
    }

    // Simple checksum over first 63 bytes
    uint8_t cs = 0;
    for (int i = 0; i < 63; ++i)
    {
        cs += m_tx[i];
    }
    m_tx[63] = cs;

    // Transmit
    Serial1.write(m_tx, sizeof(m_tx));
}



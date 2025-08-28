#include "uart_comm.h"
#include "main.h"
#include "i2c.h"
#include "Alarm.h"
#include <Arduino.h>


static uint8_t cnt_TX = 0;

// Non-blocking 8-byte assembler
static uint8_t s_rx8[8];
static uint8_t s_idx = 0;

static bool checksum_ok(const uint8_t* b) {
  uint8_t sum = 0;
  for (int i = 0; i < 7; ++i) sum += b[i];
  return b[7] == sum;
}

static void Service_Input_Command(uint8_t* Rx) {
  // Optional checksummed frame if starts with 0xA5
  if (Rx[0] == 0xA5 && !checksum_ok(Rx)) {
    Serial.println("CMD checksum fail");
    return;
  }
  Serial.println(Rx[1]);
  switch (Rx[1]) {
    case 0x01: // Qwiic ON -> power + reinit
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      digitalWrite(BUZZER_PIN, !digitalRead(BUZZER_PIN));
      Serial.println("CMD: QWIIC ON");
      if (i2c_power_on()) Serial.println("Sensors OK");
      else               Serial.println("Sensors FAIL");
      break;

    case 0x02: // Qwiic OFF
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      digitalWrite(BUZZER_PIN, !digitalRead(BUZZER_PIN));
      Serial.println("CMD: QWIIC OFF");
      i2c_power_off();
      break;

    case 0x03: // Reinit sensors only
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);

      Serial.println("CMD: REINIT");
      if (i2c_reinit()) Serial.println("Reinit OK");
      else              Serial.println("Reinit FAIL");
      break;

    case 0x04: // Start reporting
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      
      Serial.println("CMD: REPORT START");
      g_report_enabled = true;
      break;

    case 0x05: // Stop reporting
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
    
      Serial.println("CMD: REPORT STOP");
      g_report_enabled = false;
      break;

    case 0x06: // Set IMU ODR: Rx[2]=acc_code, Rx[3]=gyr_code
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);

      Serial.print("CMD: IMU ODR ");
      Serial.print(Rx[2]); Serial.print(", "); Serial.println(Rx[3]);
      imu_set_rates(Rx[2], Rx[3]);
      break;

    default:
      // Your legacy slots (0x00,0x02,...) can be added here
      // e.g. case 0x02: ...; break;
      break;
  }
  // tiny visual
  //digitalWrite(LED_PIN, !digitalRead(LED_PIN));
}

void UART_init() {
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW);
  s_idx = 0;
}

void UART_poll() {
  while (Serial1.available()) {
    int b = Serial1.read();
    if (b < 0) break;
    s_rx8[s_idx++] = (uint8_t)b;
    if (s_idx == 8) {
      Service_Input_Command(s_rx8);
      s_idx = 0;
    }
  }
}

void Report_Measured_Data(void) {
  if (!g_report_enabled || !sensors_ready()) return;

  cnt_TX++;

  // Header (keep your layout)
  tx_Buffer[0] = 0;        // sync/custom
  tx_Buffer[1] = cnt_TX;
  tx_Buffer[2] = 0x55;
  tx_Buffer[3] = 0x55;

  // Temperature
  tx_Buffer[ 4] = (g_Temperature >> 8) & 0xFF;
  tx_Buffer[ 5] = (g_Temperature >> 0) & 0xFF;

  // Pressure
  tx_Buffer[ 6] = (g_Pressure >> 8) & 0xFF;
  tx_Buffer[ 7] = (g_Pressure >> 0) & 0xFF;

  tx_Buffer[8] = 0; // reserved/status

  // Humidity
  tx_Buffer[ 9] = (g_Humidity >> 8) & 0xFF;
  tx_Buffer[10] = (g_Humidity >> 0) & 0xFF;

  // Accel (mm/s^2 for better integer precision)
  int32_t accX_i32 = (int32_t)(Acc_X * 1000.0f);
  int32_t accY_i32 = (int32_t)(Acc_Y * 1000.0f);
  int32_t accZ_i32 = (int32_t)(Acc_Z * 1000.0f);
  tx_Buffer[11] = (accX_i32 >> 24) & 0xFF;
  tx_Buffer[12] = (accX_i32 >> 16) & 0xFF;
  tx_Buffer[13] = (accX_i32 >> 8)  & 0xFF;
  tx_Buffer[14] =  accX_i32        & 0xFF;

  tx_Buffer[15] = (accY_i32 >> 24) & 0xFF;
  tx_Buffer[16] = (accY_i32 >> 16) & 0xFF;
  tx_Buffer[17] = (accY_i32 >> 8)  & 0xFF;
  tx_Buffer[18] =  accY_i32        & 0xFF;

  tx_Buffer[19] = (accZ_i32 >> 24) & 0xFF;
  tx_Buffer[20] = (accZ_i32 >> 16) & 0xFF;
  tx_Buffer[21] = (accZ_i32 >> 8)  & 0xFF;
  tx_Buffer[22] =  accZ_i32        & 0xFF;

  // Gyro (milli-rad/s)
  int32_t gyroX_i32 = (int32_t)(GyroX * 1000.0f);
  int32_t gyroY_i32 = (int32_t)(GyroY * 1000.0f);
  int32_t gyroZ_i32 = (int32_t)(GyroZ * 1000.0f);
  tx_Buffer[23] = (gyroX_i32 >> 24) & 0xFF;
  tx_Buffer[24] = (gyroX_i32 >> 16) & 0xFF;
  tx_Buffer[25] = (gyroX_i32 >> 8)  & 0xFF;
  tx_Buffer[26] =  gyroX_i32        & 0xFF;

  tx_Buffer[27] = (gyroY_i32 >> 24) & 0xFF;
  tx_Buffer[28] = (gyroY_i32 >> 16) & 0xFF;
  tx_Buffer[29] = (gyroY_i32 >> 8)  & 0xFF;
  tx_Buffer[30] =  gyroY_i32        & 0xFF;

  tx_Buffer[31] = (gyroZ_i32 >> 24) & 0xFF;
  tx_Buffer[32] = (gyroZ_i32 >> 16) & 0xFF;
  tx_Buffer[33] = (gyroZ_i32 >> 8)  & 0xFF;
  tx_Buffer[34] =  gyroZ_i32        & 0xFF;

  // Mag (milli-uT)
  int32_t MagX_i32 = (int32_t)(Mag_X * 1000.0f);
  int32_t MagY_i32 = (int32_t)(Mag_Y * 1000.0f);
  int32_t MagZ_i32 = (int32_t)(Mag_Z * 1000.0f);
  tx_Buffer[35] = (MagX_i32 >> 24) & 0xFF;
  tx_Buffer[36] = (MagX_i32 >> 16) & 0xFF;
  tx_Buffer[37] = (MagX_i32 >> 8)  & 0xFF;
  tx_Buffer[38] =  MagX_i32        & 0xFF;

  tx_Buffer[39] = (MagY_i32 >> 24) & 0xFF;
  tx_Buffer[40] = (MagY_i32 >> 16) & 0xFF;
  tx_Buffer[41] = (MagY_i32 >> 8)  & 0xFF;
  tx_Buffer[42] =  MagY_i32        & 0xFF;

  tx_Buffer[43] = (MagZ_i32 >> 24) & 0xFF;
  tx_Buffer[44] = (MagZ_i32 >> 16) & 0xFF;
  tx_Buffer[45] = (MagZ_i32 >> 8)  & 0xFF;
  tx_Buffer[46] =  MagZ_i32        & 0xFF;

  // pad zeros
  for (int i = 47; i < 63; ++i) tx_Buffer[i] = 0;

  // checksum
  uint8_t cs = 0;
  for (int i = 0; i < 63; ++i) cs += tx_Buffer[i];
  tx_Buffer[63] = cs;

  Serial1.write(tx_Buffer, 64);
}

#include <stdint.h>
#include "uart_comm.h"
#include "main.h"
#include "i2c.h"
#include "Alarm.h"
#include <Arduino.h>
#include <math.h>

static uint8_t cnt_TX = 0;
static float time_ticks = 0;

// Exactly your Service_Input_Command logic, using rx_Buffer[]
static void Service_Input_Command(uint8_t* RxBuffer) {
  switch (RxBuffer[1]) {
    case 0x01: // Qwiic ON -> power + reinit
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
      Serial.println("CMD: QWIIC ON");
      if (i2c_power_on()) Serial.println("Sensors OK");
      else               Serial.println("Sensors FAIL");
      break;

    case 0x02: // Qwiic OFF
      Alarm(SHORT_BEEP_X1, 1, 32, BEEP_ON);
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
      Serial.print(RxBuffer[2]); Serial.print(", "); Serial.println(RxBuffer[3]);
      imu_set_rates(RxBuffer[2], RxBuffer[3]);
      break;

    default:
      // Your legacy slots (0x00,0x02,...) can be added here
      // e.g. case 0x02: ...; break;
      break;
  }
}
float Get_Sin(float amp, float T) 
{
  float omega = 2 * M_PI / T; // Angular frequency: omega = 2 * pi * f
  float sin_answer = amp * sin(omega * time_ticks); // Calculate the sine value
  return sin_answer;
}

float Get_Cos(float amp, float T) 
{
  float omega = 2 * M_PI / T; // Angular frequency: omega = 2 * pi * f
  float cos_answer = amp * cos(omega * time_ticks); // Calculate the sine value
  return cos_answer;
}

// Builds & sends tx_Buffer based on your Report_Measured_Data example
void Report_Measured_Data(void) {
    cnt_TX++;
    time_ticks += 0.125;
    // header
    tx_Buffer[0] = 0;         //0x1A or your chosen sync byte
    tx_Buffer[1] = cnt_TX;
    tx_Buffer[2] = 0x55;
    tx_Buffer[3] = 0x55;

    // Temperature
    tx_Buffer[ 4] = (g_Temperature & 0xFF00) >> 8;
    tx_Buffer[ 5] = (g_Temperature & 0x00FF) >> 0;

    // Pressure
    tx_Buffer[ 6] = (g_Pressure & 0xFF00) >> 8;
    tx_Buffer[ 7] = (g_Pressure & 0x00FF) >> 0;


    // Humidity
    tx_Buffer[8] = (g_Humidity & 0xFF00) >> 8;
    tx_Buffer[9] = (g_Humidity & 0x00FF) >> 0;

    Acc_X = Get_Cos(10, 2);
    // Acc_N
    int32_t accX_i32 = (int32_t)(Acc_X * 1000.0f);  // mm/s²
    tx_Buffer[10] = (accX_i32 >> 24) & 0xFF;
    tx_Buffer[11] = (accX_i32 >> 16) & 0xFF;
    tx_Buffer[12] = (accX_i32 >> 8) & 0xFF;
    tx_Buffer[13] = accX_i32 & 0xFF;

    // Acc_E
    Acc_Y = Get_Sin(10, 2);
    int32_t accY_i32 = (int32_t)(Acc_Y * 1000.0f);
    tx_Buffer[14] = (accY_i32 >> 24) & 0xFF;
    tx_Buffer[15] = (accY_i32 >> 16) & 0xFF;
    tx_Buffer[16] =  (accY_i32 >> 8)  & 0xFF;
    tx_Buffer[17] = accY_i32        & 0xFF;
    // Acc_D
    int32_t accZ_i32 = (int32_t)(Acc_Z * 1000.0f);
    tx_Buffer[18] = (accZ_i32 >> 24) & 0xFF;
    tx_Buffer[19] = (accZ_i32 >> 16) & 0xFF;
    tx_Buffer[20] = (accZ_i32 >> 8)  & 0xFF;
    tx_Buffer[21] =  accZ_i32        & 0xFF;


    int32_t gyroX_i32 = (int32_t)(GyroX * 1000.0f);
    int32_t gyroY_i32 = (int32_t)(GyroY * 1000.0f);
    int32_t gyroZ_i32 = (int32_t)(GyroZ * 1000.0f);
    // GyroX
    tx_Buffer[22] = (gyroX_i32 >> 24) & 0xFF;
    tx_Buffer[23] = (gyroX_i32 >> 16) & 0xFF;
    tx_Buffer[24] = (gyroX_i32 >> 8)  & 0xFF;
    tx_Buffer[25] =  gyroX_i32        & 0xFF;
    // GyroY
    tx_Buffer[26] = (gyroY_i32 >> 24) & 0xFF;
    tx_Buffer[27] = (gyroY_i32 >> 16) & 0xFF;
    tx_Buffer[28] = (gyroY_i32 >> 8)  & 0xFF;
    tx_Buffer[29] =  gyroY_i32        & 0xFF;
    
    // GyroZ
    tx_Buffer[30] = (gyroZ_i32 >> 24) & 0xFF;
    tx_Buffer[31] = (gyroZ_i32 >> 16) & 0xFF;
    tx_Buffer[32] = (gyroZ_i32 >> 8)  & 0xFF;
    tx_Buffer[33] =  gyroZ_i32        & 0xFF;

    int32_t MagX_i32 = (int32_t)(Mag_X * 1000.0f);
    int32_t MagY_i32 = (int32_t)(Mag_Y * 1000.0f);
    int32_t MagZ_i32 = (int32_t)(Mag_Z * 1000.0f);
    
    //MagX
    tx_Buffer[34] = (MagX_i32 >> 24) & 0xFF;
    tx_Buffer[35] = (MagX_i32 >> 16) & 0xFF;
    tx_Buffer[36] = (MagX_i32 >> 8)  & 0xFF;
    tx_Buffer[37] =  MagX_i32        & 0xFF;

    //MagY
    tx_Buffer[38] = (MagY_i32 >> 24) & 0xFF;
    tx_Buffer[39] = (MagY_i32 >> 16) & 0xFF;
    tx_Buffer[40] = (MagY_i32 >> 8)  & 0xFF;
    tx_Buffer[41] =  MagY_i32        & 0xFF;

    //MagZ
    tx_Buffer[42] = (MagZ_i32 >> 24) & 0xFF;
    tx_Buffer[43] = (MagZ_i32 >> 16) & 0xFF;
    tx_Buffer[44] = (MagZ_i32 >> 8)  & 0xFF;
    tx_Buffer[45] =  MagZ_i32        & 0xFF;


    for(int i = 46; i < 63; i++) tx_Buffer[i] = 0;

    // calculate simple checksum over first 16 bytes
    uint8_t cs = 0;
    for (int i = 0; i < 63; i++) cs += tx_Buffer[i];
    tx_Buffer[63] = cs;

    // send over UART1
    Serial1.write(tx_Buffer, 64);

    // blink
    // digitalWrite(LED_PIN, HIGH);
    // delay(5);
    // digitalWrite(LED_PIN, LOW);
}

// Initializes UART pins/LED
void UART_init() {
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW);
}

// Called every 10 ms to check for new commands
void UART_poll() {
  if (Serial1.available() < 8) return;

  // read 16 bytes into rx_Buffer
  Serial1.readBytes(rx_Buffer, 8);

  // dispatch to your service routine
  Service_Input_Command(rx_Buffer);
}

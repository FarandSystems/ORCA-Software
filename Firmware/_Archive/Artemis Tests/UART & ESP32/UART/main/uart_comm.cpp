#include "uart_comm.h"
#include "main.h"
#include <Arduino.h>

static const int LED_PIN = 19;
static uint8_t cnt_TX = 0;

// Exactly your Service_Input_Command logic, using rx_Buffer[]
static void Service_Input_Command(uint8_t* RxBuffer) {
  switch (RxBuffer[1]) {
    case 0x00: break;
    case 0x02:
      // example: single-shot
      // Alarm(SHORT_BEEP_X1,1,32,1);
      break;
    // … add other cases exactly as your snippet …
    default: break;
  }
}

// Builds & sends tx_Buffer based on your Report_Measured_Data example
void Report_Measured_Data(void) {
    cnt_TX++;
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

    tx_Buffer[8] = 0; // Reserved for Coding 0x1A

    // Humidity
    tx_Buffer[9] = (g_Humidity & 0xFF00) >> 8;
    tx_Buffer[10] = (g_Humidity & 0x00FF) >> 0;

    // Acc_N

    int32_t accX_i32 = (int32_t)(Acc_X * 1000.0f);  // mm/s²
    tx_Buffer[11] = (accX_i32 >> 24) & 0xFF;
    tx_Buffer[12] = (accX_i32 >> 16) & 0xFF;
    tx_Buffer[13] = (accX_i32 >> 8) & 0xFF;
    tx_Buffer[14] = accX_i32 & 0xFF;
    // Acc_E
    int32_t accY_i32 = (int32_t)(Acc_Y * 1000.0f);
    tx_Buffer[15] = (accY_i32 >> 24) & 0xFF;
    tx_Buffer[16] = 0x00;
    tx_Buffer[17] = (accY_i32 >> 16) & 0xFF;
    tx_Buffer[18] =  (accY_i32 >> 8)  & 0xFF;
    tx_Buffer[19] = accY_i32        & 0xFF;
    // Acc_D
    int32_t accZ_i32 = (int32_t)(Acc_Z * 1000.0f);
    tx_Buffer[20] = (accZ_i32 >> 24) & 0xFF;
    tx_Buffer[21] = (accZ_i32 >> 16) & 0xFF;
    tx_Buffer[22] = (accZ_i32 >> 8)  & 0xFF;
    tx_Buffer[23] =  accZ_i32        & 0xFF;

    tx_Buffer[24] = 0x00; // Reserved for Coding 0x1A


    int32_t gyroX_i32 = (int32_t)(GyroX * 1000.0f);
    int32_t gyroY_i32 = (int32_t)(GyroY * 1000.0f);
    int32_t gyroZ_i32 = (int32_t)(GyroZ * 1000.0f);
    // GyroX
    tx_Buffer[25] = (gyroX_i32 >> 24) & 0xFF;
    tx_Buffer[26] = (gyroX_i32 >> 16) & 0xFF;
    tx_Buffer[27] = (gyroX_i32 >> 8)  & 0xFF;
    tx_Buffer[28] =  gyroX_i32        & 0xFF;
    // GyroY
    tx_Buffer[29] = (gyroY_i32 >> 24) & 0xFF;
    tx_Buffer[30] = (gyroY_i32 >> 16) & 0xFF;
    tx_Buffer[31] = (gyroY_i32 >> 8)  & 0xFF;
    tx_Buffer[32] = 0x00; // Reserved for Coding 0x1A
    tx_Buffer[33] =  gyroY_i32        & 0xFF;
    
    // GyroZ
    tx_Buffer[34] = (gyroZ_i32 >> 24) & 0xFF;
    tx_Buffer[35] = (gyroZ_i32 >> 16) & 0xFF;
    tx_Buffer[36] = (gyroZ_i32 >> 8)  & 0xFF;
    tx_Buffer[37] =  gyroZ_i32        & 0xFF;

    int32_t MagX_i32 = (int32_t)(Mag_X * 1000.0f);
    int32_t MagY_i32 = (int32_t)(Mag_Y * 1000.0f);
    int32_t MagZ_i32 = (int32_t)(Mag_Z * 1000.0f);
    
    //MagX
    tx_Buffer[38] = (MagX_i32 >> 24) & 0xFF;
    tx_Buffer[39] = (MagX_i32 >> 16) & 0xFF;
    tx_Buffer[40] = 0x00; // Reserved for Coding 0x1A
    tx_Buffer[41] = (MagX_i32 >> 8)  & 0xFF;
    tx_Buffer[42] =  MagX_i32        & 0xFF;

    //MagY
    tx_Buffer[43] = (MagY_i32 >> 24) & 0xFF;
    tx_Buffer[44] = (MagY_i32 >> 16) & 0xFF;
    tx_Buffer[45] = (MagY_i32 >> 8)  & 0xFF;
    tx_Buffer[46] =  MagY_i32        & 0xFF;

    //MagZ
    tx_Buffer[47] = (MagZ_i32 >> 24) & 0xFF;
    tx_Buffer[48] = 0x00; // Reserved for Coding 0x1A
    tx_Buffer[49] = (MagZ_i32 >> 16) & 0xFF;
    tx_Buffer[50] = (MagZ_i32 >> 8)  & 0xFF;
    tx_Buffer[51] =  MagZ_i32        & 0xFF;


    for(int i = 52; i < 63; i++) tx_Buffer[i] = 0;

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

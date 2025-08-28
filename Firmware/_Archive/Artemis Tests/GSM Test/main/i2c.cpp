#include "i2c.h"

static TwoWire agtWire(9, 8);
// static Adafruit_ISM330DHCX ism330dhcx;
static MS8607 baro;
// static Adafruit_LIS3MDL lis3mdl;

int32_t g_Temperature = 0;
int32_t g_Humidity    = 0;
int32_t g_Pressure    = 0;

void i2cInit() {
  agtWire.begin();
  baro.begin(agtWire);
  // if (!ism330dhcx.begin_I2C(0x6A, &agtWire)) while (1) delay(10);
  // ism330dhcx.setAccelRange(LSM6DS_ACCEL_RANGE_2_G);
  // ism330dhcx.setGyroRange(LSM6DS_GYRO_RANGE_125_DPS);
  // ism330dhcx.setAccelDataRate(LSM6DS_RATE_833_HZ);
  // ism330dhcx.setGyroDataRate(LSM6DS_RATE_833_HZ);
  // if (!lis3mdl.begin_I2C(0x1C, &agtWire)) while (1) delay(10);
  // lis3mdl.setPerformanceMode(LIS3MDL_MEDIUMMODE);
  // lis3mdl.setOperationMode(LIS3MDL_CONTINUOUSMODE);
  // lis3mdl.setDataRate(LIS3MDL_DATARATE_155_HZ);
  // lis3mdl.setRange(LIS3MDL_RANGE_4_GAUSS);
}

void i2cRead() {
  float Tf = baro.getTemperature();
  float Pf = baro.getPressure();
  float Hf = baro.getHumidity();
  g_Temperature = (int32_t)Tf;
  g_Humidity    = (int32_t)Hf;
  g_Pressure    = (int32_t)Pf;
}


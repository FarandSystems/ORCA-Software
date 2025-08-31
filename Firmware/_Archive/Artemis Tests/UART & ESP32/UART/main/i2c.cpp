#include "i2c.h"
#include "main.h"



#include <Adafruit_ISM330DHCX.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_LIS3MDL.h>
#include <SparkFun_PHT_MS8607_Arduino_Library.h>

// I2C on D9/D8 for Artemis Global Tracker
TwoWire agtWire(9, 8);

// existing
static Adafruit_ISM330DHCX ism330dhcx;
static MS8607             baro;
QWIIC_POWER qwiic_switch;

// magnetometer
static Adafruit_LIS3MDL lis3mdl;

void i2c_init() {
  agtWire.begin();
  delay(100);
  agtWire.setClock(1000000);
  delay(100);

  if (!qwiic_switch.begin(agtWire)) {
    Serial.println("Qwiic Power Switch not found!");
    while (1);
  }

  qwiic_switch.powerOn();
  delay(100);
  Serial.println("Qwiic is powered on");


  baro.begin(agtWire);

  // — IMU init (unchanged) —
  if (!ism330dhcx.begin_I2C(0x6A, &agtWire)) {
    Serial.println("ISM330DHCX not found!");
    while (1) delay(10);
  }
  ism330dhcx.setAccelRange(LSM6DS_ACCEL_RANGE_2_G);
  ism330dhcx.setGyroRange(LSM6DS_GYRO_RANGE_125_DPS);
  ism330dhcx.setAccelDataRate(LSM6DS_RATE_833_HZ);
  ism330dhcx.setGyroDataRate(LSM6DS_RATE_833_HZ);
  ism330dhcx.configInt1(false, false, true);
  ism330dhcx.configInt2(false, true, false);

  // — LIS3MDL init & config from the demo —
  // Default I2C addr is 0x1C (0x1E if SA1 pulled high)
  if (! lis3mdl.begin_I2C(0x1C, &agtWire)) {
    Serial.println("LIS3MDL not found!");
    while (1) delay(10);
  }
  Serial.println("LIS3MDL Found!");

  // performance / power mode
  lis3mdl.setPerformanceMode(LIS3MDL_MEDIUMMODE);
  Serial.print("Performance mode: ");
  switch (lis3mdl.getPerformanceMode()) {
    case LIS3MDL_LOWPOWERMODE:  Serial.println("Low");  break;
    case LIS3MDL_MEDIUMMODE:    Serial.println("Medium"); break;
    case LIS3MDL_HIGHMODE:      Serial.println("High"); break;
    case LIS3MDL_ULTRAHIGHMODE: Serial.println("Ultra-High"); break;
  }

  // continuous conversion
  lis3mdl.setOperationMode(LIS3MDL_CONTINUOUSMODE);
  Serial.print("Operation mode: ");
  switch (lis3mdl.getOperationMode()) {
    case LIS3MDL_CONTINUOUSMODE: Serial.println("Continuous"); break;
    case LIS3MDL_SINGLEMODE:     Serial.println("Single"); break;
    case LIS3MDL_POWERDOWNMODE:  Serial.println("Power-down"); break;
  }

  // output data rate
  lis3mdl.setDataRate(LIS3MDL_DATARATE_155_HZ);
  Serial.print("Data rate: ");
  Serial.println(lis3mdl.getDataRate());

  // full-scale range
  lis3mdl.setRange(LIS3MDL_RANGE_4_GAUSS);
  Serial.print("Range: ");
  Serial.println(lis3mdl.getRange());

  // interrupt threshold & pin config (if you want INT on Z)
  lis3mdl.setIntThreshold(500);  // 500 LSB’s
  lis3mdl.configInterrupt(
    false,  // x axis?
    false,  // y axis?
    true,   // z axis
    true,   // active-high
    false,  // non-latched
    true    // enabled
  );

  delay(500);
}

void i2c_read() {
  // — baro —
  float Tf = baro.getTemperature();
  float Pf = baro.getPressure();
  float Hf = baro.getHumidity();
  g_Temperature = (int32_t)Tf;
  g_Pressure    = (int32_t)Pf;
  g_Humidity    = (int32_t)Hf;

  // — IMU —
  sensors_event_t accel, gyro, temp;
  if (ism330dhcx.getEvent(&accel, &gyro, &temp)) {
    Acc_X = accel.acceleration.x;
    Acc_Y = accel.acceleration.y;
    Acc_Z = accel.acceleration.z;
    GyroX = gyro.gyro.x;
    GyroY = gyro.gyro.y;
    GyroZ = gyro.gyro.z;
    // … Serial.print() as before …
  } else {
    Serial.println("IMU read failed!");
  }

  // — LIS3MDL raw read (from demo) —
  lis3mdl.read();  
  //Serial.print("Mag raw: X="); Serial.print(lis3mdl.x);
  //Serial.print(" Y=");              Serial.print(lis3mdl.y);
  //Serial.print(" Z=");              Serial.println(lis3mdl.z);

  // — LIS3MDL normalized event (µTesla) —
  sensors_event_t mag;
  lis3mdl.getEvent(&mag);
  Mag_X = mag.magnetic.x;
  Mag_Y = mag.magnetic.y;
  Mag_Z = mag.magnetic.z;
  //Serial.print("Mag uT: X="); Serial.print(Mag_X, 1);
  //Serial.print(" Y=");        Serial.print(Mag_Y, 1);
  //Serial.print(" Z=");        Serial.println(Mag_Z, 1);
}

void IMU_print_settings() {
  Serial.println(F("=== ISM330DHCX Settings ==="));

  Serial.print("Accel range: ");
  switch (ism330dhcx.getAccelRange()) {
    case LSM6DS_ACCEL_RANGE_2_G: Serial.println("±2G"); break;
    case LSM6DS_ACCEL_RANGE_4_G: Serial.println("±4G"); break;
    case LSM6DS_ACCEL_RANGE_8_G: Serial.println("±8G"); break;
    case LSM6DS_ACCEL_RANGE_16_G: Serial.println("±16G"); break;
  }

  Serial.print("Gyro range: ");
  switch (ism330dhcx.getGyroRange()) {
    case LSM6DS_GYRO_RANGE_125_DPS: Serial.println("125 DPS"); break;
    case LSM6DS_GYRO_RANGE_250_DPS: Serial.println("250 DPS"); break;
    case LSM6DS_GYRO_RANGE_500_DPS: Serial.println("500 DPS"); break;
    case LSM6DS_GYRO_RANGE_1000_DPS: Serial.println("1000 DPS"); break;
    case LSM6DS_GYRO_RANGE_2000_DPS: Serial.println("2000 DPS"); break;
    case ISM330DHCX_GYRO_RANGE_4000_DPS: Serial.println("4000 DPS"); break;
  }

  Serial.print("Accel data rate: ");
  switch (ism330dhcx.getAccelDataRate()) {
    case LSM6DS_RATE_SHUTDOWN:   Serial.println("OFF"); break;
    case LSM6DS_RATE_12_5_HZ:    Serial.println("12.5 Hz"); break;
    case LSM6DS_RATE_26_HZ:      Serial.println("26 Hz"); break;
    case LSM6DS_RATE_52_HZ:      Serial.println("52 Hz"); break;
    case LSM6DS_RATE_104_HZ:     Serial.println("104 Hz"); break;
    case LSM6DS_RATE_208_HZ:     Serial.println("208 Hz"); break;
    case LSM6DS_RATE_416_HZ:     Serial.println("416 Hz"); break;
    case LSM6DS_RATE_833_HZ:     Serial.println("833 Hz"); break;
    case LSM6DS_RATE_1_66K_HZ:   Serial.println("1.66 kHz"); break;
    case LSM6DS_RATE_3_33K_HZ:   Serial.println("3.33 kHz"); break;
    case LSM6DS_RATE_6_66K_HZ:   Serial.println("6.66 kHz"); break;
  }

  Serial.print("Gyro data rate: ");
  switch (ism330dhcx.getGyroDataRate()) {
    case LSM6DS_RATE_SHUTDOWN:   Serial.println("OFF"); break;
    case LSM6DS_RATE_12_5_HZ:    Serial.println("12.5 Hz"); break;
    case LSM6DS_RATE_26_HZ:      Serial.println("26 Hz"); break;
    case LSM6DS_RATE_52_HZ:      Serial.println("52 Hz"); break;
    case LSM6DS_RATE_104_HZ:     Serial.println("104 Hz"); break;
    case LSM6DS_RATE_208_HZ:     Serial.println("208 Hz"); break;
    case LSM6DS_RATE_416_HZ:     Serial.println("416 Hz"); break;
    case LSM6DS_RATE_833_HZ:     Serial.println("833 Hz"); break;
    case LSM6DS_RATE_1_66K_HZ:   Serial.println("1.66 kHz"); break;
    case LSM6DS_RATE_3_33K_HZ:   Serial.println("3.33 kHz"); break;
    case LSM6DS_RATE_6_66K_HZ:   Serial.println("6.66 kHz"); break;
  }

  Serial.println();
}
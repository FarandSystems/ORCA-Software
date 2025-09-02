#include "i2c.h"
#include "main.h"

#include <Arduino.h>
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_ISM330DHCX.h>
#include <Adafruit_LIS3MDL.h>
#include <SparkFun_PHT_MS8607_Arduino_Library.h>
#include <SparkFun_Qwiic_Power_Switch_Arduino_Library.h>

// I2C on D9/D8 for Artemis Global Tracker
static TwoWire agtWire(9, 8);

// Qwiic power switch
static QWIIC_POWER qwiic_switch;
static bool qwiic_present = false;

// Sensors
static Adafruit_ISM330DHCX ism330;
static Adafruit_LIS3MDL    lis3;
static MS8607              ms8607;

static bool s_sensors_ready = false;

// top of file (or static in i2c.c)
static uint32_t last_imu_us = 0;
static const uint32_t imu_period_us = 1200; // 833 Hz


// ---- helpers ----
static bool init_imu() {
  if (!ism330.begin_I2C(0x6A, &agtWire)) {
    Serial.println("ISM330DHCX not found!");
    return false;
  }
  
  // 1) Use finest LSB and a sane ODR
  ism330.setAccelRange(LSM6DS_ACCEL_RANGE_2_G);      // finest resolution
  ism330.setGyroRange (LSM6DS_GYRO_RANGE_125_DPS);
  ism330.setAccelDataRate(LSM6DS_RATE_833_HZ);       // start here
  ism330.setGyroDataRate (LSM6DS_RATE_833_HZ);

  ism330.configInt1(false, false, true);
  ism330.configInt2(false, true, false);
  return true;
}

static bool init_lis3() {
  // Default I2C addr 0x1C (0x1E if SA1 high)
  if (!lis3.begin_I2C(0x1C, &agtWire)) {
    Serial.println("LIS3MDL not found!");
    return false;
  }
  lis3.setPerformanceMode(LIS3MDL_MEDIUMMODE);
  lis3.setOperationMode(LIS3MDL_CONTINUOUSMODE);
  lis3.setDataRate(LIS3MDL_DATARATE_155_HZ);
  lis3.setRange(LIS3MDL_RANGE_4_GAUSS);
  lis3.setIntThreshold(500);
  lis3.configInterrupt(false, false, true, true, false, true);
  return true;
}

static bool init_ms8607() {
  bool ok = ms8607.begin(agtWire);
  if (!ok) Serial.println("MS8607 not found!");
  return ok;
}

static void tiny_delay_ms(uint32_t ms) {
  // keep any waits tiny (library calls are blocking anyway)
  delay(ms);
}

// ---- API ----
void i2c_init(void) {
  pinMode(TOGGLE_PIN, OUTPUT);
  digitalWrite(TOGGLE_PIN, LOW);

  agtWire.begin();
  delay(100);
  agtWire.setClock(1000000);
  delay(100);

  // Try Qwiic power switch
  qwiic_present = qwiic_switch.begin(agtWire);
  if (qwiic_present) {
    qwiic_switch.powerOn();
    tiny_delay_ms(50);
    Serial.println("Qwiic Power Switch present -> ON");
  } else {
    Serial.println("Qwiic Power Switch NOT found (continuing).");
  }

  s_sensors_ready = init_ms8607() & init_imu() & init_lis3();
  if (s_sensors_ready) Serial.println("Sensors initialized.");
}

bool i2c_power_on(void) {
  if (qwiic_present) {
    qwiic_switch.powerOn();
    tiny_delay_ms(50);
  }
  s_sensors_ready = init_ms8607() & init_imu() & init_lis3();
  return s_sensors_ready;
}

void i2c_power_off(void) {
  if (qwiic_present) {
    qwiic_switch.powerOff();
  }
  s_sensors_ready = false;
}

bool i2c_reinit(void) {
  s_sensors_ready = init_ms8607() & init_imu() & init_lis3();
  return s_sensors_ready;
}

bool sensors_ready(void) { return s_sensors_ready; }

void imu_set_rates(uint8_t acc_code, uint8_t gyr_code) {
  // Map 0..9
  lsm6ds_data_rate_t acc_map[] = {
    LSM6DS_RATE_12_5_HZ, LSM6DS_RATE_26_HZ,  LSM6DS_RATE_52_HZ,
    LSM6DS_RATE_104_HZ,  LSM6DS_RATE_208_HZ, LSM6DS_RATE_416_HZ,
    LSM6DS_RATE_833_HZ,  LSM6DS_RATE_1_66K_HZ, LSM6DS_RATE_3_33K_HZ,
    LSM6DS_RATE_6_66K_HZ
  };
  lsm6ds_data_rate_t gyr_map[] = {
    LSM6DS_RATE_12_5_HZ, LSM6DS_RATE_26_HZ,  LSM6DS_RATE_52_HZ,
    LSM6DS_RATE_104_HZ,  LSM6DS_RATE_208_HZ, LSM6DS_RATE_416_HZ,
    LSM6DS_RATE_833_HZ,  LSM6DS_RATE_1_66K_HZ, LSM6DS_RATE_3_33K_HZ,
    LSM6DS_RATE_6_66K_HZ
  };

  if (acc_code < 10) ism330.setAccelDataRate(acc_map[acc_code]);
  if (gyr_code < 10) ism330.setGyroDataRate (gyr_map[gyr_code]);
}

void i2c_read() {
  digitalWrite(TOGGLE_PIN, HIGH);     // start of tick
  uint32_t t0 = micros();

  // ---- IMU ONLY here ----
  sensors_event_t accel, gyro, temp;
  if (ism330.getEvent(&accel, &gyro, &temp)) {
    Acc_X = accel.acceleration.x;
    Acc_Y = accel.acceleration.y;
    Acc_Z = accel.acceleration.z;
    GyroX = gyro.gyro.x;
    GyroY = gyro.gyro.y;
    GyroZ = gyro.gyro.z;
  }

  // ---- THROTTLE slow sensors (don’t run every tick) ----
  static uint32_t lastSlow = 0;
  uint32_t now = millis();
  if (now - lastSlow >= 50) {         // 20 Hz
    lastSlow = now;

    // If these lines are inside every tick, they will kill 100 Hz:
    float Tf = ms8607.getTemperature();
    float Pf = ms8607.getPressure();
    float Hf = ms8607.getHumidity();
    g_Temperature = (int32_t)Tf;
    g_Pressure    = (int32_t)Pf;
    g_Humidity    = (int32_t)Hf;

    sensors_event_t mag;
    lis3.getEvent(&mag);
    Mag_X = mag.magnetic.x;
    Mag_Y = mag.magnetic.y;
    Mag_Z = mag.magnetic.z;
  }

  uint32_t dt = micros() - t0;
  digitalWrite(TOGGLE_PIN, LOW);      // end of tick

  // Log once a second so we can see average execution time
  static uint32_t acc=0, cnt=0, lastLog=0;
  acc += dt; cnt++;
  uint32_t ms = millis();
  if (ms - lastLog >= 1000) {
    lastLog = ms;
    Serial.print("i2c_read avg us: ");
    Serial.println(acc / (cnt ? cnt : 1)); // expect << 10000 for 100 Hz
    acc = cnt = 0;
  }
}

void IMU_print_settings(void) {
  Serial.println(F("=== ISM330DHCX Settings ==="));
  Serial.print("Accel data rate: "); Serial.println((int)ism330.getAccelDataRate());
  Serial.print("Gyro  data rate: "); Serial.println((int)ism330.getGyroDataRate());
}

#include "main.h"

// I2C on D9/D8 for Artemis Global Tracker
TwoWire agtWire(9, 8);
QWIIC_POWER qwiic_switch;

Adafruit_ISM330DHCX ism330dhcx;

uint8_t imu_counter = 0;

volatile float ax, ay, az;
volatile float gx, gy, gz;
void setup_i2c(void);
void read_IMU(void);

void setup_i2c(void)
{
  agtWire.begin();
  Serial.println("AGT 2Wire started!");
  delay(100);
  agtWire.setClock(1000000);
  delay(100);

  if (!qwiic_switch.begin(agtWire)) 
  {
    Serial.println("Qwiic Power Switch not found!");
    while (1);
  }

  qwiic_switch.powerOn();
  delay(100);
  Serial.println("Qwiic is powered on");

  // Initialize IMU
  if (!ism330dhcx.begin_I2C(0x6A, &agtWire)) 
  {
    Serial.println("Failed to find ISM330DHCX!");
    while (1) delay(10);
  }

  ism330dhcx.setAccelRange(LSM6DS_ACCEL_RANGE_16_G);
  ism330dhcx.setGyroRange(LSM6DS_GYRO_RANGE_2000_DPS);
  ism330dhcx.setAccelDataRate(LSM6DS_RATE_833_HZ);
  ism330dhcx.setGyroDataRate(LSM6DS_RATE_833_HZ);

  Serial.println("IMU is Started!");
}

void read_IMU(void)
{
    // --- Read IMU here ---
  sensors_event_t accel, gyro, temp;
  ism330dhcx.getEvent(&accel, &gyro, &temp);

  // store results to globals (atomic, simple)
  ax = accel.acceleration.x;
  ay = accel.acceleration.y;
  az = accel.acceleration.z;
  gx = gyro.gyro.x;
  gy = gyro.gyro.y;
  gz = gyro.gyro.z;
  
//  imu_counter++;
//  if (imu_counter >= 8)
//  {
//    Serial.print("AX:"); Serial.print(ax, 2);
//    Serial.print(" AY:"); Serial.print(ay, 2);
//    Serial.print(" AZ:"); Serial.print(az, 2);
//    Serial.print("  GX:"); Serial.print(gx, 2);
//    Serial.print(" GY:"); Serial.print(gy, 2);
//    Serial.print(" GZ:"); Serial.print(gz, 2);
//    Serial.println();
//    imu_counter = 0;
//  }
}

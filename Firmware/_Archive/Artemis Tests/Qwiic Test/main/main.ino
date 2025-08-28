#include <Wire.h>
#include <SparkFun_Qwiic_Power_Switch_Arduino_Library.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_ISM330DHCX.h>

TwoWire ArtemisWire(9, 8);
QWIIC_POWER qwiic_switch;
Adafruit_ISM330DHCX imu;

#define iridiumPwrEN        22
#define IRIDIUM_SLEEP_PIN   17

bool sensorOn = false;

void setup() {
  pinMode(iridiumPwrEN, OUTPUT);
  digitalWrite(iridiumPwrEN, LOW);
  pinMode(IRIDIUM_SLEEP_PIN, OUTPUT);
  digitalWrite(IRIDIUM_SLEEP_PIN, LOW);

  Serial.begin(115200);
  while (!Serial); // Wait for Serial if USB

  ArtemisWire.begin();

  if (!qwiic_switch.begin(ArtemisWire)) {
    Serial.println("Qwiic Power Switch not found!");
    while (1);
  }

  qwiic_switch.powerOn();
  delay(100);
  Serial.println("Qwiic is powered on");

  sensorOn = initIMU();
}

bool initIMU() {
  if (!imu.begin_I2C(0x6A, &ArtemisWire)) {
    Serial.println("ISM330DHCX not found!");
    return false;
  }

  imu.setAccelRange(LSM6DS_ACCEL_RANGE_2_G);
  imu.setGyroRange(LSM6DS_GYRO_RANGE_125_DPS);
  imu.setAccelDataRate(LSM6DS_RATE_833_HZ);
  imu.setGyroDataRate(LSM6DS_RATE_833_HZ);
  imu.configInt1(false, false, true);
  imu.configInt2(false, true, false);

  Serial.println("IMU ready.");
  return true;
}

void loop() {
  // Handle serial commands
  if (Serial.available()) {
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();
    cmd.toUpperCase();

    if (cmd == "OFF") {
      Serial.println("Turning Qwiic OFF");
      qwiic_switch.powerOff();
      sensorOn = false;
    } else if (cmd == "ON") {
      Serial.println("Turning Qwiic ON");
      qwiic_switch.powerOn();
      delay(100);
      sensorOn = initIMU(); // Try to reinit IMU
    }
  }

  // Try reading the sensor if it's ON
  if (sensorOn) {
    sensors_event_t accel, gyro, temp;
    imu.getEvent(&accel, &gyro, &temp);

    Serial.print("Accel X: "); Serial.println(accel.acceleration.x);
    Serial.print("Gyro Z: "); Serial.println(gyro.gyro.z);
  } else {
    Serial.println("[Sensor OFF]");
  }

  delay(1000);
}

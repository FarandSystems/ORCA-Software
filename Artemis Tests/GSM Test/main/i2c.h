// I2CModule.h
#pragma once
#include <Wire.h>
// #include <Adafruit_ISM330DHCX.h>
// #include <Adafruit_LIS3MDL.h>
#include <SparkFun_PHT_MS8607_Arduino_Library.h>

void i2cInit();
void i2cRead();

extern int32_t g_Temperature;
extern int32_t g_Humidity;
extern int32_t g_Pressure;

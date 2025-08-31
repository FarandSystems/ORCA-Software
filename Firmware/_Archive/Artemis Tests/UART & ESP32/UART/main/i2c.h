#ifndef i2c_H
#define i2c_H
#pragma once
#include <Wire.h>
#include <SparkFun_Qwiic_Power_Switch_Arduino_Library.h>
extern TwoWire agtWire;
extern QWIIC_POWER qwiic_switch;
void i2c_init(void);

void IMU_print_settings(void);
void i2c_read(void);

#endif
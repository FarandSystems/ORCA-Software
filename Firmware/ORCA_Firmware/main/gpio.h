#pragma once
#ifndef GPIO_H
#define GPIO_H
#include <stdint.h>
#include <stdbool.h>
#include <Arduino.h>
#include "am_bsp.h"
#include "am_hal_gpio.h"

// Pin definitions (extern - defined in main)
extern uint8_t pin_800;
extern uint8_t pin_100;
extern uint8_t pin_8;
extern uint8_t pin_1;
extern uint8_t pin_LED;

extern void setup_GPIO(void);
extern void i2c_pins_init(void);
extern void uart_pins_init(void);

#endif //GPIO_H

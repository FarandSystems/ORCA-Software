#pragma once
#ifndef GPIO_H
#define GPIO_H
#include <stdint.h>
#include <stdbool.h>
#include <Arduino.h>
#include "am_bsp.h"
#include "am_hal_gpio.h"

// Pin definitions (extern - defined in main)
extern uint8_t pin_Toggle;
extern uint8_t pin_UART;
extern uint8_t pin_100;
extern uint8_t pin_8;
extern uint8_t pin_1;
extern uint8_t pin_LED;
extern uint8_t pin_Buzzer;

#ifndef gnssEN
  #define gnssEN             26  // LOW = enable GNSS
#endif
#ifndef iridiumPwrEN
  #define iridiumPwrEN       22  // HIGH = power Iridium
#endif
#ifndef superCapChgEN
  #define superCapChgEN      27  // HIGH = enable supercap charger
#endif
#ifndef iridiumSleep
  #define iridiumSleep       17  // HIGH = Iridium on; LOW = sleep/off
#endif

extern void setup_GPIO(void);
extern void i2c_pins_init(void);
extern void uart_pins_init(void);
extern void turn_iridium_off(void);
extern void turn_iridium_on(void);
extern void turn_gnss_on(void);
extern void turn_gnss_off(void);


#endif //GPIO_H

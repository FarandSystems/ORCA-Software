#ifndef MAIN_H
#define MAIN_H

#include "am_mcu_apollo.h"
#include "am_hal_cachectrl.h"
#include "am_hal_sysctrl.h"
#include "am_hal_gpio.h"
#include <Arduino.h>

#include "tim.h"
#include "gpio.h"
#include "i2c.h"
#include "uart.h"
#include "command_Rx.h"
#include "Alarm.h"
#include "gnss_manager.h"

enum System_State
{
    STARTUP_STATE = 0,
    GNSS_ACQUIRE_STATE = 1,
    NORMAL_STATE = 2,
    ERROR_STATE = 3
};

#endif // MAIN_H

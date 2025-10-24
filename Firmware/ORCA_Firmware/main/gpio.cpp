#include "main.h"

// Pin definitions
uint8_t pin_800 = 4;
//uint8_t pin_100 = 43;
//uint8_t pin_8 = 37;
//uint8_t pin_1 = 4;

uint8_t pin_LED = 19;


void i2c_pins_init(void);
void uart_pins_init(void);

void setup_GPIO()
{
    // Set the pins as outputs
  pinMode(pin_800, OUTPUT);
//  pinMode(pin_100, OUTPUT);
//  pinMode(pin_8, OUTPUT);
//  pinMode(pin_1, OUTPUT);

  pinMode(pin_LED, OUTPUT);
}

void i2c_pins_init()
{
  am_hal_gpio_pinconfig(8, g_AM_BSP_GPIO_IOM1_SCL); // D8 = SCL
  am_hal_gpio_pinconfig(9, g_AM_BSP_GPIO_IOM1_SDA); // D9 = SDA
}

void uart_pins_init()
{
   // Configure the TX pin (Pin 42) for UART1
  am_hal_gpio_pincfg_t pinConfigTx = g_AM_BSP_GPIO_COM_UART_TX;
  pinConfigTx.uFuncSel = AM_HAL_PIN_42_UART1TX;  // Set Pin 42 to UART1TX function
  pin_config(D42, pinConfigTx);  // Configure Pin 42 for UART TX
  
  // Configure the RX pin (Pin 43) for UART1
  am_hal_gpio_pincfg_t pinConfigRx = g_AM_BSP_GPIO_COM_UART_RX;
  pinConfigRx.uFuncSel = AM_HAL_PIN_43_UART1RX;  // Set Pin 43 to UART1RX function
  pinConfigRx.ePullup = AM_HAL_GPIO_PIN_PULLUP_WEAK;  // Enable weak pull-up for RX pin
  pin_config(D43, pinConfigRx);  // Configure Pin 43 for UART RX
}

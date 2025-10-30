#include "main.h"

// Pin definitions
uint8_t pin_Toggle = 4;
//uint8_t pin_100 = 43;
//uint8_t pin_1 = 4;

uint8_t pin_LED = 19;

uint8_t pin_Buzzer = 37;
uint8_t pin_UART = 7;

uint8_t pin_IRIDIUM_EN = 17;


void i2c_pins_init(void);
void uart_pins_init(void);
void disable_iridium(void);

void setup_GPIO()
{
    // Set the pins as outputs
  pinMode(pin_Toggle, OUTPUT);
  pinMode(pin_UART, OUTPUT);


  pinMode(pin_LED, OUTPUT);
  pinMode(pin_Buzzer, OUTPUT);
  pinMode(pin_IRIDIUM_EN, OUTPUT);
}

void disable_iridium()
{
  am_hal_gpio_output_clear(pin_IRIDIUM_EN);
}

void i2c_pins_init()
{
  am_hal_gpio_pinconfig(8, g_AM_BSP_GPIO_IOM1_SCL); // D8 = SCL
  am_hal_gpio_pinconfig(9, g_AM_BSP_GPIO_IOM1_SDA); // D9 = SDA
}

void uart_pins_init()
{
  // === TX (MCU → PC) ===
  am_hal_gpio_pincfg_t pinConfigTx = g_AM_BSP_GPIO_COM_UART_TX;
  pinConfigTx.uFuncSel = AM_HAL_PIN_42_UART1TX;
  am_hal_gpio_pinconfig(42, pinConfigTx);

  // === RX (PC → MCU) ===
  am_hal_gpio_pincfg_t pinConfigRx = g_AM_BSP_GPIO_COM_UART_RX;
  pinConfigRx.uFuncSel = AM_HAL_PIN_43_UART1RX;
  pinConfigRx.ePullup = AM_HAL_GPIO_PIN_PULLUP_WEAK;
  am_hal_gpio_pinconfig(43, pinConfigRx);
}

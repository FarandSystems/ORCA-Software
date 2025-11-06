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
void turn_iridium_off(void);
void turn_iridium_on(void);
void turn_gnss_on(void);
void turn_gnss_off(void);

void setup_GPIO()
{
    // Set the pins as outputs
  pinMode(pin_Toggle, OUTPUT);
  pinMode(pin_UART, OUTPUT);


  pinMode(pin_LED, OUTPUT);
  pinMode(pin_Buzzer, OUTPUT);

  // IRIDUIM PINs
  pinMode(iridiumPwrEN, OUTPUT); 
  pinMode(superCapChgEN, OUTPUT);
  pinMode(iridiumSleep, OUTPUT); 

  // GNSS PINs
  pinMode(gnssEN, OUTPUT); 
}

// --- Power helpers (minimum set) ---
void turn_iridium_off(void)
{
  Serial.println(F("turn iridium off"));
  digitalWrite(iridiumPwrEN, LOW);
  digitalWrite(superCapChgEN, LOW);
  digitalWrite(iridiumSleep, LOW);
}

void turn_gnss_on(void)
{
  Serial.println(F("turn gnss on"));
  turn_iridium_off();
  delay(100);
  pinMode(gnssEN, OUTPUT); digitalWrite(gnssEN, LOW); // enable GNSS
  delay(1000);
}
void turn_gnss_off(void)
{
  Serial.println(F("turn gnss off"));
  digitalWrite(gnssEN, HIGH); // disable GNSS
  delay(10);
  pinMode(gnssEN, INPUT_PULLUP); // avoid backfeed
}

void turn_iridium_on(void)
{
  Serial.println(F("turn iridium on"));
  turn_gnss_off();
  delay(100);
  digitalWrite(superCapChgEN, HIGH);
  digitalWrite(iridiumPwrEN, HIGH);
  delay(1000);
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

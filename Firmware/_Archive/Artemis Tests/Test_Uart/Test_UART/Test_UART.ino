// UART Test with Iridium Disable on D17
// Ensures the Iridium modem (on D17) is powered down before sending a test pattern on Serial1

#include <Arduino.h>

const uint8_t IRIDIUM_POWER_PIN = 17;  // D17 controls Iridium power

void setup() {
  // Disable Iridium by driving D17 low
  pinMode(IRIDIUM_POWER_PIN, OUTPUT);
  digitalWrite(IRIDIUM_POWER_PIN, LOW);

  // Initialize Serial1 at 9600 baud for scope testing
  Serial1.begin(9600);
  delay(100);  // Allow line to settle
}

void loop() {
  // Send 0x0F (binary 00001111) for a clear alternating-bit pattern
  Serial1.write(0x0F);
  delay(100);  // Pause so you can distinguish individual bytes on the scope
}

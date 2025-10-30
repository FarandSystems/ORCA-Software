// Works on ESP32 / ESP32-S2/S3/C3 / ESP8266 with Arduino core
// Only uses Serial over USB/UART at 115200

void setup() {
  Serial.begin(115200);

  // If your board has native USB (ESP32-S2/S3/C3), give the host a moment to enumerate.
  #if defined(ARDUINO_USB_CDC_ON_BOOT) || defined(USBCON)
    unsigned long start = millis();
    while (!Serial && millis() - start < 3000) { delay(10); }
  #endif

  Serial.println("\n[Debug] Serial ready @115200");
}

void loop() {
  // Echo whatever you type in the Serial Monitor
  while (Serial.available()) {
    int c = Serial.read();
    Serial.write(c);
  }

  // Heartbeat once per second
  static uint32_t last = 0;
  if (millis() - last >= 1000) {
    last = millis();
    Serial.printf("Uptime: %lu ms\r\n", (unsigned long)millis());
  }
}

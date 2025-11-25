# Orca1 ESP32 Firmware

PlatformIO project for the ESP32 DevKit V1. The firmware bridges UART traffic to TCP/Wi‑Fi, using modules in `src/` such as `uart.cpp`, `wifi.cpp`, and GPIO/timer helpers declared in `main.h`.

## Build and upload
1. Install [PlatformIO](https://platformio.org/install) with ESP32 board support.
2. Connect the ESP32 DevKit V1 over USB.
3. From this folder run:
   - Build: `pio run`
   - Upload: `pio run -t upload`
   - Serial monitor: `pio device monitor`
4. Set `upload_port` or `monitor_port` in `platformio.ini` if your serial port is not auto-detected. The default monitor speed is 115200 baud.

`platformio.ini` sets the environment to `esp32doit-devkit-v1` (Arduino framework) so you can flash and debug without extra configuration.
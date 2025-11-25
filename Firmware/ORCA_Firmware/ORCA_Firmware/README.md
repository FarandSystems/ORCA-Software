# ORCA Firmware (Artemis)

PlatformIO project targeting the SparkFun RedBoard Artemis ATP (Apollo3, Arduino framework). Source lives in `src/` with peripheral-focused modules (`gpio.cpp`, `uart.cpp`, `i2c.cpp`, etc.) and hardware definitions in `include/`.

## Build and upload
1. Install [PlatformIO](https://platformio.org/install) and ensure the SparkFun Artemis drivers are available on your host.
2. Connect the Artemis ATP board over USB.
3. From this folder run:
   - Build: `pio run`
   - Upload: `pio run -t upload`
   - Serial monitor: `pio device monitor`
4. If your COM port differs, add `upload_port = <port>` and `monitor_port = <port>` to `platformio.ini`.

`platformio.ini` already pins the environment as `SparkFun_RedBoard_Artemis_ATP` with a 115200 baud monitor for consistent flashing and logging.
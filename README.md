# ORCA Software

Comprehensive repository for ORCA firmware, embedded experiments, Windows utilities, and supporting client applications. Each subproject lives in its own folder so you can develop and test components independently.

## Repository layout
- `Firmware/ORCA_Firmware/ORCA_Firmware/` – Production PlatformIO firmware targeting the SparkFun RedBoard Artemis ATP (Apollo3). Archived iterations live in sibling `_Archive` folders.
- `Orca1_ESP/` – PlatformIO project for the ESP32 DevKit V1 that bridges UART, Wi‑Fi, and GPIO functionality.
- `Test_App_C#/` – Collection of C# WinForms solutions for hardware validation, charting, and simple LAN control experiments.
- `Windows App/` – Windows desktop utilities, including the ORCA simulator and visualization components.

## Getting started
1. Install the toolchain for the project you want to work on:
   - Firmware: [PlatformIO](https://platformio.org/install) via VS Code or the `platformio` CLI.
   - Windows and test apps: Visual Studio 2019+ with .NET Framework 4.8 workloads.
2. Clone the repository and open the subproject folder in your IDE of choice.
3. Follow the project-specific README in each subfolder for build and run instructions.

## PlatformIO quickstart
Both firmware projects include a `platformio.ini` that pins the target board and serial speed. From the project folder you can:
- Build: `pio run`
- Upload to the connected board: `pio run -t upload`
- Monitor the serial output: `pio device monitor`

Adjust the `upload_port` and `monitor_port` options in `platformio.ini` if your serial port differs from the defaults.
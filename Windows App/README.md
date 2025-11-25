# Windows App Suite

Desktop tools that accompany the ORCA firmware projects.

## Projects
- `ORCA_Simulator_Application/ORCA_Simulator_Application.sln` – Simulator and visualization WinForms app with packaged dependencies in `packages/`.
- `Visualizer_Component/` – Visualization-focused project components.
- `CNC_Axis_Component` and `CNC_XYZ_Component - Laptop_AutoReconnect` – CNC-related utilities.
- `Movement_Functions_Control` and `Command_Control` – Motion and command control helpers.
- `Resistor_Testboard_Calibrator` – Calibration utility for the resistor test board.
- `Farand_Chart_Lib_Ver3` – Shared charting library used by multiple tools.

## Building and running
1. Install Visual Studio 2019 or later with the **.NET Framework 4.8** workload.
2. Open the appropriate `.sln` file for the tool you want to run (e.g., `ORCA_Simulator_Application.sln`).
3. Restore NuGet packages when prompted; the `packages/` folder already contains pinned versions for offline builds.
4. Build and run in Debug or Release mode to launch the WinForms application.
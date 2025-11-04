#include <Adafruit_ISM330DHCX.h>
#include <Wire.h>
#include <SparkFun_Qwiic_Power_Switch_Arduino_Library.h>
extern QWIIC_POWER qwiic_switch;
extern Adafruit_ISM330DHCX ism330dhcx;

extern volatile float ax, ay, az;
extern volatile float gx, gy, gz;
extern bool is_qwiic_on;

extern void setup_i2c(void);
extern void read_IMU(void);
extern void Switch_Qwiic(bool is_On);

extern void Init_Sensors(void);

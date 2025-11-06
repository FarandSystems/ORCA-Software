#include <SparkFun_u-blox_GNSS_Arduino_Library.h>
extern SFE_UBLOX_GNSS gnss;

extern volatile float gnss_lat, gnss_lon, gnss_alt_m;
extern volatile float gnss_h_acc_m, gnss_speed_mps, gnss_cog_deg;

extern bool is_Gnss_Ready;
extern bool is_Gnss_Data_Valid;

extern bool start_GNSS(TwoWire &bus, uint8_t navHz, bool persist);

extern void gnss_pause(void);;

extern void gnss_resume(uint8_t hz);
extern void Read_Gnss(void);
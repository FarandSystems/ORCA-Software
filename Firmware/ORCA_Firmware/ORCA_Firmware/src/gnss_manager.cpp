#include "main.h"

SFE_UBLOX_GNSS gnss;

volatile float gnss_lat, gnss_lon, gnss_alt_m;
volatile float gnss_h_acc_m, gnss_speed_mps, gnss_cog_deg;

bool is_Gnss_Ready = false;
bool is_Gnss_Data_Valid = false;

bool start_GNSS(TwoWire &bus, uint8_t navHz, bool persist);
void gnss_pause(void);
void gnss_resume(uint8_t hz);
void Read_Gnss(void);

bool start_GNSS(TwoWire &bus, uint8_t navHz = 5, bool persist = false)
{
  // bus.begin() + setClock(400000) should have been done elsewhere
  if (!gnss.begin(bus, 0x42)) 
  {
    Serial.println(F("GNSS not detected on I2C (0x42)."));
    is_Gnss_Ready = false;
    return false;
  }

  // UBX-only over I2C, auto PVT, sane rate/model
  gnss.setI2COutput(COM_TYPE_UBX);
  gnss.setAutoPVT(true);
  gnss.setNavigationFrequency(navHz);
  gnss.setDynamicModel(DYN_MODEL_PORTABLE);

  if (persist) 
  {
    // Call this sparingly (NVM write). Use once after you settle on config.
    gnss.saveConfiguration();
  }

  is_Gnss_Ready = true;
  return true;
}

void Read_Gnss(void)
{
    if (gnss.getPVT()) 
    {
        uint8_t fixType = gnss.getFixType();
        uint8_t siv = gnss.getSIV();
        Serial.print("Fix=");  Serial.print(fixType);
        Serial.print(" SIV=");  Serial.print(siv);
        Serial.println();

        if (gnss.getGnssFixOk() && fixType >= 2 && siv >= 4) // Valid Data
        {
            is_Gnss_Data_Valid = true;
            gnss_lat = gnss.getLatitude()  / 1e7f;
            gnss_lon = gnss.getLongitude() / 1e7f;
            gnss_alt_m = gnss.getAltitudeMSL() / 1000.0f;
            gnss_h_acc_m = gnss.getHorizontalAccEst() / 1000.0f;
            gnss_speed_mps = gnss.getGroundSpeed() / 1000.0f;
            gnss_cog_deg =  gnss.getHeading() / 1e5f;

            Serial.print(" Lat=");  Serial.print(gnss_lat, 7);
            Serial.print(" Lon=");  Serial.print(gnss_lon, 7);
        
            Serial.print(" Hacc(m)="); Serial.println(gnss_h_acc_m, 3);
        }
        else
        {
            is_Gnss_Data_Valid = false;
        }
    }
    else
    {
        Serial.println("Failed to read GNSS!");
    }
}

// Disable without cold-start risk (quiet + low power)
void gnss_pause() 
{
  gnss.setAutoPVT(false);          // stop auto messages
  gnss.powerSaveMode(true);        // enter PSM / cyclic tracking
  gnss.setNavigationFrequency(1);  // reduce workload
}

// Re-enable quickly
void gnss_resume(uint8_t hz = 5) 
{
  gnss.powerSaveMode(false);       // back to continuous
  gnss.setNavigationFrequency(hz);
  gnss.setAutoPVT(true);
}
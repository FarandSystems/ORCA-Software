#include <Wire.h>
#include <SparkFun_u-blox_GNSS_Arduino_Library.h>

// --- Only the pins you need ---
#ifndef gnssEN
  #define gnssEN             26  // LOW = enable GNSS
#endif
#ifndef iridiumPwrEN
  #define iridiumPwrEN       22  // HIGH = power Iridium
#endif
#ifndef superCapChgEN
  #define superCapChgEN      27  // HIGH = enable supercap charger
#endif
#ifndef iridiumSleep
  #define iridiumSleep       17  // HIGH = Iridium on; LOW = sleep/off
#endif

TwoWire agtWire(9, 8);        // your existing bus
SFE_UBLOX_GNSS gnss;

static bool gnssReady = false;

// --- Power helpers (minimum set) ---
void turn_iridium_off(void)
{
  Serial.println(F("turn iridium off"));
  pinMode(iridiumPwrEN, OUTPUT); digitalWrite(iridiumPwrEN, LOW);
  pinMode(superCapChgEN, OUTPUT); digitalWrite(superCapChgEN, LOW);
  pinMode(iridiumSleep, OUTPUT);  digitalWrite(iridiumSleep, LOW);
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
  pinMode(gnssEN, OUTPUT); digitalWrite(gnssEN, HIGH); // disable GNSS
  delay(10);
  pinMode(gnssEN, INPUT_PULLUP); // avoid backfeed
}

void turn_iridium_on(void){
  Serial.println(F("turn iridium on"));
  turn_gnss_off();
  delay(100);
  pinMode(superCapChgEN, OUTPUT); digitalWrite(superCapChgEN, HIGH);
  pinMode(iridiumPwrEN, OUTPUT);  digitalWrite(iridiumPwrEN, HIGH);
  delay(1000);
}

// --- GNSS wrappers ---
bool startGNSS(uint8_t navHz = 5)
{
  turn_gnss_on();
  agtWire.begin();
  agtWire.setClock(400000);

  if (!gnss.begin(agtWire, 0x42)) 
  {
    Serial.println(F("GNSS not detected on agtWire (addr 0x42)."));
    gnssReady = false;
    return false;
  }
  gnss.setI2COutput(COM_TYPE_UBX);
  gnss.setAutoPVT(true);           // ensure auto messages are enabled
  gnss.setNavigationFrequency(navHz);
  gnss.setDynamicModel(DYN_MODEL_PORTABLE);
  gnss.saveConfiguration(); // (optional, once)
  gnssReady = true;
  return true;
}


void setup() 
{
  Serial.begin(115200);
  startGNSS(5); // 5 Hz
}
void loop() 
{
  if (gnss.getPVT()) 
  {
    Serial.print("Fix=");  Serial.print(gnss.getFixType());
    Serial.print(" SIV=");  Serial.print(gnss.getSIV());
    Serial.print(" Lat=");  Serial.print(gnss.getLatitude()/1e7, 7);
    Serial.print(" Lon=");  Serial.print(gnss.getLongitude()/1e7, 7);
  
    uint32_t hAcc_mm = gnss.getHorizontalAccEst(); // horizontal accuracy (mm)
    Serial.print(" Hacc(m)="); Serial.println(hAcc_mm / 1000.0, 3);
  }
}

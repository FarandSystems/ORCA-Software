#include <GSMSimSMS.h>
#include "i2c.h"                    // ← include your i2c interface

#define RESET_PIN 37
#define IRIDIUM_PWR_PIN  22
#define IRIDIUM_SLEEP_PIN 17

static volatile int num = 0;
GSMSimSMS sms(Serial1, RESET_PIN);

void setup() 
{
  // power‐control pins
  pinMode(IRIDIUM_PWR_PIN, OUTPUT);
  digitalWrite(IRIDIUM_PWR_PIN, LOW);
  pinMode(IRIDIUM_SLEEP_PIN, OUTPUT);
  digitalWrite(IRIDIUM_SLEEP_PIN, LOW);

  Serial1.begin(115200);
  while(!Serial1) { }

  Serial.begin(115200);

  // Init I²C sensors
  i2cInit();

  // Init GSM module
  sms.init();
  sms.setPhoneFunc(1);
  delay(1000);
  sms.isRegistered();
  delay(1000);
  sms.signalQuality();
  delay(1000);
  sms.operatorNameFromSim();
  delay(1000);
  sms.initSMS();
  delay(1000);
  sms.list(true);
  delay(1000);

  Serial.println("Begin to listen incoming messages...");
}

void loop() {
  if (Serial1.available()) {
    String buffer = Serial1.readString();
    num++;
    Serial.print(num);
    Serial.print(". ");
    Serial.println(buffer);

    // incoming SMS notification
    if (buffer.indexOf("+CMTI:") != -1) {
      int indexno = sms.indexFromSerial(buffer);
      String sender = sms.getSenderNo(indexno);
      String msg    = sms.readFromSerial(buffer);
      Serial.print("From: "); Serial.println(sender);
      Serial.print("Msg: ");  Serial.println(msg);

      msg.trim();
      // if the message is exactly "PHT" (case‑sensitive)
      if (msg.indexOf("PHT") != -1) {
        // read sensors
        i2cRead();
        // format reply
        String reply = "T: " + String(g_Temperature) + " C, "
                     + "H: " + String(g_Humidity)    + " %, "
                     + "P: " + String(g_Pressure)    + " Pa";
        // send SMS back to sender
        // after you build your Strings:
        const char* numPtr   = sender.c_str();
        const char* textPtr  = reply.c_str();

        // GSMSimSMS::send expects non‑const char*, so do a const_cast
        sms.send(const_cast<char*>(numPtr),
                const_cast<char*>(textPtr));

        Serial.print("Replied with: "); Serial.println(reply);
      }
      else
      {
        Serial.print("PHT not found!");
      }
    }
  }
}

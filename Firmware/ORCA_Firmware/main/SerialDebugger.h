#pragma once
#include <Arduino.h>
#include <mbed.h>
#include <stdarg.h>

enum DebugLevel
{
  DBG_FATAL = 0,
  DBG_ERROR = 1,
  DBG_WARN  = 2,
  DBG_INFO  = 3,
  DBG_DEBUG = 4,
  DBG_TRACE = 5
};

class SerialDebugger
{
public:
  SerialDebugger();

  void begin(uint32_t baud);
  void setLevel(int level);
  int  level() const;

  void print(const char* s);
  void println(const char* s);
  void printf(const char* fmt, ...);
  void hexdump(const void* data, size_t len, uint32_t addr_base = 0);

private:
  void vprintf_(const char* fmt, va_list ap);

private:
  rtos::Mutex m_mtx;
  int         m_level;
};

extern SerialDebugger serial_debugger;

// ── quick macros with timestamp + file:line ───────────────────────────────────
#define LOGF(fmt, ...) do { if (serial_debugger.level() >= DBG_FATAL) serial_debugger.printf("[F][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGE(fmt, ...) do { if (serial_debugger.level() >= DBG_ERROR) serial_debugger.printf("[E][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGW(fmt, ...) do { if (serial_debugger.level() >= DBG_WARN ) serial_debugger.printf("[W][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGI(fmt, ...) do { if (serial_debugger.level() >= DBG_INFO ) serial_debugger.printf("[I][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGD(fmt, ...) do { if (serial_debugger.level() >= DBG_DEBUG) serial_debugger.printf("[D][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGT(fmt, ...) do { if (serial_debugger.level() >= DBG_TRACE) serial_debugger.printf("[T][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)

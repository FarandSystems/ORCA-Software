#pragma once
#include <Arduino.h>
#include <mbed.h>
#include <stdarg.h>

// ── levels ─────────────────────────────────────────────────────────────────────
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

  // ── internal non-blocking backend ────────────────────────────────────────────
  void enqueue_(const char* data, size_t n);
  void enqueue_char_(char c);
  void tx_task_();
  void emit_dropped_notice_();

  // Small wrappers to handle mbed variants with push/pop vs try_put/try_get
  inline bool cb_try_put_(uint8_t b);
  inline bool cb_try_get_(uint8_t& b);

private:
  rtos::Mutex m_mtx;
  int         m_level;

  rtos::Thread     m_txThread;
  rtos::Semaphore  m_kick{0};
  volatile bool    m_run = false;

  static constexpr size_t TX_BUF_SIZE = 4096;
  mbed::CircularBuffer<uint8_t, TX_BUF_SIZE> m_q;

  volatile uint32_t m_dropped = 0;
};

extern SerialDebugger serial_debugger;

// ── quick macros with timestamp + file:line ───────────────────────────────────
#define LOGF(fmt, ...) do { if (serial_debugger.level() >= DBG_FATAL) serial_debugger.printf("[F][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGE(fmt, ...) do { if (serial_debugger.level() >= DBG_ERROR) serial_debugger.printf("[E][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGW(fmt, ...) do { if (serial_debugger.level() >= DBG_WARN ) serial_debugger.printf("[W][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGI(fmt, ...) do { if (serial_debugger.level() >= DBG_INFO ) serial_debugger.printf("[I][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGD(fmt, ...) do { if (serial_debugger.level() >= DBG_DEBUG) serial_debugger.printf("[D][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)
#define LOGT(fmt, ...) do { if (serial_debugger.level() >= DBG_TRACE) serial_debugger.printf("[T][%lu] %s:%d: " fmt "\r\n", (unsigned long)millis(), __FILE__, __LINE__, ##__VA_ARGS__); } while(0)

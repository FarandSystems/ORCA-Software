#include "SerialDebugger.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

using namespace std::chrono_literals;

SerialDebugger serial_debugger;

SerialDebugger::SerialDebugger()
{
  m_level = DBG_DEBUG;
}

void SerialDebugger::begin(uint32_t baud)
{
  Serial.begin(baud);

  m_run = true;
  // Qualify to satisfy some mbed variants
  m_txThread.start(mbed::callback(this, &SerialDebugger::tx_task_));
}

void SerialDebugger::setLevel(int level)
{
  m_level = level;
}

int SerialDebugger::level() const
{
  return m_level;
}

// --- CircularBuffer compatibility wrappers ------------------------------------
// Some mbed versions have push()/pop() returning void.
// We wrap them so we always return a bool.
inline bool SerialDebugger::cb_try_put_(uint8_t b)
{
  if (m_q.full()) return false;
  m_q.push(b);        // may be void; if it succeeds, we just return true
  return true;
}

inline bool SerialDebugger::cb_try_get_(uint8_t& b)
{
  if (m_q.empty()) return false;
  m_q.pop(b);         // may be void; if it succeeds, we just return true
  return true;
}
// -----------------------------------------------------------------------------

void SerialDebugger::enqueue_char_(char c)
{
  if (!cb_try_put_((uint8_t)c))
  {
    m_dropped++;
  }
}

void SerialDebugger::enqueue_(const char* data, size_t n)
{
  for (size_t i = 0; i < n; ++i)
  {
    if (!cb_try_put_((uint8_t)data[i]))
    {
      m_dropped += (uint32_t)(n - i);
      break;
    }
  }
  m_kick.release();
}

void SerialDebugger::print(const char* s)
{
  if (!s) return;
  m_mtx.lock();
  enqueue_(s, strlen(s));
  m_mtx.unlock();
}

void SerialDebugger::println(const char* s)
{
  m_mtx.lock();
  if (s) enqueue_(s, strlen(s));
  enqueue_("\r\n", 2);
  m_mtx.unlock();
}

void SerialDebugger::vprintf_(const char* fmt, va_list ap)
{
  char buf[256];

  va_list ap2;
  va_copy(ap2, ap);
  int n = vsnprintf(buf, sizeof(buf), fmt, ap2);
  va_end(ap2);

  m_mtx.lock();

  if (n >= 0 && n < (int)sizeof(buf))
  {
    enqueue_(buf, (size_t)n);
  }
  else
  {
    if (n > 0)
    {
      char* dyn = (char*)malloc((size_t)n + 1u);
      if (dyn)
      {
        vsnprintf(dyn, (size_t)n + 1u, fmt, ap);
        enqueue_(dyn, (size_t)n);
        free(dyn);
      }
      else
      {
        static const char trunc[] = "[SerialDebugger: format truncated]\r\n";
        enqueue_(trunc, sizeof(trunc) - 1);
      }
    }
  }

  m_mtx.unlock();
}

void SerialDebugger::printf(const char* fmt, ...)
{
  va_list ap;
  va_start(ap, fmt);
  vprintf_(fmt, ap);
  va_end(ap);
}

void SerialDebugger::hexdump(const void* data, size_t len, uint32_t addr_base)
{
  const uint8_t* p = (const uint8_t*)data;

  m_mtx.lock();

  for (size_t i = 0; i < len; i += 16)
  {
    char line[128];
    int  off = snprintf(line, sizeof(line), "%08lX  ",
                        (unsigned long)(addr_base + (uint32_t)i));

    for (size_t j = 0; j < 16; ++j)
    {
      if (i + j < len)
        off += snprintf(line + off, sizeof(line) - (size_t)off, "%02X ", p[i + j]);
      else
        off += snprintf(line + off, sizeof(line) - (size_t)off, "   ");
    }

    off += snprintf(line + off, sizeof(line) - (size_t)off, " |");
    for (size_t j = 0; j < 16 && i + j < len; ++j)
    {
      char c = (char)p[i + j];
      off += snprintf(line + off, sizeof(line) - (size_t)off,
                      "%c", (c >= 32 && c <= 126) ? c : '.');
    }
    (void)snprintf(line + off, sizeof(line) - (size_t)off, "|\r\n");

    enqueue_(line, strlen(line));
  }

  m_mtx.unlock();
}

void SerialDebugger::emit_dropped_notice_()
{
  uint32_t dropped = m_dropped;
  if (dropped == 0) return;
  m_dropped = 0;

  char note[64];
  int n = snprintf(note, sizeof(note),
                   "\r\n[SerialDebugger: dropped %lu bytes]\r\n",
                   (unsigned long)dropped);
  if (n > 0) enqueue_(note, (size_t)n);
}

void SerialDebugger::tx_task_()
{
  while (m_run)
  {
    m_kick.try_acquire_for(10ms);

    emit_dropped_notice_();

#if ARDUINO >= 10600
    size_t room = Serial.availableForWrite();
    if (room == 0) continue;
#else
    size_t room = 64;
#endif

    size_t sent = 0;
    while (sent < room)
    {
      uint8_t b;
      if (!cb_try_get_(b)) break;
      Serial.write(&b, 1);
      sent++;
    }
  }
}

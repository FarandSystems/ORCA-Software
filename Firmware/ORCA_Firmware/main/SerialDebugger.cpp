#include "SerialDebugger.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

SerialDebugger serial_debugger;

SerialDebugger::SerialDebugger()
{
  m_level = DBG_DEBUG;
}

void SerialDebugger::begin(uint32_t baud)
{
  Serial.begin(baud);
  while (!Serial)
  {
    // wait for USB serial if needed
  }
}

void SerialDebugger::setLevel(int level)
{
  m_level = level;
}

int SerialDebugger::level() const
{
  return m_level;
}

void SerialDebugger::print(const char* s)
{
  m_mtx.lock();
  Serial.print(s);
  m_mtx.unlock();
}

void SerialDebugger::println(const char* s)
{
  m_mtx.lock();
  Serial.println(s);
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
    Serial.print(buf);
  }
  else
  {
    // allocate exact size if longer than stack buffer
    if (n < 0)
    {
      // formatting error; print nothing
    }
    else
    {
      char* dyn = (char*)malloc((size_t)n + 1u);
      if (dyn)
      {
        vsnprintf(dyn, (size_t)n + 1u, fmt, ap);
        Serial.print(dyn);
        free(dyn);
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
    int  off = snprintf(line, sizeof(line), "%08lX  ", (unsigned long)(addr_base + (uint32_t)i));

    for (size_t j = 0; j < 16; ++j)
    {
      if (i + j < len)
      {
        off += snprintf(line + off, sizeof(line) - (size_t)off, "%02X ", p[i + j]);
      }
      else
      {
        off += snprintf(line + off, sizeof(line) - (size_t)off, "   ");
      }
    }

    off += snprintf(line + off, sizeof(line) - (size_t)off, " |");
    for (size_t j = 0; j < 16 && i + j < len; ++j)
    {
      char c = (char)p[i + j];
      off += snprintf(line + off, sizeof(line) - (size_t)off, "%c", (c >= 32 && c <= 126) ? c : '.');
    }
    snprintf(line + off, sizeof(line) - (size_t)off, "|\r\n");

    Serial.print(line);
  }

  m_mtx.unlock();
}

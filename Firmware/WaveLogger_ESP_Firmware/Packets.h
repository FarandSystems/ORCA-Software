#pragma once
#include <stddef.h>
#include <stdint.h>

static constexpr size_t kCmdSize = 8;    // PC -> Artemis command size
static constexpr size_t kPktSize = 64;   // Artemis -> PC packet size
static constexpr int    kQDepth  = 8;    // queue depth for bursts

struct Packet {
  uint8_t data[kPktSize];
  size_t  len;
};
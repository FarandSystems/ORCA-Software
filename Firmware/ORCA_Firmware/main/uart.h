

constexpr int RX_Buffer_Size = 8;
constexpr int TX_Buffer_Size = 650;
extern void uart_begin(uint32_t baud);

extern uint8_t Calculate_Checksum(uint8_t *buffer, uint8_t length);

extern uint8_t rx_buffer[RX_Buffer_Size];
extern uint8_t rx_index;
extern bool frame_ready;

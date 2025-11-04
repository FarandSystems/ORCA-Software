

constexpr int RX_Buffer_Size = 8;
constexpr int TX_Buffer_Size = 16;
const int UART_BAUDRATE = 9600;
const int uart_timeout = 40;
extern uint8_t uart_timeout_counter;
extern bool uart_reset_request;
extern bool rx_frame_ready;
extern bool rx_reading_request;

static const uint16_t MAX_BYTES_PER_CALL   = 24;    // byte budget
static const uint32_t RX_GAP_RESET_MS      = 200;    // idle gap → resync
static inline bool isSync(uint8_t b) { return (b == 0xFA) || (b == 0x55); } // 0xFA : PC , 0x55: ESP

extern uint8_t encoded_length;
extern void uart_begin(uint32_t baud);
extern void Reset_UART(void);
extern void check_rx_ready(void);

extern uint8_t Calculate_Checksum(uint8_t *buffer, uint8_t length);

extern uint8_t rx_buffer[RX_Buffer_Size];
extern uint8_t rx_index;

extern uint8_t tx_buffer[TX_Buffer_Size];

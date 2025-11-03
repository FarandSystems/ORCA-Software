

constexpr int RX_Buffer_Size = 8;
constexpr int TX_Buffer_Size = 16;
const int UART_BAUDRATE = 9600;
const int uart_timeout = 80;
extern uint8_t uart_timeout_counter;
extern bool uart_reset_request;

extern uint8_t encoded_length;
extern void uart_begin(uint32_t baud);
extern void Reset_UART(void);

extern uint8_t Calculate_Checksum(uint8_t *buffer, uint8_t length);

extern uint8_t rx_buffer[RX_Buffer_Size];
extern uint8_t rx_index;

extern uint8_t tx_buffer[TX_Buffer_Size];

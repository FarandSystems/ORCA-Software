

extern bool rx_frame_ready;
extern bool report_to_pc_ready;
extern bool is_power_requested;

extern void check_rx_ready(void);
extern void Service_Input_Command(uint8_t* RxBuffer);
extern void Report_To_PC(void);
extern uint8_t Encode_0x1A(uint8_t *input, uint8_t length, uint8_t *output);


extern bool report_to_pc_ready;
extern bool is_power_requested;
extern bool is_skip_gnss_requested;

extern void Service_Input_Command(uint8_t* RxBuffer);
extern void Report_To_PC(void);
extern uint8_t Encode_0x1A(uint8_t *input, uint8_t length, uint8_t *output);

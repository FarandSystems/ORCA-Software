#include "I2CModule.h"

I2CModule::I2CModule(uint32_t iom_index, uint32_t clock_freq, uint32_t trigger_every_n_ticks, osPriority priority)
    : m_iom_index(iom_index),
      m_clock_freq(clock_freq),
      m_trigger_every_n_ticks(trigger_every_n_ticks),
      m_tick_count(0),
      m_iom_handle(nullptr),
      m_thread(priority, 1536),
      m_trigger_sem(0),
      m_busy(false),
      m_rx_buffer(nullptr),
      m_rx_len(0),
      m_current_addr(0),
      m_current_reg(0)
{
}

I2CModule::~I2CModule()
{
    if (m_iom_handle)
    {
        am_hal_iom_disable(m_iom_handle);
        am_hal_iom_uninitialize(m_iom_handle);
    }
}

bool I2CModule::init()
{
    if (m_iom_index == 1)
    {
        am_hal_gpio_pinconfig(AM_BSP_GPIO_IOM1_SCL, g_AM_BSP_GPIO_IOM1_SCL);
        am_hal_gpio_pinconfig(AM_BSP_GPIO_IOM1_SDA, g_AM_BSP_GPIO_IOM1_SDA);
    }

    if (am_hal_iom_initialize(m_iom_index, &m_iom_handle) != AM_HAL_STATUS_SUCCESS)
    {
        return false;
    }
    if (am_hal_iom_power_ctrl(m_iom_handle, AM_HAL_SYSCTRL_WAKE, false) != AM_HAL_STATUS_SUCCESS)
    {
        return false;
    }

    am_hal_iom_config_t cfg = {};
    cfg.eInterfaceMode = AM_HAL_IOM_I2C_MODE;
    cfg.ui32ClockFreq = m_clock_freq;
    if (am_hal_iom_configure(m_iom_handle, &cfg) != AM_HAL_STATUS_SUCCESS)
    {
        return false;
    }
    if (am_hal_iom_enable(m_iom_handle) != AM_HAL_STATUS_SUCCESS)
    {
        return false;
    }

    am_hal_iom_interrupt_clear(m_iom_handle, 0xFFFFFFFF);
    am_hal_iom_interrupt_enable(m_iom_handle, AM_HAL_IOM_INT_CMDCMP | AM_HAL_IOM_INT_THR | AM_HAL_IOM_INT_DERR);
    NVIC_EnableIRQ(static_cast<IRQn_Type>(IOMSTR0_IRQn + m_iom_index));
    am_hal_interrupt_master_enable();

    return true;
}

void I2CModule::start()
{
    m_thread.start(mbed::callback(this, &I2CModule::i2cThread));
}

void I2CModule::onTick()
{
    m_tick_count++;
    if (m_tick_count >= m_trigger_every_n_ticks)
    {
        m_tick_count = 0;
        m_trigger_sem.release();
    }
}

void I2CModule::i2cThread()
{
    while (true)
    {
        m_trigger_sem.acquire();
        if (!m_busy)
        {
            initiateRead();
        }
    }
}

bool I2CModule::readBytes(uint8_t addr, uint8_t reg, uint8_t* buf, uint32_t len, std::function<void()> complete_callback)
{
    if (m_busy)
    {
        return false;
    }
    m_rx_buffer = buf;
    m_rx_len = len;
    m_current_addr = addr;
    m_current_reg = reg;
    m_complete_callback = complete_callback;
    return true;
}

void I2CModule::initiateRead()
{
    if (m_rx_buffer == nullptr)
    {
        return;
    }
    m_busy = true;

    am_hal_iom_transfer_t x = {};
    x.uPeerInfo.ui32I2CDevAddr = m_current_addr;
    x.ui32InstrLen = 1;
    x.ui32Instr = m_current_reg;
    x.eDirection = AM_HAL_IOM_RX;
    x.ui32NumBytes = m_rx_len;
    x.pui32RxBuffer = (uint32_t*)m_rx_buffer;
    x.bContinue = false;
    x.ui8RepeatCount = 0;
    x.ui8Priority = 1;

    if (am_hal_iom_nonblocking_transfer(m_iom_handle, &x, [](void* ctx, uint32_t status) {
        I2CModule* self = static_cast<I2CModule*>(ctx);
        self->handleComplete();
    }, this) != AM_HAL_STATUS_SUCCESS)
    {
        m_busy = false;
        m_rx_buffer = nullptr;
        m_rx_len = 0;
    }
}

void I2CModule::handleComplete()
{
    m_busy = false;
    if (m_complete_callback)
    {
        m_complete_callback();
    }
    m_rx_buffer = nullptr;
    m_rx_len = 0;
}

void I2CModule::handleISR(uint32_t status)
{
    am_hal_iom_interrupt_clear(m_iom_handle, status);
    if (status & AM_HAL_IOM_INT_CMDCMP)
    {
        handleComplete();
    }
    am_hal_iom_interrupt_service(m_iom_handle, status);
}

bool I2CModule::writeByte(uint8_t addr, uint8_t reg, uint8_t val)
{
    if (m_busy)
    {
        return false;
    }
    uint32_t word = val;
    am_hal_iom_transfer_t x = {};
    x.uPeerInfo.ui32I2CDevAddr = addr;
    x.ui32InstrLen = 1;
    x.ui32Instr = reg;
    x.eDirection = AM_HAL_IOM_TX;
    x.ui32NumBytes = 1;
    x.pui32TxBuffer = &word;
    x.ui8Priority = 1;
    return am_hal_iom_blocking_transfer(m_iom_handle, &x) == AM_HAL_STATUS_SUCCESS;
}

extern "C" void am_iomaster1_isr(void)
{
    extern I2CModule g_i2c_module;
    uint32_t status = 0;
    am_hal_iom_interrupt_status_get(g_i2c_module.m_iom_handle, true, &status);
    if (status)
    {
        g_i2c_module.handleISR(status);
    }
}
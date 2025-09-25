using FluentModbus;
using System.IO.MemoryMappedFiles;
using static log4net.Appender.FileAppender;

namespace EQX.InOut
{
    public class SimulationInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        private readonly ModbusTcpClient _client;
        private readonly object _lock = new object();

        public SimulationInputDevice()
            : base()
        {
            _client = new ModbusTcpClient();
            _client.Connect();

        }
        public bool this[int index] => ActualGetInput(index);
        ~SimulationInputDevice()
        {
            _client.Dispose();
        }
        public void Connect()
        {
            try
            {
                _client.Connect();
            }
            catch (Exception ex)
            {

            }
        }
        protected override bool ActualGetInput(int index)
        {
            lock (_lock)
            {
                var result = _client.ReadCoils(0, index, 1);
                return (result[0] & 0x01) != 0;
            }
        }
    }
}
using FluentModbus;
using System.IO.MemoryMappedFiles;
using static log4net.Appender.FileAppender;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EQX.InOut
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public class SimulationInputDeviceServer<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        private readonly ModbusTcpServer _server;
        private readonly object _lock = new object();

        public SimulationInputDeviceServer()
            : base()
        {
            _server = new ModbusTcpServer();
        }

        ~SimulationInputDeviceServer()
        {
            _server.Dispose();
        }
        public bool this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, value);
        }
        public void SetValue(int index, bool value)
        {
            _server.GetCoils().Set(index, value);
        }
        public bool GetValue(int index)
        {
            return (_server.GetCoils()[index / 8] & (1 << index % 8)) != 0;
        }
        public void Start()
        {
            _server.Start();
        }
        private int _offset;
    }
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
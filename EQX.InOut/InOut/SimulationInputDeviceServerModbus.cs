using FluentModbus;

namespace EQX.InOut
{
    public class SimulationInputDeviceServerModbus<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        private readonly ModbusTcpServer _server;
        private readonly object _lock = new object();

        public override bool IsConnected => true;

        public SimulationInputDeviceServerModbus()
            : base()
        {
            _server = new ModbusTcpServer();
        }

        ~SimulationInputDeviceServerModbus()
        {
            _server.Dispose();
        }
        public void SetValue(int index, bool value)
        {
            _server.GetCoils().Set(index, value);
        }

        public void ToggleInput(int index)
        {
            _server.GetCoils().Toggle(index);
        }

        public bool GetValue(int index)
        {
            return (_server.GetCoils()[index / 8] & (1 << index % 8)) != 0;
        }

        protected override bool ActualGetInput(int index)
        {
            lock (_lock)
            {
                return GetValue(index);
            }
        }

        public void Start()
        {
            _server.Start();
        }
        private int _offset;
    }
}
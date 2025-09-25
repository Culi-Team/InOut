using FluentModbus;

namespace EQX.InOut
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
}
using EQX.Core.InOut;

namespace EQX.InOut.Virtual
{
    public class MappableInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        private readonly Dictionary<int, (IDOutputDevice outputDevice, int outputPin)> _mappings = new();
        private readonly Dictionary<int, bool> _manualOverrides = new();
        //private Dictionary<int, (IDOutputDevice outputDevice, int outputPin)> _mappings = new();
        public MappableInputDevice() : base()
        {
            IsConnected = true;
        }

        public void Mapping(int inputPin, IDOutputDevice outputDevice, int outputPin)
        {
            _mappings.Add(inputPin, (outputDevice, outputPin));
        }

        protected override bool ActualGetInput(int index)
        {
            return _mappings.ContainsKey(index) && _mappings[index].outputDevice[_mappings[index].outputPin];
        }
    }
}


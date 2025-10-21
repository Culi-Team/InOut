using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;
using EQX.InOut.Virtual;

namespace EQX.InOut
{
    public static class FlagHelpers
    {
        public static void MapTo(this IDInput input, IDOutput output)
        {
            if (input is DInput dInput && output is DOutput dOutput)
            {
                var inDevice = dInput.GetInputDevice();
                var outDevice = dOutput.GetOutputDevice();

                if (outDevice.GetType().GetGenericTypeDefinition() != typeof(Virtual.MappableOutputDevice<>))
                {
                    throw new InvalidOperationException("Output device is not of type VirtualOutputDevice");
                }

                if (inDevice.GetType().IsGenericType &&
                    inDevice.GetType().GetGenericTypeDefinition() == typeof(MappableInputDevice<>))
                {
                    dynamic device = inDevice;
                    device.Mapping(input.Id, outDevice, dOutput.Id);
                }
                else
                {
                    throw new InvalidOperationException("Input device is not of type VirtualInputDevice");
                }
            }
            else
            {
                throw new InvalidOperationException("Input/Output is not of type DInput/DOutput");
            }
        }
    }
    public class DInput : ObservableObject, IDInput
    {
        public event EventHandler? ValueUpdated;
        public event EventHandler? ValueChanged;
        public int Id { get; init; }
        public string Name { get; init; }
        public bool Value => _dInputDevice[Id];

        public DInput(int id, string name, IDInputDevice dInputDevice)
        {
            Id = id;
            Name = name;

            _dInputDevice = dInputDevice;
        }

        public void RaiseValueUpdated()
        {
            OnPropertyChanged(nameof(Value));
            ValueUpdated?.Invoke(this, EventArgs.Empty);

            _oldValue = _currentValue;
            _currentValue = Value;

            if (_oldValue != _currentValue)
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal IDInputDevice GetInputDevice() => _dInputDevice;

        private bool _oldValue;
        private bool _currentValue;
        private readonly IDInputDevice _dInputDevice;
    }
}

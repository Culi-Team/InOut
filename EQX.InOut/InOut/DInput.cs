using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;
using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EQX.InOut
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
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

        private bool _oldValue;
        private bool _currentValue;
        private readonly IDInputDevice _dInputDevice;
    }
}

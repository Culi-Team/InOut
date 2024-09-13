using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;

namespace EQX.InOut
{
    public class DInput : ObservableObject, IDInput
    {
        private bool _oldValue;
        private bool _currentValue;
        public event EventHandler ValueUpdated;
        public event EventHandler ValueChanged;
        public int Id { get; init; }
        public string Name { get; init; }
        public bool Value
        {
            get
            {

                _oldValue = _currentValue;
                _currentValue = _dInputDevice[Id];

                if (_oldValue != _currentValue)
                {
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }

                return _currentValue;
            }
        }
        public DInput(int id, string name, IDInputDevice dInputDevice)
        {
            Id = id;
            Name = name;

            _dInputDevice = dInputDevice;
        }

        public void RaiseValueUpdated()
        {
            OnPropertyChanged("Value");
            ValueUpdated?.Invoke(this, EventArgs.Empty);
        }

        private readonly IDInputDevice _dInputDevice;
    }
}

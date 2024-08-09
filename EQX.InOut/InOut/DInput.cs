using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;

namespace EQX.InOut
{
    public class DInput : ObservableObject, IDInput
    {
        public event EventHandler ValueUpdated;
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
            OnPropertyChanged("Value");
            ValueUpdated?.Invoke(this, EventArgs.Empty);
        }

        private readonly IDInputDevice _dInputDevice;
    }
}

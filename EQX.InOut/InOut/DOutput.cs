using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;

namespace EQX.InOut
{
    public class DOutput : ObservableObject, IDOutput
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public bool Value
        {
            get { return _dOutputDevice[Id]; }
            set
            {
                _dOutputDevice[Id] = value;
                OnPropertyChanged();
            }
        }

        public DOutput(int id, string name, IDOutputDevice dOutputDevice)
        {
            Id = id;
            Name = name;
            _dOutputDevice = dOutputDevice;
        }

        internal IDOutputDevice GetOutputDevice() => _dOutputDevice;

        private readonly IDOutputDevice _dOutputDevice;
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EQX.InOut
#pragma warning restore IDE0130 // Namespace does not match folder structure
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

        private readonly IDOutputDevice _dOutputDevice;
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.InOut
{
    public class VDOutput : ObservableObject, IDOutput
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

        public VDOutput(int id, string name, IDOutputDevice dOutputDevice)
        {
            Id = id;
            Name = name;
            _dOutputDevice = dOutputDevice;
        }

        private readonly IDOutputDevice _dOutputDevice;
    }
}

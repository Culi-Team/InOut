using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.InOut.Analog
{
    public class AInput : IAInput
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public double Volt => _aInputDevice.GetVolt(Id);

        public double Current => _aInputDevice.GetCurrent(Id);

        public AInput(int id,string name ,IAInputDevice aInputDevice)
        {
            Id = id;
            Name = name;
            _aInputDevice = aInputDevice;
        }

        private IAInputDevice _aInputDevice;
    }
}

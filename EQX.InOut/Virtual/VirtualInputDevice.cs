using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.Virtual
{
    public class VirtualInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        public VirtualInputDevice() : base()
        {
            IsConnected = true;
        }

        public void Mapping(int inputPin, IDOutputDevice outputDevice, int outputPin)
        {
            _mappings.Add(inputPin, (outputDevice, outputPin));
        }

        private Dictionary<int, (IDOutputDevice outputDevice, int outputPin)> _mappings = new();

        protected override bool ActualGetInput(int index)
        {
            return _mappings.ContainsKey(index) && _mappings[index].outputDevice[_mappings[index].outputPin];
            //return Inputs[index].Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.Virtual
{
    public class VirtualOutputDevice<TEnum> : OutputDeviceBase<TEnum> where TEnum : Enum
    {
        public VirtualOutputDevice() : base()
        {
            var outputList = Enum.GetNames(typeof(TEnum)).ToList();

            _outputs = new bool[outputList.Count];
        }

        protected override bool GetOutput(int index)
        {
            return _outputs[index];
        }

        protected override bool SetOutput(int index, bool value)
        {
            _outputs[index] = value;

            return true;
        }

        public void Clear()
        {
            var outputList = Enum.GetValues(typeof(TEnum));

            foreach (var output in outputList)
            {
                _outputs[(int)output] = false;
            }
        }
        private readonly bool[] _outputs;
    }
}

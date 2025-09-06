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
            
        }

        protected override bool ActualGetInput(int index)
        {
            return Inputs[index].Value;
        }
    }
}

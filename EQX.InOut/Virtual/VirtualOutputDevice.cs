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
            
        }

        protected override bool GetOutput(int index)
        {
            return Outputs[index].Value;
        }

        protected override bool SetOutput(int index, bool value)
        {
            Outputs[index].Value = value;

            return true;
        }
    }
}

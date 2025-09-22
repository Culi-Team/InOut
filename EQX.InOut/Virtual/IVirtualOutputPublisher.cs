using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.Virtual
{
    internal interface IVirtualOutputPublisher
    {
        void RegisterSubscriber(int outputPin, Action<bool> subscriber);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.InOut;

namespace EQX.InOut
{
    public class SimulationVacuumFactory : IVacuumFactory
    {
        public IVacuum Create(IDOutput? suctionOutput, IDOutput? blowOutput = null, IDInput? vacuumDetectInput = null)
        {
            return new SimulationVacuum(suctionOutput, blowOutput, vacuumDetectInput);
        }
    }
}

using EQX.Core.InOut;
using EQX.Core.InOut.Conveyor;

namespace EQX.InOut
{
    public class ConveyorFactory : IConveyorFactory
    {
        public IConveyor Create(IDInput? inError, IDOutput? outRun, IDOutput? outReverseRun, IDOutput? outStop)
        {
            return new Conveyor(inError, outRun, outReverseRun, outStop);
        }
    }
}

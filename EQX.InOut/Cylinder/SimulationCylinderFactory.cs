using EQX.Core.InOut;

namespace EQX.InOut
{
    public class SimulationCylinderFactory : ICylinderFactory
    {
        public ICylinder Create(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward, Func<bool>? forwardInterlock, Func<bool> backwardInterlock)
        {
            return new SimulationCylinder(inForward, inBackward, outForward, outBackward, forwardInterlock, backwardInterlock);
        }
    }
}

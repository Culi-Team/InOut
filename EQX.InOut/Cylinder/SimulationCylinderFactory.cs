using EQX.Core.InOut;

namespace EQX.InOut
{
    public class SimulationCylinderFactory : ICylinderFactory
    {
        public ICylinder Create(List<IDInput> inForward, List<IDInput> inBackward, IDOutput? outForward, IDOutput? outBackward)
        {
            return new SimulationCylinder(inForward, inBackward, outForward, outBackward);
        }
    }
}

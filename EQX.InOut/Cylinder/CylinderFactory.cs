using EQX.Core.InOut;

namespace EQX.InOut
{
    public class CylinderFactory : ICylinderFactory
    {
        public ICylinder Create(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward)
        {
            return new Cylinder(inForward, inBackward, outForward, outBackward);
        }
    }
}

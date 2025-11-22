using EQX.Core.InOut;

namespace EQX.InOut
{
    public class CylinderFactory : ICylinderFactory
    {
        public ICylinder Create(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward, Func<bool>? forwardInterlock, Func<bool>? backwardInterlock)
        {
            return new Cylinder(inForward, inBackward, outForward, outBackward, forwardInterlock, backwardInterlock);
        }
    }
}

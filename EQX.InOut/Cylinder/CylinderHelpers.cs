using EQX.Core.InOut;

namespace EQX.InOut
{
    public static class CylinderHelpers
    {
        public static ICylinder SetIdentity(this ICylinder cylinder, int id, string name)
        {
            ((CylinderBase)cylinder).Id = id;
            ((CylinderBase)cylinder).Name = name;

            return cylinder;
        }
    }
}

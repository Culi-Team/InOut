using EQX.Core.InOut;

namespace EQX.InOut
{
    public static class ConveyorHelpers
    {
        public static IConveyor SetIdentity(this IConveyor conveyor, int id, string name)
        {
            ((ConveyorBase)conveyor).Id = id;
            ((ConveyorBase)conveyor).Name = name;

            return conveyor;
        }
    }
}

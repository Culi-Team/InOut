using EQX.Core.InOut;
using EQX.InOut.Virtual;

namespace EQX.InOut
{
    public static class FlagHelpers
    {
        public static void MapTo(this IDInput input, IDOutput output)
        {
            if (input is DInput dInput && output is DOutput dOutput)
            {
                var inDevice = dInput.GetInputDevice();
                var outDevice = dOutput.GetOutputDevice();

                if (outDevice.GetType().GetGenericTypeDefinition() != typeof(Virtual.MappableOutputDevice<>))
                {
                    throw new InvalidOperationException("Output device is not of type VirtualOutputDevice");
                }

                if (inDevice.GetType().IsGenericType &&
                    inDevice.GetType().GetGenericTypeDefinition() == typeof(MappableInputDevice<>))
                {
                    dynamic device = inDevice;
                    device.Mapping(input.Id, outDevice, dOutput.Id);
                }
                else
                {
                    throw new InvalidOperationException("Input device is not of type VirtualInputDevice");
                }
            }
            else
            {
                throw new InvalidOperationException("Input/Output is not of type DInput/DOutput");
            }
        }
    }
}

using EQX.Core.InOut;
using System.IO.MemoryMappedFiles;

namespace EQX.InOut
{
    public static class SimulationInputSetter
    {
        public static void SetSimInput(IDInput? input, bool value)
        {
            if (input == null) return;

            using (MemoryMappedViewStream stream = _mmf.CreateViewStream(input.Id, 0))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                if (value)
                {
                    writer.Write((char)1);
                }
                else
                {
                    writer.Write((char)0);
                }
            }
        }

        static MemoryMappedFile _mmf = MemoryMappedFile.OpenExisting("SimInputData");
    }
}

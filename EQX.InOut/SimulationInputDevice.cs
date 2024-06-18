using System.IO.MemoryMappedFiles;

namespace EQX.InOut
{
    public class SimulationInputDevice : InputDeviceBase
    {
        MemoryMappedFile mmf;

        public SimulationInputDevice(int id, string name, List<string> inputs)
            : base(id, name, inputs)
        {
            mmf = MemoryMappedFile.CreateNew("SimInputData", 256);
        }

        ~SimulationInputDevice()
        {
            mmf.Dispose();
        }

        protected override bool GetInput(int index)
        {
            byte[] values;

            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            {
                BinaryReader reader = new BinaryReader(stream);
                values = reader.ReadBytes(256);
            }

            return values[index] == 1;
        }
    }
}
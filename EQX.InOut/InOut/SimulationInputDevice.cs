using System.IO.MemoryMappedFiles;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EQX.InOut
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public class SimulationInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        readonly MemoryMappedFile mmf;

        public SimulationInputDevice()
            : base()
        {
            mmf = MemoryMappedFile.CreateOrOpen("SimInputData", 256);
        }

        ~SimulationInputDevice()
        {
            mmf.Dispose();
        }

        protected override bool ActualGetInput(int index)
        {
            byte[] values;

            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            {
                BinaryReader reader = new BinaryReader(stream);
                values = reader.ReadBytes(256);
            }

            return values[_offset + index] == 1;
        }

        public void SetInput(int index, bool value)
        {
            using (MemoryMappedViewStream stream = mmf.CreateViewStream(_offset + index, 0))
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

        private int _offset;
    }
}
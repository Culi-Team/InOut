using System.IO.MemoryMappedFiles;
using System.Reflection;

namespace EQX.InOut
{
    public class SimulationInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        MemoryMappedFile mmf;

        public SimulationInputDevice(int id, string name, int maxPin, int offset = 0)
            : base(id, name, maxPin, offset)
        {
            _offset = offset;

            mmf = MemoryMappedFile.CreateOrOpen("SimInputData", 256);
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
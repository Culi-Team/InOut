using System.IO.MemoryMappedFiles;

namespace EQX.InOut.InOut
{
    public class SimulationInputDevice_ServerMMF<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        MemoryMappedFile? _memoryMapFile;

        public override bool IsConnected => true;
        public SimulationInputDevice_ServerMMF()
        {
            _memoryMapFile = MemoryMappedFile.CreateOrOpen("SimInputData", MaxPin);
        }

        public void SetValue(int index, bool value)
        {
            using (MemoryMappedViewStream stream = _memoryMapFile.CreateViewStream(index, 0))
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

        protected override bool ActualGetInput(int index)
        {
            using (MemoryMappedViewStream stream = _memoryMapFile.CreateViewStream(index, 0))
            {
                BinaryReader reader = new BinaryReader(stream);
                char value = reader.ReadChar();

                return value != 0;
            }
        }

        public void ToggleInput(int index)
        {
            bool currentValue = ActualGetInput(index);
            SetValue(index, !currentValue);
        }
    }
}

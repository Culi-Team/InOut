using System.IO.MemoryMappedFiles;

namespace EQX.InOut
{
    public class SimulationInputDevice_ClientMMF<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        //readonly ModbusTcpClient client;
        MemoryMappedFile? _memoryMapFile;

        public SimulationInputDevice_ClientMMF()
            : base()
        {
        }

        public override bool Connect()
        {
            try
            {
                _memoryMapFile = MemoryMappedFile.OpenExisting("SimInputData");
                IsConnected = true;
            }
            catch
            {
                IsConnected = false;
            }
            return IsConnected;
        }

        public override bool Disconnect()
        {
            _memoryMapFile = null;
            IsConnected = false;
            return true;
        }

        protected override bool ActualGetInput(int index)
        {
            if (_memoryMapFile == null) return false;

            using var stream = _memoryMapFile.CreateViewStream(index, 1);
            int value = stream.ReadByte();
            return value == 1;
        }
    }
}
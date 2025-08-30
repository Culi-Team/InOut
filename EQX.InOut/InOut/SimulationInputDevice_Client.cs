using EQX.Core.Common;
using FluentModbus;
using System.IO.MemoryMappedFiles;
using System.Net;

namespace EQX.InOut
{
    public class SimulationInputDevice_Client<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        MemoryMappedFile? _memoryMapFile;

        public SimulationInputDevice_Client()
            : base()
        {
        }

        ~SimulationInputDevice_Client()
        {
            _memoryMapFile.Dispose();
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
            _memoryMapFile?.Dispose();
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
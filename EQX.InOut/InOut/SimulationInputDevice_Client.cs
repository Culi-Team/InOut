using EQX.Core.Common;
using FluentModbus;
using System.IO.MemoryMappedFiles;
using System.Net;

namespace EQX.InOut
{
    public class SimulationInputDevice_HardwareDevice : IIdentifier
    {
        readonly ModbusTcpServer server;

        public int Id { get; init; }
        public string Name { get; init; }

        public bool this[int index]
        {
            set => SetInput(index, value);
        }

        public SimulationInputDevice_HardwareDevice()
        {
            server = new ModbusTcpServer
            {
                EnableRaisingEvents = true,
            };
        }

        public void Start()
        {
            server.Start(new IPEndPoint(IPAddress.Loopback, 502 + Id));
        }

        ~SimulationInputDevice_HardwareDevice()
        {
            server.Dispose();
        }

        private void SetInput(int index, bool value)
        {
            server.GetCoils().Set(index, value);
        }

        public void ToggleInput(int index)
        {
            server.GetCoils().Toggle(index);
        }
    }

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
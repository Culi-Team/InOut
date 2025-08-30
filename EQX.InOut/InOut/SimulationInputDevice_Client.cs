using EQX.Core.Common;
using FluentModbus;
using System.IO.MemoryMappedFiles;
using System.Net;

namespace EQX.InOut
{
    public class SimulationInputDevice_Client<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        //readonly ModbusTcpClient client;
        MemoryMappedFile? _memoryMapFile;

        public SimulationInputDevice_Client()
            : base()
        {
            //client = new ModbusTcpClient();
        }

        ~SimulationInputDevice_Client()
        {
            //client.Dispose();
            _memoryMapFile.Dispose();
        }

        public override bool Connect()
        {
            try
            {
                //client.Connect(new IPEndPoint(IPAddress.Loopback, 502 + Id));

                //IsConnected = client.IsConnected;
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
            //client.Disconnect();

            //IsConnected = client.IsConnected;
            //return !IsConnected;
            _memoryMapFile?.Dispose();
            _memoryMapFile = null;
            IsConnected = false;
            return true;
        }

        protected override bool ActualGetInput(int index)
        {
            //var response = client.ReadCoils(0, 0, MaxPin);
            if (_memoryMapFile == null) return false;

            //return (response[index / 8] & (0x01 << (index % 8))) != 0;
            using var stream = _memoryMapFile.CreateViewStream(index, 1);
            int value = stream.ReadByte();
            return value == 1;
        }
    }
}
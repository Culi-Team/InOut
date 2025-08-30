using FluentModbus;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static log4net.Appender.FileAppender;

namespace EQX.InOut.InOut
{
    public class SimulationInputDeviceClientModbus<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        readonly ModbusTcpClient _client;
        private readonly object _lock = new object();
        public SimulationInputDeviceClientModbus()
                : base()
        {
            _client = new ModbusTcpClient();
        }

        public override bool IsConnected => _client.IsConnected;

        public override bool Connect()
        {
            try
            {
                _client.Connect();
                return _client.IsConnected;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override bool Disconnect()
        {
            _client.Disconnect();
            return true;
        }

        protected override bool ActualGetInput(int index)
        {
            try
            {
                lock (_lock)
                {
                    int coilIndex = _offset + index;
                    var result = _client.ReadCoils(0, coilIndex, 1);
                    return (result[0] & 0x01) != 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private int _offset;

    }
}

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
    public class SimulationInputDeviceFluentModbus<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        readonly ModbusTcpClient _client;
        private readonly object _lock = new object();
        public SimulationInputDeviceFluentModbus()
    : base()
        {
            _client = new ModbusTcpClient();
            try
            {
                _client.Connect();
            }
            catch (Exception ex)
            {

            }
        }
        ~SimulationInputDeviceFluentModbus()
        {
            _client.Disconnect();
        }
        protected override bool ActualGetInput(int index)
        {
            //lock (_lock)
            //{
            //    var coils = _client.ReadCoils(0, 0, 256);
            //    int i = _offset + index;
            //    return (coils[i / 8] & (1 << i % 8)) != 0;
            //}
            lock (_lock)
            {
                int coilIndex = _offset + index;
                var result = _client.ReadCoils(0, coilIndex, 1);
                return (result[0] & 0x01) != 0;
            }
        }

        private int _offset;

    }
}

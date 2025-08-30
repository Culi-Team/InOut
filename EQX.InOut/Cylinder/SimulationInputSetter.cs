using EQX.Core.InOut;
using FluentModbus;
using System.IO.MemoryMappedFiles;

namespace EQX.InOut
{
    public static class SimulationInputSetter
    {
        public static void SetSimInput(IDInput? input, bool value)
        {
            if (input == null) return;

            using (MemoryMappedViewStream stream = _mmf.CreateViewStream(input.Id, 0))
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

        public static void SetSimModbusInput(IDInput? input, bool value)
        {
            if (input == null) return;
            try
            {
                client.Connect();
                client.WriteSingleCoil(0,input.Id, value);
            }
            catch
            {
            }
        }
        static public readonly ModbusTcpClient client = new ModbusTcpClient();
        static MemoryMappedFile _mmf/* = MemoryMappedFile.OpenExisting("SimInputData")*/;
    }
}

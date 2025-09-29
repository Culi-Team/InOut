using System.IO.MemoryMappedFiles;

namespace EQX.InOut
{
    public class SimulationInputDevice_ClientMMF<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        MemoryMappedFile? _memoryMapFile;
		readonly object _sync = new();

        public SimulationInputDevice_ClientMMF()
            : base()
        {
        }

		public override bool Connect()
        {
            try
            {
				lock (_sync)
				{
					_memoryMapFile = MemoryMappedFile.OpenExisting("SimInputData");
					IsConnected = true;
				}
            }
            catch
            {
                IsConnected = false;
            }
            return IsConnected;
        }

		public override bool Disconnect()
        {
			lock (_sync)
			{
				IsConnected = false;
				_memoryMapFile?.Dispose();
				_memoryMapFile = null;
			}
            return true;
        }

		protected override bool ActualGetInput(int index)
        {
			lock (_sync)
			{
				if (_memoryMapFile == null) return false;

				using var stream = _memoryMapFile.CreateViewStream(index, 1);
				int value = stream.ReadByte();
				return value == 1;
			}
        }
    }
}
using System.IO.MemoryMappedFiles;

namespace EQX.InOut.Virtual
{
    internal static class FlagSharedMemory
    {
        public static FlagInputMemoryBlock CreateInputBlock(string key, int capacity)
        {
            return new FlagInputMemoryBlock(BuildMapName("Input", key), capacity);
        }

        public static FlagOutputMemoryBlock CreateOutputBlock(string key, int capacity)
        {
            return new FlagOutputMemoryBlock(BuildMapName("Output", key), capacity);
        }

        private static string BuildMapName(string prefix, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key must not be null or whitespace", nameof(key));
            }

            var sanitized = key.Replace('\\', '_').Replace('/', '_').Replace(':', '_');
            return $"Local\\PIFilmFlag_{prefix}_{sanitized}";
        }
    }

    internal sealed class FlagInputMemoryBlock : IDisposable
    {
        private const int EntrySize = 3;
        private readonly MemoryMappedFile _memory;
        private readonly MemoryMappedViewAccessor _accessor;
        private readonly Mutex _mutex;
        private readonly int _capacity;

        public FlagInputMemoryBlock(string mapName, int capacity)
        {
            _capacity = Math.Max(1, capacity);
            long mapSize = (long)_capacity * EntrySize;
            _memory = MemoryMappedFile.CreateOrOpen(mapName, mapSize, MemoryMappedFileAccess.ReadWrite);
            _accessor = _memory.CreateViewAccessor(0, mapSize, MemoryMappedFileAccess.ReadWrite);
            _mutex = new Mutex(false, mapName + "_Mutex");
        }

        public bool HasManual(int index)
        {
            return ReadManual(index).hasManual;
        }

        public bool TryGetManual(int index, out bool manualValue)
        {
            var (hasManual, manual) = ReadManual(index);
            manualValue = manual;
            return hasManual;
        }

        public bool GetEffectiveValue(int index)
        {
            int normalized = Normalize(index);
            _mutex.WaitOne();
            try
            {
                long offset = normalized * EntrySize;
                byte manualFlag = _accessor.ReadByte(offset);
                if (manualFlag != 0)
                {
                    return _accessor.ReadByte(offset + 1) != 0;
                }

                return _accessor.ReadByte(offset + 2) != 0;
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public bool GetAutoValue(int index)
        {
            int normalized = Normalize(index);
            _mutex.WaitOne();
            try
            {
                long offset = normalized * EntrySize;
                return _accessor.ReadByte(offset + 2) != 0;
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void SetManual(int index, bool? manualValue)
        {
            int normalized = Normalize(index);
            _mutex.WaitOne();
            try
            {
                long offset = normalized * EntrySize;
                if (manualValue.HasValue)
                {
                    byte value = manualValue.Value ? (byte)1 : (byte)0;
                    _accessor.Write(offset, (byte)1);
                    _accessor.Write(offset + 1, value);
                    _accessor.Write(offset + 2, value);
                }
                else
                {
                    _accessor.Write(offset, (byte)0);
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void ClearManuals()
        {
            _mutex.WaitOne();
            try
            {
                for (int i = 0; i < _capacity; i++)
                {
                    long offset = i * EntrySize;
                    _accessor.Write(offset, (byte)0);
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void SetAutoValue(int index, bool value)
        {
            int normalized = Normalize(index);
            _mutex.WaitOne();
            try
            {
                long offset = normalized * EntrySize;
                _accessor.Write(offset + 2, value ? (byte)1 : (byte)0);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        private (bool hasManual, bool manualValue) ReadManual(int index)
        {
            int normalized = Normalize(index);
            _mutex.WaitOne();
            try
            {
                long offset = normalized * EntrySize;
                byte manualFlag = _accessor.ReadByte(offset);
                bool manualValue = _accessor.ReadByte(offset + 1) != 0;
                return (manualFlag != 0, manualValue);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        private int Normalize(int index)
        {
            if (_capacity == 0)
            {
                return 0;
            }

            int normalized = index % _capacity;
            if (normalized < 0)
            {
                normalized += _capacity;
            }

            return normalized;
        }

        public void Dispose()
        {
            _accessor.Dispose();
            _memory.Dispose();
            _mutex.Dispose();
        }
    }

    internal sealed class FlagOutputMemoryBlock : IDisposable
    {
        private const int EntrySize = 1;
        private readonly MemoryMappedFile _memory;
        private readonly MemoryMappedViewAccessor _accessor;
        private readonly Mutex _mutex;
        private readonly int _capacity;

        public FlagOutputMemoryBlock(string mapName, int capacity)
        {
            _capacity = Math.Max(1, capacity);
            long mapSize = (long)_capacity * EntrySize;
            _memory = MemoryMappedFile.CreateOrOpen(mapName, mapSize, MemoryMappedFileAccess.ReadWrite);
            _accessor = _memory.CreateViewAccessor(0, mapSize, MemoryMappedFileAccess.ReadWrite);
            _mutex = new Mutex(false, mapName + "_Mutex");
        }

        public void SetValue(int index, bool value)
        {
            int normalized = Normalize(index);
            _mutex.WaitOne();
            try
            {
                long offset = normalized * EntrySize;
                _accessor.Write(offset, value ? (byte)1 : (byte)0);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public bool GetValue(int index)
        {
            int normalized = Normalize(index);
            _mutex.WaitOne();
            try
            {
                long offset = normalized * EntrySize;
                return _accessor.ReadByte(offset) != 0;
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void Clear()
        {
            _mutex.WaitOne();
            try
            {
                for (int i = 0; i < _capacity; i++)
                {
                    long offset = i * EntrySize;
                    _accessor.Write(offset, (byte)0);
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        private int Normalize(int index)
        {
            if (_capacity == 0)
            {
                return 0;
            }

            int normalized = index % _capacity;
            if (normalized < 0)
            {
                normalized += _capacity;
            }

            return normalized;
        }

        public void Dispose()
        {
            _accessor.Dispose();
            _memory.Dispose();
            _mutex.Dispose();
        }
    }
}

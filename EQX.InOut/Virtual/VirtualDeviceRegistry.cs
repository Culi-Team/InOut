using EQX.Core.InOut;

namespace EQX.InOut.Virtual
{
    public static class VirtualDeviceRegistry
    {
        public static VirtualInputDevice<TEnum> GetOrAddInputDevice<TEnum>(string key)
            where TEnum : Enum
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (_syncLock)
            {
                if (_inputDevices.TryGetValue(key, out var existing))
                {
                    if (existing is VirtualInputDevice<TEnum> typed)
                    {
                        return typed;
                    }

                    throw new InvalidOperationException($"Input device with key '{key}' has already been registered with type '{existing.GetType().FullName}'.");
                }

                var device = CreateInputDevice<TEnum>(key);
                _inputDevices.Add(key, device);
                return device;
            }
        }

        public static VirtualOutputDevice<TEnum> GetOrAddOutputDevice<TEnum>(string key)
            where TEnum : Enum
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (_syncLock)
            {
                if (_outputDevices.TryGetValue(key, out var existing))
                {
                    if (existing is VirtualOutputDevice<TEnum> typed)
                    {
                        return typed;
                    }

                    throw new InvalidOperationException($"Output device with key '{key}' has already been registered with type '{existing.GetType().FullName}'.");
                }

                var device = CreateOutputDevice<TEnum>(key);
                _outputDevices.Add(key, device);
                return device;
            }
        }
        private static VirtualInputDevice<TEnum> CreateInputDevice<TEnum>(string key)
            where TEnum : Enum
        {
            var device = new VirtualInputDevice<TEnum>
            {
                Id = _nextDeviceId++,
                Name = key,
                MaxPin = Enum.GetValues(typeof(TEnum)).Length
            };


            device.BindToSharedMemory(key);
            return device;
        }

        private static VirtualOutputDevice<TEnum> CreateOutputDevice<TEnum>(string key)
            where TEnum : Enum
        {
            var device = new VirtualOutputDevice<TEnum>
            {
                Id = _nextDeviceId++,
                Name = key,
                MaxPin = Enum.GetValues(typeof(TEnum)).Length
            };

            device.BindToSharedMemory(key);
            return device;
        }
        private static readonly object _syncLock = new();
        private static readonly Dictionary<string, IDInputDevice> _inputDevices = new(StringComparer.Ordinal);
        private static readonly Dictionary<string, IDOutputDevice> _outputDevices = new(StringComparer.Ordinal);
        private static int _nextDeviceId = 1;
    }
}

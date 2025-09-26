using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.Virtual
{
    public class VirtualInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        private readonly Dictionary<int, (IDOutputDevice outputDevice, int outputPin)> _mappings = new();
        private readonly Dictionary<int, bool> _manualOverrides = new();
        private FlagInputMemoryBlock? _sharedMemory;
        //private Dictionary<int, (IDOutputDevice outputDevice, int outputPin)> _mappings = new();
        public VirtualInputDevice() : base()
        {
            IsConnected = true;
        }

        internal void BindToSharedMemory(string key)
        {
            try
            {
                _sharedMemory = FlagSharedMemory.CreateInputBlock(key, MaxPin);
            }
            catch
            {
                _sharedMemory = null;
            }
        }

        public void Mapping(int inputPin, IDOutputDevice outputDevice, int outputPin)
        {
#if !SIMULATION
            _mappings.Add(inputPin, (outputDevice, outputPin));
#else
            if (outputDevice == null)
            {
                throw new ArgumentNullException(nameof(outputDevice));
            }
            int normalizedIndex = Normalize(inputPin);
            _mappings[normalizedIndex] = (outputDevice, outputPin);

            UpdateAutoValue(normalizedIndex, outputDevice[outputPin]);

            if (outputDevice is IVirtualOutputPublisher publisher)
            {
                publisher.RegisterSubscriber(outputPin, value => UpdateAutoValue(normalizedIndex, value));
            }
#endif
        }
#if SIMULATION
        public void SetManualInput(int index, bool value)
        {
            int normalizedIndex = Normalize(index);
            _manualOverrides[normalizedIndex] = value;
            _sharedMemory?.SetManual(normalizedIndex, null);
        }

        public void ClearManualInput(int index)
        {
            int normalizedIndex = index % MaxPin;
            _manualOverrides.Remove(normalizedIndex);
        }

        public void ClearManualInputs()
        {
            _manualOverrides.Clear();
            _sharedMemory?.ClearManuals();
        }

        public bool HasManualInput(int index)
        {
            int normalizedIndex = Normalize(index);

            if (_sharedMemory != null)
            {
                return _sharedMemory.HasManual(normalizedIndex);
            }
            return _manualOverrides.ContainsKey(normalizedIndex);
        }
#endif


        protected override bool ActualGetInput(int index)
        {
#if !SIMULATION
            return _mappings.ContainsKey(index) && _mappings[index].outputDevice[_mappings[index].outputPin];
#else
            int normalizedIndex = Normalize(index);

            if (_sharedMemory != null)
            {
                return _sharedMemory.GetEffectiveValue(normalizedIndex);
            }

            if (_manualOverrides.TryGetValue(normalizedIndex, out bool manualValue))
            {
                return manualValue;
            }

            if (_mappings.TryGetValue(normalizedIndex, out var mapping))
            {
                return mapping.outputDevice[mapping.outputPin];
            }

            return false;
#endif

        }

#if SIMULATION
        private void UpdateAutoValue(int index, bool value)
        {
            _sharedMemory?.SetAutoValue(index, value);
        }

        private int Normalize(int index)
        {
            if (MaxPin <= 0)
            {
                return 0;
            }

            int normalized = index % MaxPin;
            if (normalized < 0)
            {
                normalized += MaxPin;
            }

            return normalized;
        }
#endif

    }
}


namespace EQX.InOut.Virtual
{
    public class VirtualOutputDevice<TEnum> : OutputDeviceBase<TEnum>, IVirtualOutputPublisher where TEnum : Enum
    {
        private readonly bool[] _outputs;
        private readonly Dictionary<int, List<Action<bool>>> _subscribers;
        private readonly object _syncRoot = new();
        private FlagOutputMemoryBlock? _sharedMemory;
        public VirtualOutputDevice() : base()
        {
            var outputList = Enum.GetNames(typeof(TEnum)).ToList();

            _outputs = new bool[outputList.Count];
#if SIMULATION
            _subscribers = new Dictionary<int, List<Action<bool>>>();
#endif
        }
        internal void BindToSharedMemory(string key)
        {
            try
            {
                _sharedMemory = FlagSharedMemory.CreateOutputBlock(key, MaxPin);
                for (int i = 0; i < _outputs.Length; i++)
                {
                    _outputs[i] = _sharedMemory.GetValue(i);
                }
            }
            catch
            {
                _sharedMemory = null;
            }
        }

        protected override bool GetOutput(int index)
        {
#if !SIMULATION
            return _outputs[index];
#else
            int normalizedIndex = Normalize(index);

            if (_sharedMemory != null)
            {
                return _sharedMemory.GetValue(normalizedIndex);
            }

            return _outputs[normalizedIndex];
#endif
        }

        protected override bool SetOutput(int index, bool value)
        {
#if !SIMULATION
            _outputs[index] = value;
#else
            int normalizedIndex = Normalize(index);
            _outputs[normalizedIndex] = value;
            _sharedMemory?.SetValue(normalizedIndex, value);
            NotifySubscribers(normalizedIndex, value);
#endif
            return true;
        }

        public void Clear()
        {
            var outputList = Enum.GetValues(typeof(TEnum));
            foreach (var output in outputList)
            {
                _outputs[(int)output] = false;
            }
            _sharedMemory?.Clear();

            for (int i = 0; i < _outputs.Length; i++)
            {
                _outputs[i] = false;
                NotifySubscribers(i, false);
            }
        }
        void IVirtualOutputPublisher.RegisterSubscriber(int outputPin, Action<bool> subscriber)
        {
            if (subscriber == null)
            {
                return;
            }

            int normalizedPin = Normalize(outputPin);
            lock (_syncRoot)
            {
                if (!_subscribers.TryGetValue(normalizedPin, out var list))
                {
                    list = new List<Action<bool>>();
                    _subscribers.Add(normalizedPin, list);
                }

                list.Add(subscriber);
            }

            subscriber(GetOutput(normalizedPin));
        }

        private void NotifySubscribers(int outputPin, bool value)
        {
            List<Action<bool>>? callbacks = null;
            lock (_syncRoot)
            {
                if (_subscribers.TryGetValue(outputPin, out var list) && list.Count > 0)
                {
                    callbacks = new List<Action<bool>>(list);
                }
            }

            if (callbacks == null)
            {
                return;
            }

            foreach (var callback in callbacks)
            {
                callback(value);
            }
        }

        private int Normalize(int index)
        {
            if (_outputs.Length == 0)
            {
                return 0;
            }

            int normalized = index % _outputs.Length;
            if (normalized < 0)
            {
                normalized += _outputs.Length;
            }

            return normalized;
        }
    }
}

namespace EQX.InOut.Virtual
{
    public class VirtualOutputDevice<TEnum> : OutputDeviceBase<TEnum> where TEnum : Enum
    {
        private readonly bool[] _outputs;
        private FlagOutputMemoryBlock? _sharedMemory;
        public VirtualOutputDevice() : base()
        {
            var outputList = Enum.GetNames(typeof(TEnum)).ToList();

            _outputs = new bool[outputList.Count];
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
            return _outputs[index];
        }

        protected override bool SetOutput(int index, bool value)
        {
            _outputs[index] = value;
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
            }
        }
    }
}

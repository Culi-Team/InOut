namespace EQX.InOut.Virtual
{
    public class VirtualOutputDevice<TEnum> : OutputDeviceBase<TEnum> where TEnum : Enum
    {
        private readonly bool[] _outputs;
        public VirtualOutputDevice() : base()
        {
            var outputList = Enum.GetNames(typeof(TEnum)).ToList();

            _outputs = new bool[outputList.Count];
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

            for (int i = 0; i < _outputs.Length; i++)
            {
                _outputs[i] = false;
            }
        }
    }
}

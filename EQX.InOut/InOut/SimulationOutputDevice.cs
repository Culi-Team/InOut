namespace EQX.InOut
{
    public class SimulationOutputDevice<TEnum> : OutputDeviceBase<TEnum> where TEnum : Enum
    {
        public SimulationOutputDevice()
            : base()
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

        private readonly bool[] _outputs;
    }
}
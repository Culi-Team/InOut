namespace EQX.InOut
{
    public class SimulationOutputDevice<TEnum> : OutputDeviceBase<TEnum>
    {
        public SimulationOutputDevice(int id, string name, int offset = 0, int count = -1)
            : base(id, name, offset, count)
        {
            _outputs = new bool[256];
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
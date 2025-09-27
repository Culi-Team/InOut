namespace EQX.InOut.InOut.Analog
{
    public class SimulationAnalogInputDevice<TEnum> : AnalogInputDeviceBase<TEnum> where TEnum : Enum
    {
        public override bool IsConnected { get; protected set; }

        public SimulationAnalogInputDevice()
            : base()
        {
        }

        public override bool Connect()
        {
            IsConnected = true;
            return IsConnected;
        }

        public override bool Disconnect()
        {
            IsConnected = false;
            return true;
        }

        public override double GetVolt(int channel)
        {
            Random rand = new Random();
            return Math.Round(rand.NextDouble() * 10.0, 2); 
        }
        public override double GetCurrent(int channel)
        {
            Random rand = new Random();
            return Math.Round(rand.NextDouble() * 20.0, 2); 
        }
    }
}

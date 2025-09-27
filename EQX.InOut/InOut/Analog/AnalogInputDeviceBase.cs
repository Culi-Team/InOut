using EQX.Core.InOut;

namespace EQX.InOut.InOut.Analog
{
    public class AnalogInputDeviceBase<TEnum> : IAInputDevice where TEnum : Enum
    {
        public List<IAInput> AnalogInputs { get; }

        public int Id { get; init; }

        public string Name { get; init; }

        public virtual bool IsConnected { get; protected set; }

        public AnalogInputDeviceBase()
        {
            Name ??= GetType().Name;
            AnalogInputs = new List<IAInput>();
        }

        public virtual bool Connect()
        {
            return true;
        }

        public virtual bool Disconnect()
        {
            return true;
        }

        public bool Initialize()
        {
            var inputList = Enum.GetNames(typeof(TEnum)).ToList();
            var inputIndex = (int[])Enum.GetValues(typeof(TEnum));
            for (int i = 0; i < inputList.Count; i++)
            {
                AnalogInputs.Add(new AInput(inputIndex[i], inputList[i], this));
            }

            return true;
        }

        public virtual double GetVolt(int channel)
        {
            return 0.0;
        }

        public virtual double GetCurrent(int channel)
        {
            return 0.0;
        }
    }
}

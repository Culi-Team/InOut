using EQX.Core.InOut;

namespace EQX.InOut
{
    public class InputDeviceBase<TEnum> : IDInputDevice where TEnum : Enum
    {
        #region Properties
        public List<IDInput> Inputs { get; }
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual bool IsConnected { get; protected set; }

        public bool this[int index] => GetInput(index % _maxPin);
        #endregion

        #region Constructor(s)
        public InputDeviceBase(int id, string name, int maxPin, int offset = 0)
        {
            Id = id;
            Name = name;

            _maxPin = maxPin;

            var inputList = Enum.GetNames(typeof(TEnum)).ToList();
            var inputIndex = (int[])Enum.GetValues(typeof(TEnum));

            if (offset + maxPin > inputList.Count) throw new ArgumentOutOfRangeException();

            Inputs = new List<IDInput>();
            for (int i = offset; i < offset + maxPin; i++)
            {
                Inputs.Add(new DInput(inputIndex[i], inputList[i], this));
            }
        }
        #endregion

        #region Public methods
        public virtual bool Connect()
        {
            return true;
        }

        public virtual bool Disconnect()
        {
            return true;
        }
        #endregion

        protected virtual bool ActualGetInput(int index)
        {
            return true;
        }

        private bool GetInput(int index)
        {
            bool newValue = ActualGetInput(index);
            return newValue;
        }

        #region Privates
        private int _maxPin;
        #endregion
    }
}
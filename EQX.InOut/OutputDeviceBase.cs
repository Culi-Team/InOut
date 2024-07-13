using EQX.Core.InOut;
using EQX.InOut;

namespace EQX.InOut
{
    public class OutputDeviceBase<TEnum> : IDOutputDevice
    {
        #region Properties
        public List<IDOutput> Outputs { get; }
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual bool IsConnected { get; protected set; }
        public bool this[int index]
        {
            get => GetOutput(index % _maxPin);
            set => SetOutput(index % _maxPin, value);
        }
        #endregion

        #region Constructor(s)
        public OutputDeviceBase(int id, string name, int maxPin, int offset = 0)
        {
            Id = id;
            Name = name;

            _maxPin = maxPin;

            var outputList = Enum.GetNames(typeof(TEnum)).ToList();
            var outputIndex = (int[])Enum.GetValues(typeof(TEnum));

            Outputs = new List<IDOutput>();
            for (int i = offset; i < offset + maxPin; i++)
            {
                Outputs.Add(new DOutput(outputIndex[i], outputList[i], this));
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

        protected virtual bool GetOutput(int index)
        {
            return true;
        }

        protected virtual bool SetOutput(int index, bool value)
        {
            return true;
        }

        #region Privates
        private int _maxPin;
        #endregion
    }
}
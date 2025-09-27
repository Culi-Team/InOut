using EQX.Core.InOut;

namespace EQX.InOut
{
    public class OutputDeviceBase<TEnum> : IDOutputDevice where TEnum : Enum
    {
        #region Properties
        public List<IDOutput> Outputs { get; private set; }
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual bool IsConnected { get; protected set; }

        public bool this[int index]
        {
            get => GetOutput(index % MaxPin);
            set => SetOutput(index % MaxPin, value);
        }

        public int MaxPin { get; init; } = 32;
        #endregion

        #region Constructor(s)
        public OutputDeviceBase()
        {
            Name ??= GetType().Name;
            Outputs = new List<IDOutput>();
        }
        #endregion

        #region Public methods
        public bool Initialize()
        {
            var outputList = Enum.GetNames(typeof(TEnum)).ToList();
            var outputIndex = (int[])Enum.GetValues(typeof(TEnum));

            for (int i = 0; i < MaxPin; i++)
            {
                if (i >= outputList.Count) break;

                Outputs.Add(new DOutput(outputIndex[i], outputList[i], this));
            }

            return true;
        }

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
    }
}
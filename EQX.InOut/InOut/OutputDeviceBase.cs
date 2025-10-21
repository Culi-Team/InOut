using EQX.Core.InOut;
using EQX.InOut.InOut.Analog;

namespace EQX.InOut
{
    public class OutputDeviceBase<TEnum> : IDOutputDevice where TEnum : Enum
    {
        #region Properties
        public List<IDOutput> Outputs { get; private set; }
        private List<bool> _outputs;
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual bool IsConnected { get; protected set; }

        public bool this[int index]
        {
            get => GetOutput(index % MaxPin);
            set => SetOutput(index % MaxPin, value);
        }

        public IDOutput this[Enum key]
        {
            get
            {
                if (typeof(TEnum) != key.GetType())
                {
                    throw new ArgumentException($"Key type must be of type {typeof(TEnum).Name}");
                }

                return Outputs.First(i => i.Id == Convert.ToInt32(key));
            }
        }

        public int MaxPin { get; init; } = 32;
        #endregion

        #region Constructor(s)
        public OutputDeviceBase()
        {
            Name ??= GetType().Name;
            Outputs = new List<IDOutput>();
            _outputs = new List<bool>(new bool[MaxPin]);
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

        public virtual void ClearOutputs()
        {
            for (int i = 0; i < Outputs.Count; i++)
            {
                this[i] = false;
            }
        }

        public virtual bool Disconnect()
        {
            return true;
        }
        #endregion

        protected virtual bool GetOutput(int index)
        {
            return _outputs[index];
        }

        protected virtual bool SetOutput(int index, bool value)
        {
            _outputs[index] = value;
            return _outputs[index] == value;
        }
    }
}
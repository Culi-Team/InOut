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
        public bool this[int index] => GetInput(index);
        #endregion

        #region Constructor(s)
        public InputDeviceBase(int id, string name, int offset = 0, int count = -1)
        {
            Id = id;
            Name = name;

            var inputList = Enum.GetNames(typeof(TEnum)).ToList();
            var inputIndex = (int[])Enum.GetValues(typeof(TEnum));

            if (count == -1) count = inputList.Count;
            if (offset + count > inputList.Count) throw new ArgumentOutOfRangeException();

            Inputs = new List<IDInput>();
            for (int i = offset; i < offset + count; i++)
            {
                Inputs.Add(new DInput(inputIndex[i] % count, inputList[i], this));
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

        protected virtual bool GetInput(int index)
        {
            return true;
        }

        #region Privates
        #endregion
    }
}
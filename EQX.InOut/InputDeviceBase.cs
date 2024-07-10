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
        public InputDeviceBase(int id, string name)
        {
            Id = id;
            Name = name;

            var inputList = Enum.GetNames(typeof(TEnum)).ToList();
            var inputIndex = (int[])Enum.GetValues(typeof(TEnum));

            Inputs = new List<IDInput>();
            for (int i = 0; i< inputList.Count; i++)
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

        protected virtual bool GetInput(int index)
        {
            return true;
        }

        #region Privates
        #endregion
    }
}
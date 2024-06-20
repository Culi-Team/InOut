using EQX.Core.InOut;

namespace EQX.InOut
{
    public class InputDeviceBase : IDInputDevice
    {
        #region Properties
        public List<IDInput> Inputs { get; }
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual bool IsConnected { get; protected set; }
        public bool this[int index] => GetInput(index);
        #endregion

        #region Constructor(s)
        public InputDeviceBase(int id, string name, List<string> inputs, List<int> indexes)
        {
            Id = id;
            Name = name;
            _inputs = inputs;

            Inputs = new List<IDInput>();
            for (int i = 0; i< _inputs.Count; i++)
            {
                Inputs.Add(new DInput(indexes[i], _inputs[i], this));
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
        private readonly List<string> _inputs;
        #endregion
    }
}
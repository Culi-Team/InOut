using EQX.Core.InOut;

namespace EQX.InOut
{
    public class OutputDeviceBase : IDOutputDevice
    {
        #region Properties
        public List<IDOutput> Outputs { get; }
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual bool IsConnected { get; }
        public bool this[int index]
        {
            get => GetOutput(index);
            set => SetOutput(index, value);
        }
        #endregion

        #region Constructor(s)
        public OutputDeviceBase(int id, string name, List<string> outputs)
        {
            Id = id;
            Name = name;
            _outputs = outputs;

            Outputs = new List<IDOutput>();
            for (int i = 0; i < _outputs.Count; i++)
            {
                Outputs.Add(new DOutput(i, _outputs[i], this));
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
        private readonly List<string> _outputs;
        #endregion
    }
}
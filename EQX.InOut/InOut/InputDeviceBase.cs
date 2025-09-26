using EQX.Core.InOut;
using EQX.InOut.InOut;
using EQX.InOut.Virtual;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EQX.InOut
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Passing the <typeparamref name="TEnum"/> enum as IO List for the Input Device
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class InputDeviceBase<TEnum> : IDInputDevice where TEnum : Enum
    {
        #region Properties
        public List<IDInput> Inputs { get; private set; }
        public int Id { get; init; }
        public string Name { get; init; }
        public virtual bool IsConnected { get; protected set; }

        public bool this[int index] => GetInput(index % MaxPin);

        public int MaxPin { get; init; } = 32;
        #endregion

        #region Constructor(s)
        public InputDeviceBase()
        {
            Name ??= GetType().Name;
            Inputs = new List<IDInput>();
        }
        #endregion

        #region Public methods
        public bool Initialize()
        {
            var inputList = Enum.GetNames(typeof(TEnum)).ToList();
            var inputIndex = (int[])Enum.GetValues(typeof(TEnum));

            for (int i = 0; i < MaxPin; i++)
            {
                if (i >= inputList.Count) break;

                Inputs.Add(new DInput(inputIndex[i], inputList[i], this));
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

        protected virtual bool ActualGetInput(int index)
        {
            return true;
        }

        private bool GetInput(int index)
        {
            if (IsConnected == false) return false;

            return ActualGetInput(index);
        }
    }
}
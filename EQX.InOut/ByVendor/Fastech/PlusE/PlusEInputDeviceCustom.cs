using EQX.ThirdParty.Fastech;

namespace EQX.InOut
{
    public class PlusEInputDeviceCustom<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        #region Constructor(s)
        public PlusEInputDeviceCustom()
            : base()
        {
            nativeLib = new EziPlusEDIOLib(Id, Name);
        }
        #endregion

        #region Public methods
        public override bool Connect()
        {
            bool result = nativeLib.Connect();
            IsConnected = result;
            return result;
        }

        public override bool Disconnect()
        {
            bool result = nativeLib.Disconnect();
            IsConnected = false;
            return result;
        }
        #endregion

        #region Private methods
        protected override bool ActualGetInput(int index)
        {
            uint inputStatus = 0, latchStatus = 0;
            int result = nativeLib.GetInput(ref inputStatus, ref latchStatus);
            if (result == EziPlusEDIOLib.FMM_OK)
            {
                return (inputStatus & (0x01 << index)) > 0;
            }

            return false;
        }
        #endregion

        #region Privates
        private readonly EziPlusEDIOLib nativeLib;
        #endregion
    }
}
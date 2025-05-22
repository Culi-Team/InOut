#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EQX.InOut
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public class AjinInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        #region Properties
        public override bool IsConnected => AXL.AxlIsOpened() == 0x01;
        #endregion

        #region Constructor(s)
        #endregion

        #region Public methods
        public override bool Connect()
        {
            if (IsConnected) return true;

            if (AXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return false;

            return true;
        }

        public override bool Disconnect()
        {
            return AXL.AxlClose() == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }
        #endregion

        #region Private methods
        protected override bool ActualGetInput(int index)
        {
            _ = CAXD.AxdiReadInport(index, ref oldValue);

            return oldValue == 1;
        }
        #endregion

        #region Privates
        private uint oldValue = 0;
        #endregion
    }
}

namespace EQX.InOut
{
    public class AjinOutputDevice<TEnum> : OutputDeviceBase<TEnum> where TEnum : Enum
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
            AXL.AxlClose();

            return true;
        }
        #endregion

        #region Private methods
        protected override bool SetOutput(int index, bool value)
        {
            return CAXD.AxdoWriteOutport(index, value ? 1u : 0u) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }

        protected override bool GetOutput(int index)
        {
            CAXD.AxdoReadOutport(index, ref oldValue);

            return oldValue == 1;
        }
        #endregion

        #region Privates
        private uint oldValue = 0;
        #endregion
    }
}

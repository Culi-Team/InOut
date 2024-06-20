using EQX.Core.InOut;

namespace EQX.InOut
{
    public class AjinOutputDevice : OutputDeviceBase
    {
        #region Properties
        public override bool IsConnected => CAXL.AxlIsOpened() == 0x01;
        #endregion

        #region Constructor(s)
        public AjinOutputDevice(int id, string name, List<string> inputs, List<int> indexes)
            : base(id, name, inputs, indexes)
        {
        }
        #endregion

        #region Public methods
        public override bool Connect()
        {
            if (IsConnected) return true;

            if (CAXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return false;

            return true;
        }

        public override bool Disconnect()
        {
            CAXL.AxlClose();

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

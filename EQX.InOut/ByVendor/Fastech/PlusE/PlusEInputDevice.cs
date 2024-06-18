using EQX.Motion.ThirdParty.Fastech.PlusE;
using System.Threading;

namespace EQX.InOut
{
    public class PlusEInputDevice : InputDeviceBase
    {
        public override bool IsConnected => CAXL.AxlIsOpened() == (int)AXT_FUNC_RESULT.AXT_RT_SUCCESS;

        #region Constructor(s)
        public PlusEInputDevice(int id, string name, List<string> inputs)
            : base(id, name, inputs)
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
        protected override bool GetInput(int index)
        {
            return oldValue == 1;
        }
        #endregion

        #region Privates
        private uint oldValue = 0;
        #endregion
    }
}
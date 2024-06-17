using EQX.Core.InOut;
using EQX.Motion.ThirdParty.Fastech.PlusE;

namespace EQX.InOut
{
    public class PlusEInputDevice : IDInputDevice
    {
        #region Properties
        public int Id { get; init; }
        public string Name { get; init; }
        public bool IsConnected => CAXL.AxlIsOpened() == (int)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        #endregion

        public bool this[int index] => GetInput(index);

        #region Public methods
        public bool Connect()
        {
            if (IsConnected) return true;

            if (CAXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return false;

            return true;
        }

        public bool Disconnect()
        {
            CAXL.AxlClose();

            return true;
        }
        #endregion

        #region Private methods
        private bool GetInput(int index)
        {
            return oldValue == 1;
        }
        #endregion

        #region Privates
        private uint oldValue = 0;
        private EziPlusEMotionLib nativeLib;
        #endregion
    }
}

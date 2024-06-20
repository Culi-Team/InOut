using EQX.Motion.ThirdParty.Fastech.PlusE;
using System.Threading;

namespace EQX.InOut
{
    public class PlusEInputDevice : InputDeviceBase
    {
        #region Constructor(s)
        public PlusEInputDevice(int id, string name, List<string> inputs, List<int> indexes)
            : base(id, name, inputs, indexes)
        {
            nativeLib = new EziPlusEDIOLib(id, name);
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
        protected override bool GetInput(int index)
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
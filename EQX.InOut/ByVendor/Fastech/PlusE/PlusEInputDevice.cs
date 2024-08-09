using EQX.ThirdParty.Fastech;
using System.Net;
using NativeLib = FASTECH.EziMOTIONPlusELib;

namespace EQX.InOut
{
    public class PlusEInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        #region Constructor(s)
        public PlusEInputDevice(int id, string name, int maxPin, int offset = 0)
            : base(id, name, maxPin, offset)
        {
            iPAddress = IPAddress.Parse($"192.168.0.{id}");
        }
        #endregion

        #region Public methods
        public override bool Connect()
        {
            bool result = NativeLib.FAS_Connect(iPAddress, Id);
            IsConnected = result;
            return result;
        }

        public override bool Disconnect()
        {
            NativeLib.FAS_Close(Id);
            IsConnected = false;
            return true;
        }
        #endregion

        #region Private methods
        protected override bool ActualGetInput(int index)
        {
            uint inputStatus = 0, latchStatus = 0;
            int result = NativeLib.FAS_GetInput(Id, ref inputStatus, ref latchStatus);
            if (result == EziPlusEDIOLib.FMM_OK)
            {
                return (inputStatus & (0x01 << index)) > 0;
            }

            return false;
        }
        #endregion

        #region Privates
        private IPAddress iPAddress;
        #endregion
    }
}
using EQX.ThirdParty.Fastech;
using System.Net;
using NativeLib = FASTECH.EziMOTIONPlusELib;

namespace EQX.InOut
{
    public class PlusEOutputDevice<TEnum> : OutputDeviceBase<TEnum>
    {
        public PlusEOutputDevice()
            : base()
        {
            iPAddress = IPAddress.Parse($"192.168.0.{Id}");
        }

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

        protected override bool SetOutput(int index, bool value)
        {
            uint setOutputData = 0x00, resetOutputData = 0x00;

            if (value) setOutputData = ((uint)(0x01 << index));
            else resetOutputData = ((uint)(0x01 << index));

            NativeLib.FAS_SetOutput(Id, setOutputData, resetOutputData);

            return true;
        }

        protected override bool GetOutput(int index)
        {
            uint uOutput = 0;
            uint uStatus = 0;
            int result = NativeLib.FAS_GetOutput(Id, ref uOutput, ref uStatus);

            if (result == EziPlusEDIOLib.FMM_OK)
            {
                return (uOutput & (0x01ul << index)) > 0;
            }

            return false;
        }

        private IPAddress iPAddress;
    }
}
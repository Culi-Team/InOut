using EQX.ThirdParty.Fastech;

namespace EQX.InOut
{
    public class PlusEOutputDevice<TEnum> : OutputDeviceBase<TEnum>
    {
        public PlusEOutputDevice(int id, string name)
            : base(id, name)
        {
            nativeLib = new EziPlusEDIOLib(id, name);
        }

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

        protected override bool SetOutput(int index, bool value)
        {
            uint setOutputData = 0x00, resetOutputData = 0x00;

            if (value) setOutputData = ((uint)(0x01 << index));
            else resetOutputData = ((uint)(0x01 << index));

            nativeLib.SetOutput(setOutputData, resetOutputData);

            return true;
        }

        protected override bool GetOutput(int index)
        {
            ulong outputStatus = 0;
            int result = nativeLib.GetOutput(ref outputStatus);

            if (result == EziPlusEDIOLib.FMM_OK)
            {
                return (outputStatus & (0x01ul << index)) > 0;
            }

            return false;
        }

        private readonly EziPlusEDIOLib nativeLib;
    }
}
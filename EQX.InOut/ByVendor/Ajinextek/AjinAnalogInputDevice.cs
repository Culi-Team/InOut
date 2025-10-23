using EQX.InOut.InOut.Analog;

namespace EQX.InOut.ByVendor.Ajinextek
{
    public class AjinAnalogInputDevice<TEnum> : AnalogInputDeviceBase<TEnum> where TEnum : Enum
    {
        #region Properties
        public override bool IsConnected => AXL.AxlIsOpened() == 0x01;
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

        protected override void ExtendInit()
        {
            //foreach (var input in AnalogInputs)
            //{
            //    AXA.AxaiEventSetChannelEnable((ushort)input.Id, 1);
            //}
        }

        public override double GetVolt(int channel)
        {
            bool ret = false;
            int moduleCount = 0;
            ret = AXA.AxaInfoGetModuleCount(ref moduleCount) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;

            uint status = 0;
            ret = AXA.AxaInfoIsAIOModule(ref status) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;

            double volt = 0.0;
            uint digitValue = 0;

            ret = AXA.AxaiSwReadDigit((ushort)channel, ref digitValue) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
            ret = AXA.AxaiSwReadVoltage((ushort)channel, ref volt) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;

            return volt;
        }

        public override double GetCurrent(int channel)
        {
            uint digitValue = 0;
            AXA.AxaiSwReadDigit((ushort)channel, ref digitValue);

            if (digitValue > 8191) return 20.0;

            return (digitValue / 8191.0) * 20.0;
        }
        #endregion
    }
}

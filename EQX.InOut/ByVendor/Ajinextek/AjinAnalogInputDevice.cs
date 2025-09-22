using EQX.InOut.InOut.Analog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override double GetVolt(int channel)
        {
            double volt = 0.0;
            AXA.AxaiSwReadVoltage((ushort)channel, ref volt);

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

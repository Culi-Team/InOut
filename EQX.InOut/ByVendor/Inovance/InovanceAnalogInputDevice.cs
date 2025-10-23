using EQX.Core.Motion;
using EQX.InOut.InOut.Analog;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;

namespace EQX.InOut.ByVendor.Inovance
{
    public class InovanceAnalogInputDevice<TEnum> : AnalogInputDeviceBase<TEnum> where TEnum : Enum
    {
        #region Properties
        public IMotionMaster MotionMaster { get; init; }

        public override bool IsConnected { get => MotionMaster.IsConnected; }
        #endregion

        #region Public methods
        protected override void ExtendInit()
        {
            foreach (var input in AnalogInputs)
            {
                
            }
        }

        public override double GetVolt(int channel)
        {
            short sValue = 0;
            ImcApi.IMC_GetEcatAdVal(MotionMaster.ControllerId, (short)channel, ref sValue);

            return sValue;
        }

        public override double GetCurrent(int channel)
        {
            short sValue = 0;
            ImcApi.IMC_GetEcatAdVal(MotionMaster.ControllerId, (short)channel, ref sValue);

            return sValue;
        }
        #endregion
    }
}

using EQX.Core.Motion;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;

namespace EQX.InOut.ByVendor.Inovance
{
    public class InovanceOutputDevice<TEnum> : OutputDeviceBase<TEnum> where TEnum : Enum
    {
        public IMotionMaster MotionMaster { get; init; }
        public override bool IsConnected { get => MotionMaster.IsConnected; }

        public InovanceOutputDevice()
        {
        }

        protected override bool GetOutput(int index)
        {
            short sValue = 0;
            ImcApi.IMC_GetEcatDoBit(MotionMaster.ControllerId, (short)index, ref sValue);

            return sValue == 1;
        }

        protected override bool SetOutput(int index, bool value)
        {
            return ImcApi.IMC_SetEcatDoBit(MotionMaster.ControllerId, (short)index, (short)(value ? 1 : 0)) == ImcApi.EXE_SUCCESS;
        }
    }
}

using EQX.Core.Motion;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;

namespace EQX.InOut.ByVendor.Inovance
{
    public class InovanceInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        public IMotionMaster MotionMaster { get; init; }

        public override bool IsConnected { get => MotionMaster.IsConnected; }

        public InovanceInputDevice()
        {
        }

        protected override bool ActualGetInput(int index)
        {
            if (MotionMaster is null) return false;
            if (IsConnected == false) return false;

            short sValue = 0;
            ImcApi.IMC_GetEcatDiBit(MotionMaster.ControllerId, (short)index, ref sValue);

            return sValue == 1;
        }
    }
}

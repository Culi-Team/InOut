using EQX.Core.Motion;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;

namespace EQX.InOut.ByVendor.Inovance
{
    public class InovanceInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        public IMotionController MotionController { get; init; }

        public override bool IsConnected { get => MotionController.IsConnected; }

        public InovanceInputDevice()
        {
        }

        protected override bool ActualGetInput(int index)
        {
            if (MotionController is null) return false;
            if (IsConnected == false) return false;

            short sValue = 0;
            ImcApi.IMC_GetEcatDiBit(MotionController.ControllerId, (short)index, ref sValue);

            return sValue == 1;
        }
    }
}

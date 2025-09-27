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

            short sValue = 0;
            ImcApi.IMC_GetEcatDiBit(MotionController.DeviceId, (short)index, ref sValue);

            return sValue == 1;
        }
    }
}

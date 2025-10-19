using EQX.Core.InOut;
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

        public override void InverseStatus(IDInput input)
        {
            if (MotionMaster is null) return;
            if (IsConnected == false) return;

            short groupNo = (short)(input.Id / 8);
            short index = (short)(input.Id % 8);
            short invertMask = 0;

            bool ret = ImcApi.IMC_GetEcatGrpDiInverse(MotionMaster.ControllerId, groupNo, ref invertMask) == ImcApi.EXE_SUCCESS;

            invertMask |= (short)(1 << index);

            ret &= ImcApi.IMC_SetEcatGrpDiInverse(MotionMaster.ControllerId, groupNo, invertMask) == ImcApi.EXE_SUCCESS;

            return;
        }

        protected override bool ActualGetInput(int index)
        {
            if (MotionMaster is null) return false;
            if (IsConnected == false) return false;

            short sValue = 0;
            bool ret = ImcApi.IMC_GetEcatDiBit(MotionMaster.ControllerId, (short)index, ref sValue) == ImcApi.EXE_SUCCESS;

            return ret && sValue == 1;
        }
    }
}

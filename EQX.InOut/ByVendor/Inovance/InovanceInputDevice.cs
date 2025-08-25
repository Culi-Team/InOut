using EQX.Core.Motion;
using EQX.Motion.ByVendor.Inovance;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.ByVendor.Inovance
{
    public class InovanceInputDevice<TEnum> : InputDeviceBase<TEnum> where TEnum : Enum
    {
        public MotionControllerInovance MotionController { get; init; }

        public override bool IsConnected { get => MotionController.IsConnected; }

        public InovanceInputDevice()
        {
        }

        protected override bool ActualGetInput(int index)
        {
            if (MotionController is null) return false;

            short sValue = 0;
            ImcApi.IMC_GetEcatDiBit(MotionController.CardHandle, (short)index, ref sValue);

            return sValue == 1;
        }
    }
}

using EQX.Core.Motion;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.ByVendor.Inovance
{
    public class InovanceOutputDevice<TEnum> : OutputDeviceBase<TEnum> where TEnum : Enum
    {
        public IMotionController MotionController { get; init; }
        public override bool IsConnected { get => MotionController.IsConnected; }

        public InovanceOutputDevice()
        {
        }

        protected override bool GetOutput(int index)
        {
            short sValue = 0;
            ImcApi.IMC_GetEcatDoBit(MotionController.DeviceId, (short)index, ref sValue);

            return sValue == 1;
        }

        protected override bool SetOutput(int index, bool value)
        {
            return ImcApi.IMC_SetEcatDoBit(MotionController.DeviceId, (short)index, (short)(value ? 1 : 0)) == ImcApi.EXE_SUCCESS;
        }
    }
}

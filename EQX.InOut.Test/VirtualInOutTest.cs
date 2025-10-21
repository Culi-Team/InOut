using EQX.Core.InOut;
using EQX.InOut.Virtual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.Test
{
    public class VirtualInOutTest
    {
        [Fact]
        public void VirtualInOutDeviceMappingTest()
        {
            IDInputDevice dInputDevice = new MappableInputDevice<DigitalPin>() { Id = 1, Name = "VIn1", MaxPin = 256 };
            IDOutputDevice dOutputDevice = new MappableOutputDevice<DigitalPin>() { Id = 2, Name = "VOut1", MaxPin = 256 };

            dInputDevice.Initialize();
            dOutputDevice.Initialize();

            ((MappableInputDevice<DigitalPin>)dInputDevice).Mapping(10, dOutputDevice, 20);
            ((MappableInputDevice<DigitalPin>)dInputDevice).Mapping(11, dOutputDevice, 21);
            ((MappableInputDevice<DigitalPin>)dInputDevice).Mapping(12, dOutputDevice, 22);

            Assert.False(dInputDevice[10]);
            Assert.False(dOutputDevice[20]);

            dOutputDevice[20] = true;

            Assert.True(dInputDevice[10]);
            Assert.True(dOutputDevice[20]);

            dOutputDevice[21] = true;

            Assert.True(dInputDevice[11]);
            Assert.True(dOutputDevice[21]);

            dOutputDevice[22] = true;

            Assert.True(dInputDevice[12]);
            Assert.True(dOutputDevice[22]);

            dOutputDevice[20] = false;

            Assert.False(dInputDevice[10]);
            Assert.False(dOutputDevice[20]);

            dOutputDevice[21] = false;

            Assert.False(dInputDevice[11]);
            Assert.False(dOutputDevice[21]);

            dOutputDevice[22] = false;

            Assert.False(dInputDevice[12]);
            Assert.False(dOutputDevice[22]);
        }

        [Fact]
        public void VirtualInOutSingleMappingTest()
        {
            IDInputDevice dInputDevice = new MappableInputDevice<DigitalPin>() { Id = 1, Name = "VIn1", MaxPin = 256 };
            IDOutputDevice dOutputDevice = new MappableOutputDevice<DigitalPin>() { Id = 2, Name = "VOut1", MaxPin = 256 };
            IDInput input;
            IDOutput output;
            int pin;

            dInputDevice.Initialize();
            dOutputDevice.Initialize();

            pin = 10;
            input = dInputDevice.Inputs[pin];
            output = dOutputDevice.Outputs[pin];

            input.MapTo(output);

            Assert.False(input.Value);
            Assert.False(output.Value);
            Assert.False(dInputDevice[pin]);
            Assert.False(dOutputDevice[pin]);

            output.Value = true;

            Assert.True(input.Value);
            Assert.True(output.Value);
            Assert.True(dInputDevice[pin]);
            Assert.True(dOutputDevice[pin]);

            pin = 20;
            input = dInputDevice.Inputs[pin];
            output = dOutputDevice.Outputs[pin];

            input.MapTo(output);

            Assert.False(input.Value);
            Assert.False(output.Value);
            Assert.False(dInputDevice[pin]);
            Assert.False(dOutputDevice[pin]);

            output.Value = true;

            Assert.True(input.Value);
            Assert.True(output.Value);
            Assert.True(dInputDevice[pin]);
            Assert.True(dOutputDevice[pin]);

            pin = 21;
            input = dInputDevice.Inputs[pin];
            output = dOutputDevice.Outputs[pin];

            input.MapTo(output);

            Assert.False(input.Value);
            Assert.False(output.Value);
            Assert.False(dInputDevice[pin]);
            Assert.False(dOutputDevice[pin]);

            output.Value = true;

            Assert.True(input.Value);
            Assert.True(output.Value);
            Assert.True(dInputDevice[pin]);
            Assert.True(dOutputDevice[pin]);
        }

        [Fact]
        public void VirtualInOutSingleMappingTest2()
        {
            IDInputDevice dInputDevice = new MappableInputDevice<DigitalPin>() { Id = 1, Name = "VIn1", MaxPin = 256 };
            IDOutputDevice dOutputDevice = new MappableOutputDevice<DigitalPin>() { Id = 2, Name = "VOut1", MaxPin = 256 };

            dInputDevice.Initialize();
            dOutputDevice.Initialize();

            IDOutput output = dOutputDevice.Outputs[(int)DigitalPin.Pin15];

            dInputDevice[DigitalPin.Pin10]
                .MapTo(dOutputDevice[DigitalPin.Pin15]);

            Assert.False(dInputDevice[(int)DigitalPin.Pin10]);
            Assert.False(dOutputDevice[(int)DigitalPin.Pin15]);

            output.Value = true;

            Assert.True(dInputDevice[(int)DigitalPin.Pin10]);
            Assert.True(dOutputDevice[(int)DigitalPin.Pin15]);
        }

        enum VTCamWork2CVFlag
        {
            InputReady,
            OutputReady,
        }

        enum VTCamOutCVFlag
        {
            // INPUT FLAG
            /// <summary>
            /// Unloader has completed unloading the FRONT
            /// </summary>
            OutputDone,

            // OUTPUT FLAG
            InputReady,
            OutputReady,
        }

        enum UnloaderFlag
        {
            // INPUT FLAG
            OutputReady,

            // OUTPUT FLAG
            OutputDone,
        }

        [Fact]
        public void VirtualFlagTest()
        {
            IDInputDevice outCVFlag_In = new MappableInputDevice<VTCamOutCVFlag>() { Id = 1, Name = "OutCVFlag_In", MaxPin = 256 };
            IDOutputDevice outCVFlag_Out = new MappableOutputDevice<VTCamOutCVFlag>() { Id = 2, Name = "OutCVFlag_Out", MaxPin = 256 };
            IDInputDevice unloaderFlag_In = new MappableInputDevice<UnloaderFlag>() { Id = 3, Name = "UnloaderFlag_In", MaxPin = 256 };
            IDOutputDevice unloaderFlag_Out = new MappableOutputDevice<UnloaderFlag>() { Id = 4, Name = "UnloaderFlag_Out", MaxPin = 256 };

            outCVFlag_In.Initialize();
            outCVFlag_Out.Initialize();
            outCVFlag_In.Initialize();

            ((MappableInputDevice<UnloaderFlag>)unloaderFlag_In).Mapping((int)UnloaderFlag.OutputReady, outCVFlag_Out, (int)VTCamOutCVFlag.OutputReady);

            Assert.False(unloaderFlag_In[(int)UnloaderFlag.OutputReady]);

            outCVFlag_Out[(int)VTCamOutCVFlag.OutputReady] = true;

            Assert.True(outCVFlag_Out[(int)VTCamOutCVFlag.OutputReady]);
            Assert.True(unloaderFlag_In[(int)UnloaderFlag.OutputReady]);

        }
    }
}

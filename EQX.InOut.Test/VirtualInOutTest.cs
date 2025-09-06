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
        public void VirtualInOutMappingTest()
        {
            IDInputDevice dInputDevice = new VirtualInputDevice<DigitalPin>() { Id = 1, Name = "VIn1", MaxPin = 256 };
            IDOutputDevice dOutputDevice = new VirtualOutputDevice<DigitalPin>() { Id = 2, Name = "VOut1", MaxPin = 256 };

            dInputDevice.Initialize();
            dOutputDevice.Initialize();

            ((VirtualInputDevice<DigitalPin>)dInputDevice).Mapping(10, dOutputDevice, 20);
            ((VirtualInputDevice<DigitalPin>)dInputDevice).Mapping(11, dOutputDevice, 21);
            ((VirtualInputDevice<DigitalPin>)dInputDevice).Mapping(12, dOutputDevice, 22);

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
    }
}

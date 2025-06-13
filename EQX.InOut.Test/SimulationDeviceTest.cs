using EQX.Core.InOut;
using System.Net.NetworkInformation;

namespace EQX.InOut.Test
{
    public class SimulationDeviceTest
    {
        enum DigitalPin
        {
            Pin1 = 0,
            Pin2 = 1,
            Pin3 = 2,
            Pin4 = 3,
            Pin5 = 4,
            Pin6 = 5,
            Pin7 = 6,
            Pin8 = 7
        }

        [Fact]
        public void ConnectionTest()
        {
            SimulationInputDevice_HardwareDevice deviceSide = new SimulationInputDevice_HardwareDevice
            {
                Id = 10
            };
            deviceSide.Start();

            SimulationInputDevice_Client<DigitalPin> clientSide = new SimulationInputDevice_Client<DigitalPin>
            {
                MaxPin = 256,
                Id = 10
            };
            clientSide.Connect();

            Assert.True(clientSide.IsConnected);
        }

        [Fact]
        public void CoilSetToggleTest()
        {
            SimulationInputDevice_HardwareDevice deviceSide = new SimulationInputDevice_HardwareDevice
            {
                Id = 11
            };
            deviceSide.Start();

            SimulationInputDevice_Client<DigitalPin> clientSide = new SimulationInputDevice_Client<DigitalPin>
            {
                MaxPin = 256,
                Id = 11
            };
            clientSide.Connect();

            deviceSide[10] = true;
            Assert.True(clientSide[10]);

            deviceSide[15] = true;
            Assert.True(clientSide[15]);

            deviceSide.ToggleInput(15);
            Assert.False(clientSide[15]);

            for (int i = 0; i < clientSide.MaxPin; i++)
            {
                deviceSide[i] = true;
                Assert.True(clientSide[i]);

                deviceSide[i] = false;
                Assert.False(clientSide[i]);
            }
        }

        [Fact]
        public void MultipleConnectionTest()
        {
            int maxPin = 256;

            SimulationInputDevice_HardwareDevice device1 = new SimulationInputDevice_HardwareDevice
            {
                Id = 0
            };
            SimulationInputDevice_HardwareDevice device2 = new SimulationInputDevice_HardwareDevice
            {
                Id = 2
            };
            SimulationInputDevice_HardwareDevice device3 = new SimulationInputDevice_HardwareDevice
            {
                Id = 5
            };

            SimulationInputDevice_Client<DigitalPin> client1 = new SimulationInputDevice_Client<DigitalPin>
            {
                MaxPin = maxPin,
                Id = 0
            };
            SimulationInputDevice_Client<DigitalPin> client2 = new SimulationInputDevice_Client<DigitalPin>
            {
                MaxPin = maxPin,
                Id = 2
            };
            SimulationInputDevice_Client<DigitalPin> client3 = new SimulationInputDevice_Client<DigitalPin>
            {
                MaxPin = maxPin,
                Id = 5
            };

            device1.Start();
            client1.Connect();
            Assert.True(client1.IsConnected);

            device2.Start();
            client2.Connect();
            Assert.True(client2.IsConnected);

            device3.Start();
            client3.Connect();
            Assert.True(client3.IsConnected);

            device1[10] = true;
            Assert.True(client1[10]);

            device1[15] = true;
            Assert.True(client1[15]);

            device1.ToggleInput(15);
            Assert.False(client1[15]);

            for (int i = 0; i < maxPin; i++)
            {
                device1[i] = true;
                Assert.True(client1[i]);

                device1[i] = false;
                Assert.False(client1[i]);

                device2[i] = true;
                Assert.True(client2[i]);

                device2[i] = false;
                Assert.False(client2[i]);

                device3[i] = true;
                Assert.True(client3[i]);

                device3[i] = false;
                Assert.False(client3[i]);
            }
        }
    }
}
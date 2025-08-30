using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EQX.InOut.Test
{
    public class InputSimulation
    {
        [Fact]
        public void InputSimulationTest()
        {
            SimulationInputDeviceServer<EInput1> deviceServer = new SimulationInputDeviceServer<EInput1>() { Id = 1, Name = "InDevice1Server", MaxPin = 256 };
            deviceServer.Start();
            SimulationInputDevice<EInput1> deviceClient = new SimulationInputDevice<EInput1>() { Id = 1, Name = "InDevice1Client", MaxPin = 256 };
            deviceClient.Connect();
            deviceServer[15] = true;
            Assert.True(deviceClient[15]);
            deviceServer[14] = true;
            Assert.True(deviceClient[14]);
            for (int i = 0; i < deviceServer.MaxPin; i++)
            {
                deviceServer[i] = true;
            }
            for (int i = 0; i < deviceClient.MaxPin; i++)
            {
                Assert.True(deviceClient[i]);
            }
            for (int i = 0; i < deviceServer.MaxPin; i++)
            {
                deviceServer[i] = false;
            }
            for (int i = 0; i < deviceClient.MaxPin; i++)
            {
                Assert.Equal(deviceServer[i], deviceClient[i]);
            }
        }
        public enum EInput1
        {
            IN_SW_START_MAIN = 00,
            IN_SW_START_LEFT = 01,
            IN_SW_START_RIGHT = 02,
            IN_SW_STOP = 03,
            IN_SW_RESET = 04,
            IN_SW_EMERGENCY = 05,
            IN_MAIN_MC_ON = 06,
            IN_SERVO_POWER_ON = 07,
            IN_GRIPPER_ON_L = 08,       // Rotate Unit
            IN_DOOR_FRONT = 09,
            IN_DOOR_BACK = 10,
            IN_EXIST_LEFT_MOLD_1 = 11,
            IN_EXIST_RIGHT_MOLD_1 = 12,
            IN_EXIST_LEFT_GLASS_JIG = 13,
            IN_EXIST_RIGHT_GLASS_JIG = 14,
            IN_DISPLAY_PICKER_VAC_ON = 15,
            IN_LIGHTCURTAIN_LEFT = 16,
            IN_LIGHTCURTAIN_RIGHT = 17,
            IN_DISPLAY_PICKER_LOADCELL = 18,
            IN_EXIST_LEFT_MOLD_2 = 19,
            IN_EXIST_RIGHT_MOLD_2 = 20,
            IN_GRIPPER_OFF_L = 21,       // Rotate Unit
            IN_ROTATE_UP_L = 22,       // Rotate Unit
            IN_ROTATE_DOWN_L = 23,       // Rotate Unit
            IN_ROTATE_DEGREE_0_L = 24,       // Rotate Unit
            IN_ROTATE_DEGREE_180_L = 25,       // Rotate Unit
            IN_GRIPPER_ON_R = 26,       // Rotate Unit
            IN_GRIPPER_OFF_R = 27,       // Rotate Unit
            IN_ROTATE_UP_R = 28,       // Rotate Unit
            IN_ROTATE_DOWN_R = 29,       // Rotate Unit
            IN_ROTATE_DEGREE_0_R = 30,       // Rotate Unit
            IN_ROTATE_DEGREE_180_R = 31,       // Rotate Unit
        }
    }
}

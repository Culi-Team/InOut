using System.Net.Sockets;
using System.Net;
using System.Text;

namespace EQX.Motion.ThirdParty.Fastech.PlusE
{
    internal class EziPlusEMotionLib
    {
        #region Return Codes
        public const int FMM_OK = 0;

        public const int FMM_NOT_OPEN = 1;
        public const int FMM_INVALID_PORT_NUM = 2;
        public const int FMM_INVALID_SLAVE_NUM = 3;

        public const int FMC_DISCONNECTED = 5;
        public const int FMC_TIMEOUT_ERROR = 6;
        public const int FMC_CRCFAILED_ERROR = 7;
        public const int FMC_RECVPACKET_ERROR = 8;

        public const int FMM_POSTABLE_ERROR = 9;

        public const int FMP_FRAMETYPEERROR = 0x80;
        public const int FMP_DATAERROR = 0x81;
        public const int FMP_PACKETERROR = 0x82;

        public const int FMP_RUNFAIL = 0x85;
        public const int FMP_RESETFAIL = 0x86;
        public const int FMP_SERVOONFAIL1 = 0x87;
        public const int FMP_SERVOONFAIL2 = 0x88;
        public const int FMP_SERVOONFAIL3 = 0x89;

        public const int FMP_SERVOOFF_FAIL = 0x8A;
        public const int FMP_ROMACCESS = 0x8B;

        public const int FMP_PACKETCRCERROR = 0xAA;

        public const int FMM_UNKNOWN_ERROR = 0xFF;
        #endregion

        #region Publics
        public string Name;
        #endregion

        #region Constructors
        public EziPlusEMotionLib(int index, string name)
        {
            iPAddress = IPAddress.Parse($"192.168.0.{index}");
            Name = name;

            tcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);

            syncNumber = (byte)new Random().Next(0, 255);
            transmitFrameData = new byte[260];
            responseFrameData = new byte[260];
        }
        #endregion

        #region Functions
        public bool Connect()
        {
            if (tcpClient.Connected)
            {
                tcpClient.Close();
                tcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            }

            tcpClient.Connect(iPAddress, 2002);

            return tcpClient.Connected;
        }

        public bool Disconnect()
        {
            tcpClient.Close();

            return !tcpClient.Connected;
        }

        public int EMGStop()
        {
            SetTransmitFrameData(0x32);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int SearchOrigin()
        {
            SetTransmitFrameData(0x33);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MotionOff()
        {
            byte[] data = new byte[] { 0x00 };
            SetTransmitFrameData(0x2A, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MotionOn()
        {
            byte[] data = new byte[] { 0x01 };
            SetTransmitFrameData(0x2A, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MoveAbs(int pulse, uint velocity)
        {
            byte[] dataPulse = BitConverter.GetBytes(pulse);
            byte[] dataVelocity = BitConverter.GetBytes(velocity);
            byte[] data = dataPulse.Concat(dataVelocity).ToArray();

            SetTransmitFrameData(0x34, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int AlarmReset()
        {
            SetTransmitFrameData(0x2B);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MoveInc(int pulse, uint velocity)
        {
            byte[] dataPulse = BitConverter.GetBytes(pulse);
            byte[] dataVelocity = BitConverter.GetBytes(velocity);
            byte[] data = dataPulse.Concat(dataVelocity).ToArray();

            SetTransmitFrameData(0x35, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MoveJog(uint velocity, bool direction)
        {
            byte[] dataVelocity = BitConverter.GetBytes(velocity);
            byte[] dataDirection = BitConverter.GetBytes(direction);
            byte[] data = dataVelocity.Concat(dataDirection).ToArray();

            SetTransmitFrameData(0x37, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int SoftStop()
        {
            SetTransmitFrameData(0x31);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int ClearPosition()
        {
            SetTransmitFrameData(0x56);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetBoardInfo(ref string sBoardInfo)
        {
            SetTransmitFrameData(0x01);
            SendTransmitFrameData();

            ReadResponseFrameData();

            byte[] bBoardInfo = new byte[260];
            for (int i = 7; i <= responseFrameData[1]; i++)
            {
                bBoardInfo[i - 7] = responseFrameData[i];
            }
            sBoardInfo = Encoding.ASCII.GetString(bBoardInfo);

            return communicationStatus;
        }

        public int GetMotorInfo(ref string sMotorInfo)
        {
            SetTransmitFrameData(0x05);
            SendTransmitFrameData();

            ReadResponseFrameData();

            byte[] bMotorInfo = new byte[260];
            for (int i = 7; i <= responseFrameData[1]; i++)
            {
                bMotorInfo[i - 7] = responseFrameData[i];
            }
            sMotorInfo = Encoding.ASCII.GetString(bMotorInfo);

            return communicationStatus;
        }

        public int SetIOOutput(uint setMask, uint clearMask)
        {
            byte[] dataSetMask = BitConverter.GetBytes(setMask);
            byte[] dataClearMask = BitConverter.GetBytes(clearMask);
            byte[] data = dataSetMask.Concat(dataClearMask).ToArray();

            SetTransmitFrameData(0x20, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();
            return communicationStatus;
        }

        public int SetIOInput(uint setMask, uint clearMask)
        {
            byte[] dataSetMask = BitConverter.GetBytes(setMask);
            byte[] dataClearMask = BitConverter.GetBytes(clearMask);
            byte[] data = dataSetMask.Concat(dataClearMask).ToArray();

            SetTransmitFrameData(0x21, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();
            return communicationStatus;
        }

        public int GetIOInput(ref uint inputStatus)
        {
            byte[] bInputStatus = new byte[4];
            SetTransmitFrameData(0x22);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 6; i <= responseFrameData[1]; i++)
            {
                bInputStatus[i - 6] = responseFrameData[i];
            }
            inputStatus = BitConverter.ToUInt32(bInputStatus);

            return communicationStatus;
        }

        public int GetIOOutput(ref uint outputStatus)
        {
            byte[] bOutputStatus = new byte[4];
            SetTransmitFrameData(0x23);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 6; i <= responseFrameData[1]; i++)
            {
                bOutputStatus[i - 6] = responseFrameData[i];
            }
            outputStatus = BitConverter.ToUInt32(bOutputStatus);

            return communicationStatus;
        }

        public int GetAlarmType(ref byte alarmType)
        {
            SetTransmitFrameData(0x2E);
            SendTransmitFrameData();
                
            ReadResponseFrameData();
            alarmType = responseFrameData[6];

            return communicationStatus;
        }

        public int GetAxisStatus(ref uint axisStatus)
        {
            byte[] bAxisStatus = new byte[4];
            SetTransmitFrameData(0x40);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 6; i <= responseFrameData[1]; i++)
            {
                bAxisStatus[i - 6] |= responseFrameData[i];
            }
            axisStatus = BitConverter.ToUInt32(bAxisStatus);

            return communicationStatus;
        }

        public void SetOutput(int pinNumber, bool value)
        {
            uint setMask = 0;
            uint clearMask = 0;

            if (value == true)
            {
                setMask = (uint)(0x00008000 << pinNumber);
            }
            else
            {
                clearMask = (uint)(0x00008000 << pinNumber);
            }

            SetIOOutput(setMask, clearMask);
        }

        public bool GetOutput(int pinNumber)
        {
            uint ioOutput = 0;
            GetIOOutput(ref ioOutput);

            uint pinBitMask = (uint)(0x00008000 << pinNumber);

            return (ioOutput & pinBitMask) == pinBitMask;
        }

        public int GetIOAxisStatus(ref uint inputStatus, ref uint outputStatus, ref uint axisStatus)
        {
            byte[] bInputStatus = new byte[4];
            byte[] bOutputStatus = new byte[4];
            byte[] bAxisStatus = new byte[4];
            SetTransmitFrameData(0x41);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 6; i < 10; i++)
            {
                bInputStatus[i - 6] = responseFrameData[i];
            }
            for (int i = 10; i < 14; i++)
            {
                bOutputStatus[i - 10] = responseFrameData[i];
            }
            for (int i = 14; i <= responseFrameData[1]; i++)
            {
                bAxisStatus[i - 14] = responseFrameData[i];
            }

            inputStatus = BitConverter.ToUInt32(bInputStatus);
            outputStatus = BitConverter.ToUInt32(bOutputStatus);
            axisStatus = BitConverter.ToUInt32(bAxisStatus);

            return communicationStatus;
        }

        public int GetMotionStatus(ref int cmdPos, ref int actPos, ref int posErr, ref int actVel, ref int currentRunPT)
        {
            byte[] bCmdPos = new byte[4];
            byte[] bActPos = new byte[4];
            byte[] bPosErr = new byte[4];
            byte[] bActVel = new byte[4];
            byte[] bCurrentRunPT = new byte[4];
            SetTransmitFrameData(0x42);
            SendTransmitFrameData();

            ReadResponseFrameData();

            for (int i = 6; i < 10; i++)
            {
                bCmdPos[i - 6] = responseFrameData[i];
            }
            for (int i = 10; i < 14; i++)
            {
                bActPos[i - 10] = responseFrameData[i];
            }
            for (int i = 14; i < 18; i++)
            {
                bPosErr[i - 14] = responseFrameData[i];
            }
            for (int i = 18; i < 22; i++)
            {
                bActVel[i - 18] = responseFrameData[i];
            }
            for (int i = 22; i <= responseFrameData[1]; i++)
            {
                bCurrentRunPT[i - 22] = responseFrameData[i];
            }

            cmdPos = BitConverter.ToInt32(bCmdPos);
            actPos = BitConverter.ToInt32(bActPos);
            posErr = BitConverter.ToInt32(bPosErr);
            actVel = BitConverter.ToInt32(bActVel);
            currentRunPT = BitConverter.ToInt32(bCurrentRunPT);

            return communicationStatus;

        }

        public int GetAllStatus(ref uint inputStatus, ref uint outputStatus, ref uint axisStatus, ref int cmdPos, ref int actPos, ref int posErr, ref int actVel, ref int currentRunPT)
        {
            byte[] bInputStatus = new byte[4];
            byte[] bOutputStatus = new byte[4];
            byte[] bAxisStatus = new byte[4];
            byte[] bCmdPos = new byte[4];
            byte[] bActPos = new byte[4];
            byte[] bPosErr = new byte[4];
            byte[] bActVel = new byte[4];
            byte[] bCurrentRunPT = new byte[4];
            SetTransmitFrameData(0x43);
            SendTransmitFrameData();

            ReadResponseFrameData();

            for (int i = 6; i < 10; i++)
            {
                bInputStatus[i - 6] = responseFrameData[i];
            }
            for (int i = 10; i < 14; i++)
            {
                bOutputStatus[i - 10] = responseFrameData[i];
            }
            for (int i = 14; i < 18; i++)
            {
                bAxisStatus[i - 14] = responseFrameData[i];
            }
            for (int i = 18; i < 22; i++)
            {
                bCmdPos[i - 18] = responseFrameData[i];
            }
            for (int i = 22; i < 26; i++)
            {
                bActPos[i - 22] = responseFrameData[i];
            }
            for (int i = 26; i < 30; i++)
            {
                bPosErr[i - 26] = responseFrameData[i];
            }
            for (int i = 30; i < 34; i++)
            {
                bActVel[i - 30] = responseFrameData[i];
            }
            for (int i = 34; i <= responseFrameData[1]; i++)
            {
                bCurrentRunPT[i - 34] = responseFrameData[i];
            }

            inputStatus = BitConverter.ToUInt32(bInputStatus);
            outputStatus = BitConverter.ToUInt32(bOutputStatus);
            axisStatus = BitConverter.ToUInt32(bAxisStatus);
            cmdPos = BitConverter.ToInt32(bCmdPos);
            actPos = BitConverter.ToInt32(bActPos);
            posErr = BitConverter.ToInt32(bPosErr);
            actVel = BitConverter.ToInt32(bActVel);
            currentRunPT = BitConverter.ToInt32(bCurrentRunPT);

            return communicationStatus;
        }

        public int SetCommandPos(int cmdPos)
        {
            byte[] data = BitConverter.GetBytes(cmdPos);
            SetTransmitFrameData(0x50, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetCommandPos(ref int cmdPos)
        {
            byte[] bCmdPos = new byte[4];
            SetTransmitFrameData(0x51);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 6; i <= responseFrameData[1]; i++)
            {
                bCmdPos[i - 6] = responseFrameData[i];
            }
            cmdPos = BitConverter.ToInt32(bCmdPos);

            return communicationStatus;
        }

        public int SetActualPos(int actPos)
        {
            byte[] data = BitConverter.GetBytes(actPos);
            SetTransmitFrameData(0x52, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();
            return communicationStatus;
        }

        public int GetActualPos(ref int actPos)
        {
            byte[] bActPos = new byte[4];
            SetTransmitFrameData(0x53);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 6; i <= responseFrameData[1]; i++)
            {
                bActPos[i - 6] = responseFrameData[i];
            }
            actPos = BitConverter.ToInt32(bActPos);

            return communicationStatus;
        }

        public int GetPosError(ref int posErr)
        {
            byte[] bPosErr = new byte[4];
            SetTransmitFrameData(0x54);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 0; i <= responseFrameData[1]; i++)
            {
                bPosErr[i - 6] = responseFrameData[i];
            }
            posErr = BitConverter.ToInt32(bPosErr);

            return communicationStatus;
        }

        public int GetActualVel(ref int actVel)
        {
            byte[] bActVel = new byte[4];
            SetTransmitFrameData(0x55);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 0; i <= responseFrameData[1]; i++)
            {
                bActVel[i - 6] = responseFrameData[i];
            }
            actVel = BitConverter.ToInt32(bActVel);

            return communicationStatus;
        }

        public int SetAllParameters()
        {
            SetTransmitFrameData(0x10);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetROMParameter(byte paraNumber, ref int paraValue)
        {
            byte[] bParaValue = new byte[4];
            byte[] bParaNumber = new byte[1];
            bParaNumber[0] = paraNumber;
            SetTransmitFrameData(0x11, 1, bParaNumber);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 6; i <= responseFrameData[1]; i++)
            {
                bParaValue[i - 6] |= responseFrameData[i];
            }
            paraValue = BitConverter.ToInt32(bParaValue);

            return communicationStatus;
        }

        public int SetParameter(byte paraNumber, int paraValue)
        {
            byte[] dataParaNumber = new byte[1];

            dataParaNumber[0] = paraNumber;
            byte[] dataParaValue = BitConverter.GetBytes(paraValue);
            byte[] data = dataParaNumber.Concat(dataParaValue).ToArray();

            SetTransmitFrameData(0x12, data.Length, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetParameter(byte paraNumber, ref int paraValue)
        {
            byte[] dataParaValue = new byte[4];

            byte[] dataParaNumber = new byte[1];
            dataParaNumber[0] = paraNumber;

            SetTransmitFrameData(0x13, dataParaNumber.Length, dataParaNumber);
            SendTransmitFrameData();

            ReadResponseFrameData();
            for (int i = 6; i <= responseFrameData[1]; i++)
            {
                dataParaValue[i - 6] = responseFrameData[i];
            }
            paraValue = BitConverter.ToInt32(dataParaValue);

            return communicationStatus;
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Set transmit frame data, which the frame type is null data type (data length is 0 byte)
        /// </summary>
        /// <param name="frameType"></param>
        private void SetTransmitFrameData(byte frameType)
        {
            SetTransmitFrameData(frameType, 0, null);
        }

        private void SetTransmitFrameData(byte frameType, int dataLength, byte[]? data)
        {
            // Header : 0xAA
            transmitFrameData[0] = 0xAA;
            // Length : Length of Data after Length (SyncNumber + Reserved + Frame Type + Data)
            transmitFrameData[1] = (byte)(3 + dataLength);
            // Sync Number. The value should change every time when send a new command
            transmitFrameData[2] = ++syncNumber;
            // Reserved : 0x00
            transmitFrameData[3] = 0x00;
            // Frame type : Specify the command type of the Frame
            transmitFrameData[4] = frameType;
            // Data : The data structure and length of this clause are determinated by the Frame type
            if(data !=null)
            {
                for (int i = 0; i < dataLength; i++)
                {
                    transmitFrameData[5 + i] = data[i];
                }
            }
        }

        private void SendTransmitFrameData()
        {
            tcpClient.Send(transmitFrameData, transmitFrameData[1] + 2, SocketFlags.None);
        }

        private int ReadResponseFrameData()
        {
            return tcpClient.Receive(responseFrameData);
        }
        #endregion

        #region Privates
        IPAddress iPAddress;
        Socket tcpClient;

        const int frameDataLength = 260;

        /// <summary>
        /// The Sync number of the packet, which is used to check whether the command is executed in the drive module
        /// </summary>
        byte syncNumber;
        /// <summary>
        /// Controller (PC, Rpi) to Ezi, transmit frame data
        /// </summary>
        byte[] transmitFrameData;
        /// <summary>
        /// Ezi to Controller (PC, Rpi), response frame data
        /// </summary>
        byte[] responseFrameData;

        byte communicationStatus => responseFrameData[5];
        #endregion
    }
}

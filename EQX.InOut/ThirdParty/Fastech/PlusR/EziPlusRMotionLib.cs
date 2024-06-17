using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EQX.Motion.Helpers;

namespace EQX.Motion.ThirdParty.Fastech.PlusR
{
    internal partial class EziPlusRMotionLib
    {
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

        #region Properties
        private byte communicationStatus
        {
            get
            {
                if (responseFrameData == null) return 0xFF;
                if (responseFrameData.Count <= 4) return 0xFF;

                return responseFrameData[4];
            }
        }

        public string CommunicationErrorDesciptions
        {
            get
            {
                // The communication may not occur yet, or not yet finished
                if (responseFrameData == null) return "";
                if (responseFrameData.Count <= 4) return "";

                string? errorDesciptions;
                CommunicationStatuses.TryGetValue(responseFrameData[4], out errorDesciptions);

                return errorDesciptions ?? "Not-defined error";
            }
        }
        #endregion

        #region Constructor(s)
        public EziPlusRMotionLib(string portName, int slaveID, int baudrate = 115200)
        {
            // Guard clause : Đảm bảo thông số truyền vào không null
            if (portName == null) throw new ArgumentNullException(nameof(portName));
            if (portName.Length <= 0) throw new ArgumentException("Argument Empty Value", nameof(portName));

            this.portName = portName;
            this.slaveId = slaveID;
            this.baudRate = baudrate;

            checkSumBytes = new byte[2];
            serialPort = new SerialPort(portName, baudrate);
        }
        #endregion

        #region Functions
        public bool Connect()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort = new SerialPort(portName, baudRate);
            }
            serialPort.Open();
            return serialPort.IsOpen;
        }

        public bool Disconnect()
        {
            serialPort.Close();
            return !serialPort.IsOpen;
        }

        public int GetBoardInfo(ref string sBoardInfo)
        {
            SetTransmitFrameData(0x01);
            SendTransmitFrameData();
            if (ReadResponseFrameData())
            {
                byte[] bBoardInfo = new byte[256];
                for (int i = 5; i < responseFrameData.Count - 4; i++)
                {
                    bBoardInfo[i - 5] = responseFrameData[i];
                }
                byte bType = bBoardInfo[0];
                string Type = "Type = " + bType.ToString();

                string BoardName = BoardNameList[bType];

                string Version = " . Version : " + Encoding.ASCII.GetString(bBoardInfo);
                sBoardInfo = Type + " . " + BoardName + Version;
            }

            return communicationStatus;
        }

        public int GetMotorInfo(ref string sMotorInfo)
        {
            SetTransmitFrameData(0x05);
            SendTransmitFrameData();

            if (ReadResponseFrameData())
            {
                byte[] bMotorInfo = new byte[responseFrameData.Count];
                for (int i = 6; i < responseFrameData.Count - 4; i++)
                {
                    bMotorInfo[i - 6] = responseFrameData[i];
                }
                sMotorInfo = Encoding.ASCII.GetString(bMotorInfo);
            }

            return communicationStatus;
        }

        public int SaveAllParameters()
        {
            SetTransmitFrameData(0x10);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetROMParameter(byte paraNumber, ref int paraValue)
        {
            byte[] data = new byte[1] { paraNumber };
            SetTransmitFrameData(0x11, data);
            SendTransmitFrameData();

            byte[] bParaValue = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bParaValue[i - 5] = responseFrameData[i];
                }
            }
            paraValue = BitConverter.ToInt32(bParaValue);

            return communicationStatus;
        }

        public int SetParameter(byte paraNumber, int paraValue)
        {
            byte[] dataParaNumber = new byte[1] { paraNumber };
            byte[] dataValue = BitConverter.GetBytes(paraValue);
            byte[] data = dataParaNumber.Concat(dataValue).ToArray();

            SetTransmitFrameData(0x12, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetParameter(byte paraNumber, ref int paraValue)
        {
            byte[] data = new byte[1] { paraNumber };

            SetTransmitFrameData(0x13, data);
            SendTransmitFrameData();

            byte[] bParaValue = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bParaValue[i - 5] = responseFrameData[i];
                }
            }
            paraValue = BitConverter.ToInt32(bParaValue);

            return communicationStatus;
        }

        public int SetIOOutput(uint setMask, uint clearMask)
        {
            byte[] dataSetMask = BitConverter.GetBytes(setMask);
            byte[] dataClearMask = BitConverter.GetBytes(clearMask);
            byte[] data = dataSetMask.Concat(dataClearMask).ToArray();

            SetTransmitFrameData(0x20, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

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

        public int SetIOInput(uint setMask, uint clearMask)
        {
            byte[] dataSetMask = BitConverter.GetBytes(setMask);
            byte[] dataClearMask = BitConverter.GetBytes(clearMask);
            byte[] data = dataSetMask.Concat(dataClearMask).ToArray();

            SetTransmitFrameData(0x21, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetIOInput(ref uint inputValue)
        {
            SetTransmitFrameData(0x22);
            SendTransmitFrameData();

            byte[] bInputValue = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bInputValue[i - 5] = responseFrameData[i];
                }
            }
            inputValue = BitConverter.ToUInt32(bInputValue);

            return communicationStatus;
        }

        public int GetIOOutput(ref uint outputValue)
        {
            SetTransmitFrameData(0x23);
            SendTransmitFrameData();

            byte[] bOutputValue = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bOutputValue[i - 5] = responseFrameData[i];
                }
            }
            outputValue = BitConverter.ToUInt32(bOutputValue);

            return communicationStatus;
        }

        public bool GetOutput(int pinNumber)
        {
            uint ioOutput = 0;
            GetIOOutput(ref ioOutput);

            uint pinBitMask = (uint)(0x00008000 << pinNumber);

            return (ioOutput & pinBitMask) == pinBitMask;
        }

        public int SetIOAssignMap(byte ioNumber, uint maskingData, byte settingLevel)
        {
            byte[] bIONumber = new byte[1] { ioNumber };
            byte[] bMaskingData = BitConverter.GetBytes(maskingData);
            byte[] bSettingLevel = new byte[1] { settingLevel };

            byte[] data = bIONumber.Concat(bMaskingData).Concat(bSettingLevel).ToArray();

            SetTransmitFrameData(0x24, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetIOAssignMap(byte ioNumber, ref uint maskingValue, ref byte levelStatus)
        {
            byte[] data = new byte[1] { ioNumber };

            SetTransmitFrameData(0x25, data);
            SendTransmitFrameData();

            if (ReadResponseFrameData())
            {
                byte[] bMaskingValue = new byte[4];
                for (int i = 5; i < 9; i++)
                {
                    bMaskingValue[i - 5] = responseFrameData[i];
                }
                levelStatus = responseFrameData[9];
                maskingValue = BitConverter.ToUInt32(bMaskingValue);
            }

            return communicationStatus;
        }

        public int ServoEnable(byte bOnOff)
        {
            byte[] data = new byte[1] { bOnOff };

            SetTransmitFrameData(0x2A, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        //For Driver Ezi Servo
        public int ServoAlarmReset()
        {
            SetTransmitFrameData(0x2B);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }


        //For Driver Ezi Step
        public int StepAlarmReset()
        {
            SetTransmitFrameData(0x2C);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetAlarmType(ref byte alarmType)
        {
            SetTransmitFrameData(0x2E);
            SendTransmitFrameData();

            ReadResponseFrameData();

            alarmType = responseFrameData[5];

            return communicationStatus;
        }

        public int SoftStop()
        {
            SetTransmitFrameData(0x31);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
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

        public int MoveAbs(int pulse, uint velocity)
        {
            byte[] dataPulse = BitConverter.GetBytes(pulse);
            byte[] dataVelocity = BitConverter.GetBytes(velocity);
            byte[] data = dataPulse.Concat(dataVelocity).ToArray();

            SetTransmitFrameData(0x34, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MoveInc(int pulse, uint velocity)
        {
            byte[] dataPulse = BitConverter.GetBytes(pulse);
            byte[] dataVelocity = BitConverter.GetBytes(velocity);
            byte[] data = dataPulse.Concat(dataVelocity).ToArray();

            SetTransmitFrameData(0x35, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MoveToLimit(uint velocity, byte direction) // direction : 0-> -Limit , 1-> +Limit
        {
            byte[] dataVelocity = BitConverter.GetBytes(velocity);
            byte[] dataDirection = new byte[1] { direction };
            byte[] data = dataVelocity.Concat(dataDirection).ToArray();

            SetTransmitFrameData(0x36, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MoveJog(uint velocity, byte direction) //direction : 0-> -Jog , 1-> +Jog
        {
            byte[] dataVelocity = BitConverter.GetBytes(velocity);
            byte[] dataDirection = new byte[1] { direction };
            byte[] data = dataVelocity.Concat(dataDirection).ToArray();

            SetTransmitFrameData(0x37, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetAxisStatus(ref uint axisStatus)
        {
            SetTransmitFrameData(0x40);
            SendTransmitFrameData();

            byte[] bAxisStatus = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bAxisStatus[i - 5] = responseFrameData[i];
                }
            }
            axisStatus = BitConverter.ToUInt32(bAxisStatus);

            return communicationStatus;
        }

        public int GetIOAxisStatus(ref uint inputValue, ref uint outputValue, ref uint axisStatus)
        {
            SetTransmitFrameData(0x41);
            SendTransmitFrameData();

            byte[] bInputValue = new byte[4];
            byte[] bOutputValue = new byte[4];
            byte[] bAxisStatus = new byte[4];

            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bInputValue[i - 5] = responseFrameData[i];
                }

                for (int i = 9; i < 13; i++)
                {
                    bOutputValue[i - 9] = responseFrameData[i];
                }

                for (int i = 13; i < 17; i++)
                {
                    bAxisStatus[i - 13] = responseFrameData[i];
                }
            }

            inputValue = BitConverter.ToUInt32(bInputValue);
            outputValue = BitConverter.ToUInt32(bOutputValue);
            axisStatus = BitConverter.ToUInt32(bAxisStatus);

            return communicationStatus;
        }

        public int GetMotionStatus(ref int cmdPos, ref int actPos, ref int posErr, ref uint actVel, ref int currentRunPT)
        {
            SetTransmitFrameData(0x42);
            SendTransmitFrameData();

            byte[] bCmdPos = new byte[4];
            byte[] bActPos = new byte[4];
            byte[] bPosErr = new byte[4];
            byte[] bActVel = new byte[4];
            byte[] bCurrentRunPT = new byte[4];
            if (ReadResponseFrameData())
            {

                for (int i = 5; i < 9; i++)
                {
                    bCmdPos[i - 5] = responseFrameData[i];
                }

                for (int i = 9; i < 13; i++)
                {
                    bActPos[i - 9] = responseFrameData[i];
                }

                for (int i = 13; i < 17; i++)
                {
                    bPosErr[i - 13] = responseFrameData[i];
                }

                for (int i = 17; i < 21; i++)
                {
                    bActVel[i - 17] = responseFrameData[i];
                }

                for (int i = 21; i < 25; i++)
                {
                    bCurrentRunPT[i - 21] = responseFrameData[i];
                }
            }
            cmdPos = BitConverter.ToInt32(bCmdPos);
            actPos = BitConverter.ToInt32(bActPos);
            posErr = BitConverter.ToInt32(bPosErr);
            actVel = BitConverter.ToUInt32(bActVel);
            currentRunPT = BitConverter.ToInt32(bCurrentRunPT);

            return communicationStatus;
        }

        public int GetAllStatus(ref uint inputValue, ref uint outputValue, ref uint axisStatus, ref int cmdPos, ref int actPos, ref int posErr, ref uint actVel, ref int currentRunPT)
        {
            SetTransmitFrameData(0x43);
            SendTransmitFrameData();

            byte[] bInputValue = new byte[4];
            byte[] bOutputValue = new byte[4];
            byte[] bAxisStatus = new byte[4];
            byte[] bCmdPos = new byte[4];
            byte[] bActPos = new byte[4];
            byte[] bPosErr = new byte[4];
            byte[] bActVel = new byte[4];
            byte[] bCurrentRunPT = new byte[4];

            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bInputValue[i - 5] = responseFrameData[i];
                }

                for (int i = 9; i < 13; i++)
                {
                    bOutputValue[i - 9] = responseFrameData[i];
                }

                for (int i = 13; i < 17; i++)
                {
                    bAxisStatus[i - 13] = responseFrameData[i];
                }

                for (int i = 17; i < 21; i++)
                {
                    bCmdPos[i - 17] = responseFrameData[i];
                }

                for (int i = 21; i < 25; i++)
                {
                    bActPos[i - 21] = responseFrameData[i];
                }

                for (int i = 25; i < 29; i++)
                {
                    bPosErr[i - 25] = responseFrameData[i];
                }

                for (int i = 29; i < 33; i++)
                {
                    bActVel[i - 29] = responseFrameData[i];
                }

                for (int i = 33; i < 37; i++)
                {
                    bCurrentRunPT[i - 33] = responseFrameData[i];
                }
                inputValue = BitConverter.ToUInt32(bInputValue);
                outputValue = BitConverter.ToUInt32(bOutputValue);
                axisStatus = BitConverter.ToUInt32(bAxisStatus);
                cmdPos = BitConverter.ToInt32(bCmdPos);
                actPos = BitConverter.ToInt32(bActPos);
                posErr = BitConverter.ToInt32(bPosErr);
                actVel = BitConverter.ToUInt32(bActVel);
                currentRunPT = BitConverter.ToInt32(bCurrentRunPT);
            }
            return communicationStatus;
        }

        public int SetCommandPos(int cmdPosValue)
        {
            byte[] data = BitConverter.GetBytes(cmdPosValue);

            SetTransmitFrameData(0x50, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetCommandPos(ref int cmdPosValue)
        {
            SetTransmitFrameData(0x51);
            SendTransmitFrameData();

            byte[] bCmdPosValue = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bCmdPosValue[i - 5] = responseFrameData[i];
                }
            }
            cmdPosValue = BitConverter.ToInt32(bCmdPosValue);

            return communicationStatus;
        }

        public int SetActualPos(int actPosValue)
        {
            byte[] data = BitConverter.GetBytes(actPosValue);

            SetTransmitFrameData(0x52, data);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetActualPos(ref int actPosValue)
        {
            SetTransmitFrameData(0x53);
            SendTransmitFrameData();

            byte[] bActPosValue = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bActPosValue[i - 5] = responseFrameData[i];
                }
            }
            actPosValue = BitConverter.ToInt32(bActPosValue);

            return communicationStatus;
        }

        public int GetPosError(ref int posErr)
        {
            SetTransmitFrameData(0x54);
            SendTransmitFrameData();

            byte[] bPosErr = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bPosErr[i - 5] = responseFrameData[i];
                }
            }
            posErr = BitConverter.ToInt32(bPosErr);

            return communicationStatus;
        }

        public int GetActualVel(ref uint actVel)
        {
            SetTransmitFrameData(0x55);
            SendTransmitFrameData();

            byte[] bActVel = new byte[4];
            if (ReadResponseFrameData())
            {
                for (int i = 5; i < 9; i++)
                {
                    bActVel[i - 5] = responseFrameData[i];
                }
            }
            actVel = BitConverter.ToUInt32(bActVel);

            return communicationStatus;
        }

        public int ClearPosition()
        {
            SetTransmitFrameData(0x56);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int MovePause()
        {
            SetTransmitFrameData(0x58);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        #endregion

        #region Privates Functions
        private ushort CalcCheckSumbyAlgorithm(byte[] dataFrame, ulong DataLen)
        {
            const ushort POLYNOMIAL = 0XA001;
            ushort wCrc;
            ulong iByte, iBit;

            wCrc = 0xFFFF;
            for (iByte = 0; iByte < DataLen; iByte++)
            {
                wCrc ^= dataFrame[iByte];
                for (iBit = 0; iBit <= 7; iBit++)
                {
                    if ((wCrc & 0x0001) == 0x0001)
                    {
                        wCrc >>= 1;
                        wCrc ^= POLYNOMIAL;
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            return wCrc;
        }

        private void SetTransmitFrameData(byte frameType)
        {
            SetTransmitFrameData(frameType, null);
        }

        private void SetTransmitFrameData(byte frameType, byte[]? data)
        {
            List<byte> transmitData = new List<byte>
            {
                0xAA, // Header
                0xCC, // Header
                (byte)slaveId, // SlaveID
                frameType // FrameType
            };
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    transmitData.Add(data[i]); // Data
                }
            }

            byte[] CRCDataToCalc = MakeDataToCheckSumCalc(frameType, data);
            ushort uCrc = CalcCheckSumbyAlgorithm(CRCDataToCalc, (ulong)CRCDataToCalc.Length);
            checkSumBytes = uCrc.Uint16toByteArray();

            transmitData.Add(checkSumBytes[0]); //Checksum
            transmitData.Add(checkSumBytes[1]); //Checksum

            transmitData.Add(0xAA); // Tail
            transmitData.Add(0xEE);// Tail

            transmitFrameData = transmitData.ToArray();
        }

        private byte[] MakeDataToCheckSumCalc(byte frameType, byte[]? data)
        {
            List<byte> CRCDataToCalc = new List<byte>();

            CRCDataToCalc.Add((byte)slaveId);
            CRCDataToCalc.Add(frameType);

            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    CRCDataToCalc.Add(data[i]);
                }
            }

            return CRCDataToCalc.ToArray();
        }

        private void SendTransmitFrameData()
        {
            serialPort.Write(transmitFrameData, 0, transmitFrameData.Length);
        }

        private bool ReadResponseFrameData(int timeoutMs = 2000)
        {
            bool result = false;
            responseFrameData = new List<byte>();
            int processTime = Environment.TickCount;
            byte[] lastBytes = new byte[2];
            do
            {
                if (Environment.TickCount - processTime > timeoutMs)
                {
                    // TODO: Handle timeout exception
                    throw new Exception("Data received timeout");
                }

                if (!serialPort.IsOpen) return false;
                int bytes = serialPort.BytesToRead;
                byte[] buffer = new byte[bytes];
                serialPort.Read(buffer, 0, bytes);

                responseFrameData.AddRange(buffer);

                if (responseFrameData.Count >= 2)
                {
                    lastBytes[0] = responseFrameData[responseFrameData.Count - 2];
                    lastBytes[1] = responseFrameData[responseFrameData.Count - 1];
                }
            }
            while ((lastBytes[0] == 0xAA & lastBytes[1] == 0xEE) == false);


            byte[] dataResponse = new byte[responseFrameData.Count - 8];
            for (int i = 4; i < responseFrameData.Count - 4; i++)
            {
                dataResponse[i - 4] = responseFrameData[i];
            }
            byte[] dataToCheckSumCalc = MakeDataToCheckSumCalc(responseFrameData[3], dataResponse);
            ushort uCheckSumResponse = CalcCheckSumbyAlgorithm(dataToCheckSumCalc, (ulong)dataToCheckSumCalc.Length);
            byte[] bCheckSumResponse = uCheckSumResponse.Uint16toByteArray();
            if (bCheckSumResponse[0] == responseFrameData[responseFrameData.Count - 4] && bCheckSumResponse[1] == responseFrameData[responseFrameData.Count - 3])
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Privates
        SerialPort serialPort;
        string portName;
        int slaveId;
        int baudRate;

        byte[] checkSumBytes;
        byte[] transmitFrameData;
        List<byte> responseFrameData;
        #endregion
    }
}

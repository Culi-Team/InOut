using System.Net.Sockets;
using System.Net;
using System.Text;

namespace EQX.Motion.ThirdParty.Fastech.PlusE
{
    public class EziPlusEDIOLib
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
        public EziPlusEDIOLib(int index, string name)
        {
            iPAddress = IPAddress.Parse($"192.168.1.{index}");
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

            try
            {
                tcpClient.Connect(iPAddress, 2002);
                return tcpClient.Connected;
            }
            catch
            {
                return false;
            }
        }

        public bool Disconnect()
        {
            tcpClient.Close();

            return !tcpClient.Connected;
        }

        #region IO Board
        public int GetInput(ref uint inputStatus, ref uint latchStatus)
        {
            byte[] bInputStatus = new byte[4];
            byte[] bLatchStatus = new byte[4];

            SetTransmitFrameData(0xC0);
            SendTransmitFrameData();

            ReadResponseFrameData();

            for (int i = 6; i < 10; i++)
            {
                bInputStatus[i - 6] = responseFrameData[i];
            }
            for (int i = 10; i < 14; i++)
            {
                bLatchStatus[i - 10] = responseFrameData[i];
            }
            inputStatus = BitConverter.ToUInt32(bInputStatus);
            latchStatus = BitConverter.ToUInt32(bLatchStatus);
            return communicationStatus;
        }

        public int ClearLatch(uint latchStatus)
        {
            byte[] bLatchStatus = BitConverter.GetBytes(latchStatus);

            SetTransmitFrameData(0xC1, bLatchStatus.Length, bLatchStatus);
            SendTransmitFrameData();

            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetLatchCount(byte latchCount, ref uint latchStatus)
        {
            byte[] bLatchStatus = new byte[4];

            byte[] bLatchCount = new byte[1] { latchCount };

            SetTransmitFrameData(0xC2, bLatchCount.Length, bLatchCount);
            SendTransmitFrameData();
            ReadResponseFrameData();
            for (int i = 6; i < 10; i++)
            {
                bLatchStatus[i - 6] = responseFrameData[i];
            }

            latchStatus = BitConverter.ToUInt32(bLatchStatus);

            return communicationStatus;
        }

        public int GetLatchCountAll(ref uint[] data)
        {
            data = new uint[16];

            SetTransmitFrameData(0xC3);
            SendTransmitFrameData();
            ReadResponseFrameData();

            for (int i = 0; i < 16; i++)
            {
                byte[] bLatchData = new byte[4];
                for (int j = 6 + 4 * i; j < 10 + 4 * i; j++)
                {
                    bLatchData[j - (6 + 4 * i)] = responseFrameData[j];
                }
                data[i] = BitConverter.ToUInt32(bLatchData);
            }

            return communicationStatus;
        }

        public int ClearLatchCount(uint latchCount)
        {
            byte[] bLatchCount = BitConverter.GetBytes(latchCount);
            SetTransmitFrameData(0xC4, bLatchCount.Length, bLatchCount);
            SendTransmitFrameData();
            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetOutput(ref ulong outputStatus)
        {
            byte[] bOutputStatus = new byte[8];
            SetTransmitFrameData(0xC5);
            SendTransmitFrameData();
            ReadResponseFrameData();

            for (int i = 6; i < 14; i++)
            {
                bOutputStatus[i - 6] = responseFrameData[i];
            }

            outputStatus = BitConverter.ToUInt64(bOutputStatus);

            return communicationStatus;
        }

        public int SetOutput(uint setOutputData, uint resetOutputData)
        {
            byte[] bSetOutputData = BitConverter.GetBytes(setOutputData);
            byte[] bResetOutputData = BitConverter.GetBytes(resetOutputData);

            byte[] data = bSetOutputData.Concat(bResetOutputData).ToArray();

            SetTransmitFrameData(0xC6, data.Length, data);
            SendTransmitFrameData();
            ReadResponseFrameData();

            return communicationStatus;
        }

        public int SetTrigger(uint count, ushort blank1, ushort ontime, ushort blank2, ushort period, byte outputNo)
        {
            byte[] bCount = BitConverter.GetBytes(count);
            byte[] bBlank1 = BitConverter.GetBytes(blank1);
            byte[] bOnTime = BitConverter.GetBytes(ontime);
            byte[] bBlank2 = BitConverter.GetBytes(blank2);
            byte[] bPeriod = BitConverter.GetBytes(period);
            byte[] bOutputNo = new byte[1] { outputNo };

            byte[] data = bCount.Concat(bBlank1).Concat(bOnTime).Concat(bBlank2).Concat(bPeriod).Concat(bOutputNo).ToArray();

            SetTransmitFrameData(0xC7, data.Length, data);
            SendTransmitFrameData();
            ReadResponseFrameData();

            return communicationStatus;
        }

        public int GetTrigger(byte outputCount, ref uint triggerOutput)
        {
            byte[] bOutputCount = new byte[1] { outputCount };

            SetTransmitFrameData(0xC9, bOutputCount.Length, bOutputCount);
            SendTransmitFrameData();
            ReadResponseFrameData();

            byte[] bTriggerOutput = new byte[4];

            for (int i = 6; i < 10; i++)
            {
                bTriggerOutput[i - 6] = responseFrameData[i];
            }

            triggerOutput = BitConverter.ToUInt32(bTriggerOutput);

            return communicationStatus;
        }

        public int GetIOLevel(ref uint ioLevel)
        {
            SetTransmitFrameData(0xCA);
            SendTransmitFrameData();
            ReadResponseFrameData();

            byte[] bIOLevel = new byte[4];
            for (int i = 6; i < 10; i++)
            {
                bIOLevel[i - 6] = responseFrameData[i];
            }

            ioLevel = BitConverter.ToUInt32(bIOLevel);

            return communicationStatus;
        }

        public int SetIOLevel(uint ioLevel)
        {
            byte[] bIOLevel = BitConverter.GetBytes(ioLevel);
            SetTransmitFrameData(0xCB, bIOLevel.Length, bIOLevel);
            SendTransmitFrameData();
            ReadResponseFrameData();

            return communicationStatus;
        }

        public int LoadIOLevel()
        {
            SetTransmitFrameData(0xCC);
            SendTransmitFrameData();
            ReadResponseFrameData();

            return communicationStatus;
        }

        public int SaveIOLevel()
        {
            SetTransmitFrameData(0xCD);
            SendTransmitFrameData();
            ReadResponseFrameData();

            return communicationStatus;
        }
        #endregion
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
            if (data != null)
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

namespace EQX.Motion.ThirdParty.Fastech.PlusR
{
    internal partial class EziPlusRMotionLib
    {
        private readonly Dictionary<int, string> BoardNameList = new Dictionary<int, string>()
        {
             {1,"Ezi-SERVO Plus-R-ST" },
             {3,"Ezi-SERVO Plus-R-ADC" },
             {10,"Ezi-MotionLink" },
             {11,"Ezi-MotionLink2" },
             {20,"Ezi-STEP Plus-R-ST" },
             {30,"Ezi-SERVO Plus-R-ALL" },
             {35,"Ezi-SERVO Plus-R-ALL Abs" },
             {40,"Ezi-STEP Plus-R-ALL" },
             {50,"Ezi-SERVO Plus-R-Mini" },
             {60,"Ezi-STEP Plus-R-Mini" },
             {31,"Ezi-SERVO Plus-R-ALL 60i" },
             {100,"Ezi-SERVO II Plus-R-ST" },
             {102,"S-SERVO Plus-R-ST" },
             {150,"S-SERVO Plus-R-ST" },
             {151,"Ezi-IO Plus-R-IN32" },
             {160,"Ezi-IO Plus-R-OUT16" },
             {161,"Ezi-IO Plus-R-OUT32" },
             {155,"Ezi-IO Plus-R-I8O8" },
             {156,"Ezi-IO Plus-R-I16O16" },
        };

        private readonly Dictionary<byte, string> CommunicationStatuses = new Dictionary<byte, string>()
        {
            {0x00,"Communication is normal" },
            {0x80,"Frame Type Error" },
            {0x81,"Data error. ROM data read/write error" },
            {0x82,"Received Frame Error" },
            {0x85,"Running Command Failure" },
            {0x86,"RESET Failure" },
            {0xAA,"CRC Error" },
        };

        public const int DIR_INC = 1;
        public const int DIR_DEC = 0;
    }
}

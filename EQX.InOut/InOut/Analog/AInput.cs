using EQX.Core.InOut;

namespace EQX.InOut.InOut.Analog
{
    public class AInput : IAInput
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public double Volt => _aInputDevice.GetVolt(Id);

        public double Current => _aInputDevice.GetCurrent(Id);

        public IAInputParameter Parameter { get; }

        public AInput(int id,string name, IAInputDevice aInputDevice)
        {
            Id = id;
            Name = name;
            _aInputDevice = aInputDevice;
        }

        private IAInputDevice _aInputDevice;
    }
}

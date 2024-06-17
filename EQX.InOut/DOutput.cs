using EQX.Core.InOut;

namespace EQX.InOut
{
    public class DOutput : IDOutput
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public bool Value { get; set; }
    }
}

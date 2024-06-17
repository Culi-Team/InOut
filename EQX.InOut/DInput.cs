using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut
{
    public class DInput : IDInput
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public bool Value { get; internal set; }
    }
}

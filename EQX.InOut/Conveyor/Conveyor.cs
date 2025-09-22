using EQX.Core;
using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut
{
    public class Conveyor : ConveyorBase
    {
        public Conveyor(IDInput inError, IDOutput outRun, IDOutput outRunReverse, IDOutput outStop)
            : base(inError, outRun, outRunReverse, outStop)
        {
        }

        #region Override methods
        protected override void RunAction()
        {
            OutRun.Value = true;
        }

        protected override void RunReverseAction()
        {
            OutReverseRun.Value = true;
        }

        protected override void StopAction()
        {
            if (OutStop != null)
            {
                OutStop!.Value = true;
            }
            else
            {
                OutRun!.Value = false;
            }
        }

        #endregion
    }
}

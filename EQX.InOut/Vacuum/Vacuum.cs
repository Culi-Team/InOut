using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.InOut;
using log4net;

namespace EQX.InOut
{
    public class Vacuum : VacuumBase
    {
        public Vacuum(IDOutput? suctionOutput, IDOutput? blowOutput = null, IDInput? vacuumDetectInput = null)
            : base(suctionOutput, blowOutput, vacuumDetectInput)
        {
        }

        protected override void SuctionOnAction()
        {
            LogManager.GetLogger($"{Name}").Debug("Vacuum On");
            base.SuctionOnAction();
        }

        protected override void BlowOnAction()
        {
            LogManager.GetLogger($"{Name}").Debug("Blow On");
            base.BlowOnAction();
        }

        protected override void OffAction()
        {
            LogManager.GetLogger($"{Name}").Debug("Vacuum Off");
            base.OffAction();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.InOut;

namespace EQX.InOut
{
    public class SimulationVacuum : VacuumBase
    {
        public SimulationVacuum(IDOutput? suctionOutput, IDOutput? blowOutput = null, IDInput? vacuumDetectInput = null)
            : base(suctionOutput, blowOutput, vacuumDetectInput)
        {
        }

        protected override void SuctionOnAction()
        {
            base.SuctionOnAction();
            SetSimInput(true);
        }

        protected override void BlowOnAction()
        {
            base.BlowOnAction();
            SetSimInput(false);
        }

        protected override void OffAction()
        {
            base.OffAction();
            SetSimInput(false);
        }

        private void SetSimInput(bool value)
        {
            if (InVacuum != null)
            {
                SimulationInputSetter.SetSimInput(InVacuum, value);
            }
        }
    }
}

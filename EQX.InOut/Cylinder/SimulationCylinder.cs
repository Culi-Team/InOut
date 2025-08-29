using EQX.Core.InOut;

namespace EQX.InOut
{
    public class SimulationCylinder : CylinderBase
    {
        #region Constructors
        public SimulationCylinder(List<IDInput> inForward, List<IDInput> inBackward, IDOutput? outForward, IDOutput? outBackward)
            : base(inForward, inBackward, outForward, outBackward)
        {
        }
        #endregion

        #region Override methods
        protected override void ForwardAction()
        {
            if (OutForward != null & OutBackward != null)
            {
                // Both input not null
                OutForward!.Value = true;
                OutBackward!.Value = false;

                SetSimInput(InForward, true);
                SetSimInput(InBackward, false);
            }
            else if (OutForward == null & OutBackward == null)
            {
                // Both input is null
                return;
            }
            else if (OutBackward != null)
            {
                // Only backward is not null
                OutBackward!.Value = false;

                SetSimInput(InBackward, false);
                SetSimInput(InForward, true);
            }
            else
            {
                // Only forward is not null
                OutForward!.Value = true;

                SetSimInput(InForward, true);
                SetSimInput(InBackward, false);
            }
        }

        protected override void BackwardAction()
        {
            if (OutForward != null & OutBackward != null)
            {
                // Both input not null
                OutBackward!.Value = true;
                OutForward!.Value = false;

                SetSimInput(InBackward, true);
                SetSimInput(InForward, false);
            }
            else if (OutForward == null & OutBackward == null)
            {
                // Both input is null
                return;
            }
            else if (OutBackward != null)
            {
                // Only backward is not null
                OutBackward!.Value = true;
                SetSimInput(InBackward, true);
                SetSimInput(InForward, false);
            }
            else
            {
                // Only forward is not null
                OutForward!.Value = false;
                SetSimInput(InForward, false);
                SetSimInput(InBackward, true);
            }
        }
        #endregion

        #region Private methods
        private void SetSimInput(List<IDInput> input, bool value)
        {
            foreach (var inp in input)
            {
                SimulationInputSetter.SetSimInput(inp, value);
            }
        }
        #endregion

        #region Privates
        #endregion
    }
}

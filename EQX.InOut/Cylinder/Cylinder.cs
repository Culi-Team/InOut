using EQX.Core.InOut;
using EQX.Core.Sequence;

namespace EQX.InOut
{
    public class Cylinder : CylinderBase
    {
        public Cylinder(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward)
            : base(inForward, inBackward, outForward, outBackward)
        {
        }

        #region Override methods
        protected override void ForwardAction()
        {
            if (OutForward != null & OutBackward != null)
            {
                // Both input not null
                OutForward!.Value = true;
                OutBackward!.Value = false;
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
            }
            else
            {
                // Only forward is not null
                OutForward!.Value = true;
            }
        }

        protected override void BackwardAction()
        {
            if (OutForward != null & OutBackward != null)
            {
                // Both input not null
                OutBackward!.Value = true;
                OutForward!.Value = false;
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
            }
            else
            {
                // Only forward is not null
                OutForward!.Value = false;
            }
        }
        #endregion
    }
}

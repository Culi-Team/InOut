using EQX.Core.InOut;

namespace EQX.InOut
{
    public class CylinderBase : ICylinder
    {
        #region Properties
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public bool IsForward
        {
            get
            {
                if (InForward != null & InBackward != null)
                {
                    // Both input not null
                    return InForward!.Value & !InBackward!.Value;
                }
                else if (InForward == null & InBackward == null)
                {
                    // Both input is null
                    return false;
                }
                else if (InBackward != null)
                {
                    // Only backward is not null
                    return !InBackward!.Value;
                }
                else
                {
                    // Only forward is not null
                    return InForward!.Value;
                }
            }
        }

        public bool IsBackward
        {
            get
            {
                if (InForward != null & InBackward != null)
                {
                    // Both input not null
                    return !InForward!.Value & InBackward!.Value;
                }
                else if (InForward == null & InBackward == null)
                {
                    // Both input is null
                    return false;
                }
                else if (InBackward != null)
                {
                    // Only backward is not null
                    return InBackward!.Value;
                }
                else
                {
                    // Only forward is not null
                    return !InForward!.Value;
                }
            }
        }
        #endregion

        #region Constructors
        public CylinderBase(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward)
        {
            InForward = inForward;
            InBackward = inBackward;
            OutForward = outForward;
            OutBackward = outBackward;
        }
        #endregion

        #region Public methods
        public virtual void Forward()
        {
        }

        public virtual void Backward()
        {
        }
        #endregion

        #region Protected
        protected IDOutput? OutForward { get; }
        protected IDOutput? OutBackward { get; }
        protected IDInput? InForward { get; }
        protected IDInput? InBackward { get; }
        #endregion

        public override string ToString() => Name;
    }
}

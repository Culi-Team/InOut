using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;
using log4net;

namespace EQX.InOut
{
    public class CylinderBase : ObservableObject, ICylinder
    {
        #region Properties
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public ECylinderType CylinderType { get; set; }

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

            if (InForward != null) InForward.ValueUpdated += InForward_ValueUpdated;
            if (InBackward != null) InBackward.ValueUpdated += InForward_ValueUpdated;
        }

        private void InForward_ValueUpdated(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsForward));
            OnPropertyChanged(nameof(IsBackward));
        }
        #endregion

        #region Public methods
        public void Forward()
        {
            LogManager.GetLogger($"{Name}").Debug("Forward");
            ForwardAction();
        }

        public void Backward()
        {
            LogManager.GetLogger($"{Name}").Debug("Backward");
            BackwardAction();
        }

        protected virtual void ForwardAction() { }
        protected virtual void BackwardAction() { }
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

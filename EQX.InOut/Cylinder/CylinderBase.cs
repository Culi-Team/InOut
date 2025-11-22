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

        private string? interlockKey;
        public string? InterlockKey
        {
            get => interlockKey;
            set => SetProperty(ref interlockKey, value);
        }

        private Func<bool>? interlockCondition;
        public Func<bool>? InterlockCondition
        {
            get => interlockCondition;
            set => SetProperty(ref interlockCondition, value);
        }

        public bool IsInterlockSatisfied()
        {
            return InterlockCondition?.Invoke() ?? true;
        }

        public event EventHandler? StateChanged;

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsForward
        {
            get
            {
                if (InForward != null & InBackward != null)
                {
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
        public CylinderBase(IDInput? inForwards, IDInput? inBackwards, IDOutput? outForward, IDOutput? outBackward, Func<bool> forwardInterlock, Func<bool> backwardInterlock)
        {
            InForward = inForwards;
            InBackward = inBackwards;
            OutForward = outForward;
            OutBackward = outBackward;
            ForwardInterlock = forwardInterlock;
            BackwardInterlock = backwardInterlock;


            if (InForward != null)
            {
                InForward.ValueUpdated += InForward_ValueUpdated;

            }
            if (InBackward != null)
            {
                InBackward.ValueUpdated += InForward_ValueUpdated;
            }
        }

        private void InForward_ValueUpdated(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsForward));
            OnPropertyChanged(nameof(IsBackward));
            OnStateChanged();
        }
        #endregion


        public Func<bool> ForwardInterlock;
        public Func<bool> BackwardInterlock;

        #region Public methods
        public void Forward()
        {
            if (ForwardInterlock != null && ForwardInterlock() == false) return;
            LogManager.GetLogger($"{Name}").Debug("Forward");
            ForwardAction();
            OnStateChanged();
        }

        public void Backward()
        {
            if (BackwardInterlock != null && BackwardInterlock() == false) return;
            LogManager.GetLogger($"{Name}").Debug("Backward");
            BackwardAction();
            OnStateChanged();
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

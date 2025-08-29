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
                    bool isForward = true;
                    bool isBackward = true;

                    foreach (var input in InForward!)
                    {
                        isForward &= input.Value;
                    }
                    foreach (var input in InBackward!)
                    {
                        isBackward &= input.Value;
                    }

                    return isForward & !isBackward;
                }
                else if (InForward == null & InBackward == null)
                {
                    // Both input is null
                    return false;
                }
                else if (InBackward != null)
                {
                    // Only backward is not null
                    bool isBackward = true;

                    foreach (var input in InBackward)
                    {
                        isBackward &= input.Value;
                    }
                    return !isBackward;
                }
                else
                {
                    // Only forward is not null
                    bool isForward = true;
                    foreach (var input in InForward!)
                    {
                        isForward &= input.Value;
                    }
                    return isForward;
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
                    bool isForward = true;
                    bool isBackward = true;

                    foreach (var input in InForward!)
                    {
                        isForward &= input.Value;
                    }
                    foreach (var input in InBackward!)
                    {
                        isBackward &= input.Value;
                    }

                    return !isForward & isBackward;
                }
                else if (InForward == null & InBackward == null)
                {
                    // Both input is null
                    return false;
                }
                else if (InBackward != null)
                {
                    // Only backward is not null
                    bool isBackward = true;

                    foreach (var input in InBackward)
                    {
                        isBackward &= input.Value;
                    }
                    return isBackward;
                }
                else
                {
                    // Only forward is not null
                    bool isForward = true;
                    foreach (var input in InForward!)
                    {
                        isForward &= input.Value;
                    }
                    return !isForward;
                }
            }
        }
        #endregion

        #region Constructors
        public CylinderBase(List<IDInput>? inForwards, List<IDInput>? inBackwards, IDOutput? outForward, IDOutput? outBackward)
        {
            InForward = inForwards;
            InBackward = inBackwards;
            OutForward = outForward;
            OutBackward = outBackward;

            if (InForward != null)
            {
                foreach (var input in InForward)
                {
                    input.ValueUpdated += InForward_ValueUpdated;
                }
            }
            if (InBackward != null)
            {
                foreach (var input in InBackward)
                {
                    input.ValueUpdated += InForward_ValueUpdated;
                }
            }
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
        protected List<IDInput>? InForward { get; }
        protected List<IDInput>? InBackward { get; }
        #endregion

        public override string ToString() => Name;
    }
}

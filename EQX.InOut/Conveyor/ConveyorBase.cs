using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;
using log4net;

namespace EQX.InOut
{
    public class ConveyorBase : ObservableObject, IConveyor
    {
        #region Properties
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public bool IsError
        {
            get
            {
                if (InError != null)
                    return InError.Value;
                else
                    return false; 
            }
        }
        #endregion

        #region Constructor
        public ConveyorBase(IDInput? inError, IDOutput? outRun, IDOutput? outReverseRun, IDOutput? outStop)
        {
            InError = inError;
            OutRun = outRun;
            OutReverseRun = outReverseRun;
            OutStop = outStop;
            if (InError != null)
            {
                InError.ValueUpdated += InError_ValueUpdated;
            }
        }

        private void InError_ValueUpdated(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(InError));
        }
        #endregion

        #region Public Methods
        public void Run()
        {
            LogManager.GetLogger($"{Name}").Debug("Run");
            RunAction();
        }

        public void RunReverse()
        {
            LogManager.GetLogger($"{Name}").Debug("Reverse Run");
            RunReverseAction();
        }

        public void Stop()
        {
            LogManager.GetLogger($"{Name}").Debug("Stop");
            StopAction();
        }

        protected virtual void RunAction() { }
        protected virtual void RunReverseAction() { }
        protected virtual void StopAction() { }
        #endregion

        #region Protected
        protected IDInput InError { get; }
        protected IDOutput OutRun { get; }
        protected IDOutput OutStop { get; }
        protected IDOutput OutReverseRun { get; }
        #endregion

        public override string ToString() => Name;
    }
}

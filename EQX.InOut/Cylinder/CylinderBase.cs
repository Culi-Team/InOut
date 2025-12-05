using System.Diagnostics;
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
            set
            {
                if(SetProperty(ref interlockCondition, value)) 
                { 
                    OnPropertyChanged(nameof(IsInterlockSatisfiedState));
                }
            }
        }

        public bool IsInterlockSatisfied()
        {
            return InterlockCondition?.Invoke() ?? true;
        }

        public bool IsInterlockSatisfiedState => IsInterlockSatisfied();

        public event EventHandler? StateChanged;

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public Func<bool>? ForwardInterlock { get; set; }
        public Func<bool>? BackwardInterlock { get; set; }

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

        public bool HasForwardSensor => InForward != null;

        public bool HasBackwardSensor => InBackward != null;

        private double waitTimeOutOccurredSeconds = 6;

        public double WaitTimeOutOccurredSeconds
        {
            get => waitTimeOutOccurredSeconds;
            set
            {
                if (SetProperty(ref waitTimeOutOccurredSeconds, value))
                {
                    if (!forwardTimeoutOverridden)
                    {
                        ForwardTimeoutSeconds = value;
                    }

                    if (!backwardTimeoutOverridden)
                    {
                        BackwardTimeoutSeconds = value;
                    }
                }
            }
        }

        private double forwardTimeoutSeconds;

        public double ForwardTimeoutSeconds
        {
            get => forwardTimeoutSeconds;
            set
            {
                forwardTimeoutOverridden = true;
                SetProperty(ref forwardTimeoutSeconds, value);
            }
        }

        private double backwardTimeoutSeconds;

        public double BackwardTimeoutSeconds
        {
            get => backwardTimeoutSeconds;
            set
            {
                backwardTimeoutOverridden = true;
                SetProperty(ref backwardTimeoutSeconds, value);
            }
        }

        private bool isActuating;

        public bool IsActuating
        {
            get => isActuating;
            private set => SetProperty(ref isActuating, value);
        }

        private bool isForwarding;

        public bool IsForwarding
        {
            get => isForwarding;
            private set => SetProperty(ref isForwarding, value);
        }

        private bool isBackwarding;

        public bool IsBackwarding
        {
            get => isBackwarding;
            private set => SetProperty(ref isBackwarding, value);
        }
        #endregion

        #region Constructors
        public CylinderBase(IDInput? inForwards, IDInput? inBackwards, IDOutput? outForward, IDOutput? outBackward)
        {
            InForward = inForwards;
            InBackward = inBackwards;
            OutForward = outForward;
            OutBackward = outBackward;

            forwardTimeoutSeconds = waitTimeOutOccurredSeconds;
            backwardTimeoutSeconds = waitTimeOutOccurredSeconds;


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
            OnPropertyChanged(nameof(IsInterlockSatisfiedState));
            OnStateChanged();
        }
        #endregion

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

        public async Task<bool> MoveForwardWithTimeoutAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            if (!await operationLock.WaitAsync(0, cancellationToken))
            {
                return false;
            }

            try
            {
                SetOperationState(isForwarding: true, isBackwarding: false);
                Forward();
                var actualTimeout = timeout ?? TimeSpan.FromSeconds(ForwardTimeoutSeconds);
                return await WaitForSensorAsync(
                    () => IsForward,
                    HasForwardSensor,
                    actualTimeout,
                    "forward",
                    elapsed => ForwardElapsedSeconds = elapsed.TotalSeconds,
                    cancellationToken);
            }
            finally
            {
                ResetOperationState();
                operationLock.Release();
            }
        }

        public async Task<bool> MoveBackwardWithTimeoutAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            if (!await operationLock.WaitAsync(0, cancellationToken))
            {
                return false;
            }

            try
            {
                SetOperationState(isForwarding: false, isBackwarding: true);
                Backward();
                var actualTimeout = timeout ?? TimeSpan.FromSeconds(BackwardTimeoutSeconds);
                return await WaitForSensorAsync(
                    () => IsBackward,
                    HasBackwardSensor,
                    actualTimeout,
                    "backward",
                    elapsed => BackwardElapsedSeconds = elapsed.TotalSeconds,
                    cancellationToken);
            }
            finally
            {
                ResetOperationState();
                operationLock.Release();
            }
        }

        private double forwardElapsedSeconds;

        public double ForwardElapsedSeconds
        {
            get => forwardElapsedSeconds;
            private set => SetProperty(ref forwardElapsedSeconds, value);
        }

        private double backwardElapsedSeconds;

        public double BackwardElapsedSeconds
        {
            get => backwardElapsedSeconds;
            private set => SetProperty(ref backwardElapsedSeconds, value);
        }

        protected virtual void ForwardAction() { }
        protected virtual void BackwardAction() { }
        #endregion

        #region Protected
        protected IDOutput? OutForward { get; }
        protected IDOutput? OutBackward { get; }
        protected IDInput? InForward { get; }
        protected IDInput? InBackward { get; }

        private readonly SemaphoreSlim operationLock = new(1, 1);
        private bool forwardTimeoutOverridden;
        private bool backwardTimeoutOverridden;

        private void SetOperationState(bool isForwarding, bool isBackwarding)
        {
            IsActuating = true;
            IsForwarding = isForwarding;
            IsBackwarding = isBackwarding;
        }

        private void ResetOperationState()
        {
            IsActuating = false;
            IsForwarding = false;
            IsBackwarding = false;
        }

        private async Task<bool> WaitForSensorAsync(
            Func<bool> completed,
            bool hasSensor,
            TimeSpan timeout,
            string direction,
            Action<TimeSpan>? onSuccess,
            CancellationToken cancellationToken)
        {
            if (!hasSensor)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
                return true;
            }

            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < timeout)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (completed())
                {
                    onSuccess?.Invoke(stopwatch.Elapsed);
                    return true;
                }

                await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            }

            LogManager.GetLogger($"{Name}").Error($"{Name} {direction} timeout after {timeout.TotalSeconds:0.##}s");
            return false;
        }
        public override string ToString() => Name;
        #endregion
    }
}

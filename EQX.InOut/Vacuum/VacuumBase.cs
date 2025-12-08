using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;

namespace EQX.InOut
{
    public class VacuumBase : ObservableObject, IVacuum
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; } = string.Empty;
        public event EventHandler? StateChanged;

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsSuctionOn => OutSuction?.Value ?? false;
        public bool IsBlowOn => OutBlow?.Value ?? false;
        public bool IsVacuumDetected => InVacuum?.Value ?? false;
        public bool HasBlow => OutBlow != null;
        public bool HasVacuumDetect => InVacuum != null;
        public bool IsDetectDurationActive => _suctionStartTime.HasValue && !_hasRecordedDetectDuration;
        public TimeSpan LastVacuumDetectDuration => _lastVacuumDetectDuration;

        public string LastVacuumDetectDurationText => _lastVacuumDetectDuration == TimeSpan.Zero
            ? "-"
            : $"{_lastVacuumDetectDuration.TotalSeconds:0.00} s";

        public VacuumBase(IDOutput? suctionOutput, IDOutput? blowOutput = null, IDInput? vacuumDetectInput = null)
        {
            OutSuction = suctionOutput;
            OutBlow = blowOutput;
            InVacuum = vacuumDetectInput;

            if (InVacuum != null)
            {
                InVacuum.ValueUpdated += (_, _) => OnVacuumDetectChanged();
            }
        }

        public void SuctionOn()
        {
            SuctionOnAction();
            NotifyStateChanged();
        }

        public void BlowOn()
        {
            BlowOnAction();
            NotifyStateChanged();
        }

        public void Off()
        {
            OffAction();
            NotifyStateChanged();
        }

        protected virtual void SuctionOnAction()
        {
            _suctionStartTime = DateTimeOffset.UtcNow;
            _hasRecordedDetectDuration = false;
            ResetDetectDuration();
            SetOutput(OutSuction, true);
            SetOutput(OutBlow, false);
            RecordDetectIfAlreadyReached();
            OnPropertyChanged(nameof(IsDetectDurationActive));
        }

        protected virtual void BlowOnAction()
        {
            _suctionStartTime = null;
            SetOutput(OutBlow, true);
            SetOutput(OutSuction, false);
            OnPropertyChanged(nameof(IsDetectDurationActive));
        }

        protected virtual void OffAction()
        {
            _suctionStartTime = null;
            SetOutput(OutSuction, false);
            SetOutput(OutBlow, false);
        }

        protected IDOutput? OutSuction { get; }
        protected IDOutput? OutBlow { get; }
        protected IDInput? InVacuum { get; }

        public override string ToString() => Name;

        private void SetOutput(IDOutput? output, bool value)
        {
            if (output != null)
            {
                output.Value = value;
            }
        }

        private void NotifyStateChanged()
        {
            OnPropertyChanged(nameof(IsSuctionOn));
            OnPropertyChanged(nameof(IsBlowOn));
            OnStateChanged();
        }

        public void UpdateDetectDuration()
        {
            if (!_suctionStartTime.HasValue || _hasRecordedDetectDuration)
            {
                return;
            }

            _lastVacuumDetectDuration = DateTimeOffset.UtcNow - _suctionStartTime.Value;
            OnPropertyChanged(nameof(LastVacuumDetectDuration));
            OnPropertyChanged(nameof(LastVacuumDetectDurationText));
        }

        private void OnVacuumDetectChanged()
        {

            OnPropertyChanged(nameof(IsVacuumDetected));
            if (IsVacuumDetected && _suctionStartTime.HasValue && !_hasRecordedDetectDuration)
            {
                _lastVacuumDetectDuration = DateTimeOffset.UtcNow - _suctionStartTime.Value;
                _hasRecordedDetectDuration = true;
                OnPropertyChanged(nameof(LastVacuumDetectDuration));
                OnPropertyChanged(nameof(LastVacuumDetectDurationText));
            }
            OnStateChanged();
        }
        private void ResetDetectDuration()
        {
            _lastVacuumDetectDuration = TimeSpan.Zero;
            OnPropertyChanged(nameof(LastVacuumDetectDuration));
            OnPropertyChanged(nameof(LastVacuumDetectDurationText));
        }

        private void RecordDetectIfAlreadyReached()
        {
            if (IsVacuumDetected)
            {
                _lastVacuumDetectDuration = TimeSpan.Zero;
                _hasRecordedDetectDuration = true;
                OnPropertyChanged(nameof(LastVacuumDetectDuration));
                OnPropertyChanged(nameof(LastVacuumDetectDurationText));
            }
        }

        private DateTimeOffset? _suctionStartTime;
        private TimeSpan _lastVacuumDetectDuration;
        private bool _hasRecordedDetectDuration;
    }
}

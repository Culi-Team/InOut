using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmFlagSimulationWindow
{
    public class FlagItemViewModel : ObservableObject
    {
        private readonly Func<bool> _getter;
        private readonly Action<bool>? _setter;
        private readonly Func<bool>? _manualStateGetter;
        private bool _isSynchronizing;
        private bool _value;
        private bool _isManual;

        public FlagItemViewModel(int index, string displayName, Func<bool> getter, Action<bool>? setter, Func<bool>? manualStateGetter)
        {
            Index = index;
            DisplayName = displayName;
            IsEditable = setter != null;
            _getter = getter;
            _setter = setter;
            _manualStateGetter = manualStateGetter;
        }

        public int Index { get; }

        public string DisplayName { get; }

        public bool IsEditable { get; }

        public bool IsManual
        {
            get => _isManual;
            private set => SetProperty(ref _isManual, value);
        }

        public bool Value
        {
            get => _value;
            set
            {
                if (SetProperty(ref _value, value))
                {
                        if (!_isSynchronizing && IsEditable)
                        {
                            _setter?.Invoke(value);
                            if (_manualStateGetter != null)
                            {
                                IsManual = _manualStateGetter();
                            }
                        }
                }
            }
        }

        public void Refresh()
        {
            _isSynchronizing = true;
            try
            {
                Value = _getter();
                if (_manualStateGetter != null)
                {
                    IsManual = _manualStateGetter();
                }
            }
            finally
            {
                _isSynchronizing = false;
            }
        }
    }
}

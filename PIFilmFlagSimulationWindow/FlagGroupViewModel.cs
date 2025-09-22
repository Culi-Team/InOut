using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PIFilmFlagSimulationWindow
{
    public class FlagGroupViewModel : ObservableObject
    {
        private readonly System.Action? _clearInputsAction;
        private readonly System.Action? _clearOutputsAction;
        private RelayCommand? _clearInputsCommand;
        private RelayCommand? _clearOutputsCommand;

        public FlagGroupViewModel(string name, System.Action? clearInputsAction, System.Action? clearOutputsAction)
        {
            Name = name;
            _clearInputsAction = clearInputsAction;
            _clearOutputsAction = clearOutputsAction;
            Inputs = new ObservableCollection<FlagItemViewModel>();
            Outputs = new ObservableCollection<FlagItemViewModel>();

            Inputs.CollectionChanged += OnInputsCollectionChanged;
        }

        public string Name { get; }

        public ObservableCollection<FlagItemViewModel> Inputs { get; }

        public ObservableCollection<FlagItemViewModel> Outputs { get; }

        public bool HasOutputs => Outputs.Any();

        public bool CanClearInputs => _clearInputsAction != null;
        public bool CanClearOutputs => _clearOutputsAction != null;

        public bool HasManualOverrides
        {
            get => _hasManualOverrides;
            private set
            {
                if (SetProperty(ref _hasManualOverrides, value))
                {
                    _clearInputsCommand?.NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand ClearInputsCommand => _clearInputsCommand ??= new RelayCommand(
            () =>
            {
                _clearInputsAction?.Invoke();

                foreach (var input in Inputs)
                {
                    input.Refresh();
                }

                UpdateManualOverrideState();
            },
            () => _clearInputsAction != null && HasManualOverrides);

        public ICommand ClearOutputsCommand => _clearOutputsCommand ??= new RelayCommand(
            () => _clearOutputsAction?.Invoke(),
            () => _clearOutputsAction != null && HasOutputs);

        public IEnumerable<FlagItemViewModel> AllItems => Inputs.Concat(Outputs);

        private void OnInputsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems.OfType<FlagItemViewModel>())
                {
                    newItem.PropertyChanged += OnInputItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems.OfType<FlagItemViewModel>())
                {
                    oldItem.PropertyChanged -= OnInputItemPropertyChanged;
                }
            }

            UpdateManualOverrideState();
        }

        private void OnInputItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FlagItemViewModel.IsManual))
            {
                UpdateManualOverrideState();
            }
        }

        private void UpdateManualOverrideState()
        {
            HasManualOverrides = Inputs.Any(item => item.IsManual);
        }

        private bool _hasManualOverrides;
    }
}

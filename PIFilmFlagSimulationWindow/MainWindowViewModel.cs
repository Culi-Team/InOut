using CommunityToolkit.Mvvm.ComponentModel;
using EQX.InOut.Virtual;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PIFilmFlagSimulationWindow
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly FlagSimulationService _simulationService;
        private readonly DispatcherTimer _updateTimer;
        private readonly List<FlagItemViewModel> _itemsToUpdate;

        public MainWindowViewModel()
        {
            _simulationService = new FlagSimulationService();
            FlagGroups = new ObservableCollection<FlagGroupViewModel>();
            _itemsToUpdate = new List<FlagItemViewModel>();

            BuildGroups();

            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _updateTimer.Tick += (_, _) => RefreshItems();
            _updateTimer.Start();
        }

        public ObservableCollection<FlagGroupViewModel> FlagGroups { get; }

        private void BuildGroups()
        {
            AddGroup(CreateInputGroup("In Conveyor", _simulationService.InConveyorInput));

            AddGroup(CreateGroup("In Work Conveyor", _simulationService.InWorkConveyorInput, _simulationService.InWorkConveyorOutput));
            AddGroup(CreateGroup("Buffer Conveyor", _simulationService.BufferConveyorInput, _simulationService.BufferConveyorOutput));
            AddGroup(CreateGroup("Out Work Conveyor", _simulationService.OutWorkConveyorInput, _simulationService.OutWorkConveyorOutput));
            AddGroup(CreateGroup("Out Conveyor", _simulationService.OutConveyorInput, _simulationService.OutConveyorOutput));

            AddGroup(CreateGroup("Robot Load", _simulationService.RobotLoadInput, _simulationService.RobotLoadOutput));
            AddGroup(CreateGroup("Vinyl Clean", _simulationService.VinylCleanInput, _simulationService.VinylCleanOutput));
            AddGroup(CreateGroup("Fixture Align", _simulationService.FixtureAlignInput, _simulationService.FixtureAlignOutput));
            AddGroup(CreateGroup("Transfer Fixture", _simulationService.TransferFixtureInput, _simulationService.TransferFixtureOutput));
            AddGroup(CreateGroup("Detach", _simulationService.DetachInput, _simulationService.DetachOutput));
            AddGroup(CreateGroup("Remove Film", _simulationService.RemoveFilmInput, _simulationService.RemoveFilmOutput));

            AddGroup(CreateGroup("Glass Transfer", _simulationService.GlassTransferInput, _simulationService.GlassTransferOutput));
            AddGroup(CreateGroup("Glass Align - Left", _simulationService.GlassAlignLeftInput, _simulationService.GlassAlignLeftOutput));
            AddGroup(CreateGroup("Glass Align - Right", _simulationService.GlassAlignRightInput, _simulationService.GlassAlignRightOutput));

            AddGroup(CreateGroup("Transfer In Shuttle - Left", _simulationService.TransferInShuttleLeftInput, _simulationService.TransferInShuttleLeftOutput));
            AddGroup(CreateGroup("Transfer In Shuttle - Right", _simulationService.TransferInShuttleRightInput, _simulationService.TransferInShuttleRightOutput));

            AddGroup(CreateGroup("WET Clean - Left", _simulationService.WetCleanLeftInput, _simulationService.WetCleanLeftOutput));
            AddGroup(CreateGroup("WET Clean - Right", _simulationService.WetCleanRightInput, _simulationService.WetCleanRightOutput));

            AddGroup(CreateGroup("Transfer Rotation - Left", _simulationService.TransferRotationLeftInput, _simulationService.TransferRotationLeftOutput));
            AddGroup(CreateGroup("Transfer Rotation - Right", _simulationService.TransferRotationRightInput, _simulationService.TransferRotationRightOutput));

            AddGroup(CreateGroup("AF Clean - Left", _simulationService.AfCleanLeftInput, _simulationService.AfCleanLeftOutput));
            AddGroup(CreateGroup("AF Clean - Right", _simulationService.AfCleanRightInput, _simulationService.AfCleanRightOutput));

            AddGroup(CreateGroup("Unload Transfer - Left", _simulationService.UnloadTransferLeftInput, _simulationService.UnloadTransferLeftOutput));
            AddGroup(CreateGroup("Unload Transfer - Right", _simulationService.UnloadTransferRightInput, _simulationService.UnloadTransferRightOutput));
            AddGroup(CreateGroup("Unload Align", _simulationService.UnloadAlignInput, _simulationService.UnloadAlignOutput));

            AddGroup(CreateGroup("Robot Unload", _simulationService.RobotUnloadInput, _simulationService.RobotUnloadOutput));
        }

        private void AddGroup(FlagGroupViewModel group)
        {
            FlagGroups.Add(group);
            _itemsToUpdate.AddRange(group.AllItems);
        }

        private FlagGroupViewModel CreateInputGroup<TInputEnum>(string name, VirtualInputDevice<TInputEnum> input)
            where TInputEnum : Enum
        {
            var group = new FlagGroupViewModel(name,
                () => input.ClearManualInputs(),
                null);
            foreach (var value in Enum.GetValues(typeof(TInputEnum)).Cast<TInputEnum>())
            {
                int index = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                var item = new FlagItemViewModel(index, FormatFlagName(value.ToString()),
                    () => input[index],
                    newValue => input.SetManualInput(index, newValue),
                    () => input.HasManualInput(index));
                group.Inputs.Add(item);
            }
            return group;
        }

        private FlagGroupViewModel CreateGroup<TInputEnum, TOutputEnum>(string name,
            VirtualInputDevice<TInputEnum> input,
            VirtualOutputDevice<TOutputEnum> output)
            where TInputEnum : Enum
            where TOutputEnum : Enum
        {
            var group = new FlagGroupViewModel(name,
                () => input.ClearManualInputs(),
                () => output.Clear());

            foreach (var value in Enum.GetValues(typeof(TInputEnum)).Cast<TInputEnum>())
            {
                int index = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                var item = new FlagItemViewModel(index, FormatFlagName(value.ToString()),
                    () => input[index],
                    newValue => input.SetManualInput(index, newValue),
                    () => input.HasManualInput(index));
                group.Inputs.Add(item);
            }

            foreach (var value in Enum.GetValues(typeof(TOutputEnum)).Cast<TOutputEnum>())
            {
                int index = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                var item = new FlagItemViewModel(index, FormatFlagName(value.ToString()),
                    () => output[index],
                    newValue => output[index] = newValue,
                    null);
                group.Outputs.Add(item);
            }

            return group;
        }

        private void RefreshItems()
        {
            foreach (var item in _itemsToUpdate)
            {
                item.Refresh();
            }
        }

        private static string FormatFlagName(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                return string.Empty;
            }

            var formatted = rawName.Replace('_', ' ').ToLowerInvariant();
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formatted);
        }
    }
}

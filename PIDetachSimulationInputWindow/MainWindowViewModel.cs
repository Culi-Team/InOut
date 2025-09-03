using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.InOut;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace PIDetachSimulationInputWindow
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private Timer timerUpdateValue;

        public SimulationInputDeviceServerModbus<EInput> InputServer { get; }
        public MainWindowViewModel()
        {
            InputServer = new SimulationInputDeviceServerModbus<EInput>() { Id = 1, Name = "Simulation", MaxPin = 500 };

            InputServer.Initialize();
            InputServer.Start();

            timerUpdateValue = new System.Timers.Timer();
            timerUpdateValue.Interval = 100;
            timerUpdateValue.Elapsed += (s, e) =>
            {
                UpdateValue();
            };
            timerUpdateValue.Start();
        }

        public uint SelectedInputDeviceIndex { get; set; }
        public uint SelectedOutputDeviceIndex { get; set; }

        public ICommand InputDeviceIndexDecrease
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedInputDeviceIndex > 0)
                    {
                        SelectedInputDeviceIndex--;
                        OnPropertyChanged(nameof(SelectedInputDeviceIndex));
                    }
                });
            }
        }
        public ICommand InputDeviceIndexIncrease
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedInputDeviceIndex < InputServer.Inputs.Count / 32 - 1)
                    {
                        SelectedInputDeviceIndex++;
                        OnPropertyChanged(nameof(SelectedInputDeviceIndex));
                    }
                });
            }
        }

        private void UpdateValue()
        {
            foreach (var input in InputServer.Inputs)
            {
                input.RaiseValueUpdated();
            }
        }

        public ICommand SetInputDoorCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    InputServer.SetValue((int)EInput.DOOR_LOCK_1_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_1_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_2_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_2_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_3_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_3_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_4_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_4_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_5_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_5_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_6_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_6_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_7_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LOCK_7_R, true);
                });
            }
        }

        public ICommand SetInputOrigin
        {
            get
            {
                return new RelayCommand(() =>
                {
                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_1_1_UNCLAMP, true);
                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_1_2_UNCLAMP, true);

                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_2_1_UNCLAMP, true);
                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_2_2_UNCLAMP, true);
                });
            }
        }
    }
}

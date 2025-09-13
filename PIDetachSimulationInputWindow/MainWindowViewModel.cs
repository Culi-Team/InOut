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

                    InputServer.SetValue((int)EInput.ROBOT_FIXTURE_1_CLAMP, true);
                    InputServer.SetValue((int)EInput.ROBOT_FIXTURE_2_CLAMP, true);

                    InputServer.SetValue((int)EInput.ROBOT_FIXTURE_ALIGN_1_BW, true);
                    InputServer.SetValue((int)EInput.ROBOT_FIXTURE_ALIGN_2_BW, true);
                });
            }
        }

        public ICommand SetInputOrigin
        {
            get
            {
                return new RelayCommand(() =>
                {
                    InputServer.SetValue((int)EInput.IN_CST_LIGHT_CURTAIN_ALARM_DETECT, true);
                    InputServer.SetValue((int)EInput.OUT_CST_LIGHT_CURTAIN_ALARM_DETECT, true);

                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_1_1_UNCLAMP, true);
                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_1_2_UNCLAMP, true);

                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_2_1_UNCLAMP, true);
                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_2_2_UNCLAMP, true);
                });
            }
        }

        public ICommand SetInputRun
        {
            get
            {
                return new RelayCommand(() =>
                {
                    InputServer.SetValue((int)EInput.IN_CST_WORK_DETECT_1, true);
                    InputServer.SetValue((int)EInput.IN_CST_WORK_DETECT_2, true);
                    InputServer.SetValue((int)EInput.IN_CST_WORK_DETECT_3, true);
                    InputServer.SetValue((int)EInput.IN_CST_WORK_DETECT_4, true);

                    InputServer.SetValue((int)EInput.OUT_CST_WORK_DETECT_1, true);
                    InputServer.SetValue((int)EInput.OUT_CST_WORK_DETECT_2, true);
                    InputServer.SetValue((int)EInput.OUT_CST_WORK_DETECT_3, true);

                });
            }
        }

        public ICommand OffAllInputsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var values = Enum.GetValues(typeof(EInput));
                    foreach (var value in values)
                    {
                        InputServer.SetValue((int)value, false);
                    }
                });
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.InOut;
using EQX.InOut;
using EQX.InOut.InOut;
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

        public SimulationInputDevice_ServerMMF<EInput> InputServer { get; }

        public MainWindowViewModel()
        {
            InputServer = new SimulationInputDevice_ServerMMF<EInput>() { MaxPin = 1024 };

            InputServer.Initialize();
            try
            {
                UpdateValue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            timerUpdateValue = new System.Timers.Timer();
            timerUpdateValue.Interval = 100;
            timerUpdateValue.Elapsed += (s, e) =>
            {
                UpdateValue();
            };
            timerUpdateValue.Start();

            InputServer.SetValue((int)EInput.POWER_MC_ON_1, true);
            InputServer.SetValue((int)EInput.POWER_MC_ON_2, true);

            InputServer.SetValue((int)EInput.MAIN_AIR_1, true);
            InputServer.SetValue((int)EInput.MAIN_AIR_2, true);
            InputServer.SetValue((int)EInput.MAIN_AIR_3, true);
            InputServer.SetValue((int)EInput.MAIN_AIR_4, true);

            InputServer.SetValue((int)EInput.ROBOT_LOAD_USER_SAF, true);
            InputServer.SetValue((int)EInput.UNLOAD_ROB_USER_SAF, true);

            InputServer.SetValue((int)EInput.ROBOT_LOAD_ALARM_STOP, true);
            InputServer.SetValue((int)EInput.UNLOAD_ROB_ALARM_STOP, true);
            InputServer.SetValue((int)EInput.SHUTTLE_L_AVOID_NOT_COLLISION, true);
            InputServer.SetValue((int)EInput.SHUTTLE_R_AVOID_NOT_COLLISION, true);
            InputServer.SetValue((int)EInput.UNLOAD_TRANSFER_AVOID_NOT_COLLISION, true);
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

                    InputServer.SetValue((int)EInput.DOOR_LATCH_1_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_1_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_2_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_2_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_3_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_3_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_4_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_4_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_5_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_5_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_6_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_6_R, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_7_L, true);
                    InputServer.SetValue((int)EInput.DOOR_LATCH_7_R, true);

                    InputServer.SetValue((int)EInput.POWER_MC_ON_1, true);
                    InputServer.SetValue((int)EInput.POWER_MC_ON_2, true);

                    InputServer.SetValue((int)EInput.MAIN_AIR_1, true);
                    InputServer.SetValue((int)EInput.MAIN_AIR_2, true);
                    InputServer.SetValue((int)EInput.MAIN_AIR_3, true);
                    InputServer.SetValue((int)EInput.MAIN_AIR_4, true);
                });
            }
        }

        public ICommand SetInputOrigin
        {
            get
            {
                return new RelayCommand(() =>
                {
                    InputServer.SetValue((int)EInput.ROBOT_LOAD_CYL_1_UNCLAMP, true);
                    InputServer.SetValue((int)EInput.ROBOT_LOAD_CYL_2_UNCLAMP, true);

                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_CYL_1_UNCLAMP, true);
                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_CYL_2_UNCLAMP, true);

                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_CYL_3_UNCLAMP, true);
                    InputServer.SetValue((int)EInput.TRANSFER_FIXTURE_CYL_4_UNCLAMP, true);
                });
            }
        }

        public ICommand SetInputRun
        {
            get
            {
                return new RelayCommand(() =>
                {
                    InputServer.SetValue((int)EInput.OUT_WORK_CV_CST_DETECT_1, true);
                    InputServer.SetValue((int)EInput.OUT_WORK_CV_CST_DETECT_2, true);
                    InputServer.SetValue((int)EInput.OUT_WORK_CV_CST_DETECT_3, true);

                    InputServer.SetValue((int)EInput.WET_CLEAN_LEFT_FEEDING_ROLLER_DETECT, false);
                    InputServer.SetValue((int)EInput.WET_CLEAN_RIGHT_FEEDING_ROLLER_DETECT, false);
                    InputServer.SetValue((int)EInput.AF_CLEAN_LEFT_FEEDING_ROLLER_DETECT, false);
                    InputServer.SetValue((int)EInput.AF_CLEAN_RIGHT_FEEDING_ROLLER_DETECT, false);

                    InputServer.SetValue((int)EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_1, false);
                    InputServer.SetValue((int)EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_2, false);
                    InputServer.SetValue((int)EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_3, false);

                    InputServer.SetValue((int)EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_1, false);
                    InputServer.SetValue((int)EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_2, false);
                    InputServer.SetValue((int)EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_3, false);

                    InputServer.SetValue((int)EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_1, false);
                    InputServer.SetValue((int)EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_2, false);
                    InputServer.SetValue((int)EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_3, false);

                    InputServer.SetValue((int)EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_1, false);
                    InputServer.SetValue((int)EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_2, false);
                    InputServer.SetValue((int)EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_3, false);

                    InputServer.SetValue((int)EInput.IN_CV_CST_DETECT_1, true);
                    InputServer.SetValue((int)EInput.IN_CV_CST_DETECT_2, true);

                    InputServer.SetValue((int)EInput.VINYL_CLEAN_FULL_DETECT, true);

                    InputServer.SetValue((int)EInput.WET_CLEAN_LEFT_ALCOHOL_LEAK_DETECT, true);
                    InputServer.SetValue((int)EInput.WET_CLEAN_LEFT_PUMP_LEAK_DETECT, true);

                    InputServer.SetValue((int)EInput.WET_CLEAN_RIGHT_ALCOHOL_LEAK_DETECT, true);
                    InputServer.SetValue((int)EInput.WET_CLEAN_RIGHT_PUMP_LEAK_DETECT, true);

                    InputServer.SetValue((int)EInput.AF_CLEAN_LEFT_ALCOHOL_LEAK_DETECT, true);
                    InputServer.SetValue((int)EInput.AF_CLEAN_LEFT_PUMP_LEAK_DETECT, true);

                    InputServer.SetValue((int)EInput.AF_CLEAN_RIGHT_ALCOHOL_LEAK_DETECT, true);
                    InputServer.SetValue((int)EInput.AF_CLEAN_RIGHT_PUMP_LEAK_DETECT, true);
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

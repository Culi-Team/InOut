using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.InOut.InOut;
using System.Windows;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace FrontCameraAssembleSimulationInput
{
    public class MainWindowViewModel : ObservableObject
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

        public ICommand SetInputOrigin
        {
            get
            {
                return new RelayCommand(() =>
                {
                    InputServer.SetValue((int)EInput.FRONT_DOOR, true);
                    InputServer.SetValue((int)EInput.REAR_DOOR, true);
                    InputServer.SetValue((int)EInput.RIGHT_DOOR, true);
                    InputServer.SetValue((int)EInput.LIGHT_CURTAIN_MUTING_SW, false);
                });
            }
        }

        public ICommand SetInputRun
        {
            get
            {
                return new RelayCommand(() =>
                {

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

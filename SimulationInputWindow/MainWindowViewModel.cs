using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationInputWindow.Controls;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimulationInputWindow
{
    public enum EMachineType
    {
        Tray2CST,
        CST2CST
    }
    public partial class MainWindowViewModel : ObservableObject
    {
        private System.Timers.Timer timerUpdateValue;
        public ObservableCollection<SetInputViewModel> InputList { get; set; }
        MemoryMappedFile _memoryMapFile;

        public MainWindowViewModel()
        {
            timerUpdateValue = new System.Timers.Timer();
            timerUpdateValue.Interval = 100;
            timerUpdateValue.Elapsed += (s, e) =>
            {
                UpdateValue();
            };
            timerUpdateValue.Start();

            InputList = new ObservableCollection<SetInputViewModel>();
            SelectedMachineType = EMachineType.CST2CST;
            try
            {
                _memoryMapFile = MemoryMappedFile.CreateOrOpen("SimInputData", 256);
                UpdateValue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand SetInputToOrigin
        {
            get
            {
                return new RelayCommand(() =>
                {
                    switch (SelectedMachineType)
                    {
                        case EMachineType.Tray2CST:
                            foreach (var input in InputListToOriginTray2CST)
                            {
                                SetValue((int)input, true);
                            }
                            break;
                        case EMachineType.CST2CST:
                            foreach (var input in InputListToOriginCST2CST)
                            {
                                SetValue((int)input, true);
                            }
                            break;
                        default:
                            break;
                    }
                });
            }
        }
        
        public ICommand SetInputToStart
        {
            get
            {
                return new RelayCommand(() =>
                {
                    switch (SelectedMachineType)
                    {
                        case EMachineType.Tray2CST:
                            foreach (var input in InputListToStartTray2CST)
                            {
                                SetValue((int)input, true);
                            }
                            break;
                        case EMachineType.CST2CST:
                            foreach (var input in InputListToStartCST2CST)
                            {
                                SetValue((int)input, true);
                            }
                            break;
                        default:
                            break;
                    }
                });
            }
        }
        public ICommand SetInputToAutoRun
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }
        public ICommand OffAllInputs
        {
            get
            {
                return new RelayCommand(() =>
                {
                    foreach (SetInputViewModel input in InputList)
                    {
                        using (MemoryMappedViewStream stream = _memoryMapFile.CreateViewStream(input.Id, 0))
                        {
                            BinaryWriter writer = new BinaryWriter(stream);
                            writer.Write((char)0);
                        }
                    }
                });
            }
        }
        private void UpdateValue()
        {
            byte[] values;

            using (MemoryMappedViewStream stream = _memoryMapFile.CreateViewStream())
            {
                BinaryReader reader = new BinaryReader(stream);
                values = reader.ReadBytes(256);
            }

            for (int i = 0; i < InputList.Count; i++)
            {
                InputList.First(input => input.Id == i).Value = values[i] == 1;
            }
        }

        private void SetValue(object sender, EventArgs e)
        {
            int index = (sender as SetInputViewModel).Id;

            using (MemoryMappedViewStream stream = _memoryMapFile.CreateViewStream(index, 0))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                if ((sender as SetInputViewModel).Value)
                {
                    writer.Write((char)1);
                }
                else
                {
                    writer.Write((char)0);

                }
            }
        }

        private void SetValue(int index, bool bOnOff)
        {
            using (MemoryMappedViewStream stream = _memoryMapFile.CreateViewStream(index, 0))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                if (bOnOff)
                {
                    writer.Write((char)1);
                }
                else
                {
                    writer.Write((char)0);
                }
            }
        }
        private void LoadInput<T>() where T : Enum
        {
            timerUpdateValue.Stop();
            InputList.Clear();

            var inputList = Enum.GetNames(typeof(T)).ToList();
            var inputValues = (int[])Enum.GetValues(typeof(T));

            for (int i = 0; i < inputList.Count / 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    SetInputViewModel inputNew = new SetInputViewModel((int)inputValues[i + 16 * j]);
                    inputNew.Name = inputList[i + 16 * j];
                    inputNew.SetValueEvent += SetValue;

                    InputList.Add(inputNew);
                }
            }
            OnPropertyChanged(nameof(InputList));

            timerUpdateValue.Start();
        }

        public EMachineType SelectedMachineType
        {
            get
            {
                return _SelectedMachineType;
            }
            set
            {
                _SelectedMachineType = value;
                switch (value)
                {
                    case EMachineType.Tray2CST:
                        LoadInput<EInputTray2CST>();
                        break;
                    case EMachineType.CST2CST:
                        LoadInput<EInputCST2CST>();
                        break;
                }
            }
        }

        private EMachineType _SelectedMachineType;

        private List<EInputTray2CST> InputListToOriginTray2CST = new List<EInputTray2CST>
        {
            EInputTray2CST.MAIN_AIR,
            EInputTray2CST.LEFTIN_SLIDER_END,
            EInputTray2CST.RIGHTIN_SLIDER_END,
            EInputTray2CST.SUPPLIER_SLIDER_END,
            EInputTray2CST.NGTRAY_SLIDER_END
        };

        private List<EInputCST2CST> InputListToOriginCST2CST = new List<EInputCST2CST>
        {
            EInputCST2CST.MAIN_AIR,
            EInputCST2CST.LEFTIN_SLIDER_END,
            EInputCST2CST.RIGHTIN_SLIDER_END,
            EInputCST2CST.SUPPLIER_SLIDER_END,
            EInputCST2CST.NGTRAY_SLIDER_END
        };

        private List<EInputTray2CST> InputListToStartTray2CST = new List<EInputTray2CST>
        {
            EInputTray2CST.MAIN_AIR,
            EInputTray2CST.LEFTIN_DOOR_LOCK,
            EInputTray2CST.LEFTIN_SLIDER_LOCK,
            EInputTray2CST.LEFTIN_SLIDER_END,
            EInputTray2CST.LEFTIN_TRAY_DET1,
            EInputTray2CST.LEFTIN_TRAY_DET2,
            EInputTray2CST.LEFTIN_TRAY_END_UP,
            EInputTray2CST.LEFTIN_TRAY_ALIGN,
            EInputTray2CST.LEFTIN_TRANS_BW,
            EInputTray2CST.LEFTIN_TRANS_UP,
            EInputTray2CST.LEFTIN_TRANS_UNGRIP,
            EInputTray2CST.RIGHTIN_DOOR_LOCK,
            EInputTray2CST.RIGHTIN_SLIDER_LOCK,
            EInputTray2CST.RIGHTIN_SLIDER_END,
            EInputTray2CST.RIGHTIN_CST_DET,
            EInputTray2CST.RIGHTIN_CST_UP,
            EInputTray2CST.RIGHTIN_SPT_UP,
            EInputTray2CST.NGTRAY_DOOR_LOCK,
            EInputTray2CST.NGTRAY_SLIDER_LOCK,
            EInputTray2CST.NGTRAY_SLIDER_END,
            EInputTray2CST.NGTRAY_TRAY_DET1,
            EInputTray2CST.NGTRAY_TRAY_DET2,
            EInputTray2CST.NGTRAY_TRAY_END_UP,
            EInputTray2CST.NGTRAY_TRAY_UNALIGN,
            EInputTray2CST.NGTRAY_TRANS_BW,
            EInputTray2CST.NGTRAY_TRANS_UP,
            EInputTray2CST.SUPPLIER_SLIDER_END,
            EInputTray2CST.STOPMESS,
            EInputTray2CST.PERI_RDY,
            EInputTray2CST.USER_SAF,
            EInputTray2CST.IO_ACTCONF,
            EInputTray2CST.ON_PATH,
        };

        private List<EInputCST2CST> InputListToStartCST2CST = new List<EInputCST2CST>
        {
            EInputCST2CST.MAIN_AIR,
            EInputCST2CST.LEFTIN_DOOR_LOCK,
            EInputCST2CST.LEFTIN_SLIDER_LOCK,
            EInputCST2CST.LEFTIN_SLIDER_END,
            EInputCST2CST.RIGHTIN_DOOR_LOCK,
            EInputCST2CST.RIGHTIN_SLIDER_LOCK,
            EInputCST2CST.RIGHTIN_SLIDER_END,
            EInputCST2CST.RIGHTIN_CST_DET,
            EInputCST2CST.RIGHTIN_CST_UP,
            EInputCST2CST.RIGHTIN_SPT_UP,
            EInputCST2CST.NGTRAY_DOOR_LOCK,
            EInputCST2CST.NGTRAY_SLIDER_LOCK,
            EInputCST2CST.NGTRAY_SLIDER_END,
            EInputCST2CST.NGTRAY_TRAY_DET1,
            EInputCST2CST.NGTRAY_TRAY_DET2,
            EInputCST2CST.NGTRAY_TRAY_END_UP,
            EInputCST2CST.NGTRAY_TRAY_UNALIGN,
            EInputCST2CST.NGTRAY_TRANS_BW,
            EInputCST2CST.NGTRAY_TRANS_UP,
            EInputCST2CST.SUPPLIER_SLIDER_END,
            EInputCST2CST.STOPMESS,
            EInputCST2CST.PERI_RDY,
            EInputCST2CST.USER_SAF,
            EInputCST2CST.IO_ACTCONF,
            EInputCST2CST.ON_PATH,
        };
    }
}

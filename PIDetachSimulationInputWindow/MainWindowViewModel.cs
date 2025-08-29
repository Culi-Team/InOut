using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PIDetachSimulationInputWindow.Controls;
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

namespace PIDetachSimulationInputWindow
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
                _memoryMapFile = MemoryMappedFile.CreateOrOpen("SimInputData", 512);
                UpdateValue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //public ICommand SetInputToOrigin
        //{
        //    get
        //    {
        //        return new RelayCommand(() =>
        //        {
        //            switch (SelectedMachineType)
        //            {
        //                case EMachineType.Tray2CST:
        //                    foreach (var input in InputListToOriginTray2CST)
        //                    {
        //                        SetValue((int)input, true);
        //                    }
        //                    break;
        //                case EMachineType.CST2CST:
        //                    foreach (var input in InputListToOriginCST2CST)
        //                    {
        //                        SetValue((int)input, true);
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //        });
        //    }
        //}

        //public ICommand SetInputToStart
        //{
        //    get
        //    {
        //        return new RelayCommand(() =>
        //        {
        //            switch (SelectedMachineType)
        //            {
        //                case EMachineType.Tray2CST:
        //                    foreach (var input in InputListToStartTray2CST)
        //                    {
        //                        SetValue((int)input, true);
        //                    }
        //                    break;
        //                case EMachineType.CST2CST:
        //                    foreach (var input in InputListToStartCST2CST)
        //                    {
        //                        SetValue((int)input, true);
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //        });
        //    }
        //}
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

            foreach (var input in InputList)
            {
                if (input.Id < values.Length)
                    input.Value = values[input.Id] == 1;
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

            var values = Enum.GetValues(typeof(T)).Cast<int>().OrderBy(v => v).ToList();
            if (values.Count == 0) return;

            const int rows = 32;                               
            int cols = (int)Math.Ceiling(values.Count / (double)rows); 

            for (int r = 0; r < rows; r++)                    
            {
                for (int c = 0; c < cols; c++)               
                {
                    int val = c * rows + r;                  
                    if (val >= values.Count) continue;

                    string name = Enum.GetName(typeof(T), val)!;

                    var vm = new SetInputViewModel(val) { Name = name };
                    vm.SetValueEvent += SetValue;
                    InputList.Add(vm);
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
                        LoadInput<PIDetachSimulationInputWindow.MainWindowViewModel.EInput>();
                        break;
                    case EMachineType.CST2CST:
                        LoadInput<PIDetachSimulationInputWindow.MainWindowViewModel.EInput>();
                        break;
                }
            }
        }

        private EMachineType _SelectedMachineType;

        //private List<EInputT2C> InputListToOriginTray2CST = new List<EInputT2C>
        //{
        //    EInputT2C.MAIN_AIR,
        //    EInputT2C.LEFTIN_SLIDER_END,
        //    EInputT2C.RIGHTIN_SLIDER_END,
        //    EInputT2C.SUPPLIER_SLIDER_END,
        //    EInputT2C.NGTRAY_SLIDER_END
        //};

        //private List<EInputC2C> InputListToOriginCST2CST = new List<EInputC2C>
        //{
        //    EInputC2C.MAIN_AIR,
        //    EInputC2C.LEFTIN_SLIDER_END,
        //    EInputC2C.RIGHTIN_SLIDER_END,
        //    EInputC2C.SUPPLIER_SLIDER_END,
        //    EInputC2C.NGTRAY_SLIDER_END
        //};

        //private List<EInputT2C> InputListToStartTray2CST = new List<EInputT2C>
        //{
        //    EInputT2C.MAIN_AIR,
        //    EInputT2C.LEFTIN_DOOR_LOCK,
        //    EInputT2C.LEFTIN_SLIDER_LOCK,
        //    EInputT2C.LEFTIN_SLIDER_END,
        //    EInputT2C.LEFTIN_TRAY_DET1,
        //    EInputT2C.LEFTIN_TRAY_TYPE,
        //    EInputT2C.LEFTIN_TRAY_END_UP,
        //    EInputT2C.LEFTIN_TRAY_ALIGN,
        //    EInputT2C.LEFTIN_TRANS_BW,
        //    EInputT2C.LEFTIN_TRANS_UP,
        //    EInputT2C.LEFTIN_TRANS_UNGRIP,
        //    EInputT2C.RIGHTIN_DOOR_LOCK,
        //    EInputT2C.RIGHTIN_SLIDER_LOCK,
        //    EInputT2C.RIGHTIN_SLIDER_END,
        //    EInputT2C.RIGHTIN_CST_DET,
        //    EInputT2C.RIGHTIN_CST_UP,
        //    EInputT2C.RIGHTIN_SPT_UP,
        //    EInputT2C.NGTRAY_DOOR_LOCK,
        //    EInputT2C.NGTRAY_SLIDER_LOCK,
        //    EInputT2C.NGTRAY_SLIDER_END,
        //    EInputT2C.NGTRAY_TRAY_DET1,
        //    EInputT2C.NGTRAY_TRAY_DET2,
        //    EInputT2C.NGTRAY_TRAY_END_UP,
        //    EInputT2C.NGTRAY_TRAY_UNALIGN,
        //    EInputT2C.NGTRAY_TRANS_BW,
        //    EInputT2C.NGTRAY_TRANS_UP,
        //    EInputT2C.SUPPLIER_SLIDER_LOCK,
        //    EInputT2C.SUPPLIER_SLIDER_END,
        //    EInputT2C.SUPPLIER_DOOR_LOCK,
        //    EInputT2C.TRAYSUPPLIER_TRAY_DET1,
        //    EInputT2C.PERI_RDY,
        //    EInputT2C.ALARM_STOP,
        //    EInputT2C.USER_SAF,
        //    EInputT2C.IO_ACTCONF,
        //    EInputT2C.ON_PATH,
        //    EInputT2C.PRO_ACT,
        //};

        //private List<EInputC2C> InputListToStartCST2CST = new List<EInputC2C>
        //{
        //    EInputC2C.MAIN_AIR,
        //    EInputC2C.LEFTIN_DOOR_LOCK,
        //    EInputC2C.LEFTIN_SLIDER_LOCK,
        //    EInputC2C.LEFTIN_SLIDER_END,
        //    EInputC2C.LEFTIN_CST_DET,
        //    EInputC2C.LEFTIN_CST_UP,
        //    EInputC2C.LEFTIN_SPT_UP,
        //    EInputC2C.RIGHTIN_DOOR_LOCK,
        //    EInputC2C.RIGHTIN_SLIDER_LOCK,
        //    EInputC2C.RIGHTIN_SLIDER_END,
        //    EInputC2C.RIGHTIN_CST_DET,
        //    EInputC2C.RIGHTIN_CST_UP,
        //    EInputC2C.RIGHTIN_SPT_DOWN,
        //    EInputC2C.NGTRAY_DOOR_LOCK,
        //    EInputC2C.NGTRAY_SLIDER_LOCK,
        //    EInputC2C.NGTRAY_SLIDER_END,
        //    EInputC2C.NGTRAY_TRAY_DET1,
        //    EInputC2C.NGTRAY_TRAY_END_UP,
        //    EInputC2C.NGTRAY_TRANS_BW,
        //    EInputC2C.NGTRAY_TRANS_UP,
        //    EInputC2C.SUPPLIER_SLIDER_LOCK,
        //    EInputC2C.SUPPLIER_SLIDER_END,
        //    EInputC2C.SUPPLIER_DOOR_LOCK,
        //    EInputC2C.TRAYSUPPLIER_TRAY_DET1,
        //    EInputC2C.PERI_RDY,
        //    EInputC2C.ALARM_STOP,
        //    EInputC2C.USER_SAF,
        //    EInputC2C.IO_ACTCONF,
        //    EInputC2C.ON_PATH,
        //    EInputC2C.PRO_ACT,
        //};
    }
}

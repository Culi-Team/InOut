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

namespace SimulationInputWindow
{
    public partial class MainWindowViewModel
    {
        public ObservableCollection<SetInputViewModel> InputList { get; set; }

        MemoryMappedFile _memoryMapFile;

        public MainWindowViewModel()
        {
            InputList = new ObservableCollection<SetInputViewModel>();
            try
            {
                _memoryMapFile = MemoryMappedFile.CreateOrOpen("SimInputData", 256);
                LoadInput<EInput>();
                UpdateValue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void LoadInput()
        {
            var inputList = Enum.GetNames(typeof(EInput)).ToList();
            var inputValues = (EInput[])Enum.GetValues(typeof(EInput));

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
        }
    }
}

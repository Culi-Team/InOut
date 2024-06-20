using SimulationInputWindow.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationInputWindow
{
    public class MainWindowViewModel 
    {
        public ObservableCollection<SetInputViewModel> InputList { get; set; }

        MemoryMappedFile _memoryMapFile;

        public MainWindowViewModel() 
        {
            InputList = new ObservableCollection<SetInputViewModel>();
            _memoryMapFile = MemoryMappedFile.OpenExisting("SimInputData");
            LoadInput();
        }

        private void SetValue(object sender, EventArgs e) 
        {
            int index = (sender as SetInputViewModel).Id;

            using (MemoryMappedViewStream stream = _memoryMapFile.CreateViewStream(index, 0))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                if((sender as SetInputViewModel).Value)
                {
                    writer.Write((char)1);
                }
                else
                {
                    writer.Write((char)0);

                }
            }
        }

        public enum EInput
        {
            START_SW,
            STOP_SW,
        }

        private void LoadInput()
        {
            var inputList = Enum.GetNames(typeof(EInput)).ToList();
            var inputValues = (EInput[])Enum.GetValues(typeof(EInput));

            for (int i = 0; i < inputList.Count; i++)
            {
                SetInputViewModel inputNew = new SetInputViewModel((int)inputValues[i]);
                inputNew.Name = inputList[i];
                inputNew.SetValueEvent += SetValue;

                InputList.Add(inputNew);
            }
        }
    }
}

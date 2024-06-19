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
            LoadInputFromFile("InputDefine.txt");
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

        private void LoadInputFromFile(string filePath)
        {
            string inputNames = File.ReadAllText(filePath);
            string[] inputNamesArray = inputNames.Split(",\r\n");

            for(int i =0;i<inputNamesArray.Length;i++) 
            {
                inputNamesArray[i] = inputNamesArray[i].Remove(0,1);
                inputNamesArray[i] = inputNamesArray[i].Remove(inputNamesArray[i].Length-1, 1);
            }

            for (int i = 0; i < 256; i++)
            {
                if (i >= inputNamesArray.Length -1) break;

                SetInputViewModel inputNew = new SetInputViewModel(i);
                inputNew.Name = inputNamesArray[i];
                inputNew.SetValueEvent += SetValue;

                InputList.Add(inputNew);
            }
        }
    }
}

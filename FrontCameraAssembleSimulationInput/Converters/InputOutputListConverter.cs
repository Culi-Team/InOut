using EQX.Core.InOut;
using System.Globalization;
using System.Windows.Data;

namespace FrontCameraAssembleSimulationInput.Converters
{
    public class InputOutputListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] is uint == false) return Binding.DoNothing;

            if (values[0] is List<IDInput> inputs)
            {
                var subList = inputs.GetRange((int)((uint)values[1] * 32), 32);
                IDInput[] newList = new IDInput[subList.Count];
                for (int i = 0; i < subList.Count / 2; i++)
                {
                    newList[i * 2] = subList[i];
                    newList[i * 2 + 1] = subList[i + 16];
                }

                return newList.ToList();
            }
            else if (values[0] is List<IDOutput> outputs)
            {
                var subList = outputs.GetRange((int)((uint)values[1] * 32), 32);
                IDOutput[] newList = new IDOutput[subList.Count];
                for (int i = 0; i < subList.Count / 2; i++)
                {
                    newList[i * 2] = subList[i];
                    newList[i * 2 + 1] = subList[i + 16];
                }

                return newList.ToList();
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

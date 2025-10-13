using FrontCameraAssembleSimulationInput;
using System.Windows;
using System.Windows.Controls;

namespace Simulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;

            if (this.DataContext is MainWindowViewModel vm)
            {
                EInput input = (EInput)Enum.Parse(typeof(EInput), button.Content.ToString());
                vm.InputServer.ToggleInput((int)input);
            }
        }
    }
}
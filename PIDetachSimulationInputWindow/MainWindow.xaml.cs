using PIDetachSimulationInputWindow;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PIDetachSimulationInputWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new PIDetachSimulationInputWindow.MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;

            if(this.DataContext is MainWindowViewModel vm)
            {
                EInput input = (EInput)Enum.Parse(typeof(EInput), button.Content.ToString());
                vm.InputServer.ToggleInput((int)input);
            }
        }
    }
}
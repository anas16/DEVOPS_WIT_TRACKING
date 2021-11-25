using DEVOPS_V2.View_Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DEVOPS_V2
{
    /// <summary>
    /// Interaction logic for Check_Progress.xaml
    /// </summary>
    public partial class Check_Progress : Page
    {
        public Check_Progress()
        {
            InitializeComponent();
            vm = new TaskViewModel();
            DataContext = vm;
        }

        private readonly TaskViewModel vm;

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void LstData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

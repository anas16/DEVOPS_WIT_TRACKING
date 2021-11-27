using DEVOPS_V2.Models;
using DEVOPS_V2.View_Models;
using System.Windows;
using System.Windows.Controls;

namespace DEVOPS_V2
{
    /// <summary>
    /// Interaction logic for Post_Issue.xaml
    /// </summary>
    public partial class Post_Issue : Page
    {
        public Post_Issue()
        {
            InitializeComponent();
            vm = new TaskViewModel();
            vm.OnUploadSuccess += Vm_OnUploadSuccess;
            DataContext = vm;
            vm.Model = App.Model;
        }

        private void Vm_OnUploadSuccess(bool state)
        {
            string stateMsg = state ? "Upload Success" : "Upload Failed";
            MessageBox.Show(stateMsg, "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            vm.Data = new Fields();
        }

        private readonly TaskViewModel vm;
    }
}

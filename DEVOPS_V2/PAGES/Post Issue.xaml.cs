using DEVOPS_V2.View_Models;
using System;
using System.Collections.Generic;
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
using DEVOPS_V2.Models;

namespace DEVOPS_V2 {
    /// <summary>
    /// Interaction logic for Post_Issue.xaml
    /// </summary>
    public partial class Post_Issue : Page {
        public Post_Issue() {
            InitializeComponent();
            vm = new TaskViewModel();
            vm.OnUploadSuccess += Vm_OnUploadSuccess;
            DataContext = vm;
            vm.Model = App.Model;
        }

        private void Vm_OnUploadSuccess(bool state) {
            var stateMsg = state ? "Upload Success" : "Upload Failed";
            MessageBox.Show(stateMsg, "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            vm.Data = new Fields();
        }

        private readonly TaskViewModel vm;
    }
}

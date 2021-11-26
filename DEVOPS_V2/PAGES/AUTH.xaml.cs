using DEVOPS_V2.View_Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DEVOPS_V2
{
    /// <summary>
    /// Interaction logic for AUTH.xaml
    /// </summary>
    public partial class AUTH : Page
    {
        public AUTH()
        {
            InitializeComponent();
            vm = new TaskViewModel();
            vm.OnStartChanged += Vm_OnStartChanged;
            DataContext = vm;
        }

        private void Vm_OnStartChanged(bool state)
        {
            if (!state)
            {
                MessageBox.Show("Login Error", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                App.Model = vm.Model;

                // Find the frame.
                Frame pageFrame = null;
                DependencyObject currParent = VisualTreeHelper.GetParent(this);
                while (currParent != null && pageFrame == null)
                {
                    pageFrame = currParent as Frame;
                    currParent = VisualTreeHelper.GetParent(currParent);
                }

                // Change the page of the frame.
                if (pageFrame != null)
                {
                    pageFrame.Source = new Uri("PAGES/Menu.xaml", UriKind.Relative);
                }
            }
        }

        private readonly TaskViewModel vm;

        //private void Btn_Click_Menu(object sender, RoutedEventArgs e) {
        //    //Models.Uri_var Cred = new Models.Uri_var {
        //    //    PAT = Text_PAT.Text,
        //    //    Organisasi = Text_ORG.Text
        //    //};
        //}
    }
}

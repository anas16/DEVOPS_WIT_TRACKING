using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DEVOPS_V2
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Btn_Post_View(object sender, RoutedEventArgs e)
        {
            Menu_View.Content = new Post_Issue();
        }

        private void Btn_Check_View(object sender, RoutedEventArgs e)
        {
            Menu_View.Content = new Check_Progress();
        }

        private void Btn_Click_Main(object sender, RoutedEventArgs e)
        {
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
                pageFrame.Source = new Uri("PAGES/AUTH.xaml", UriKind.Relative);
            }
        }
    }
}

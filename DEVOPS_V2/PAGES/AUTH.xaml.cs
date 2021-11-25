using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void Btn_Click_Menu(object sender, RoutedEventArgs e)
        {
            Models.Uri_var Cred = new();

            Cred.PAT = Text_PAT.Text;
            Cred.Organisasi = Text_ORG.Text;

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
}

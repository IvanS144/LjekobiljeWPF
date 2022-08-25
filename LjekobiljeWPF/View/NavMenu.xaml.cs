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

namespace Ljekobilje
{
    /// <summary>
    /// Interaction logic for NavMeny.xaml
    /// </summary>
    public partial class NavMenu : UserControl
    {
        public NavMenu()
        {
            InitializeComponent();
        }

        private void SettingsMenuItem_Clicked(object sender, RoutedEventArgs e)
        {
            new SettingsDialog().ShowDialog();
        }
    }
}

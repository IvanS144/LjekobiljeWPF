using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Cooperants.xaml
    /// </summary>
    public partial class CooperantsPage : Page
    {
        public CooperantsPage()
        {
            InitializeComponent();
            DataContext = new CooperantViewModel();
        }
    }
}

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
using System.Windows.Shapes;

namespace Ljekobilje.Dialogs
{
    /// <summary>
    /// Interaction logic for AddPurchaseDialog.xaml
    /// </summary>
    public partial class AddPurchaseDialog : Window
    {
        public AddPurchaseViewModel AddPurchaseViewModel { get; set; }
        public AddPurchaseDialog()
        {
            InitializeComponent();
            AddPurchaseViewModel = new AddPurchaseViewModel(this);
            DataContext = AddPurchaseViewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

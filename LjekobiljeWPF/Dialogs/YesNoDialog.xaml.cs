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
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class YesNoDialog : Window
    {
        public YesNoDialog()
        {
            InitializeComponent();
        }

        public bool? ShowDialog(String message)
        {
            TextBlock.Text = message;
            return ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

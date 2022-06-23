using Ljekobilje.Dialogs;
using Ljekobilje.Projections;
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

namespace Ljekobilje
{
    /// <summary>
    /// Interaction logic for UpdatePlantDialog.xaml
    /// </summary>
    public partial class UpdateCooperantAccountDialog : Window
    {
        public CooperantAccountProjection CooperantAccount { get; set; }
        public UpdateCooperantAccountDialog(CooperantAccountProjection projection)
        {
            InitializeComponent();
            CooperantAccount = projection;
            DataContext = projection;
  
        }

        public (String, String) getFields()
        {
            return (BankTextBox.Text, AccountNumberTextBox.Text);
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            foreach(TextBox t in Utils.FindVisualChilds<TextBox>(this))
            {
                if(t.Text == null || t.Text.Equals(""))
                {
                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Morate popuniti sva polja" : "Please fill out all fields");
                    return;
                }
            }
            DialogResult = true;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

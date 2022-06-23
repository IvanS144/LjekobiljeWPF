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
    /// Interaction logic for UpdateStationDialog.xaml
    /// </summary>
    public partial class UpdateStationDialog : Window
    {
        public Stationsview Station { get; set; }
        public UpdateStationDialog(Stationsview stationVw)
        {
            InitializeComponent();
            Station= stationVw;
            DataContext = Station;
        }

        public (String, String) getFields()
        {
            return (AddressTextBox.Text, CityTextBox.Text);
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            foreach (TextBox t in Utils.FindVisualChilds<TextBox>(this))
            {
                if (t.Text == null || t.Text.Equals(""))
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

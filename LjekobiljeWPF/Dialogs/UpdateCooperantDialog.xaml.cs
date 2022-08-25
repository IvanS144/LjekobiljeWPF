using LjekobiljeWPF.Projections;
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
    /// Interaction logic for UpdateCooperantDialog.xaml
    /// </summary>
    public partial class UpdateCooperantDialog : Window
    {
        public CooperantProjection CooperantProjection { get; set; }
        public UpdateCooperantDialog(CooperantProjection c)
        {
            InitializeComponent();
            CooperantProjection = c;
            DataContext = CooperantProjection;
            List<Station> stations = new List<Station>();
            using (LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                stations=(from station in entities.Stations select station).ToList();
            }
            StationsComboBox.ItemsSource=stations;
            StationsComboBox.SelectedItem = stations.Where(stat => stat.StationId == c.StationId).FirstOrDefault();
        }

        public (String, String, String, String, String, String, int) getFields()
        {
            return (FirstNameTextBox.Text, LastNameTextBox.Text, AddressTextBox.Text, EmailTextBox.Text, PhoneTextBox.Text, CityTextBox.Text, (StationsComboBox.SelectedItem as Station).StationId);
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

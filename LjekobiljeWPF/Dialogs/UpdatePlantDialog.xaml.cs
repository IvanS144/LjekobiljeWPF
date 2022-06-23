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
    /// Interaction logic for UpdatePlantDialog.xaml
    /// </summary>
    public partial class UpdatePlantDialog : Window
    {
        public PlantProjection PlantProjection { get; set; }
        public UpdatePlantDialog(PlantProjection projection)
        {
            InitializeComponent();
            PlantProjection = projection;
            DataContext = projection;
  
        }

        public (String, String, String, decimal) getFields()
        {
            return (PlantNameTextBox.Text, LatinNameTextBox.Text, MedicNameTextBox.Text, RetailPrice.Value.Value);
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

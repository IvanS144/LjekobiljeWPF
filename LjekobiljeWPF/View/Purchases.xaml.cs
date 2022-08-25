using Ljekobilje.Dialogs;
using LjekobiljeWPF.ViewModel;
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
    /// Interaction logic for Otkupi.xaml
    /// </summary>
    public partial class PurchasesPage : Page
    {
        public PurchaseViewModel PurchaseViewModel { get; set; }
        public PurchasesPage()
        {
            InitializeComponent();
            PurchaseViewModel = new PurchaseViewModel();
            DataContext = PurchaseViewModel;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = e.Source;

            ScrollViewer scv = (ScrollViewer)sender;
            scv.RaiseEvent(eventArg);
            e.Handled = true;
        }
    }
}

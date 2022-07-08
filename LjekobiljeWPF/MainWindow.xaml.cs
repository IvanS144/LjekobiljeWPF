using Ljekobilje.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlantsPage plantsPage;
        private StationsPage stationsPage;
        private PurchasesPage purchasesPage;
        private PaymentsPage paymnentsPage;
        private CooperantsPage coopernatsPage;


        public MainWindow()
        {
            App app = Application.Current as App;
            switch (app.CurrentUser.Language)
            {
                //case 2: { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Language.en.xaml", UriKind.Relative); app.Resources.MergedDictionaries[0] = dictionary; break; }
                //case 1: { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Languages.sr.xaml", UriKind.Relative); app.Resources.MergedDictionaries[0] = dictionary; break; }
                default: break;
            }
            switch(app.CurrentUser.Theme)
            {
                //case 1: { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Language.en.xaml", UriKind.Relative); app.Resources.MergedDictionaries[0]=dictionary; break;  }
                //case 2: { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\DarkTheme.xaml", UriKind.Relative); app.Resources.MergedDictionaries[1]=dictionary; break; }
                default: break;
            }
            //ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Languages.sr.xaml", UriKind.Relative); app.Resources.MergedDictionaries.Add(dictionary);
            InitializeComponent();
            Menu.Plants.Click += Plants_Click;
            Menu.Stations.Click += Stations_Click;
            Menu.Purchases.Click += Purchases_Click;
            Menu.Payments.Click += Payments_Click;
            Menu.Cooperants.Click += Cooperants_Click;
            Menu.Logout.Click += Logout_Click;
            if(app.CurrentUser.UserType == 3)
            {
                Menu.Administration.Visibility = Visibility.Visible;
                Menu.Administration.Click += Administration_Click;
            }
            else
            {
                Menu.Administration.Visibility = Visibility.Collapsed;
            }
            Frame.Navigated += Frame_Navigated;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            Frame.NavigationService.RemoveBackEntry();
        }

        private void Administration_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = null;
            Frame.Content = new AdministratorPage();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }

        private void Cooperants_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = null;
            Frame.Content = new CooperantsPage();
        }

        private void Payments_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = null;
            Frame.Content = new PaymentsPage();
        }

        private void Purchases_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = null;
            Frame.Content = new PurchasesPage();
        }

        private void Stations_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = null;
            Frame.Content= new StationsPage();
        }

        private void Plants_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = null;
            Frame.Content = new PlantsPage();
        }

        private void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;
            //if(app.Resources.MergedDictionaries.Count>0)
            //app.Resources.MergedDictionaries.RemoveAt(0);
            MenuItem item = sender as MenuItem;
            switch(item.Tag)
            {
                case "Engleski": { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Language.en.xaml", UriKind.Relative); app.Resources.MergedDictionaries[0]=dictionary; break;  }
                case "Srpski": { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Languages.sr.xaml", UriKind.Relative); app.Resources.MergedDictionaries[0]=dictionary; break; }
                default: break;
            }
        }
    }
}

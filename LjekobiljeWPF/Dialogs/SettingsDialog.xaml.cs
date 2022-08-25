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
using System.Windows.Shapes;

namespace Ljekobilje
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        private int theme;

        private int language;
        public SettingsDialog()
        {
            InitializeComponent();
           theme = (Application.Current as App).CurrentUser.Theme.Value;
language = (Application.Current as App).CurrentUser.Language.Value;
            switch(theme)
            {
                case 1: ThemeButton2.IsChecked = true; break;
                case 2: ThemeButton1.IsChecked = true; break;
                case 3: ThemeButton3.IsChecked = true; break;
                default: break;
            }
            switch (language)
            {
                case 1: LanguageButton1.IsChecked = true; break;
                case 2: LanguageButton2.IsChecked = true; break; 
                default:
                    break;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LanguageRadioButtonClick(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;
            RadioButton button = sender as RadioButton;
            switch(button.Tag)
            {
                case "English": { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Language.en.xaml", UriKind.Relative); app.Resources.MergedDictionaries[0] = dictionary; language = 2; break; }
                case "Srpski": { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Language.sr.xaml", UriKind.Relative); app.Resources.MergedDictionaries[0] = dictionary; language=1; break; }
                default: break;
            }
        }

        private void OkButton_Clicked(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            DialogResult = true;
        }

        private void ThemeClick(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;
            RadioButton button = sender as RadioButton;
            switch (button.Tag)
            {
                case "Light": { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\Yelow.xaml", UriKind.Relative); app.Resources.MergedDictionaries[1] = dictionary; theme = 1; break; }
                case "Dark": { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\DarkTheme.xaml", UriKind.Relative); app.Resources.MergedDictionaries[1] = dictionary; theme= 2; break; }
                case "Green": { ResourceDictionary dictionary = new ResourceDictionary(); dictionary.Source = new Uri("..\\plant-theme.xaml", UriKind.Relative); app.Resources.MergedDictionaries[1] = dictionary; theme = 3; break; }
                default: break;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SaveSettings();
            base.OnClosing(e);
        }

        private void SaveSettings()
        {
            bool changed = false;
            User u = (Application.Current as App).CurrentUser;
            if (theme != u.Theme)
            {
                u.Theme = theme;
                changed = true;
            }





            if (language != u.Language)
            {
                u.Language = language;
                changed = true;
            }
            if (changed)
            {
                using (LjekobiljeEntities entities = new LjekobiljeEntities())
                {
                    entities.Entry(u).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}

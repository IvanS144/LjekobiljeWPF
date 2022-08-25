using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Ljekobilje
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public User CurrentUser { get; set; }
        //public bool SettingsChanged { get; set; } = false;

        public static int? GetLanguage()
        {
            return (Application.Current as App).CurrentUser.Language;
        }
    }
}

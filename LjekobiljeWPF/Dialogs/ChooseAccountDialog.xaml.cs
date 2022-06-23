using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ChooseAccountDialog.xaml
    /// </summary>
    public partial class ChooseAccountDialog : Window
    {
        public ChooseAccountDialog(int _cid)
        {
            InitializeComponent();
            List<CooperantBankAccount> accounts;
            using(LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                accounts = (from account in entities.CooperantBankAccounts.Include("Account") where account.CooperantId == _cid select account).ToList();
            }
            AccountsComboBox.ItemsSource = accounts;
        }

        public int GetAccount()
        {
            return (AccountsComboBox.SelectedItem as CooperantBankAccount).AccountId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

using LjekobiljeWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljekobilje.Projections
{
    public class CooperantAccountProjection : BaseViewModel
    {
        private CooperantBankAccount _account;

        public string Bank { get { return _account.Account.Bank; } set { _account.Account.Bank = value; NotifyPropertyChanged("Bank"); } }

        public CooperantBankAccount Account => _account;

        public int AccountId => _account.AccountId;

        public int CooperantId => _account.CooperantId;

        public string CooperantFullName => _account.Cooperant.FirstName + " " + _account.Cooperant.LastName;

        public string AccountNumber { get => _account.Account.AccountNumber; set { _account.Account.AccountNumber = value; NotifyPropertyChanged("AccountNumber");} }

        public CooperantAccountProjection(CooperantBankAccount account)
        {
            _account = account;
        }
    }
}

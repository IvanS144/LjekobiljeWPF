using Ljekobilje;
using LjekobiljeWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjekobiljeWPF.Projections
{
    public class PaymentProjection : BaseViewModel
    {
        private Payment _payment;

        public Payment Payment { get => _payment; set { _payment = value; NotifyPropertyChanged("Payment"); } }

        public int PaymentId { get => _payment.PaymentId; set { _payment.PaymentId = value; NotifyPropertyChanged("PaymentId"); } }
        public int AccountId { get => _payment.CooperantAccountId; set { _payment.CooperantAccountId = value; NotifyPropertyChanged("AccountId"); } }
        public decimal Sum { get => _payment.Sum; set { _payment.Sum = value; NotifyPropertyChanged("Sum"); } }
        public string AccountNumber { get => _payment.CooperantAccount.Account.AccountNumber; }
        public string CooperantName { get => _payment.CooperantAccount.Cooperant.FirstName + " " + _payment.CooperantAccount.Cooperant.LastName; }

        public int PlantPurchaseId { get => _payment.PlantPurchaseId; set { _payment.PlantPurchaseId = value; } }

        public PaymentProjection(Payment payment)
        {
            _payment = payment;
        }
    }
}

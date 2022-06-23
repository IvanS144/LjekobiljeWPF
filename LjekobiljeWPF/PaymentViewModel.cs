﻿using Ljekobilje.Dialogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ljekobilje
{
    public class PaymentViewModel:BaseViewModel
    {
        private ObservableCollection<PaymentProjection> _payments = new ObservableCollection<PaymentProjection>();

        public ObservableCollection<PaymentProjection> Payments { get => _payments; set { _payments = value; base.NotifyPropertyChanged("Payments"); } }

        public DelegateCommand<PaymentProjection> DeleteCommand { get; private set; }

        public ActionCommand ClearFilterCommand { get; set; }

        private String _filterText;
        private String _selectedCategory;

        public String FilterText { get { return _filterText; } set { _filterText = value; View.Refresh(); NotifyPropertyChanged("FilterText"); } }

        public String SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; View.Refresh(); NotifyPropertyChanged("SelectedCategory"); } }

        private ICollectionView View; 

        public PaymentViewModel()
        {
            using(LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                (from payment in entities.Payments.Include("CooperantAccount.Account").Include("CooperantAccount.Cooperant") select payment).ToList().ForEach(e => _payments.Add(new PaymentProjection(e)));
            }
            DeleteCommand = new DelegateCommand<PaymentProjection>(Delete);
            ClearFilterCommand = new ActionCommand(ClearFilter, true);
            View = CollectionViewSource.GetDefaultView(_payments);
            View.Filter = Filter;
            SelectedCategory = "-";

        }

        public void Delete(PaymentProjection paymentPr)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog((App.GetLanguage() == 1 ? "Da či zaista želite da obrišete isplatu čiji je id " : "Do you want to delete payment whose id is ") + paymentPr.PaymentId + " ?");
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        Payment payment = new Payment() { PaymentId = paymentPr.PaymentId };
                        entities.Entry(payment).State = EntityState.Deleted;
                        entities.SaveChanges();
                        _payments.Remove(paymentPr);
                    }
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Nije moguće obisati ovu isplatu" : "It is not possible to delete this payment");
                }
            }
        }

        public Boolean Filter(object obj)
        {
            Boolean flag = false;
            if (FilterText == null || FilterText == "")
                return true;
            if (obj is PaymentProjection payment)
                switch (SelectedCategory)
                {
                    case "PaymentID" or "ID isplate": flag = payment.PaymentId.ToString().Contains(FilterText); break;
                    case "AccountID" or "ID računa": flag = payment.AccountId.ToString().Contains(FilterText); break;
                    case "PlantPurchaseID" or "ID otkupa": flag = payment.PlantPurchaseId.ToString().Contains(FilterText); break;
                    //case "StationID": flag = cooperant.StationID.ToString().Contains(FilterText); break;
                    default: flag = true; break;
                }
            return flag;


        }

        public void ClearFilter()
        {
            FilterText = ""; SelectedCategory = "-";
        }
    }
}

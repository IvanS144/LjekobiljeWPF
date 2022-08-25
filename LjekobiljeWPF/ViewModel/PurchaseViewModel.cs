using Ljekobilje;
using Ljekobilje.Dialogs;
using LjekobiljeWPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LjekobiljeWPF.ViewModel
{
    public class PurchaseViewModel : BaseViewModel
    {
        private ObservableCollection<Purchasespaid> _purchases = new ObservableCollection<Purchasespaid>();

        public DelegateCommand<Purchasespaid> DeleteCommand { get; set; }

        public DelegateCommand<Purchasespaid> PayCommand { get; set; }

        public DelegateCommand<Purchasespaid> UpdateCommand { get; set; }

        public ActionCommand ClearFilterCommand { get; set; }

        private string _filterText;
        private string _selectedCategory;

        //public string FilterText { get; set; } public string SelectedCategory { get; set; }//Value

        public string FilterText { get { return _filterText; } set { _filterText = value; View.Refresh(); NotifyPropertyChanged("FilterText"); } }

        public string SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; View.Refresh(); NotifyPropertyChanged("SelectedCategory"); } }

        public ObservableCollection<Purchasespaid> Purchases { get => _purchases; set { _purchases = value; NotifyPropertyChanged("Purchases"); } }

        private ICollectionView View;
        public PurchaseViewModel()
        {
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                (from purchase in db.Purchasespaids select purchase).ToList().ForEach(purchase => _purchases.Add(purchase));
            }
            AddPurchaseCommand = new ActionCommand(AddPurchase);
            DeleteCommand = new DelegateCommand<Purchasespaid>(Delete);
            UpdateCommand = new DelegateCommand<Purchasespaid>(Update);
            PayCommand = new DelegateCommand<Purchasespaid>(Pay);
            ClearFilterCommand = new ActionCommand(ClearFilter, true);
            View = CollectionViewSource.GetDefaultView(_purchases);
            View.Filter = Filter;
            SelectedCategory = "-";
        }

        public ActionCommand AddPurchaseCommand { get; set; }

        public void AddPurchase()
        {
            AddPurchaseDialog dialog = new AddPurchaseDialog();
            dialog.ShowDialog();
            _purchases.Clear();
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                (from purchase in db.Purchasespaids select purchase).ToList().ForEach(purchase => _purchases.Add(purchase));
            }
        }

        public void Update(Purchasespaid plantPurchase)
        {
            new AddPurchaseDialog(plantPurchase).ShowDialog();
            _purchases.Clear();
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                (from purchase in db.Purchasespaids select purchase).ToList().ForEach(purchase => _purchases.Add(purchase));
            }
        }

        public void Delete(Purchasespaid purchase)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog(App.GetLanguage() == 1 ? "Da li zaista želite da obrišete otkup čiji je id " + purchase.PlantPurchaseId + " (ova akcija takođe će obrisati i isplatu ako je otkup isplaćen)" : "Do you want to delete purchase whose id is " + purchase.PlantPurchaseId + " (this action will also delete related payment if this purchase has been paid)");
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities db = new LjekobiljeEntities())
                    {
                        db.Entry(db.Payments.Where(p => p.PlantPurchaseId == purchase.PlantPurchaseId).First()).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        db.SaveChanges();
                        db.PlantPurchaseEntries.Where(entry => entry.PlantPurchaseId == purchase.PlantPurchaseId).ToList().ForEach(entry => db.Entry(entry).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                        db.SaveChanges();
                        PlantPurchase plantPurchase = new PlantPurchase();
                        plantPurchase.PlantPurchaseId = purchase.PlantPurchaseId;
                        db.Entry(plantPurchase).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        db.SaveChanges();
                        Purchases.Remove(purchase);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void Pay(Purchasespaid purchase)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog(App.GetLanguage() == 1 ? "Da li zaista želite da platite otkup čiji je id " + purchase.PlantPurchaseId + " ?" : "Do you want to pay purchase whose id is " + purchase.PlantPurchaseId + " ?");
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities db = new LjekobiljeEntities())
                    {
                        ChooseAccountDialog accountDialog = new ChooseAccountDialog(purchase.CooperantId);
                        result = accountDialog.ShowDialog();
                        if (result == true)
                        {
                            Payment payment = new Payment();
                            payment.PlantPurchaseId = purchase.PlantPurchaseId;
                            payment.CooperantAccountId = accountDialog.GetAccount();
                            payment.Sum = purchase.TotalValue.Value;
                            db.Payments.Add(payment);
                            db.SaveChanges();
                            _purchases.Clear();
                            (from purchase1 in db.Purchasespaids select purchase1).ToList().ForEach(purchase1 => _purchases.Add(purchase1));

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public bool Filter(object obj)
        {
            bool flag = false;
            if (FilterText == null || FilterText == "")
                return true;
            if (obj is Purchasespaid purchase)
                switch (SelectedCategory)
                {
                    case "Cooperant ID" or "ID kooperanta": flag = purchase.CooperantId.ToString().Contains(FilterText); break;
                    case "Purchase ID" or "ID otkupa": flag = purchase.PlantPurchaseId.ToString().Contains(FilterText); break;
                    case "Station number" or "Broj stanice": flag = purchase.StationId.ToString().Contains(FilterText); break;
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

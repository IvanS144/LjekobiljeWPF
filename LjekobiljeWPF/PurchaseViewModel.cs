using Ljekobilje.Dialogs;
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
    public class PurchaseViewModel : BaseViewModel
    {
        private ObservableCollection<Purchasespaid> _purchases = new ObservableCollection<Purchasespaid>();

        public DelegateCommand<Purchasespaid> DeleteCommand { get; set; }

        public DelegateCommand<Purchasespaid> PayCommand { get; set; }

        public ActionCommand ClearFilterCommand { get; set; }

        private String _filterText;
        private String _selectedCategory;

        //public string FilterText { get; set; } public string SelectedCategory { get; set; }//Value

        public String FilterText { get { return _filterText; } set { _filterText = value; View.Refresh(); NotifyPropertyChanged("FilterText"); } }

        public String SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; View.Refresh(); NotifyPropertyChanged("SelectedCategory"); } }

        public ObservableCollection<Purchasespaid> Purchases { get => _purchases; set { _purchases = value; base.NotifyPropertyChanged("Purchases"); } }

        private ICollectionView View;
        public PurchaseViewModel()
        {
            using (LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                (from purchase in entities.Purchasespaids select purchase).ToList().ForEach(purchase => _purchases.Add(purchase));
            }
            AddPurchaseCommand = new ActionCommand(AddPurchase);
            DeleteCommand = new DelegateCommand<Purchasespaid>(Delete);
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
            using (LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                (from purchase in entities.Purchasespaids select purchase).ToList().ForEach(purchase => _purchases.Add(purchase));
            }
        }

        public void Delete(Purchasespaid purchase)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog((App.GetLanguage() == 1 ? "Da li zaista želite da obrišete otkup čiji je id " + purchase.PlantPurchaseId + " (ova akcija takođe će obrisati i isplatu ako je otkup isplaćen)" : "Do you want to delete purchase whose id is " + purchase.PlantPurchaseId + " (this action will also delete related payment if this purchase has been paid)"));
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        entities.PlantPurchaseEntries.Where(entry => entry.PlantPurchaseId == purchase.PlantPurchaseId).ToList().ForEach(entry => entities.Entry(entry).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                        entities.SaveChanges();
                        PlantPurchase plantPurchase = new PlantPurchase();
                        plantPurchase.PlantPurchaseId = purchase.PlantPurchaseId;
                        entities.Entry(plantPurchase).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        entities.SaveChanges();
                        Purchases.Remove(purchase);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public void Pay(Purchasespaid purchase)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog((App.GetLanguage() == 1 ? "Da li zaista želite da platite otkup čiji je id " + purchase.PlantPurchaseId +" ?": "Do you want to pay purchase whose id is " + purchase.PlantPurchaseId + " ?"));
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        ChooseAccountDialog accountDialog = new ChooseAccountDialog(purchase.CooperantId);
                        result = accountDialog.ShowDialog();
                        if(result == true)
                        {
                            Payment payment = new Payment();
                            payment.PlantPurchaseId = purchase.PlantPurchaseId;
                            payment.CooperantAccountId = accountDialog.GetAccount();
                            payment.Sum = purchase.TotalValue.Value;
                            entities.Payments.Add(payment);
                            entities.SaveChanges();
                            _purchases.Clear();
                            (from purchase1 in entities.Purchasespaids select purchase1).ToList().ForEach(purchase1 => _purchases.Add(purchase1));
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public Boolean Filter(object obj)
        {
            Boolean flag = false;
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
            FilterText = "";SelectedCategory = "-";
        }
    }
}

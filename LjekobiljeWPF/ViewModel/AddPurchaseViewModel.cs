using Ljekobilje;
using Ljekobilje.Dialogs;
using LjekobiljeWPF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LjekobiljeWPF.ViewModel
{
    public class AddPurchaseViewModel : BaseViewModel
    {
        private ObservableCollection<Plant> _plants = new ObservableCollection<Plant>();
        private ObservableCollection<Station> _stations = new ObservableCollection<Station>();

        private Window _window;
        public int SelectedCooperantId { get; set; }
        public int SelectedPlantId { get; set; }

        public int SelectedStationId { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal Quantity { get; set; }

        private PlantPurchase purchase;

        public DateTime? DatePurchased { get; set; } = DateTime.Now.Date;
        public ObservableCollection<Plant> Plants { get => _plants; set { _plants = value; NotifyPropertyChanged("Plants"); } }
        public ObservableCollection<Station> Stations { get => _stations; set { _stations = value; NotifyPropertyChanged("Stations"); } }

        private ObservableCollection<PlantPurchaseEntry> _purchaseEntries = new ObservableCollection<PlantPurchaseEntry>();

        public ObservableCollection<PlantPurchaseEntry> PurchaseEntries { get => _purchaseEntries; set { _purchaseEntries = value; NotifyPropertyChanged("PurchaseEntries"); } }

        public ActionCommand AddEntryCommand { get; set; }
        public DelegateCommand<PlantPurchaseEntry> DeleteEntryCommand { get; set; }

        public ActionCommand AddPurchaseCommand { get; set; }


        public AddPurchaseViewModel(Window window)
        {
            _window = window;
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                (from station in db.Stations.Include("Cooperants") select station).ToList().ForEach(e => _stations.Add(e));
                (from plant in db.Plants select plant).ToList().ForEach(e => _plants.Add(e));
                AddEntryCommand = new ActionCommand(AddEntry);
                DeleteEntryCommand = new DelegateCommand<PlantPurchaseEntry>(DeleteEntry);
                AddPurchaseCommand = new ActionCommand(AddPurchase);
            }
        }

        public AddPurchaseViewModel(Window window, int purchaseId) : this(window)
        {
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                purchase = (from purchase in db.PlantPurchases.Include("PlantPurchaseEntries.Plant") where purchase.PlantPurchaseId == purchaseId select purchase).First();
                PurchaseEntries = new ObservableCollection<PlantPurchaseEntry>(purchase.PlantPurchaseEntries.ToList());
                DatePurchased = purchase.DatePurchased;
                SelectedStationId = purchase.StationId;
                SelectedCooperantId = purchase.CooperantId;
            }
        }

        public void AddEntry()
        {
            if (!PurchaseEntries.Select(e => e.PlantId).Contains(SelectedPlantId))
            {
                PlantPurchaseEntry plantPurchaseEntry = new PlantPurchaseEntry();
                plantPurchaseEntry.PlantId = SelectedPlantId;
                plantPurchaseEntry.Quantity = Quantity;
                plantPurchaseEntry.RetailPrice = RetailPrice;
                plantPurchaseEntry.Plant = GetPlantById(SelectedPlantId);
                PurchaseEntries.Add(plantPurchaseEntry);
            }


        }
        public void DeleteEntry(PlantPurchaseEntry plantPurchaseEntry)
        {
            if (purchase != null && plantPurchaseEntry.PlantPurchaseId > 0)
            {
                using (var db = new LjekobiljeEntities())
                {
                    db.Entry(plantPurchaseEntry).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            PurchaseEntries.Remove(plantPurchaseEntry);
        }

        public void AddPurchase()
        {
            if (SelectedCooperantId > 0 && SelectedStationId > 0 && PurchaseEntries.Count > 0)
            {
                using (LjekobiljeEntities db = new LjekobiljeEntities())
                {
                    PlantPurchase plantPurchase = purchase != null ? purchase : new PlantPurchase();
                    plantPurchase.Station = null;
                    plantPurchase.Cooperant = null;
                    plantPurchase.CooperantId = SelectedCooperantId;
                    plantPurchase.StationId = SelectedStationId;
                    plantPurchase.DatePurchased = DatePurchased.Value;
                    decimal total = PurchaseEntries.Select(e => e.RetailPrice * e.Quantity).Sum();
                    plantPurchase.TotalValue = total;
                    if (purchase == null)
                    {
                        db.PlantPurchases.Add(plantPurchase);
                    }
                    else
                    {
                        db.Entry(plantPurchase).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    int id = plantPurchase.PlantPurchaseId;
                    foreach (var entry in PurchaseEntries)
                    {
                        if (entry.PlantPurchaseId == 0)
                        {
                            entry.PlantPurchaseId = id;
                            entry.PlantPurchase = null;
                            db.Entry(entry).State = EntityState.Added;
                        }
                        else
                        {
                            db.Entry(entry).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                }
                _window.Close();
            }
            else
            {
                new ErrorDialog().ShowDialog(App.GetLanguage() > 1 ? "Morate popuniti sva polja u opštim podacima i dodati bar jednu stavku otkupa" : "You have to fill out all fields in general data and add at least one purchase entry");
            }
        }

        private Plant GetPlantById(int plantId)
        {
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                return db.Plants.Where(p => p.PlantId == plantId).First();
            }
        }


    }
}

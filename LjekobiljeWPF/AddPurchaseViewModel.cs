using Ljekobilje.Dialogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ljekobilje
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

        public DateTime? DatePurchased { get; set; } = DateTime.Now.Date;
        public ObservableCollection<Plant> Plants { get => _plants; set { _plants = value; NotifyPropertyChanged("Plants"); } }
        public ObservableCollection<Station> Stations { get => _stations; set { _stations = value; NotifyPropertyChanged("Stations"); } }

        private ObservableCollection<PlantPurchaseEntry> _purchaseEntries = new ObservableCollection<PlantPurchaseEntry>();

        public ObservableCollection<PlantPurchaseEntry> PurchaseEntries { get => _purchaseEntries; }

        public ActionCommand AddEntryCommand { get; set; }
        public DelegateCommand<PlantPurchaseEntry> DeleteEntryCommand { get; set; }

        public ActionCommand AddPurchaseCommand { get; set; }


        public AddPurchaseViewModel(Window window)
        {
            _window = window;
            using (LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                (from station in entities.Stations.Include("Cooperants") select station).ToList().ForEach(e => _stations.Add(e));
                (from plant in entities.Plants select plant).ToList().ForEach(e => _plants.Add(e));
                AddEntryCommand = new ActionCommand(AddEntry);
                DeleteEntryCommand = new DelegateCommand<PlantPurchaseEntry>(DeleteEntry);
                AddPurchaseCommand = new ActionCommand(AddPurchase);
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
                PurchaseEntries.Add(plantPurchaseEntry);
            }


        }
        public void DeleteEntry(PlantPurchaseEntry plantPurchaseEntry)
        {
            PurchaseEntries.Remove(plantPurchaseEntry);
        }

        public void AddPurchase()
        {
            if (SelectedCooperantId > 0 && SelectedStationId > 0 && PurchaseEntries.Count > 0)
            {
                using (LjekobiljeEntities ljekobiljeEntities = new LjekobiljeEntities())
                {
                    PlantPurchase plantPurchase = new PlantPurchase();
                    plantPurchase.CooperantId = SelectedCooperantId;
                    plantPurchase.StationId = SelectedStationId;
                    plantPurchase.DatePurchased = DatePurchased.Value;
                    decimal total = PurchaseEntries.Select(e => e.RetailPrice * e.Quantity).Sum();
                    plantPurchase.TotalValue = total;
                    ljekobiljeEntities.PlantPurchases.Add(plantPurchase);
                    ljekobiljeEntities.SaveChanges();
                    int id = plantPurchase.PlantPurchaseId;
                    foreach (var entry in PurchaseEntries)
                    {
                        entry.PlantPurchaseId = id;
                        ljekobiljeEntities.Entry(entry).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                    ljekobiljeEntities.SaveChanges();
                }
                _window.Close();
            }
            else
            {
                new ErrorDialog().ShowDialog(App.GetLanguage() > 1 ? "Morate popuniti sva polja u opštim podacima i dodati bar jednu stavku otkupa" : "You have to fill out all fields in general data and add at least one purchase entry");
            }
        }


    }
}

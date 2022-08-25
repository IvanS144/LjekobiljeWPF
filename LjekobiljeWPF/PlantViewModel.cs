using Ljekobilje.Dialogs;
using LjekobiljeWPF;
using LjekobiljeWPF.Projections;
using LjekobiljeWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Ljekobilje
{
    public class PlantViewModel : BaseViewModel
    {
        private ObservableCollection<PlantProjection> _plants = new ObservableCollection<PlantProjection>();
        private string _filterText;
        private string _selectedCategory;

        public string PlantName { get; set; }

        public string LatinName { get; set; }

        public string MedicName { get; set; }

        public decimal RetailPrice { get; set; }

        public String FilterText { get { return _filterText; } set { _filterText = value; View.Refresh(); NotifyPropertyChanged("FilterText"); } }

        public String SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; View.Refresh(); NotifyPropertyChanged("SelectedCategory"); } }

        //private DelegateCommand<PlantProjection> _deleteCommand;
        //public DelegateCommand<PlantProjection> DeleteCommand =>
        //    _deleteCommand ?? (_deleteCommand = new DelegateCommand<PlantProjection>(ExecuteDeleteCommand));
        //void ExecuteDeleteCommand(PlantProjection projection)
        //{
        //    using(LjekobiljeEntities ljekobiljeEntities = new LjekobiljeEntities())
        //    {
        //        Plant toRemove = ljekobiljeEntities.Plants.Where(e = > e.PlantID == projection.PlantID).First();
        //        ljekobiljeEntities.Plants.Remove(toRemove);
        //        ljekobiljeEntities.SaveChanges();
        //    }
        //}

        public DelegateCommand<PlantProjection> DeleteCommand { get; set; }

        public ActionCommand AddPlantCommand { get; set; }
        public DelegateCommand<PlantProjection> UpdateCommand { get; set; }

        public ActionCommand ClearFilterCommand { get; set; }

        private ICollectionView View;

        void DeletePlant(PlantProjection plantProjection)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog((App.GetLanguage() == 1 ? "Da li zaista želite da obrišete biljku čiji je id " : "Do you want to delete plant whose id is ") + plantProjection.PlantId + " ?");
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities ljekobiljeEntities = new LjekobiljeEntities())
                    {
                        Plant toRemove = ljekobiljeEntities.Plants.Where(e => e.PlantId == plantProjection.PlantId).First();
                        ljekobiljeEntities.Plants.Remove(toRemove);
                        ljekobiljeEntities.SaveChanges();
                    }
                    Plants.Remove(plantProjection);
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Nije moguće obisati ovu biljku" : "It is not possible to delete this plant");
                }
            }
        }

        public ObservableCollection<PlantProjection> Plants
        {
            get { return _plants; }
            set { _plants = value; base.NotifyPropertyChanged("Plants"); }
        }

        public PlantViewModel()
        {
            DeleteCommand = new DelegateCommand<PlantProjection>(DeletePlant);
            UpdateCommand = new DelegateCommand<PlantProjection>(UpdatePlant);
            AddPlantCommand = new ActionCommand(AddPlant);
            ClearFilterCommand = new ActionCommand(ClearFilter, true);
            using (LjekobiljeEntities ljekobiljeEntities = new LjekobiljeEntities())
            {
                List<Plant> plants = (from plant in ljekobiljeEntities.Plants select plant).ToList();
                plants.ForEach(e => Plants.Add(new PlantProjection(e)));
            }
            View=CollectionViewSource.GetDefaultView(_plants);
            View.Filter = Filter;
            SelectedCategory = "-";
        }

        public void UpdatePlant(PlantProjection arg)
        {
            UpdatePlantDialog updateDialog = new UpdatePlantDialog(arg);
            bool? result = updateDialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    (arg.PlantName, arg.LatinName, arg.MedicName, arg.RetailPrice) = updateDialog.getFields();
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        entities.Entry(arg.Plant).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        entities.SaveChanges();
                    }
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Ažuriranje nije uspjelo, pokušajte ponovo" : "Update failed, try again");
                }
            }
        }

        public void AddPlant()
        {
            try
            {
                using (LjekobiljeEntities entities = new LjekobiljeEntities())
                {
                    Plant plant = new Plant();
                    plant.PlantName = PlantName;
                    plant.LatinName = LatinName;
                    plant.MedicName = MedicName;
                    plant.RetailPrice = RetailPrice;
                    plant = entities.Plants.Add(plant).Entity;
                    entities.SaveChanges();
                    Plants.Add(new PlantProjection(plant));
                }
            }
            catch
            {

                new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Kreiranje nije uspjelo, provjerite polja koja ste unijeli i probajte ponovo" : "Failed to add new plant, check the fields you entered and try again");
            }
        }

        public Boolean Filter(object obj)
        {
            Boolean flag = false;
            if (FilterText == null || FilterText == "")
                return true;
            if (obj is PlantProjection plant)
                switch (SelectedCategory)
                {
                    case "Plant ID" or "ID biljke": flag = plant.PlantId.ToString().Contains(FilterText); break;
                    case "Plant name" or "Ime": flag = plant.PlantName.ToString().Contains(FilterText); break;
                    case "Latin name" or "Latinsko ime": flag = plant.LatinName.ToString().Contains(FilterText); break;
                    case "Medic name" or "Ljekarsko ime": flag = plant.MedicName.ToString().Contains(FilterText); break;
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

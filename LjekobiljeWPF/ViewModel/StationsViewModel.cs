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
    public class StationsViewModel : BaseViewModel
    {
        private ObservableCollection<Stationsview> _stations = new ObservableCollection<Stationsview>();
        public ObservableCollection<Stationsview> Stations { get { return _stations; } }

        public string Address { get; set; }
        public string City { get; set; }

        private string _filterText;
        private string _selectedCategory;

        public string FilterText { get { return _filterText; } set { _filterText = value; View.Refresh(); NotifyPropertyChanged("FilterText"); } }

        public string SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; View.Refresh(); NotifyPropertyChanged("SelectedCategory"); } }

        public DelegateCommand<Stationsview> DeleteCommand { get; set; }

        public ActionCommand AddCommand { get; set; }

        public DelegateCommand<Stationsview> UpdateCommand { get; set; }

        public ActionCommand ClearFilterCommand { get; set; }

        private ICollectionView View;

        public StationsViewModel()
        {
            LoadDataGrid();
            AddCommand = new ActionCommand(AddStation);
            DeleteCommand = new DelegateCommand<Stationsview>(Delete);
            UpdateCommand = new DelegateCommand<Stationsview>(Update);
            ClearFilterCommand = new ActionCommand(ClearFilter, true);
            View = CollectionViewSource.GetDefaultView(_stations);
            View.Filter = Filter;
            SelectedCategory = "-";
        }

        private void LoadDataGrid()
        {
            _stations.Clear();
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                (from station in db.Stationsviews select station).ToList().ForEach(s => _stations.Add(s));
            }
        }

        public void Delete(Stationsview stationVw)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog((App.GetLanguage() == 1 ? "Da li zaista želite da obrišete otkupnu stanicu čiji je id " : "Do you want to delete station whose id is ") + stationVw.StationId + " ?");
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities db = new LjekobiljeEntities())
                    {
                        Station station = new Station() { StationId = stationVw.StationId };
                        db.Entry(station).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        db.SaveChanges();
                    }
                    _stations.Remove(stationVw);
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Nije moguće obisati ovu stanicu" : "It is not possible to delete this station");
                }
            }
        }

        public void Update(Stationsview stationVw)
        {
            UpdateStationDialog dialog = new UpdateStationDialog(stationVw);
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities db = new LjekobiljeEntities())
                    {
                        Station station = db.Stations.Where(s => s.StationId == stationVw.StationId).First();
                        (station.Adress, station.City) = dialog.getFields();
                        db.SaveChanges();
                    }
                    LoadDataGrid();
                }
                catch (Exception)
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Ažuriranje nije uspjelo, pokušajte ponovo" : "Failed to update, try again");
                }
            }
            //LoadDataGrid();
        }

        public void AddStation()
        {
            try
            {
                using (LjekobiljeEntities db = new LjekobiljeEntities())
                {
                    Station station = new Station();
                    station.Adress = Address;
                    station.City = City;
                    db.Stations.Add(station);
                    db.SaveChanges();

                }
                LoadDataGrid();
            }
            catch
            {

                new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Kreiranje nije uspjelo, provjerite polja koja ste unjeli i pokušajte ponovo" : "Failed to add new station, check the fields you entered and try again");
            }
        }

        public bool Filter(object obj)
        {
            bool flag = false;
            if (FilterText == null || FilterText == "")
                return true;
            if (obj is Stationsview station)
                switch (SelectedCategory)
                {
                    case "Station number" or "Broj stanice": flag = station.StationId.ToString().Contains(FilterText); break;
                    case "Address" or "Adresa": flag = station.Adress.ToString().Contains(FilterText); break;
                    case "City" or "Mjesto": flag = station.City.ToString().Contains(FilterText); break;
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

using Ljekobilje.Dialogs;
using Ljekobilje.View;
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
    internal class CooperantViewModel : BaseViewModel
    {
        private ObservableCollection<CooperantProjection> _cooperants = new ObservableCollection<CooperantProjection>();
        public ObservableCollection<CooperantProjection> Cooperants { get => _cooperants; set { _cooperants = value;} }

        public DelegateCommand<CooperantProjection> UpdateCommand { get; set; }

        public DelegateCommand<CooperantProjection> DeleteCommand { get; set; }

        public ActionCommand AddCooperantCommand { get; set; }

        public DelegateCommand<CooperantProjection> ViewAccountsCommand { get; set; }

        public ActionCommand ClearFilterCommand { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int SelectedStationId { get; set; }

        public List<Station> Stations { get; set; }

        private String _filterText;
        private String _selectedCategory;

        public String FilterText { get { return _filterText; } set { _filterText = value; View.Refresh(); NotifyPropertyChanged("FilterText"); } }

        public String SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; View.Refresh(); NotifyPropertyChanged("SelectedCategory"); } }

        public CooperantViewModel()
        {
            using(LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                Stations = (from station in entities.Stations select station).ToList();
                (from cooperant in entities.Cooperants select cooperant).ToList().ForEach(cooperant => _cooperants.Add(new CooperantProjection(cooperant)));
                DeleteCommand = new DelegateCommand<CooperantProjection>(Delete);
                AddCooperantCommand = new ActionCommand(AddCooperant);
                UpdateCommand = new DelegateCommand<CooperantProjection>(Update);
                ViewAccountsCommand = new DelegateCommand<CooperantProjection>(ViewAccounts, true);
                ClearFilterCommand = new ActionCommand(ClearFilter, true);
                View = CollectionViewSource.GetDefaultView(_cooperants);
                View.Filter = Filter;
                SelectedCategory = "-";
            }
        }

        /*public Boolean Filter(object obj)
        {
            if (FilterText == null || FilterText == "")
                return true;
            if(obj is CooperantProjection)
            
                //switch (SelectedCategory)
                //{
                 //   case "CooperantID": return cooperant.CooperantID.ToString().Contains(FilterText);
                  //  case "First name": return cooperant.FirstName.ToString().Contains(FilterText);
                   // case "Last name": return cooperant.LastName.ToString().Contains(FilterText);
                    //case "StationID": return cooperant.StationID.ToString().Contains(FilterText); default: return true;

:
                //}
                return true;
            
            return false;
        }*/

        public Boolean Filter(object obj)
        {
            Boolean flag = true;
            if (FilterText == null || FilterText == "")
                return true;
            if(obj is CooperantProjection cooperant)
                switch (SelectedCategory)
                {
                    case "Cooperant ID" or "ID kooperanta": flag= cooperant.CooperantId.ToString().Contains(FilterText); break;
                    case "First name" or "Ime": flag= cooperant.FirstName.ToString().Contains(FilterText); break;
                    case "Last name" or "Prezime": flag = cooperant.LastName.ToString().Contains(FilterText); break;
                    case "Station number" or "Broj stanice": flag=cooperant.StationId.ToString().Contains(FilterText); break;
                    default: flag = true; break;
                }
                return flag;
            
           
        }

        public void Delete(CooperantProjection cooperantProjection)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog((App.GetLanguage() == 1 ? "Da li zaista želite da obrišete kooperanta čiji je id " : "Do you want to delete cooperant whose id is ") + cooperantProjection.CooperantId + " ?");
            if (result == true)
            {
                try
                {
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        entities.Entry(cooperantProjection.Cooperant).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        entities.SaveChanges();
                    }

                    _cooperants.Remove(cooperantProjection);
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Nije moguće obisati ovog kooperanta" : "It is not possible to delete this cooperant");
                }
            }
        }

        public ICollectionView View { get; }

        public void AddCooperant()
        {
            try
            {
                using (LjekobiljeEntities entities = new LjekobiljeEntities())
                {
                    Cooperant cooperant = new Cooperant();
                    cooperant.FirstName = FirstName;
                    cooperant.LastName = LastName;
                    cooperant.Phone = Phone;
                    cooperant.Email = Email;
                    cooperant.StationId = SelectedStationId;
                    cooperant.City = City;
                    cooperant.Adress = Address;
                    cooperant = entities.Cooperants.Add(cooperant).Entity;
                    entities.SaveChanges();
                    Cooperants.Add(new CooperantProjection(cooperant));
                }
            }
            catch
            {
                
                new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Kreiranje nije uspjelo, provjerite unesena polja" : "Failed to add new cooperant, check the fileds you entered");
            }
        }

        public void Update(CooperantProjection cooperant)
        {
            UpdateCooperantDialog dialog = new UpdateCooperantDialog(cooperant);
            bool? result = dialog.ShowDialog();
            if(result == true)
            {
                try
                {
                    (cooperant.FirstName, cooperant.LastName, cooperant.Address, cooperant.Email, cooperant.Phone, cooperant.City, cooperant.StationId) = dialog.getFields();
                    cooperant.Cooperant.Station = null;
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        entities.Entry(cooperant.Cooperant).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        entities.SaveChanges();
                    }
                }
                catch
                {
                    //throw;
                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Ažuriranje nije uspjelo, pokušajte ponovo" : "Update failed, try again");
                }
            }
        }

        public void ViewAccounts(CooperantProjection projection)
        {
            AccountWindow window = new AccountWindow(projection.CooperantId);
            window.Title = projection.FirstName + " " + projection.LastName + " " + "[" + projection.CooperantId + "] " + "-" + (App.GetLanguage() == 1 ? "bankovni računi" : "bank accounts");
            window.Show();
        }

        public void ClearFilter()
        {
            FilterText = ""; SelectedCategory = "-";
        }
    }
}

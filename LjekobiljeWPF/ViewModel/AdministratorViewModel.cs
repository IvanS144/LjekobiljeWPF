using Ljekobilje;
using Ljekobilje.Dialogs;
using Ljekobilje.Projections;
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
    public class AdministratorViewModel : BaseViewModel
    {
        private string _filterText;
        private string _selectedCategory;
        private ObservableCollection<UserProjection> _users = new ObservableCollection<UserProjection>();
        public ObservableCollection<UserProjection> Users { get { return _users; } set { _users = value; NotifyPropertyChanged("Users"); } }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Language { get; set; } = "English";

        public string Theme { get; set; } = "Light";

        public int UserType { get; set; }

        public string FilterText { get { return _filterText; } set { _filterText = value; View.Refresh(); NotifyPropertyChanged("FilterText"); } }

        public string SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; View.Refresh(); NotifyPropertyChanged("SelectedCategory"); } }

        public DelegateCommand<UserProjection> DeleteCommand { get; set; }

        public ActionCommand AddCommand { get; set; }

        public DelegateCommand<UserProjection> UpdateCommand { get; set; }

        public ActionCommand ClearFilterCommand { get; set; }

        private ICollectionView View;

        public AdministratorViewModel()
        {
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                (from u in db.Users select u).ToList().ForEach(u => _users.Add(new UserProjection(u)));
                DeleteCommand = new DelegateCommand<UserProjection>(Delete);
                AddCommand = new ActionCommand(Add);
                UpdateCommand = new DelegateCommand<UserProjection>(Update);
                ClearFilterCommand = new ActionCommand(ClearFilter, true);
                View = CollectionViewSource.GetDefaultView(_users);
                View.Filter = Filter;
                SelectedCategory = "-";
            }
        }

        public void Delete(UserProjection u)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog((App.GetLanguage() == 1 ? "Da li zaista želite da obrišete korisnika čiji je id " : "Do you want to delete user whose id is ") + u.UserId +" "+ "?");
            if (result == true)
            {
                using (LjekobiljeEntities db = new LjekobiljeEntities())
                {
                    db.Entry(u.User).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                    Users.Remove(u);
                }
            }
        }

        public void Add()
        {
            try
            {
                using (LjekobiljeEntities db = new LjekobiljeEntities())
                {
                    User u = new User();
                    u.Username = Username;
                    u.Password = Password;
                    u.Language = Language == "English" ? 1 : 2;
                    u.Theme = Theme == "Lignt" ? 1 : 2;
                    u.UserType = UserType + 1;
                    u = db.Users.Add(u).Entity;
                    db.SaveChanges();
                    _users.Add(new UserProjection(u));
                }
            }
            catch
            {

                new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Kreiranje nije uspjelo, provjerite polja koja ste unjeli i pokušajte ponovo" : "Failed to add new user, check the fields you entered and try again");
            }
        }

        public void Update(UserProjection user)
        {
            UpdateUserDialog dialog = new UpdateUserDialog(user);
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    (user.Username, user.Password, user.UserType) = dialog.getFields();
                    using (LjekobiljeEntities db = new LjekobiljeEntities())
                    {
                        db.Entry(user.User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Ažuriranje nije uspjelo, pokušajte ponovo" : "Failed to update user,try again");
                }
            }
        }

        public bool Filter(object obj)
        {
            bool flag = true;
            if (FilterText == null || FilterText == "")
                return true;
            if (obj is UserProjection u)
                switch (SelectedCategory)
                {
                    case "ID": flag = u.UserId.ToString().Contains(FilterText); break;
                    case "Username" or "Korisničko ime": flag = u.Username.ToString().Contains(FilterText); break;
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

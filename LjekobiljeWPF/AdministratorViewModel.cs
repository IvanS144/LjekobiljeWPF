using Ljekobilje.Dialogs;
using Ljekobilje.Projections;
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
    public class AdministratorViewModel : BaseViewModel
    {
        private String _filterText;
        private String _selectedCategory;
        private ObservableCollection<UserProjection> _users = new ObservableCollection<UserProjection>();
        public ObservableCollection<UserProjection> Users { get { return _users; } set { _users = value; NotifyPropertyChanged("Users"); } }
        public String Username { get; set; }
        public String Password { get; set; }

        public String Language { get; set; } = "English";

        public String Theme { get; set; } = "Light";

        public int UserType { get; set; }

        public String FilterText { get { return _filterText; } set { _filterText = value; View.Refresh(); NotifyPropertyChanged("FilterText"); } }

        public String SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; View.Refresh(); NotifyPropertyChanged("SelectedCategory"); } }

        public DelegateCommand<UserProjection> DeleteCommand { get; set; }

        public ActionCommand AddCommand { get; set; }

        public DelegateCommand<UserProjection> UpdateCommand { get; set; }

        public ActionCommand ClearFilterCommand { get; set; }

        private ICollectionView View;

        public AdministratorViewModel()
        {
            using(LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                (from u in entities.Users select u).ToList().ForEach(u => _users.Add(new UserProjection(u)));
                DeleteCommand= new DelegateCommand<UserProjection>(Delete);
                AddCommand = new ActionCommand(Add);
                UpdateCommand= new DelegateCommand<UserProjection>(Update);
                ClearFilterCommand = new ActionCommand(ClearFilter, true);
                View = CollectionViewSource.GetDefaultView(_users);
                View.Filter = Filter;
                SelectedCategory = "-";
            }
        }

        public void Delete(UserProjection u)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog(App.GetLanguage() == 1 ? "Da li zaista želite da obrišete korisnika čiji je id " : "Do you want to delete user whose id is " + u.UserId);
            if (result == true)
            {
                using (LjekobiljeEntities entities = new LjekobiljeEntities())
                {
                    entities.Entry(u.User).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    entities.SaveChanges();
                    Users.Remove(u);
                }
            }
        }

        public void Add()
        {
            try
            {
                using (LjekobiljeEntities entities = new LjekobiljeEntities())
                {
                    User u = new User();
                    u.Username = Username;
                    u.Password = Password;
                    u.Language = (Language == "English") ? 1 : 2;
                    u.Theme = (Theme == "Lignt") ? 1 : 2;
                    u.UserType = UserType + 1;
                    u = entities.Users.Add(u).Entity;
                    entities.SaveChanges();
                    _users.Add(new UserProjection(u));
                }
            }
            catch
            {

                new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Dodavanje nije uspjelo, provjerite polja koja ste unjeli i pokušajte ponovo" : "Failed to add new user, check the fields you entered and try again");
            }
        }

        public void Update(UserProjection user)
        {
            UpdateUserDialog dialog = new UpdateUserDialog(user);
            bool? result = dialog.ShowDialog();
            if(result == true)
            {
                try
                {
                    (user.Username, user.Password, user.UserType) = dialog.getFields();
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        entities.Entry(user.User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        entities.SaveChanges();
                    }
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Ažuriranje nije uspjelo, pokušajte ponovo" : "Failed to update user,try again");
                }
            }
        }

        public Boolean Filter(object obj)
        {
            Boolean flag = true;
            if (FilterText == null || FilterText == "")
                return true;
            if (obj is UserProjection u)
                switch (SelectedCategory)
                {
                    case "UserID" or "ID korisnika": flag = u.UserId.ToString().Contains(FilterText); break;
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

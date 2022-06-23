using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljekobilje.Projections
{
    public class UserProjection : BaseViewModel
    {
        private User _user;

        public User User => _user;

        public int UserId { get => _user.UserId; set => _user.UserId = value; }

        public String Username { get => _user.Username; set { _user.Username = value; NotifyPropertyChanged("Username"); } }

        public String Password { get => _user.Password; set { _user.Password = value; NotifyPropertyChanged("Password"); } }

        public int UserType { get => _user.UserType; set { _user.UserType = value; NotifyPropertyChanged("UserType"); } }

        public String UserTypeText { get { switch (UserType) { case 1: return "Na obuci"; case 2: return "Radnik"; case 3: return "Administrator"; default: return ""; } }}

        public UserProjection(User u)
        {
            _user = u;
        }
    }
}

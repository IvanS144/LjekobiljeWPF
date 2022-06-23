using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ljekobilje
{
    public class LoginViewModel
    {
        private Window window;
        public String Username { get; set; }
        public String Password { get; set; }

        public ActionCommand LoginCommand { get; set; }

        public LoginViewModel(Window window)
        {
            this.window = window;
            LoginCommand = new ActionCommand(Login, true);
        }

        public void Login()
        {
            List<User> users;
            using(LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                users = entities.Users.ToList();
            }
            foreach(var u in users)
            {
                if(u.Username==Username && u.Password==Password)
                {
                    (Application.Current as App).CurrentUser = u;
                    new MainWindow().Show();
                    window.Close();
                    break;
                }
            }
        }
    }
}

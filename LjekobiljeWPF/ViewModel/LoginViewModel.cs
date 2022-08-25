using Ljekobilje;
using Ljekobilje.Dialogs;
using LjekobiljeWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LjekobiljeWPF.ViewModel
{
    public class LoginViewModel
    {
        private Window window;
        public string Username { get; set; }
        public string Password { get; set; }

        public ActionCommand LoginCommand { get; set; }

        public LoginViewModel(Window window)
        {
            this.window = window;
            LoginCommand = new ActionCommand(Login, true);
        }

        public void Login()
        {
            bool found = false;
            List<User> users;
            using (LjekobiljeEntities db = new LjekobiljeEntities())
            {
                users = db.Users.ToList();
            }
            foreach (var u in users)
            {
                if (u.Username == Username && u.Password == Password)
                {
                    (Application.Current as App).CurrentUser = u;
                    new MainWindow().Show();
                    found = true;
                    window.Close();
                    break;
                }
            }
            if(!found)
            {
                new ErrorDialog().ShowDialog("Pristupni podaci nisu validni/Invalid credentials!");
            }
        }
    }
}

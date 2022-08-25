using Ljekobilje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LjekobiljeWPF
{
    public class ActionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _action;

        private bool _ignoreUserType;

        public ActionCommand(Action action)
        {
            _ignoreUserType = false;
            _action = action;
        }

        public ActionCommand(Action action, bool ignoreUser)
        {
            _ignoreUserType = ignoreUser;
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            if (_ignoreUserType)
                return true;
            return (Application.Current as App).CurrentUser.UserType > 1;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }
    }
}
